using System.Text;
using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using System.Data.SqlClient;
using HHTDCDMR.Oper.Function;

namespace DbOpertion.DBoperation
{
    public partial class DeviceInfoOper : SingleTon<DeviceInfoOper>
    {
        public List<DeviceView> GetList(MeterReq req, int size, AllInOne_UserInfo user)
        {
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var search = req.search ?? "";
            var order = req.orderField;
            var desc = Convert.ToBoolean(req.isDesc);
            var index = Convert.ToInt32(req.pageIndex);
            //var size = 5;
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
                    return new List<DeviceView>();
            }

            if (!search.IsNullOrEmpty())
                condition += $" and (deviceNo=@search2 or customerNo=@search2 or communicateNo=@search2 or customerName like @search or address like @search or meterNo like @search ) ";

            if (req.meterTypeNo != null)
                condition += $" and meterTypeNo='{req.meterTypeNo}' ";
            if (req.factoryNo != null)
                condition += $" and factoryNo='{req.factoryNo}' ";
            if (req.openState != null)
                condition += $" and openState={req.openState} ";
            if (req.fluidNo != null)
                condition += $" and fluidNo='{req.fluidNo}' ";
            if (req.Operator != null)
                condition += $" and Operator='{req.Operator}' ";

            return SqlHelper.Instance.GvpForDeviceView<DeviceView>("DeviceView", $@"select distinct meterNo,
                                communicateNo,
                                barCode,
                                customerNo,
                                meterTypeNo,
                                factoryNo,
                                openState,
                                caliber,
                                ProtocolNo,
                                baseVolume,
                                fluidNo,
                                lat,
                                lng,
                                remark,
                                defineNo1,
                                defineNo2,
                                defineNo3,
                                buildTime,
                                editTime,
                                Operator,
                                deviceNo,
                                collectorNo,
                                MeterType,
                                isConcentrate,
                                AlarmTimes,
                                AlarmInvTime,
                                CommMode,
                                LinkMode,
                                factoryName,
                                meterTypeName,
                                openStateName,
                                fluidName,
                                customerName,
                                address,
                                LoginState,
                                FMState,
                                FMStateMsg,
                                LoginStateMsg,
                                customerType,
CustTypeName from DeviceView ", condition, index, size, orderStr, dict);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (list[i].userName == "admin")
            //        list[i].userName = user.admin;
            //}
            // return list;
        }

        /// <summary>
        /// 获取设备总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(MeterReq req, AllInOne_UserInfo user)
        {
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);

            var search = req.search ?? "";
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
                    return 0;
            }

            if (!search.IsNullOrEmpty())
                condition += $" and (deviceNo=@search2 or customerNo=@search2 or communicateNo=@search2 or customerName like @search or address like @search or meterNo like @search ) ";

            if (req.meterTypeNo != null)
                condition += $" and meterTypeNo='{req.meterTypeNo}' ";
            if (req.factoryNo != null)
                condition += $" and factoryNo='{req.factoryNo}' ";
            if (req.openState != null)
                condition += $" and openState={req.openState} ";
            if (req.fluidNo != null)
                condition += $" and fluidNo='{req.fluidNo}' ";
            if (req.Operator != null)
                condition += $" and Operator='{req.Operator}' ";
            var list = SqlHelper.Instance.GdcForDeviceView<DeviceView>("DeviceView", condition, dict);
            return list.Count;
        }

        /// <summary>
        /// 获取自己有关的设备的meterNo
        /// </summary>
        /// <param name="lastCId"></param>
        /// <returns></returns>
        public List<int> GetDeviceIds(AllInOne_UserInfo user)
        {
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };

            string str = "select meterNo from deviceView where cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid ";
            var list = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
            return list.Select(p => p.meterNo).ToList();
        }

        /// <summary>
        /// 获取有关自己的设备视图
        /// </summary>
        /// <param name="lastCId"></param>
        /// <returns></returns>
        public List<DeviceView> GetDeviceViews(string lastCId)
        {
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };

            string str = "select meterNo from deviceView where cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid ";
            return SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
        }

        public List<DeviceView> GetDeviceViewByCompanyName(string name, AllInOne_UserInfo user)
        {
            //var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(name);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
                    { "@cid",cid}
            };

            var condition = "";
            if (name != null)
            {
                //var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(name);

                //dict.Add("@name",name);
                //condition += $" and username=@name";
                //var cid = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);

                if (cid != null)
                {
                    //dict.Add("@cid", cid);
                    condition += $" and  (cid1=@cid or cid2=@cid or cid3=@cid or cid4=@cid) ";
                }
                else
                    return new List<DeviceView>();
            }

            string str = "select distinct customerNo,customerName from deviceView where customerno!='' and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid ) " + condition;
            return SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
        }

        /// <summary>
        /// 获取设备的最后一个序号
        /// </summary>
        /// <returns></returns>
        public int GetLastId()
        {
            string str = "select top 1 meterNo from deviceinfo order by meterNo desc";
            var r = SqlHelper.Instance.ExecuteScalar(str);
            return Convert.ToInt32(r);
        }

        /// <summary>
        /// 通过no获取设备信息
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public DeviceInfo GetByNo(int no, AllInOne_UserInfo user)
        {
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                  { "@no", no.ToString() },
                    { "@lastcid", lastCId },
            };
            string str = $"select * from deviceview where meterNo=@no and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            var temp = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
            if (temp.Count == 0)
                return new DeviceInfo();
            str = $"select * from deviceInfo where meterNo={no}";
            var r = SqlHelper.Instance.ExecuteGetDt<DeviceInfo>(str, new Dictionary<string, string>());
            return r.FirstOrDefault();
        }

        /// <summary>
        /// 根据设备号获取设备视图
        /// </summary>
        /// <param name="no"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public DeviceView GetViewByNo(int no, AllInOne_UserInfo user)
        {
            //var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                  { "@no", no.ToString() },
                    { "@lastcid", lastCId },
            };
            string str = $"select * from deviceview where meterNo=@no and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            var temp = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
            if (temp.Count == 0)
                return new DeviceView { meterNo = 0 };
            //str = $"select * from deviceInfo where meterNo={no}";
            //var r = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, new Dictionary<string, string>());
            return temp.FirstOrDefault();
        }

        /// <summary>
        /// 根据设备号获取设备视图
        /// </summary>
        /// <param name="no"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public OneFLMeterDataView GetOneFLMeterViewViewByNo(int no, AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var dict = new Dictionary<string, string>
            {
                  { "@no", no.ToString() },
                    { "@lastcid", lastCId },
            };
            string str = $"select * from OneFLMeterDataView where flmeterNo=@no and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            var temp = SqlHelper.Instance.ExecuteGetDt<OneFLMeterDataView>(str, dict);
            if (temp.Count == 0)
                return new OneFLMeterDataView();
            //str = $"select * from deviceInfo where meterNo={no}";
            //var r = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, new Dictionary<string, string>());
            return temp.FirstOrDefault();
        }

        /// <summary>
        /// 开户，绑定用户与表时用，根据返回数判断是不是已经存在这个表了
        /// </summary>
        /// <param name="c"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int UpdateC(DeviceInfo c, SqlConnection conn, SqlTransaction tran)
        {
            var str = GetUpdateStr(c);
            var dict = GetParameters(c);
            return SqlHelper.Instance.ExcuteNonQuery(str, dict, conn, tran);
        }

        /// <summary>
        /// 开户，绑定设备和用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="customer"></param>
        /// <param name="device"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool BindInfo(AllInOne_UserInfo user, CustomerInfo customer, DeviceInfo device, SqlConnection conn, SqlTransaction tran, out int meterno)
        {
            customer.useState = 1;
            device.customerNo = customer.customerNo;
            device.openState = 1;

            if (CustomerInfoOper.Instance.UpdateC(customer, conn, tran) == 0)
            {
                CustomerInfoOper.Instance.Add(customer, conn, tran);
                //AllInOne_Customer_UserOper.Instance.AddCU(customer, user, conn, tran);
                AllFunc.Instance.AddOperateRecord($"添加用户{customer.customerNo}", user.id);//-txy
            }

            if (UpdateC(device, conn, tran) == 0)
            {
                var id = Add(device, conn, tran);
                //AllInOne_Device_AreaOper.Instance.AddDeviceWithArea(user, id, conn, tran);
                meterno = id;
                AllFunc.Instance.AddOperateRecord($"添加设备{id}", user.id);//-txy
            }
            else
                meterno = device.meterNo;

            return true;
        }

        /// <summary>
        /// 获取尚未开户的设备，和自己有关的,nbc的表不需要
        /// </summary>
        /// <param name="lastCId"></param>
        /// <returns></returns>
        public List<DeviceView> GetNotOpen(AllInOne_UserInfo user)
        {
            //var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);

            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            string str = "select * from DeviceView where defineNo1!='NBCivil' and customerno='' and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)  order by meterNo desc";
            return SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
            //return SqlHelper.Instance.GetByCondition<DeviceInfo>("deviceInfo"," openstate=0 ");
        }

        ///// <summary>
        /////获取展示在地图上的设备信息
        ///// </summary>
        ///// <param name="req"></param>
        ///// <param name="lastCId"></param>
        ///// <returns></returns>
        public List<OneFLMeterDataView> GetDeviceOnMap(DeviceMapReq req, string lastCId)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@lastcid", lastCId },
                { "@search", $"%{search}%" },
                { "@search2", search },
            };
            var condition = " 1=1 and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid ) ";

            if (!search.IsNullOrEmpty())
            {
                condition += " and (customerName like @search or address like @search or deviceNo=@search2 or communicateNo=@search2 ";
                int temp;
                if (int.TryParse(search, out temp))
                    condition += " or FLMeterNo=@search2 ";
                condition += " )";
            }

            if (req.sonId != "0")
            {
                var user = AllInOne_UserInfoOper.Instance.GetById(Convert.ToInt32(req.sonId));
                //var lastCId2 = AllInOne_UserInfoOper.Instance.GetLastCId(user);
                var ids = GetDeviceIds(user);
                if (ids.Count == 0)
                    return new List<OneFLMeterDataView>();
                var idsStr = StringHelper.Instance.ArrJoin(ids.ToArray());

                condition += $" and FLMeterNo in ({idsStr})";
            }
            string str = $@"select DISTINCT
	Id,
communicateNo,
FLMeterNo,
siteNo,
InstantTime,
ReceivTime,
StdSum,
WorkSum,
StdFlow,
WorkFlow,
Temperature,
Pressure,
FMState,
FMStateMsg,
RTUState,
RTUStateMsg,
SumTotal,
RemainMoney,
RemainVolume,
Overdraft,
RemoteChargeMoney,
RemoteChargeTimes,
Price,
ValveState,
ValveStateMsg,
PowerVoltage,
BatteryVoltage,
Reserve1,
Reserve2,
Reserve3,
Reserve4,
meterNo,
meterTypeNo,
lat,
lng,
deviceNo,
customerName,
address,
LoginState,
LoginStateMsg from OneFLMeterDataView where {condition} order by InstantTime ";
            return SqlHelper.Instance.ExecuteGetDt<OneFLMeterDataView>(str, dict);
        }

        /// <summary>
        /// 验证当前用户有没有权限搞这个设备
        /// </summary>
        /// <param name="meterNo"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ConfirmUser(int meterNo, AllInOne_UserInfo user)
        {
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            var condition = $" meterNo={meterNo} and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid )";
            string str = $"select * from deviceView where {condition}";
            return SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict).Count() > 0;
        }

        public int DeleteByMeterNo(int meterNo, AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            var condition = $" meterNo={meterNo} and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid )";
            string str2 = $"select * from deviceView where {condition}";
            var list = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str2, dict);
            if (list.Count == 0)
                return 0;
            else
            {
                if (list.First().openState == 1)
                    return 2;
                string str = "DELETE from DeviceInfo where meterNo=" + meterNo;
                var r = SqlHelper.Instance.ExcuteNonQuery(str, new Dictionary<string, string>());
                if (r > 0)
                {
                    str = "delete from allinone_device_area where deviceId=" + meterNo;
                    SqlHelper.Instance.ExcuteNonQuery(str, new Dictionary<string, string>());
                    return 1;
                }
                return 0;
            }
        }

        public List<string> GetHisTableNamesByUserLastName(string lastName)
        {
            var dict = new Dictionary<string, string>();
            string str = $"select * from DeviceView where userName=@lastName ";
            dict.Add("@lastName", lastName);
            var list = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
            var r = new List<string>();
            foreach (var item in list)
            {
                r.Add(GetHisTableName(item.meterNo));
            }

            return r;
        }

        public List<string> GetHisTableNamesByUserLastNameAndCustomerNo(string lastName, string customerNo)
        {
            var dict = new Dictionary<string, string>();

            var lastCid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(lastName);
            if (lastCid == null)
                return new List<string>();
            string str = $"select * from DeviceView where 1=1 and (cid1=@cid or cid2=@cid or cid3=@cid or cid4=@cid )  ";
            dict.Add("@lastName", lastName);
            if (customerNo != null && customerNo != "0")
            {
                str += " and customerNo =@customerNo ";
                dict.Add("@customerNo", customerNo);
            }
            dict.Add("@cid", lastCid);
            var list = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
            var r = new List<string>();
            foreach (var item in list)
            {
                r.Add(GetHisTableName(item.meterNo));
            }

            return r.Distinct().ToList();
        }

        public List<string> GetHisTableNames()
        {
            var dict = new Dictionary<string, string>();
            string str = $"select * from DeviceView where cid1='201804031310' ";
            //if (customerNo != null && customerNo != "0")
            //{
            //    str += " and customerNo =@customerNo ";
            //    dict.Add("@customerNo", customerNo);
            //}

            var list = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, dict);
            var r = new List<string>();
            foreach (var item in list)
            {
                r.Add(GetHisTableName(item.meterNo));
            }

            return r;
        }

        public string GetHisTableName(int meterNo)
        {
            return "FM" + StringHelper.Instance.GetIntStringWithZero(meterNo.ToString(), 10);
        }

        public int GetMeterNoByTableName(string name)
        {
            return Convert.ToInt32(name.Substring(2));
        }

        public int GetChargeTimesByCommNo(string commNo)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@commNo", commNo);

            string str = $@"SELECT
	                                TOP 1 r.chargeTimes
                                FROM
	                                (
		                                SELECT
			                                d.communicateNo,
			                                ic.chargeTimes
		                                FROM
			                                DeviceInfo d
		                                LEFT JOIN ICChargeRecord ic ON d.meterNo = ic.meterNo
	                                ) r
                                WHERE
	                                r.communicateNo = @commNo
                                ORDER BY
	                                r.chargeTimes DESC";

            var r = SqlHelper.Instance.ExecuteScalar(str, dict);
            if (r == null)
                return 0;
            return Convert.ToInt32(r);
        }

        public string GetDeviceViewSql(string admin)
        {
            string str = $@" 
SELECT
	DeviceInfo.meterNo,
	isnull(DeviceInfo.communicateNo,'') as communicateNo,
	isnull(DeviceInfo.barCode,'') as barCode,
	isnull(DeviceInfo.customerNo,'')as customerNo,
	DeviceInfo.meterTypeNo,
	DeviceInfo.factoryNo,
	DeviceInfo.openState,
	DeviceInfo.caliber,
DeviceInfo.ProtocolNo,
	isnull(DeviceInfo.baseVolume,'0') as baseVolume,
	DeviceInfo.fluidNo,
DeviceInfo.lat,
DeviceInfo.lng,
	isnull(DeviceInfo.remark,'') as remark,
	isnull(DeviceInfo.defineNo1,'') as defineNo1,
	isnull(DeviceInfo.defineNo2,'') as defineNo2,
	isnull(DeviceInfo.defineNo3,'') as defineNo3,
	DeviceInfo.buildTime,
	DeviceInfo.editTime,
	DeviceInfo.Operator,
isnull(DeviceInfo.deviceNo,'')as deviceNo,
	isnull(DeviceInfo.collectorNo,'') as collectorNo,
	DeviceInfo.MeterType,
	DeviceInfo.isConcentrate,
DeviceInfo.AlarmTimes,
DeviceInfo.AlarmInvTime,
DeviceInfo.CommMode,
DeviceInfo.LinkMode,
	FactoryType.factoryName,
	MeterType.meterTypeName,
	(
		CASE DeviceInfo.openState
		WHEN 0 THEN
			'未开户'
		WHEN 1 THEN
			'已开户'
		WHEN 2 THEN
			'已停用'
		END
	) AS openStateName,
	isnull((
		CASE DeviceInfo.factoryNo + DeviceInfo.meterTypeNo
		WHEN '0201' THEN
			FluidInfo0301.fluidName
		WHEN '0208' THEN
			FluidInfo0301.fluidName
		WHEN '0601' THEN
			FluidInfo0301.fluidName
		WHEN '0701' THEN
			FluidInfo0301.fluidName
		WHEN '0401' THEN
			FluidInfo0301.fluidName
		WHEN '0301' THEN
			FluidInfo0301.fluidName
		WHEN '0302' THEN
			FluidInfo0301.fluidName
		WHEN '0303' THEN
			FluidInfo0303.fluidName
		WHEN '0304' THEN
			FluidInfo0304.fluidName
		WHEN '0305' THEN
			FluidInfo0305.fluidName
		WHEN '0306' THEN
			FluidInfo0305.fluidName
		WHEN '0309' THEN
			FluidInfo0304.fluidName
		WHEN '0503' THEN
			FluidInfo0503.fluidName
		WHEN '0502' THEN
			FluidInfo0502.fluidName
		END
	),'') AS fluidName,
	isnull(CustomerInfo.customerName,'')as customerName,
	isnull(CustomerInfo.address,'') as address,
au.id AS userId,
	isnull(au.name,{admin}) AS userName,
	au. LEVEL,
	au.parentId,
	au.isStaff,
	isnull(au.cId1,'201804031310')as cId1,
	au.cId2,
	au.cId3,
	au.cId4,
ado.LoginState,
fm.FMState,
fm.FMStateMsg,
isnull(ado.LoginStateMsg,'离线') as LoginStateMsg,
CustomerInfo.customerType,
(
		CASE CustomerInfo.customerType
		WHEN 0 THEN
			'居民'
		WHEN 1 THEN
			'公建'
		WHEN 2 THEN
			'工业'
		WHEN 3 THEN
			'商福'
		END
	) AS CustTypeName
FROM
	DeviceInfo
LEFT OUTER JOIN FluidInfo0503 ON DeviceInfo.fluidNo = FluidInfo0503.fluidNo
LEFT OUTER JOIN FluidInfo0502 ON DeviceInfo.fluidNo = FluidInfo0502.fluidNo
LEFT OUTER JOIN CustomerInfo ON DeviceInfo.customerNo = CustomerInfo.customerNo
LEFT OUTER JOIN FluidInfo0303 ON DeviceInfo.fluidNo = FluidInfo0303.fluidNo
LEFT OUTER JOIN FluidInfo0301 ON DeviceInfo.fluidNo = FluidInfo0301.fluidNo
LEFT OUTER JOIN FluidInfo0305 ON DeviceInfo.fluidNo = FluidInfo0305.fluidNo
LEFT OUTER JOIN FluidInfo0304 ON DeviceInfo.fluidNo = FluidInfo0304.fluidNo
LEFT OUTER JOIN FactoryType ON DeviceInfo.factoryNo = FactoryType.factoryNo
LEFT OUTER JOIN MeterType ON DeviceInfo.meterTypeNo = MeterType.meterTypeNo
LEFT JOIN (
	SELECT
		*
	FROM
		AllInOne_Device_Area
) adaa ON DeviceInfo.meterNo = adaa.deviceId
LEFT JOIN (
	SELECT
		*
	FROM
		AllInOne_UserInfo
	WHERE
		isDeleted = 0
) au ON au.areaId = adaa.areaId
LEFT JOIN AllInOne_DeviceOnLine as ado on ado.CommNo=DeviceInfo.communicateNo
left join AllInOne_FLMeterData as fm on fm.FLMeterNo=DeviceInfo.meterNo ";

            return str;
        }

        public DeviceView GetByMeterNo(int meterno)
        {
            //var dict = new Dictionary<string, string>();
            //dict.Add("@meterno", meterno);
            var str = $"select * from deviceview where meterno={meterno}";
            var list = SqlHelper.Instance.ExecuteGetDt<DeviceView>(str, new Dictionary<string, string>());
            if (list.Count == 0)
                return null;
            return list.First();
        }

        public DeviceInfo GetByNo(int meterno)
        {
            var str = $"select * from DeviceInfo where meterno={meterno}";
            var list = SqlHelper.Instance.ExecuteGetDt<DeviceInfo>(str, new Dictionary<string, string>());
            if (list.Count == 0)
                return null;
            return list.First();
        }

        public bool IsDeviceOpen(int meterno)
        {
            var device = GetByNo(meterno);
            if (device.customerNo != null && device.customerNo != "")
                return true;
            return false;
        }

    }
}
