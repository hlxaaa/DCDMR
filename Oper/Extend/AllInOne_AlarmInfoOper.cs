using System.Text;
using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_AlarmInfoOper : SingleTon<AllInOne_AlarmInfoOper>
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<DeviceAlarmView> GetList(AlarmListReq req, int size, string lastCId)
        {
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
                    return new List<DeviceAlarmView>();
            }


            //if (req.meterNo != null && req.meterNo != "0")
            //    condition += $" and devid = {req.meterNo} ";
            if (req.customerNo != null && req.customerNo != "0")
            {
                dict.Add("@customerNo", req.customerNo);
                condition += $" and customerNo =@customerNo ";
            }

            if (!search.IsNullOrEmpty())
                condition += " and (communicateNo like @search or alarmContent like @search or DealOperator like @search or LinkMan like @search) ";
            return SqlHelper.Instance.GetViewForAlarmPaging<DeviceAlarmView>("DeviceAlarmView", @"select Id,
siteNo,
communicateNo,
Devid,
DevType,
DevTypeName,
AlarmContent,
AlarmTime,
DealFlag,
DealTime,
DealOperator,
SmsTimes,
SmsSendTimes,
SmsInvTime,
Linkman,
MobileNo,
meterNo,
barCode,
customerNo,
deviceNo,
meterTypeNo,
factoryNo,
openState,
caliber,
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
collectorNo,
MeterType,
isConcentrate,
factoryName,
meterTypeName,
openStateName,
fluidName,
customerName,
address from DeviceAlarmView ", condition, index, size, orderStr, dict);
        }

        public List<Counts> GetSafeAndAlarmCount(AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            string str = $@"select count(*) as counts from DeviceAlarmView where DealFlag=1 and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)
UNION
select count(*) as counts from DeviceAlarmView where DealFlag=0 and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)";
            return SqlHelper.Instance.ExecuteGetDt<Counts>(str, dict);
        }

        public List<Counts> GetSafeAndAlarmCountMonth(AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };

            var now = DateTime.Now;
            var year = now.Year;
            var month = now.Month;

            var dt = new DateTime(year, month, 1, 0, 0, 0);


            string str = $@"select count(*) as counts from DeviceAlarmView where DealFlag=1 and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) and dealTime>'{dt}'
UNION
select count(*) as counts from DeviceAlarmView where DealFlag=0 and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)";
            return SqlHelper.Instance.ExecuteGetDt<Counts>(str, dict);
        }


        public List<DeviceAlarmView> GetNewAlarm(AllInOne_UserInfo user, int oldLastId)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            //string str = $@"select count(*) as counts from DeviceAlarmView where DealFlag=1 and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)
            //UNION
            //select count(*) as counts from DeviceAlarmView where DealFlag=0 and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)";

            var str = $@"select * from DeviceAlarmView where ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid ) and id>{oldLastId}";

            //var r = SqlHelper.Instance.ExecuteScalar(str, dict);
            //if (r == null)
            //    return 0;
            //return Convert.ToInt32(r);

            return SqlHelper.Instance.ExecuteGetDt<DeviceAlarmView>(str, dict);
        }


        public List<AllInOne_AlarmInfo> GetNewAlarmMeterNos(AllInOne_UserInfo user, int count)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            string str = $@"select top {count} devid,alarmContent from DeviceAlarmView where 1=1 and ( cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) order by id desc";
            return SqlHelper.Instance.ExecuteGetDt<AllInOne_AlarmInfo>(str, dict);
        }

        /// <summary>
        /// 获取区域列表总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(AlarmListReq req, string lastCId)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
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

            //if (req.meterNo != null && req.meterNo != "0")
            //    condition += $" and devid = {req.meterNo} ";

            if (req.customerNo != null && req.customerNo != "0")
            {
                dict.Add("@customerNo", req.customerNo);
                condition += $" and customerNo =@customerNo ";
            }

            if (!search.IsNullOrEmpty())
                condition += " and (communicateNo like @search or alarmContent like @search or DealOperator like @search or LinkMan like @search) ";
            var list = SqlHelper.Instance.GetDistinctCount<DeviceAlarmView>("DeviceAlarmView", condition, dict);
            return list.Count;
        }

        /// <summary>
        /// 根据报警信息id获取报警信息记录
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        public AllInOne_AlarmInfo GetById(int alarmId)
        {
            return SqlHelper.Instance.GetById<AllInOne_AlarmInfo>("AllInOne_AlarmInfo", alarmId);
        }

        /// <summary>
        /// 根据设备号获取报警信息列表
        /// </summary>
        /// <param name="meterNo"></param>
        /// <returns></returns>
        public List<AllInOne_AlarmInfo> GetByMeterNo(int meterNo, AllInOne_UserInfo user)
        {
            var size = 99;
            if (DeviceInfoOper.Instance.ConfirmUser(meterNo, user))
                return SqlHelper.Instance.GetViewPaging<AllInOne_AlarmInfo>("AllInOne_AlarmInfo", " select * from AllInOne_AlarmInfo ", $" devid={meterNo}", 1, size, " order by id desc", new Dictionary<string, string>());
            else
                return new List<AllInOne_AlarmInfo>();
        }

    }
}
