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
    public partial class AllInOne_FLMeterDataOper : SingleTon<AllInOne_FLMeterDataOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_flmeterdata"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_FLMeterData allinone_flmeterdata)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_flmeterdata.Id != null)
                dict.Add("@Id", allinone_flmeterdata.Id.ToString());
            if (allinone_flmeterdata.communicateNo != null)
                dict.Add("@communicateNo", allinone_flmeterdata.communicateNo.ToString());
            if (allinone_flmeterdata.FLMeterNo != null)
                dict.Add("@FLMeterNo", allinone_flmeterdata.FLMeterNo.ToString());
            if (allinone_flmeterdata.siteNo != null)
                dict.Add("@siteNo", allinone_flmeterdata.siteNo.ToString());
            if (allinone_flmeterdata.InstantTime != null)
                dict.Add("@InstantTime", allinone_flmeterdata.InstantTime.ToString());
            if (allinone_flmeterdata.ReceivTime != null)
                dict.Add("@ReceivTime", allinone_flmeterdata.ReceivTime.ToString());
            if (allinone_flmeterdata.StdSum != null)
                dict.Add("@StdSum", allinone_flmeterdata.StdSum.ToString());
            if (allinone_flmeterdata.WorkSum != null)
                dict.Add("@WorkSum", allinone_flmeterdata.WorkSum.ToString());
            if (allinone_flmeterdata.StdFlow != null)
                dict.Add("@StdFlow", allinone_flmeterdata.StdFlow.ToString());
            if (allinone_flmeterdata.WorkFlow != null)
                dict.Add("@WorkFlow", allinone_flmeterdata.WorkFlow.ToString());
            if (allinone_flmeterdata.Temperature != null)
                dict.Add("@Temperature", allinone_flmeterdata.Temperature.ToString());
            if (allinone_flmeterdata.Pressure != null)
                dict.Add("@Pressure", allinone_flmeterdata.Pressure.ToString());
            if (allinone_flmeterdata.FMState != null)
                dict.Add("@FMState", allinone_flmeterdata.FMState.ToString());
            if (allinone_flmeterdata.FMStateMsg != null)
                dict.Add("@FMStateMsg", allinone_flmeterdata.FMStateMsg.ToString());
            if (allinone_flmeterdata.RTUState != null)
                dict.Add("@RTUState", allinone_flmeterdata.RTUState.ToString());
            if (allinone_flmeterdata.RTUStateMsg != null)
                dict.Add("@RTUStateMsg", allinone_flmeterdata.RTUStateMsg.ToString());
            if (allinone_flmeterdata.SumTotal != null)
                dict.Add("@SumTotal", allinone_flmeterdata.SumTotal.ToString());
            if (allinone_flmeterdata.RemainMoney != null)
                dict.Add("@RemainMoney", allinone_flmeterdata.RemainMoney.ToString());
            if (allinone_flmeterdata.RemainVolume != null)
                dict.Add("@RemainVolume", allinone_flmeterdata.RemainVolume.ToString());
            if (allinone_flmeterdata.Overdraft != null)
                dict.Add("@Overdraft", allinone_flmeterdata.Overdraft.ToString());
            if (allinone_flmeterdata.RemoteChargeMoney != null)
                dict.Add("@RemoteChargeMoney", allinone_flmeterdata.RemoteChargeMoney.ToString());
            if (allinone_flmeterdata.RemoteChargeTimes != null)
                dict.Add("@RemoteChargeTimes", allinone_flmeterdata.RemoteChargeTimes.ToString());
            if (allinone_flmeterdata.Price != null)
                dict.Add("@Price", allinone_flmeterdata.Price.ToString());
            if (allinone_flmeterdata.ValveState != null)
                dict.Add("@ValveState", allinone_flmeterdata.ValveState.ToString());
            if (allinone_flmeterdata.ValveStateMsg != null)
                dict.Add("@ValveStateMsg", allinone_flmeterdata.ValveStateMsg.ToString());
            if (allinone_flmeterdata.PowerVoltage != null)
                dict.Add("@PowerVoltage", allinone_flmeterdata.PowerVoltage.ToString());
            if (allinone_flmeterdata.BatteryVoltage != null)
                dict.Add("@BatteryVoltage", allinone_flmeterdata.BatteryVoltage.ToString());
            if (allinone_flmeterdata.Reserve1 != null)
                dict.Add("@Reserve1", allinone_flmeterdata.Reserve1.ToString());
            if (allinone_flmeterdata.Reserve2 != null)
                dict.Add("@Reserve2", allinone_flmeterdata.Reserve2.ToString());
            if (allinone_flmeterdata.Reserve3 != null)
                dict.Add("@Reserve3", allinone_flmeterdata.Reserve3.ToString());
            if (allinone_flmeterdata.Reserve4 != null)
                dict.Add("@Reserve4", allinone_flmeterdata.Reserve4.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_flmeterdata"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_FLMeterData allinone_flmeterdata)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_flmeterdata.communicateNo != null)
            {
                part1.Append("communicateNo,");
                part2.Append("@communicateNo,");
            }
            if (allinone_flmeterdata.FLMeterNo != null)
            {
                part1.Append("FLMeterNo,");
                part2.Append("@FLMeterNo,");
            }
            if (allinone_flmeterdata.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (allinone_flmeterdata.InstantTime != null)
            {
                part1.Append("InstantTime,");
                part2.Append("@InstantTime,");
            }
            if (allinone_flmeterdata.ReceivTime != null)
            {
                part1.Append("ReceivTime,");
                part2.Append("@ReceivTime,");
            }
            if (allinone_flmeterdata.StdSum != null)
            {
                part1.Append("StdSum,");
                part2.Append("@StdSum,");
            }
            if (allinone_flmeterdata.WorkSum != null)
            {
                part1.Append("WorkSum,");
                part2.Append("@WorkSum,");
            }
            if (allinone_flmeterdata.StdFlow != null)
            {
                part1.Append("StdFlow,");
                part2.Append("@StdFlow,");
            }
            if (allinone_flmeterdata.WorkFlow != null)
            {
                part1.Append("WorkFlow,");
                part2.Append("@WorkFlow,");
            }
            if (allinone_flmeterdata.Temperature != null)
            {
                part1.Append("Temperature,");
                part2.Append("@Temperature,");
            }
            if (allinone_flmeterdata.Pressure != null)
            {
                part1.Append("Pressure,");
                part2.Append("@Pressure,");
            }
            if (allinone_flmeterdata.FMState != null)
            {
                part1.Append("FMState,");
                part2.Append("@FMState,");
            }
            if (allinone_flmeterdata.FMStateMsg != null)
            {
                part1.Append("FMStateMsg,");
                part2.Append("@FMStateMsg,");
            }
            if (allinone_flmeterdata.RTUState != null)
            {
                part1.Append("RTUState,");
                part2.Append("@RTUState,");
            }
            if (allinone_flmeterdata.RTUStateMsg != null)
            {
                part1.Append("RTUStateMsg,");
                part2.Append("@RTUStateMsg,");
            }
            if (allinone_flmeterdata.SumTotal != null)
            {
                part1.Append("SumTotal,");
                part2.Append("@SumTotal,");
            }
            if (allinone_flmeterdata.RemainMoney != null)
            {
                part1.Append("RemainMoney,");
                part2.Append("@RemainMoney,");
            }
            if (allinone_flmeterdata.RemainVolume != null)
            {
                part1.Append("RemainVolume,");
                part2.Append("@RemainVolume,");
            }
            if (allinone_flmeterdata.Overdraft != null)
            {
                part1.Append("Overdraft,");
                part2.Append("@Overdraft,");
            }
            if (allinone_flmeterdata.RemoteChargeMoney != null)
            {
                part1.Append("RemoteChargeMoney,");
                part2.Append("@RemoteChargeMoney,");
            }
            if (allinone_flmeterdata.RemoteChargeTimes != null)
            {
                part1.Append("RemoteChargeTimes,");
                part2.Append("@RemoteChargeTimes,");
            }
            if (allinone_flmeterdata.Price != null)
            {
                part1.Append("Price,");
                part2.Append("@Price,");
            }
            if (allinone_flmeterdata.ValveState != null)
            {
                part1.Append("ValveState,");
                part2.Append("@ValveState,");
            }
            if (allinone_flmeterdata.ValveStateMsg != null)
            {
                part1.Append("ValveStateMsg,");
                part2.Append("@ValveStateMsg,");
            }
            if (allinone_flmeterdata.PowerVoltage != null)
            {
                part1.Append("PowerVoltage,");
                part2.Append("@PowerVoltage,");
            }
            if (allinone_flmeterdata.BatteryVoltage != null)
            {
                part1.Append("BatteryVoltage,");
                part2.Append("@BatteryVoltage,");
            }
            if (allinone_flmeterdata.Reserve1 != null)
            {
                part1.Append("Reserve1,");
                part2.Append("@Reserve1,");
            }
            if (allinone_flmeterdata.Reserve2 != null)
            {
                part1.Append("Reserve2,");
                part2.Append("@Reserve2,");
            }
            if (allinone_flmeterdata.Reserve3 != null)
            {
                part1.Append("Reserve3,");
                part2.Append("@Reserve3,");
            }
            if (allinone_flmeterdata.Reserve4 != null)
            {
                part1.Append("Reserve4,");
                part2.Append("@Reserve4,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_flmeterdata(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_flmeterdata"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_FLMeterData allinone_flmeterdata)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_flmeterdata set ");
            if (allinone_flmeterdata.communicateNo != null)
                part1.Append("communicateNo = @communicateNo,");
            if (allinone_flmeterdata.FLMeterNo != null)
                part1.Append("FLMeterNo = @FLMeterNo,");
            if (allinone_flmeterdata.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (allinone_flmeterdata.InstantTime != null)
                part1.Append("InstantTime = @InstantTime,");
            if (allinone_flmeterdata.ReceivTime != null)
                part1.Append("ReceivTime = @ReceivTime,");
            if (allinone_flmeterdata.StdSum != null)
                part1.Append("StdSum = @StdSum,");
            if (allinone_flmeterdata.WorkSum != null)
                part1.Append("WorkSum = @WorkSum,");
            if (allinone_flmeterdata.StdFlow != null)
                part1.Append("StdFlow = @StdFlow,");
            if (allinone_flmeterdata.WorkFlow != null)
                part1.Append("WorkFlow = @WorkFlow,");
            if (allinone_flmeterdata.Temperature != null)
                part1.Append("Temperature = @Temperature,");
            if (allinone_flmeterdata.Pressure != null)
                part1.Append("Pressure = @Pressure,");
            if (allinone_flmeterdata.FMState != null)
                part1.Append("FMState = @FMState,");
            if (allinone_flmeterdata.FMStateMsg != null)
                part1.Append("FMStateMsg = @FMStateMsg,");
            if (allinone_flmeterdata.RTUState != null)
                part1.Append("RTUState = @RTUState,");
            if (allinone_flmeterdata.RTUStateMsg != null)
                part1.Append("RTUStateMsg = @RTUStateMsg,");
            if (allinone_flmeterdata.SumTotal != null)
                part1.Append("SumTotal = @SumTotal,");
            if (allinone_flmeterdata.RemainMoney != null)
                part1.Append("RemainMoney = @RemainMoney,");
            if (allinone_flmeterdata.RemainVolume != null)
                part1.Append("RemainVolume = @RemainVolume,");
            if (allinone_flmeterdata.Overdraft != null)
                part1.Append("Overdraft = @Overdraft,");
            if (allinone_flmeterdata.RemoteChargeMoney != null)
                part1.Append("RemoteChargeMoney = @RemoteChargeMoney,");
            if (allinone_flmeterdata.RemoteChargeTimes != null)
                part1.Append("RemoteChargeTimes = @RemoteChargeTimes,");
            if (allinone_flmeterdata.Price != null)
                part1.Append("Price = @Price,");
            if (allinone_flmeterdata.ValveState != null)
                part1.Append("ValveState = @ValveState,");
            if (allinone_flmeterdata.ValveStateMsg != null)
                part1.Append("ValveStateMsg = @ValveStateMsg,");
            if (allinone_flmeterdata.PowerVoltage != null)
                part1.Append("PowerVoltage = @PowerVoltage,");
            if (allinone_flmeterdata.BatteryVoltage != null)
                part1.Append("BatteryVoltage = @BatteryVoltage,");
            if (allinone_flmeterdata.Reserve1 != null)
                part1.Append("Reserve1 = @Reserve1,");
            if (allinone_flmeterdata.Reserve2 != null)
                part1.Append("Reserve2 = @Reserve2,");
            if (allinone_flmeterdata.Reserve3 != null)
                part1.Append("Reserve3 = @Reserve3,");
            if (allinone_flmeterdata.Reserve4 != null)
                part1.Append("Reserve4 = @Reserve4,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_FLMeterData"></param>
        /// <returns></returns>
        public int Add(AllInOne_FLMeterData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_FLMeterData"></param>
        /// <returns></returns>
        public void Update(AllInOne_FLMeterData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
