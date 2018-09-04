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
 
        public string GetInsertStr2(AllFMData allfmdata)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allfmdata.communicateNo != null)
            {
                part1.Append("communicateNo,");
                part2.Append($"'{allfmdata.communicateNo}',");
            }
            if (allfmdata.FLMeterNo != null)
            {
                part1.Append("FLMeterNo,");
                part2.Append($"'{allfmdata.FLMeterNo}',");
            }
            if (allfmdata.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append($"'{allfmdata.siteNo}',");
            }
            if (allfmdata.InstantTime != null)
            {
                part1.Append("InstantTime,");
                part2.Append($"'{allfmdata.InstantTime}',");
            }
            if (allfmdata.ReceivTime != null)
            {
                part1.Append("ReceivTime,");
                part2.Append($"'{allfmdata.ReceivTime}',");
            }
            if (allfmdata.StdSum != null)
            {
                part1.Append("StdSum,");
                part2.Append($"'{allfmdata.StdSum}',");
            }
            if (allfmdata.WorkSum != null)
            {
                part1.Append("WorkSum,");
                part2.Append($"'{allfmdata.WorkSum}',");
            }
            if (allfmdata.StdFlow != null)
            {
                part1.Append("StdFlow,");
                part2.Append($"'{allfmdata.StdFlow}',");
            }
            if (allfmdata.WorkFlow != null)
            {
                part1.Append("WorkFlow,");
                part2.Append($"'{allfmdata.WorkFlow}',");
            }
            if (allfmdata.Temperature != null)
            {
                part1.Append("Temperature,");
                part2.Append($"'{allfmdata.Temperature}',");
            }
            if (allfmdata.Pressure != null)
            {
                part1.Append("Pressure,");
                part2.Append($"'{allfmdata.Pressure}',");
            }
            if (allfmdata.FMState != null)
            {
                part1.Append("FMState,");
                part2.Append($"'{allfmdata.FMState}',");
            }
            if (allfmdata.FMStateMsg != null)
            {
                part1.Append("FMStateMsg,");
                part2.Append($"'{allfmdata.FMStateMsg}',");
            }
            if (allfmdata.RTUState != null)
            {
                part1.Append("RTUState,");
                part2.Append($"'{allfmdata.RTUState}',");
            }
            if (allfmdata.RTUStateMsg != null)
            {
                part1.Append("RTUStateMsg,");
                part2.Append($"'{allfmdata.RTUStateMsg}',");
            }
            if (allfmdata.SumTotal != null)
            {
                part1.Append("SumTotal,");
                part2.Append($"'{allfmdata.SumTotal}',");
            }
            if (allfmdata.RemainMoney != null)
            {
                part1.Append("RemainMoney,");
                part2.Append($"'{allfmdata.RemainMoney}',");
            }
            if (allfmdata.RemainVolume != null)
            {
                part1.Append("RemainVolume,");
                part2.Append($"'{allfmdata.RemainVolume}',");
            }
            if (allfmdata.Overdraft != null)
            {
                part1.Append("Overdraft,");
                part2.Append($"'{allfmdata.Overdraft}',");
            }
            if (allfmdata.RemoteChargeMoney != null)
            {
                part1.Append("RemoteChargeMoney,");
                part2.Append($"'{allfmdata.RemoteChargeMoney}',");
            }
            if (allfmdata.RemoteChargeTimes != null)
            {
                part1.Append("RemoteChargeTimes,");
                part2.Append($"'{allfmdata.RemoteChargeTimes}',");
            }
            if (allfmdata.Price != null)
            {
                part1.Append("Price,");
                part2.Append($"'{allfmdata.Price}',");
            }
            if (allfmdata.ValveState != null)
            {
                part1.Append("ValveState,");
                part2.Append($"'{allfmdata.ValveState}',");
            }
            if (allfmdata.ValveStateMsg != null)
            {
                part1.Append("ValveStateMsg,");
                part2.Append($"'{allfmdata.ValveStateMsg}',");
            }
            if (allfmdata.PowerVoltage != null)
            {
                part1.Append("PowerVoltage,");
                part2.Append($"'{allfmdata.PowerVoltage}',");
            }
            if (allfmdata.BatteryVoltage != null)
            {
                part1.Append("BatteryVoltage,");
                part2.Append($"'{allfmdata.BatteryVoltage}',");
            }
            if (allfmdata.Reserve1 != null)
            {
                part1.Append("Reserve1,");
                part2.Append($"'{allfmdata.Reserve1}',");
            }
            if (allfmdata.Reserve2 != null)
            {
                part1.Append("Reserve2,");
                part2.Append($"'{allfmdata.Reserve2}',");
            }
            if (allfmdata.Reserve3 != null)
            {
                part1.Append("Reserve3,");
                part2.Append($"'{allfmdata.Reserve3}',");
            }
            if (allfmdata.Reserve4 != null)
            {
                part1.Append("Reserve4,");
                part2.Append($"'{allfmdata.Reserve4}',");
            }
            if (allfmdata.rId != null)
            {
                part1.Append("Id,");
                part2.Append($"'{allfmdata.Id}',");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append($@"
insert into allfmdata(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")  ");
            return sql.ToString();
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllFMData"></param>
        /// <returns></returns>
        public int Add2(AllFMData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr2(model)+" select @@identity";
            var dict = new Dictionary<string, string>();
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }

    }
}
