using System.Text;
using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_FLMeterDataOper : SingleTon<AllInOne_FLMeterDataOper>
    {
        /// <summary>
        /// 获取设备视图列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <param name="lastCId"></param>
        /// <returns></returns>
        public List<OneFLMeterDataView> GetList(MeterDataListReq req, int size, string lastCId)
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
                    return new List<OneFLMeterDataView>();
            }



            if (!search.IsNullOrEmpty())
                condition += " and (customerName like @search or address like @search or deviceNo=@search2 or communicateNo=@search2 ) ";

            order = order == "id" ? "" : "," + order;

            return SqlHelper.Instance.GetMutiView<OneFLMeterDataView>("Id", "Id" + order, condition, index, size, orderStr, dict);

            return SqlHelper.Instance.GetViewForMeterDataPaging<OneFLMeterDataView>("OneFLMeterDataView", @"select Id,
moneyOrVolume,
IsIC,
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
LoginStateMsg from OneFLMeterDataView ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// 获取客户总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(MeterDataListReq req, string lastCId)
        {
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
                condition += " and (customerName like @search or address like @search or deviceNo=@search2 or communicateNo=@search2 ) ";
            var list = SqlHelper.Instance.GetDistinctCount<OneFLMeterDataView>("OneFLMeterDataView", condition, dict);
            return list.Count;
        }

        /// <summary>
        /// 开关阀，估计不是我这边改变的。-txy
        /// </summary>
        /// <param name="req"></param>
        /// <param name="lastCId"></param>
        /// <returns></returns>
        public int ChangeValve(ValveReq req, string lastCId)
        {
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
            };
            string str = $"select * from oneFlMeterDataView where id={req.id} and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)";
            var list = SqlHelper.Instance.ExecuteGetDt<OneFLMeterDataView>(str, dict);
            if (list.Count == 0)
                return 2;
            var state = Convert.ToInt32(req.state);
            AllInOne_FLMeterData m = new AllInOne_FLMeterData();
            m.Id = Convert.ToInt32(req.id);
            m.ValveState = state;
            if (state == 1)
            {
                m.ValveStateMsg = "开阀";
            }
            else
            {
                m.ValveStateMsg = "关阀";
            }
            Update(m);
            if (state == 1)
                return 1;
            else
                return 0;

        }

        public int IsNewCommNo(string commNo)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@commno", commNo);
            var str = $@"select count(*) from AllInOne_FLMeterData where communicateNo=@commno";
            var r = SqlHelper.Instance.ExecuteScalar(str, dict);
            return Convert.ToInt32(r);
        }
    }
}
