using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class CustomerInfoOper : SingleTon<CustomerInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="customerinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(CustomerInfo customerinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (customerinfo.customerNo != null)
                dict.Add("@customerNo", customerinfo.customerNo.ToString());
            if (customerinfo.customerType != null)
                dict.Add("@customerType", customerinfo.customerType.ToString());
            if (customerinfo.contractNo != null)
                dict.Add("@contractNo", customerinfo.contractNo.ToString());
            if (customerinfo.customerName != null)
                dict.Add("@customerName", customerinfo.customerName.ToString());
            if (customerinfo.telNo != null)
                dict.Add("@telNo", customerinfo.telNo.ToString());
            if (customerinfo.mobileNo != null)
                dict.Add("@mobileNo", customerinfo.mobileNo.ToString());
            if (customerinfo.certNo != null)
                dict.Add("@certNo", customerinfo.certNo.ToString());
            if (customerinfo.estateNo != null)
                dict.Add("@estateNo", customerinfo.estateNo.ToString());
            if (customerinfo.address != null)
                dict.Add("@address", customerinfo.address.ToString());
            if (customerinfo.houseNo != null)
                dict.Add("@houseNo", customerinfo.houseNo.ToString());
            if (customerinfo.cellNo != null)
                dict.Add("@cellNo", customerinfo.cellNo.ToString());
            if (customerinfo.roomNo != null)
                dict.Add("@roomNo", customerinfo.roomNo.ToString());
            if (customerinfo.useState != null)
                dict.Add("@useState", customerinfo.useState.ToString());
            if (customerinfo.defineNo1 != null)
                dict.Add("@defineNo1", customerinfo.defineNo1.ToString());
            if (customerinfo.defineNo2 != null)
                dict.Add("@defineNo2", customerinfo.defineNo2.ToString());
            if (customerinfo.defineNo3 != null)
                dict.Add("@defineNo3", customerinfo.defineNo3.ToString());
            if (customerinfo.remark != null)
                dict.Add("@remark", customerinfo.remark.ToString());
            if (customerinfo.payWay != null)
                dict.Add("@payWay", customerinfo.payWay.ToString());
            if (customerinfo.bankNo != null)
                dict.Add("@bankNo", customerinfo.bankNo.ToString());
            if (customerinfo.bankAuthNo != null)
                dict.Add("@bankAuthNo", customerinfo.bankAuthNo.ToString());
            if (customerinfo.accountNo != null)
                dict.Add("@accountNo", customerinfo.accountNo.ToString());
            if (customerinfo.accountName != null)
                dict.Add("@accountName", customerinfo.accountName.ToString());
            if (customerinfo.bankCheck != null)
                dict.Add("@bankCheck", customerinfo.bankCheck.ToString());
            if (customerinfo.buildTime != null)
                dict.Add("@buildTime", customerinfo.buildTime.ToString());
            if (customerinfo.editTime != null)
                dict.Add("@editTime", customerinfo.editTime.ToString());
            if (customerinfo.Operator != null)
                dict.Add("@Operator", customerinfo.Operator.ToString());
            if (customerinfo.loginName != null)
                dict.Add("@loginName", customerinfo.loginName.ToString());
            if (customerinfo.Password != null)
                dict.Add("@Password", customerinfo.Password.ToString());
            if (customerinfo.taxNo != null)
                dict.Add("@taxNo", customerinfo.taxNo.ToString());
            if (customerinfo.enterpriseNo != null)
                dict.Add("@enterpriseNo", customerinfo.enterpriseNo.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="customerinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(CustomerInfo customerinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (customerinfo.customerNo != null)
            {
                part1.Append("customerNo,");
                part2.Append("@customerNo,");
            }
            if (customerinfo.customerType != null)
            {
                part1.Append("customerType,");
                part2.Append("@customerType,");
            }
            if (customerinfo.contractNo != null)
            {
                part1.Append("contractNo,");
                part2.Append("@contractNo,");
            }
            if (customerinfo.customerName != null)
            {
                part1.Append("customerName,");
                part2.Append("@customerName,");
            }
            if (customerinfo.telNo != null)
            {
                part1.Append("telNo,");
                part2.Append("@telNo,");
            }
            if (customerinfo.mobileNo != null)
            {
                part1.Append("mobileNo,");
                part2.Append("@mobileNo,");
            }
            if (customerinfo.certNo != null)
            {
                part1.Append("certNo,");
                part2.Append("@certNo,");
            }
            if (customerinfo.estateNo != null)
            {
                part1.Append("estateNo,");
                part2.Append("@estateNo,");
            }
            if (customerinfo.address != null)
            {
                part1.Append("address,");
                part2.Append("@address,");
            }
            if (customerinfo.houseNo != null)
            {
                part1.Append("houseNo,");
                part2.Append("@houseNo,");
            }
            if (customerinfo.cellNo != null)
            {
                part1.Append("cellNo,");
                part2.Append("@cellNo,");
            }
            if (customerinfo.roomNo != null)
            {
                part1.Append("roomNo,");
                part2.Append("@roomNo,");
            }
            if (customerinfo.useState != null)
            {
                part1.Append("useState,");
                part2.Append("@useState,");
            }
            if (customerinfo.defineNo1 != null)
            {
                part1.Append("defineNo1,");
                part2.Append("@defineNo1,");
            }
            if (customerinfo.defineNo2 != null)
            {
                part1.Append("defineNo2,");
                part2.Append("@defineNo2,");
            }
            if (customerinfo.defineNo3 != null)
            {
                part1.Append("defineNo3,");
                part2.Append("@defineNo3,");
            }
            if (customerinfo.remark != null)
            {
                part1.Append("remark,");
                part2.Append("@remark,");
            }
            if (customerinfo.payWay != null)
            {
                part1.Append("payWay,");
                part2.Append("@payWay,");
            }
            if (customerinfo.bankNo != null)
            {
                part1.Append("bankNo,");
                part2.Append("@bankNo,");
            }
            if (customerinfo.bankAuthNo != null)
            {
                part1.Append("bankAuthNo,");
                part2.Append("@bankAuthNo,");
            }
            if (customerinfo.accountNo != null)
            {
                part1.Append("accountNo,");
                part2.Append("@accountNo,");
            }
            if (customerinfo.accountName != null)
            {
                part1.Append("accountName,");
                part2.Append("@accountName,");
            }
            if (customerinfo.bankCheck != null)
            {
                part1.Append("bankCheck,");
                part2.Append("@bankCheck,");
            }
            if (customerinfo.buildTime != null)
            {
                part1.Append("buildTime,");
                part2.Append("@buildTime,");
            }
            if (customerinfo.editTime != null)
            {
                part1.Append("editTime,");
                part2.Append("@editTime,");
            }
            if (customerinfo.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            if (customerinfo.loginName != null)
            {
                part1.Append("loginName,");
                part2.Append("@loginName,");
            }
            if (customerinfo.Password != null)
            {
                part1.Append("Password,");
                part2.Append("@Password,");
            }
            if (customerinfo.taxNo != null)
            {
                part1.Append("taxNo,");
                part2.Append("@taxNo,");
            }
            if (customerinfo.enterpriseNo != null)
            {
                part1.Append("enterpriseNo,");
                part2.Append("@enterpriseNo,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into customerinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="customerinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(CustomerInfo customerinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update customerinfo set ");
            if (customerinfo.customerType != null)
                part1.Append("customerType = @customerType,");
            if (customerinfo.contractNo != null)
                part1.Append("contractNo = @contractNo,");
            if (customerinfo.customerName != null)
                part1.Append("customerName = @customerName,");
            if (customerinfo.telNo != null)
                part1.Append("telNo = @telNo,");
            if (customerinfo.mobileNo != null)
                part1.Append("mobileNo = @mobileNo,");
            if (customerinfo.certNo != null)
                part1.Append("certNo = @certNo,");
            if (customerinfo.estateNo != null)
                part1.Append("estateNo = @estateNo,");
            if (customerinfo.address != null)
                part1.Append("address = @address,");
            if (customerinfo.houseNo != null)
                part1.Append("houseNo = @houseNo,");
            if (customerinfo.cellNo != null)
                part1.Append("cellNo = @cellNo,");
            if (customerinfo.roomNo != null)
                part1.Append("roomNo = @roomNo,");
            if (customerinfo.useState != null)
                part1.Append("useState = @useState,");
            if (customerinfo.defineNo1 != null)
                part1.Append("defineNo1 = @defineNo1,");
            if (customerinfo.defineNo2 != null)
                part1.Append("defineNo2 = @defineNo2,");
            if (customerinfo.defineNo3 != null)
                part1.Append("defineNo3 = @defineNo3,");
            if (customerinfo.remark != null)
                part1.Append("remark = @remark,");
            if (customerinfo.payWay != null)
                part1.Append("payWay = @payWay,");
            if (customerinfo.bankNo != null)
                part1.Append("bankNo = @bankNo,");
            if (customerinfo.bankAuthNo != null)
                part1.Append("bankAuthNo = @bankAuthNo,");
            if (customerinfo.accountNo != null)
                part1.Append("accountNo = @accountNo,");
            if (customerinfo.accountName != null)
                part1.Append("accountName = @accountName,");
            if (customerinfo.bankCheck != null)
                part1.Append("bankCheck = @bankCheck,");
            if (customerinfo.buildTime != null)
                part1.Append("buildTime = @buildTime,");
            if (customerinfo.editTime != null)
                part1.Append("editTime = @editTime,");
            if (customerinfo.Operator != null)
                part1.Append("Operator = @Operator,");
            if (customerinfo.loginName != null)
                part1.Append("loginName = @loginName,");
            if (customerinfo.Password != null)
                part1.Append("Password = @Password,");
            if (customerinfo.taxNo != null)
                part1.Append("taxNo = @taxNo,");
            if (customerinfo.enterpriseNo != null)
                part1.Append("enterpriseNo = @enterpriseNo,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where customerNo= @customerNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="CustomerInfo"></param>
        /// <returns></returns>
        public void Add(CustomerInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model);
            var dict = GetParameters(model);
            SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction);
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="CustomerInfo"></param>
        /// <returns></returns>
        public void Update(CustomerInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
            var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
