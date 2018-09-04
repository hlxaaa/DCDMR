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
    public partial class IOT_FLMeterDataOper : SingleTon<IOT_FLMeterDataOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="iot_flmeterdata"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(IOT_FLMeterData iot_flmeterdata)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (iot_flmeterdata.Id != null)
                dict.Add("@Id", iot_flmeterdata.Id.ToString());
            if (iot_flmeterdata.communicateNo != null)
                dict.Add("@communicateNo", iot_flmeterdata.communicateNo.ToString());
            if (iot_flmeterdata.FLMeterNo != null)
                dict.Add("@FLMeterNo", iot_flmeterdata.FLMeterNo.ToString());
            if (iot_flmeterdata.siteNo != null)
                dict.Add("@siteNo", iot_flmeterdata.siteNo.ToString());
            if (iot_flmeterdata.InstantTime != null)
                dict.Add("@InstantTime", iot_flmeterdata.InstantTime.ToString());
            if (iot_flmeterdata.ReceivTime != null)
                dict.Add("@ReceivTime", iot_flmeterdata.ReceivTime.ToString());
            if (iot_flmeterdata.StdSum != null)
                dict.Add("@StdSum", iot_flmeterdata.StdSum.ToString());
            if (iot_flmeterdata.WorkSum != null)
                dict.Add("@WorkSum", iot_flmeterdata.WorkSum.ToString());
            if (iot_flmeterdata.StdFlow != null)
                dict.Add("@StdFlow", iot_flmeterdata.StdFlow.ToString());
            if (iot_flmeterdata.WorkFlow != null)
                dict.Add("@WorkFlow", iot_flmeterdata.WorkFlow.ToString());
            if (iot_flmeterdata.Temperature != null)
                dict.Add("@Temperature", iot_flmeterdata.Temperature.ToString());
            if (iot_flmeterdata.Pressure != null)
                dict.Add("@Pressure", iot_flmeterdata.Pressure.ToString());
            if (iot_flmeterdata.FMState != null)
                dict.Add("@FMState", iot_flmeterdata.FMState.ToString());
            if (iot_flmeterdata.FMStateMsg != null)
                dict.Add("@FMStateMsg", iot_flmeterdata.FMStateMsg.ToString());
            if (iot_flmeterdata.RTUState != null)
                dict.Add("@RTUState", iot_flmeterdata.RTUState.ToString());
            if (iot_flmeterdata.RTUStateMsg != null)
                dict.Add("@RTUStateMsg", iot_flmeterdata.RTUStateMsg.ToString());
            if (iot_flmeterdata.SumTotal != null)
                dict.Add("@SumTotal", iot_flmeterdata.SumTotal.ToString());
            if (iot_flmeterdata.RemainMoney != null)
                dict.Add("@RemainMoney", iot_flmeterdata.RemainMoney.ToString());
            if (iot_flmeterdata.RemainVolume != null)
                dict.Add("@RemainVolume", iot_flmeterdata.RemainVolume.ToString());
            if (iot_flmeterdata.Overdraft != null)
                dict.Add("@Overdraft", iot_flmeterdata.Overdraft.ToString());
            if (iot_flmeterdata.RemoteChargeMoney != null)
                dict.Add("@RemoteChargeMoney", iot_flmeterdata.RemoteChargeMoney.ToString());
            if (iot_flmeterdata.RemoteChargeTimes != null)
                dict.Add("@RemoteChargeTimes", iot_flmeterdata.RemoteChargeTimes.ToString());
            if (iot_flmeterdata.Price != null)
                dict.Add("@Price", iot_flmeterdata.Price.ToString());
            if (iot_flmeterdata.ValveState != null)
                dict.Add("@ValveState", iot_flmeterdata.ValveState.ToString());
            if (iot_flmeterdata.ValveStateMsg != null)
                dict.Add("@ValveStateMsg", iot_flmeterdata.ValveStateMsg.ToString());
            if (iot_flmeterdata.PowerVoltage != null)
                dict.Add("@PowerVoltage", iot_flmeterdata.PowerVoltage.ToString());
            if (iot_flmeterdata.BatteryVoltage != null)
                dict.Add("@BatteryVoltage", iot_flmeterdata.BatteryVoltage.ToString());
            if (iot_flmeterdata.Reserve1 != null)
                dict.Add("@Reserve1", iot_flmeterdata.Reserve1.ToString());
            if (iot_flmeterdata.Reserve2 != null)
                dict.Add("@Reserve2", iot_flmeterdata.Reserve2.ToString());
            if (iot_flmeterdata.Reserve3 != null)
                dict.Add("@Reserve3", iot_flmeterdata.Reserve3.ToString());
            if (iot_flmeterdata.Reserve4 != null)
                dict.Add("@Reserve4", iot_flmeterdata.Reserve4.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="iot_flmeterdata"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(IOT_FLMeterData iot_flmeterdata)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (iot_flmeterdata.communicateNo != null)
            {
                part1.Append("communicateNo,");
                part2.Append("@communicateNo,");
            }
            if (iot_flmeterdata.FLMeterNo != null)
            {
                part1.Append("FLMeterNo,");
                part2.Append("@FLMeterNo,");
            }
            if (iot_flmeterdata.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (iot_flmeterdata.InstantTime != null)
            {
                part1.Append("InstantTime,");
                part2.Append("@InstantTime,");
            }
            if (iot_flmeterdata.ReceivTime != null)
            {
                part1.Append("ReceivTime,");
                part2.Append("@ReceivTime,");
            }
            if (iot_flmeterdata.StdSum != null)
            {
                part1.Append("StdSum,");
                part2.Append("@StdSum,");
            }
            if (iot_flmeterdata.WorkSum != null)
            {
                part1.Append("WorkSum,");
                part2.Append("@WorkSum,");
            }
            if (iot_flmeterdata.StdFlow != null)
            {
                part1.Append("StdFlow,");
                part2.Append("@StdFlow,");
            }
            if (iot_flmeterdata.WorkFlow != null)
            {
                part1.Append("WorkFlow,");
                part2.Append("@WorkFlow,");
            }
            if (iot_flmeterdata.Temperature != null)
            {
                part1.Append("Temperature,");
                part2.Append("@Temperature,");
            }
            if (iot_flmeterdata.Pressure != null)
            {
                part1.Append("Pressure,");
                part2.Append("@Pressure,");
            }
            if (iot_flmeterdata.FMState != null)
            {
                part1.Append("FMState,");
                part2.Append("@FMState,");
            }
            if (iot_flmeterdata.FMStateMsg != null)
            {
                part1.Append("FMStateMsg,");
                part2.Append("@FMStateMsg,");
            }
            if (iot_flmeterdata.RTUState != null)
            {
                part1.Append("RTUState,");
                part2.Append("@RTUState,");
            }
            if (iot_flmeterdata.RTUStateMsg != null)
            {
                part1.Append("RTUStateMsg,");
                part2.Append("@RTUStateMsg,");
            }
            if (iot_flmeterdata.SumTotal != null)
            {
                part1.Append("SumTotal,");
                part2.Append("@SumTotal,");
            }
            if (iot_flmeterdata.RemainMoney != null)
            {
                part1.Append("RemainMoney,");
                part2.Append("@RemainMoney,");
            }
            if (iot_flmeterdata.RemainVolume != null)
            {
                part1.Append("RemainVolume,");
                part2.Append("@RemainVolume,");
            }
            if (iot_flmeterdata.Overdraft != null)
            {
                part1.Append("Overdraft,");
                part2.Append("@Overdraft,");
            }
            if (iot_flmeterdata.RemoteChargeMoney != null)
            {
                part1.Append("RemoteChargeMoney,");
                part2.Append("@RemoteChargeMoney,");
            }
            if (iot_flmeterdata.RemoteChargeTimes != null)
            {
                part1.Append("RemoteChargeTimes,");
                part2.Append("@RemoteChargeTimes,");
            }
            if (iot_flmeterdata.Price != null)
            {
                part1.Append("Price,");
                part2.Append("@Price,");
            }
            if (iot_flmeterdata.ValveState != null)
            {
                part1.Append("ValveState,");
                part2.Append("@ValveState,");
            }
            if (iot_flmeterdata.ValveStateMsg != null)
            {
                part1.Append("ValveStateMsg,");
                part2.Append("@ValveStateMsg,");
            }
            if (iot_flmeterdata.PowerVoltage != null)
            {
                part1.Append("PowerVoltage,");
                part2.Append("@PowerVoltage,");
            }
            if (iot_flmeterdata.BatteryVoltage != null)
            {
                part1.Append("BatteryVoltage,");
                part2.Append("@BatteryVoltage,");
            }
            if (iot_flmeterdata.Reserve1 != null)
            {
                part1.Append("Reserve1,");
                part2.Append("@Reserve1,");
            }
            if (iot_flmeterdata.Reserve2 != null)
            {
                part1.Append("Reserve2,");
                part2.Append("@Reserve2,");
            }
            if (iot_flmeterdata.Reserve3 != null)
            {
                part1.Append("Reserve3,");
                part2.Append("@Reserve3,");
            }
            if (iot_flmeterdata.Reserve4 != null)
            {
                part1.Append("Reserve4,");
                part2.Append("@Reserve4,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into iot_flmeterdata(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="iot_flmeterdata"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(IOT_FLMeterData iot_flmeterdata)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update iot_flmeterdata set ");
            if (iot_flmeterdata.communicateNo != null)
                part1.Append("communicateNo = @communicateNo,");
            if (iot_flmeterdata.FLMeterNo != null)
                part1.Append("FLMeterNo = @FLMeterNo,");
            if (iot_flmeterdata.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (iot_flmeterdata.InstantTime != null)
                part1.Append("InstantTime = @InstantTime,");
            if (iot_flmeterdata.ReceivTime != null)
                part1.Append("ReceivTime = @ReceivTime,");
            if (iot_flmeterdata.StdSum != null)
                part1.Append("StdSum = @StdSum,");
            if (iot_flmeterdata.WorkSum != null)
                part1.Append("WorkSum = @WorkSum,");
            if (iot_flmeterdata.StdFlow != null)
                part1.Append("StdFlow = @StdFlow,");
            if (iot_flmeterdata.WorkFlow != null)
                part1.Append("WorkFlow = @WorkFlow,");
            if (iot_flmeterdata.Temperature != null)
                part1.Append("Temperature = @Temperature,");
            if (iot_flmeterdata.Pressure != null)
                part1.Append("Pressure = @Pressure,");
            if (iot_flmeterdata.FMState != null)
                part1.Append("FMState = @FMState,");
            if (iot_flmeterdata.FMStateMsg != null)
                part1.Append("FMStateMsg = @FMStateMsg,");
            if (iot_flmeterdata.RTUState != null)
                part1.Append("RTUState = @RTUState,");
            if (iot_flmeterdata.RTUStateMsg != null)
                part1.Append("RTUStateMsg = @RTUStateMsg,");
            if (iot_flmeterdata.SumTotal != null)
                part1.Append("SumTotal = @SumTotal,");
            if (iot_flmeterdata.RemainMoney != null)
                part1.Append("RemainMoney = @RemainMoney,");
            if (iot_flmeterdata.RemainVolume != null)
                part1.Append("RemainVolume = @RemainVolume,");
            if (iot_flmeterdata.Overdraft != null)
                part1.Append("Overdraft = @Overdraft,");
            if (iot_flmeterdata.RemoteChargeMoney != null)
                part1.Append("RemoteChargeMoney = @RemoteChargeMoney,");
            if (iot_flmeterdata.RemoteChargeTimes != null)
                part1.Append("RemoteChargeTimes = @RemoteChargeTimes,");
            if (iot_flmeterdata.Price != null)
                part1.Append("Price = @Price,");
            if (iot_flmeterdata.ValveState != null)
                part1.Append("ValveState = @ValveState,");
            if (iot_flmeterdata.ValveStateMsg != null)
                part1.Append("ValveStateMsg = @ValveStateMsg,");
            if (iot_flmeterdata.PowerVoltage != null)
                part1.Append("PowerVoltage = @PowerVoltage,");
            if (iot_flmeterdata.BatteryVoltage != null)
                part1.Append("BatteryVoltage = @BatteryVoltage,");
            if (iot_flmeterdata.Reserve1 != null)
                part1.Append("Reserve1 = @Reserve1,");
            if (iot_flmeterdata.Reserve2 != null)
                part1.Append("Reserve2 = @Reserve2,");
            if (iot_flmeterdata.Reserve3 != null)
                part1.Append("Reserve3 = @Reserve3,");
            if (iot_flmeterdata.Reserve4 != null)
                part1.Append("Reserve4 = @Reserve4,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="IOT_FLMeterData"></param>
        /// <returns></returns>
        public int Add(IOT_FLMeterData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="IOT_FLMeterData"></param>
        /// <returns></returns>
        public void Update(IOT_FLMeterData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
