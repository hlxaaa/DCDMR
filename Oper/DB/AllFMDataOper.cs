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
    public partial class AllFMDataOper : SingleTon<AllFMDataOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allfmdata"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllFMData allfmdata)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allfmdata.rId != null)
                dict.Add("@rId", allfmdata.rId.ToString());
            if (allfmdata.communicateNo != null)
                dict.Add("@communicateNo", allfmdata.communicateNo.ToString());
            if (allfmdata.FLMeterNo != null)
                dict.Add("@FLMeterNo", allfmdata.FLMeterNo.ToString());
            if (allfmdata.siteNo != null)
                dict.Add("@siteNo", allfmdata.siteNo.ToString());
            if (allfmdata.InstantTime != null)
                dict.Add("@InstantTime", allfmdata.InstantTime.ToString());
            if (allfmdata.ReceivTime != null)
                dict.Add("@ReceivTime", allfmdata.ReceivTime.ToString());
            if (allfmdata.StdSum != null)
                dict.Add("@StdSum", allfmdata.StdSum.ToString());
            if (allfmdata.WorkSum != null)
                dict.Add("@WorkSum", allfmdata.WorkSum.ToString());
            if (allfmdata.StdFlow != null)
                dict.Add("@StdFlow", allfmdata.StdFlow.ToString());
            if (allfmdata.WorkFlow != null)
                dict.Add("@WorkFlow", allfmdata.WorkFlow.ToString());
            if (allfmdata.Temperature != null)
                dict.Add("@Temperature", allfmdata.Temperature.ToString());
            if (allfmdata.Pressure != null)
                dict.Add("@Pressure", allfmdata.Pressure.ToString());
            if (allfmdata.FMState != null)
                dict.Add("@FMState", allfmdata.FMState.ToString());
            if (allfmdata.FMStateMsg != null)
                dict.Add("@FMStateMsg", allfmdata.FMStateMsg.ToString());
            if (allfmdata.RTUState != null)
                dict.Add("@RTUState", allfmdata.RTUState.ToString());
            if (allfmdata.RTUStateMsg != null)
                dict.Add("@RTUStateMsg", allfmdata.RTUStateMsg.ToString());
            if (allfmdata.SumTotal != null)
                dict.Add("@SumTotal", allfmdata.SumTotal.ToString());
            if (allfmdata.RemainMoney != null)
                dict.Add("@RemainMoney", allfmdata.RemainMoney.ToString());
            if (allfmdata.RemainVolume != null)
                dict.Add("@RemainVolume", allfmdata.RemainVolume.ToString());
            if (allfmdata.Overdraft != null)
                dict.Add("@Overdraft", allfmdata.Overdraft.ToString());
            if (allfmdata.RemoteChargeMoney != null)
                dict.Add("@RemoteChargeMoney", allfmdata.RemoteChargeMoney.ToString());
            if (allfmdata.RemoteChargeTimes != null)
                dict.Add("@RemoteChargeTimes", allfmdata.RemoteChargeTimes.ToString());
            if (allfmdata.Price != null)
                dict.Add("@Price", allfmdata.Price.ToString());
            if (allfmdata.ValveState != null)
                dict.Add("@ValveState", allfmdata.ValveState.ToString());
            if (allfmdata.ValveStateMsg != null)
                dict.Add("@ValveStateMsg", allfmdata.ValveStateMsg.ToString());
            if (allfmdata.PowerVoltage != null)
                dict.Add("@PowerVoltage", allfmdata.PowerVoltage.ToString());
            if (allfmdata.BatteryVoltage != null)
                dict.Add("@BatteryVoltage", allfmdata.BatteryVoltage.ToString());
            if (allfmdata.Reserve1 != null)
                dict.Add("@Reserve1", allfmdata.Reserve1.ToString());
            if (allfmdata.Reserve2 != null)
                dict.Add("@Reserve2", allfmdata.Reserve2.ToString());
            if (allfmdata.Reserve3 != null)
                dict.Add("@Reserve3", allfmdata.Reserve3.ToString());
            if (allfmdata.Reserve4 != null)
                dict.Add("@Reserve4", allfmdata.Reserve4.ToString());
            if (allfmdata.Id != null)
                dict.Add("@Id", allfmdata.Id.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allfmdata"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllFMData allfmdata)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allfmdata.communicateNo != null)
            {
                part1.Append("communicateNo,");
                part2.Append("@communicateNo,");
            }
            if (allfmdata.FLMeterNo != null)
            {
                part1.Append("FLMeterNo,");
                part2.Append("@FLMeterNo,");
            }
            if (allfmdata.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (allfmdata.InstantTime != null)
            {
                part1.Append("InstantTime,");
                part2.Append("@InstantTime,");
            }
            if (allfmdata.ReceivTime != null)
            {
                part1.Append("ReceivTime,");
                part2.Append("@ReceivTime,");
            }
            if (allfmdata.StdSum != null)
            {
                part1.Append("StdSum,");
                part2.Append("@StdSum,");
            }
            if (allfmdata.WorkSum != null)
            {
                part1.Append("WorkSum,");
                part2.Append("@WorkSum,");
            }
            if (allfmdata.StdFlow != null)
            {
                part1.Append("StdFlow,");
                part2.Append("@StdFlow,");
            }
            if (allfmdata.WorkFlow != null)
            {
                part1.Append("WorkFlow,");
                part2.Append("@WorkFlow,");
            }
            if (allfmdata.Temperature != null)
            {
                part1.Append("Temperature,");
                part2.Append("@Temperature,");
            }
            if (allfmdata.Pressure != null)
            {
                part1.Append("Pressure,");
                part2.Append("@Pressure,");
            }
            if (allfmdata.FMState != null)
            {
                part1.Append("FMState,");
                part2.Append("@FMState,");
            }
            if (allfmdata.FMStateMsg != null)
            {
                part1.Append("FMStateMsg,");
                part2.Append("@FMStateMsg,");
            }
            if (allfmdata.RTUState != null)
            {
                part1.Append("RTUState,");
                part2.Append("@RTUState,");
            }
            if (allfmdata.RTUStateMsg != null)
            {
                part1.Append("RTUStateMsg,");
                part2.Append("@RTUStateMsg,");
            }
            if (allfmdata.SumTotal != null)
            {
                part1.Append("SumTotal,");
                part2.Append("@SumTotal,");
            }
            if (allfmdata.RemainMoney != null)
            {
                part1.Append("RemainMoney,");
                part2.Append("@RemainMoney,");
            }
            if (allfmdata.RemainVolume != null)
            {
                part1.Append("RemainVolume,");
                part2.Append("@RemainVolume,");
            }
            if (allfmdata.Overdraft != null)
            {
                part1.Append("Overdraft,");
                part2.Append("@Overdraft,");
            }
            if (allfmdata.RemoteChargeMoney != null)
            {
                part1.Append("RemoteChargeMoney,");
                part2.Append("@RemoteChargeMoney,");
            }
            if (allfmdata.RemoteChargeTimes != null)
            {
                part1.Append("RemoteChargeTimes,");
                part2.Append("@RemoteChargeTimes,");
            }
            if (allfmdata.Price != null)
            {
                part1.Append("Price,");
                part2.Append("@Price,");
            }
            if (allfmdata.ValveState != null)
            {
                part1.Append("ValveState,");
                part2.Append("@ValveState,");
            }
            if (allfmdata.ValveStateMsg != null)
            {
                part1.Append("ValveStateMsg,");
                part2.Append("@ValveStateMsg,");
            }
            if (allfmdata.PowerVoltage != null)
            {
                part1.Append("PowerVoltage,");
                part2.Append("@PowerVoltage,");
            }
            if (allfmdata.BatteryVoltage != null)
            {
                part1.Append("BatteryVoltage,");
                part2.Append("@BatteryVoltage,");
            }
            if (allfmdata.Reserve1 != null)
            {
                part1.Append("Reserve1,");
                part2.Append("@Reserve1,");
            }
            if (allfmdata.Reserve2 != null)
            {
                part1.Append("Reserve2,");
                part2.Append("@Reserve2,");
            }
            if (allfmdata.Reserve3 != null)
            {
                part1.Append("Reserve3,");
                part2.Append("@Reserve3,");
            }
            if (allfmdata.Reserve4 != null)
            {
                part1.Append("Reserve4,");
                part2.Append("@Reserve4,");
            }
            if (allfmdata.Id != null)
            {
                part1.Append("Id,");
                part2.Append("@Id,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allfmdata(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allfmdata"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllFMData allfmdata)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allfmdata set ");
            if (allfmdata.communicateNo != null)
                part1.Append("communicateNo = @communicateNo,");
            if (allfmdata.FLMeterNo != null)
                part1.Append("FLMeterNo = @FLMeterNo,");
            if (allfmdata.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (allfmdata.InstantTime != null)
                part1.Append("InstantTime = @InstantTime,");
            if (allfmdata.ReceivTime != null)
                part1.Append("ReceivTime = @ReceivTime,");
            if (allfmdata.StdSum != null)
                part1.Append("StdSum = @StdSum,");
            if (allfmdata.WorkSum != null)
                part1.Append("WorkSum = @WorkSum,");
            if (allfmdata.StdFlow != null)
                part1.Append("StdFlow = @StdFlow,");
            if (allfmdata.WorkFlow != null)
                part1.Append("WorkFlow = @WorkFlow,");
            if (allfmdata.Temperature != null)
                part1.Append("Temperature = @Temperature,");
            if (allfmdata.Pressure != null)
                part1.Append("Pressure = @Pressure,");
            if (allfmdata.FMState != null)
                part1.Append("FMState = @FMState,");
            if (allfmdata.FMStateMsg != null)
                part1.Append("FMStateMsg = @FMStateMsg,");
            if (allfmdata.RTUState != null)
                part1.Append("RTUState = @RTUState,");
            if (allfmdata.RTUStateMsg != null)
                part1.Append("RTUStateMsg = @RTUStateMsg,");
            if (allfmdata.SumTotal != null)
                part1.Append("SumTotal = @SumTotal,");
            if (allfmdata.RemainMoney != null)
                part1.Append("RemainMoney = @RemainMoney,");
            if (allfmdata.RemainVolume != null)
                part1.Append("RemainVolume = @RemainVolume,");
            if (allfmdata.Overdraft != null)
                part1.Append("Overdraft = @Overdraft,");
            if (allfmdata.RemoteChargeMoney != null)
                part1.Append("RemoteChargeMoney = @RemoteChargeMoney,");
            if (allfmdata.RemoteChargeTimes != null)
                part1.Append("RemoteChargeTimes = @RemoteChargeTimes,");
            if (allfmdata.Price != null)
                part1.Append("Price = @Price,");
            if (allfmdata.ValveState != null)
                part1.Append("ValveState = @ValveState,");
            if (allfmdata.ValveStateMsg != null)
                part1.Append("ValveStateMsg = @ValveStateMsg,");
            if (allfmdata.PowerVoltage != null)
                part1.Append("PowerVoltage = @PowerVoltage,");
            if (allfmdata.BatteryVoltage != null)
                part1.Append("BatteryVoltage = @BatteryVoltage,");
            if (allfmdata.Reserve1 != null)
                part1.Append("Reserve1 = @Reserve1,");
            if (allfmdata.Reserve2 != null)
                part1.Append("Reserve2 = @Reserve2,");
            if (allfmdata.Reserve3 != null)
                part1.Append("Reserve3 = @Reserve3,");
            if (allfmdata.Reserve4 != null)
                part1.Append("Reserve4 = @Reserve4,");
            if (allfmdata.Id != null)
                part1.Append("Id = @Id,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where rId= @rId  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllFMData"></param>
        /// <returns></returns>
        public int Add(AllFMData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllFMData"></param>
        /// <returns></returns>
        public void Update(AllFMData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
