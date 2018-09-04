using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class CustomerInfoOper : SingleTon<CustomerInfoOper>
    {
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<CustomerView> GetList(CustomerReq req, int size, string lastCId)
        {
            var search = req.search ?? "";
            var order = req.orderField;
            var desc = Convert.ToBoolean(req.isDesc);
            var index = Convert.ToInt32(req.pageIndex);

            var orderStr = $"order by {order} ";
            if (desc)
                orderStr += " desc ";
            else
                orderStr += " asc ";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                 { "@search2", search },
                  { "@lastcid", lastCId },
            };

            var condition = $" 1=1 and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";

            var name = req.lastName;
            if (name != null)
            {
                var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(name);

                if (cid != null)
                {
                    dict.Add("cid", cid);
                    condition += $" and  (cid1=@cid or cid2=@cid or cid3=@cid or cid4=@cid) ";
                }
                else
                    return new List<CustomerView>();
            }

            if (!search.IsNullOrEmpty())
                condition += " and (customerName like @search or address like @search or customerNo=@search2 or mobileNo like @search ) ";

            if (req.useState != null)
                condition += $" and useState={req.useState} ";
            if (req.customerType != null)
                condition += $" and customerType={req.customerType}";
            if (req.estateNo != null)
                condition += $" and estateNo='{req.estateNo}'";
            if (req.factoryNo != null)
                condition += $" and factoryNo='{req.factoryNo}'";
            if (req.meterType != null)
                condition += $" and meterTypeNo='{req.meterType}'";
            if (req.operatorName != null)
                condition += $" and Operator='{req.operatorName}'";


            return SqlHelper.Instance.GvpForCustomerInfo<CustomerView>("CustomerView", @"select customerNo,
customerType,
contractNo,
customerName,
telNo,
mobileNo,
certNo,
estateNo,
estateName,
address,
houseNo,
cellNo,
roomNo,
useState,
defineNo1,
defineNo2,
defineNo3,
remark,
payWay,
bankNo,
bankAuthNo,
accountName,
accountNo,
bankCheck,
buildTime,
editTime,
Operator,
taxNo,
enterpriseNo,
CustTypeName,
useStateName,
payWayName,
bankCheckName,
enterpriseNoName,
bankName,
factoryNo,
meterTypeNo,
factoryName,
meterTypeName,
deviceNo,
communicateNo from CustomerView ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// 获取客户总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(CustomerReq req, string lastCId)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                 { "@search2", search },
                  { "@lastcid", lastCId },
            };

            var condition = $" 1=1 and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            if (!search.IsNullOrEmpty())
                condition += " and (customerName like @search or address like @search or customerNo=@search2 or mobileNo like @search ) ";

            var name = req.lastName;
            if (name != null)
            {
                var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(name);

                if (cid != null)
                {
                    dict.Add("cid", cid);
                    condition += $" and  (cid1=@cid or cid2=@cid or cid3=@cid or cid4=@cid) ";
                }
                else
                    return 0;
            }

            if (req.useState != null)
                condition += $" and useState={req.useState} ";
            if (req.customerType != null)
                condition += $" and customerType={req.customerType}";
            if (req.estateNo != null)
                condition += $" and estateNo='{req.estateNo}'";
            if (req.factoryNo != null)
                condition += $" and factoryNo='{req.factoryNo}'";
            if (req.meterType != null)
                condition += $" and meterTypeNo='{req.meterType}'";
            if (req.operatorName != null)
                condition += $" and Operator='{req.operatorName}'";
            var list = SqlHelper.Instance.GdcForCustomerInfo<CustomerView>("CustomerView", condition, dict);
            return list.Count;
        }

        /// <summary>
        /// 获取客户最后一个no
        /// </summary>
        /// <returns></returns>
        public int GetLastNo()
        {
            string str = "select top 1 customerno from customerInfo order by customerNo desc";
            var r = SqlHelper.Instance.ExecuteScalar(str);
            return Convert.ToInt32(r);
        }

        /// <summary>
        /// 开户用，因为提前生成主键
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateC(CustomerInfo c, SqlConnection conn, SqlTransaction tran)
        {
            var str = GetUpdateStr(c);
            var dict = GetParameters(c);
            return SqlHelper.Instance.ExcuteNonQuery(str, dict, conn, tran);
        }

        /// <summary>
        /// 获取未开户的客户视图列表,nbc表相关的不算
        /// </summary>
        /// <param name="lastCId"></param>
        /// <returns></returns>
        public List<CustomerView> GetNotOpen(string lastCId)
        {
            //return SqlHelper.Instance.GetByCondition<CustomerInfo>("CustomerInfo"," useState=0 ");
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            string str = "select * from CustomerView where defineNo1!='NBCivil' and usestate=0 and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) order by customerNo desc";
            return SqlHelper.Instance.ExecuteGetDt<CustomerView>(str, dict);

        }

        /// <summary>
        /// 根据客户编号获取客户的信息
        /// </summary>
        /// <param name="no"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public CustomerInfo GetByNo(string no, AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                  { "@no", no},
                    { "@lastcid", lastCId },
            };
            string str = $"select * from CustomerView where customerNo=@no and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            var temp = SqlHelper.Instance.ExecuteGetDt<CustomerView>(str, dict);
            if (temp.Count == 0)
                return new CustomerInfo();

            str = $"select * from CustomerInfo where customerNo=@no";
            var list = SqlHelper.Instance.ExecuteGetDt<CustomerInfo>(str, dict);
            return list.FirstOrDefault();
        }

        public CustomerInfo GetByNo(string no)
        {
            var dict = new Dictionary<string, string>
            {
                  { "@no", no},
            };

            var str = $"select * from CustomerInfo where customerNo=@no";
            var list = SqlHelper.Instance.ExecuteGetDt<CustomerInfo>(str, dict);
            if (list.Count == 0)
                return null;
            return list.FirstOrDefault();
        }

        public int DeleteByCustomerNo(string customerNo, AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                { "@no", customerNo},
                { "@lastcid", lastCId },
            };
            string str2 = $"select * from CustomerView where customerNo=@no and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            var list = SqlHelper.Instance.ExecuteGetDt<CustomerView>(str2, dict);
            if (list.Count == 0)
                return 0;
            else
            {
                if (list.First().useState == 1)
                    return 2;
                string str = "DELETE from customerInfo where customerNo=" + customerNo;
                var r = SqlHelper.Instance.ExcuteNonQuery(str, new Dictionary<string, string>());
                if (r > 0)
                {
                    str = "delete from allinone_customer_user where customerId=" + customerNo;
                    SqlHelper.Instance.ExcuteNonQuery(str, new Dictionary<string, string>());
                    return 1;
                }
                return 0;
            }
        }

        public List<CustomerView> GetListByUser(AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                { "@lastcid", lastCId },
            };
            string str2 = $"select * from CustomerView where 1=1 and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            return SqlHelper.Instance.ExecuteGetDt<CustomerView>(str2, dict);
        }

        public CustomerView GetViewByNo(string no, AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                  { "@no", no},
                    { "@lastcid", lastCId },
            };
            string str = $"select * from CustomerView where customerNo=@no and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            var temp = SqlHelper.Instance.ExecuteGetDt<CustomerView>(str, dict);
            if (temp.Count == 0)
                return new CustomerView();

            //str = $"select * from CustomerInfo where customerNo=@no";
            //var list = SqlHelper.Instance.ExecuteGetDt<CustomerView>(str, dict);
            return temp.FirstOrDefault();
        }

        public bool IsCustomerOpen(string no)
        {
            var customer = GetByNo(no);
            if (customer.useState == 1)
                return true;
            return false;
        }
    }
}
