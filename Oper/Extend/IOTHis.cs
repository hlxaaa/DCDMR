using Common.Helper;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HHTDCDMR.Oper.Extend
{
    public class IOTHis
    {
        ///// <summary>
        ///// iot更新表的实时数据，没有则添加
        ///// </summary>
        ///// <param name="data"></param>
        //public void UpdateData(IOT0000000001 data) {
        //    var meterNo = data.FLMeterNo;
        //    var tableName = "IOT" + StringHelper.Instance.GetIntStringWithZero(meterNo.ToString(), 10);
        //    IOT_FLMeterData meter = new IOT_FLMeterData(data);
        //    IOT_FLMeterDataOper.Instance.Update(meter);
        //}

        ///// <summary>
        ///// 向物联网表具的历史数据表中添加记录，如果是第一次添加，创建这个table
        ///// </summary>
        ///// <param name="data"></param>
        //public void AddHisData(IOT0000000001 data)
        //{
        //    var meterNo = data.FLMeterNo;
        //    var tableName = "IOT" + StringHelper.Instance.GetIntStringWithZero(meterNo.ToString(), 10);
        //    #region 没表则建表
        //    string createTableStr = $@"if not EXISTS(select * from sysobjects where id=OBJECT_ID('{tableName}')
        //                                            and OBJECTPROPERTY(id, 'IsUserTable')=1)
        //                                            CREATE TABLE [dbo].[{tableName}] (
	       //                                             [Id] [int] PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	       //                                             [communicateNo] [nvarchar] (50) NULL,
	       //                                             [FLMeterNo] [int] NULL,
	       //                                             [siteNo] [nvarchar] (50) NULL,
	       //                                             [InstantTime] [datetime] NULL,
	       //                                             [ReceivTime] [datetime] NULL,
	       //                                             [StdSum] [decimal] (18, 3) NULL,
	       //                                             [WorkSum] [decimal] (18, 3) NULL,
	       //                                             [StdFlow] [decimal] (18, 3) NULL,
	       //                                             [WorkFlow] [decimal] (18, 3) NULL,
	       //                                             [Temperature] [decimal] (18, 3) NULL,
	       //                                             [Pressure] [decimal] (18, 3) NULL,
	       //                                             [FMState] [int] NULL,
	       //                                             [FMStateMsg] [nvarchar] (200) NULL,
	       //                                             [RTUState] [int] NULL,
	       //                                             [RTUStateMsg] [nvarchar] (200) NULL,
	       //                                             [SumTotal] [decimal] (18, 3) NULL,
	       //                                             [RemainMoney] [decimal] (18, 3) NULL,
	       //                                             [RemainVolume] [decimal] (18, 3) NULL,
	       //                                             [Overdraft] [decimal] (18, 3) NULL,
	       //                                             [RemoteChargeMoney] [decimal] (18, 3) NULL,
	       //                                             [RemoteChargeTimes] [int] NULL,
	       //                                             [Price] [decimal] (18, 3) NULL,
	       //                                             [ValveState] [int] NULL,
	       //                                             [ValveStateMsg] [nvarchar] (200) NULL,
	       //                                             [PowerVoltage] [decimal] (18, 3) NULL,
	       //                                             [BatteryVoltage] [decimal] (18, 3) NULL,
	       //                                             [Reserve1] [nvarchar] (50) NULL,
	       //                                             [Reserve2] [nvarchar] (50) NULL,
	       //                                             [Reserve3] [nvarchar] (50) NULL,
	       //                                             [Reserve4] [nvarchar] (50) NULL
        //                                            )
        //                                            ";
        //    SqlHelper.Instance.ExcuteNon2(createTableStr);
        //    #endregion
        //    var id = Add(data, tableName);
        //}

        //public int Add(IOT0000000001 model, string tableName, SqlConnection connection = null, SqlTransaction transaction = null)
        //{
        //    var str = GetInsertStr(model, tableName) + " select @@identity";
        //    var dict = GetParameters(model);
        //    return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        //}

        ///// <summary>
        ///// 获取参数
        ///// </summary>
        ///// <param name="iot0000000001"></param>
        ///// <returns></returns>
        //public Dictionary<string, string> GetParameters(IOT0000000001 iot0000000001)
        //{
        //    Dictionary<string, string> dict = new Dictionary<string, string>();
        //    if (iot0000000001.Id != null)
        //        dict.Add("@Id", iot0000000001.Id.ToString());
        //    if (iot0000000001.communicateNo != null)
        //        dict.Add("@communicateNo", iot0000000001.communicateNo.ToString());
        //    if (iot0000000001.FLMeterNo != null)
        //        dict.Add("@FLMeterNo", iot0000000001.FLMeterNo.ToString());
        //    if (iot0000000001.siteNo != null)
        //        dict.Add("@siteNo", iot0000000001.siteNo.ToString());
        //    if (iot0000000001.InstantTime != null)
        //        dict.Add("@InstantTime", iot0000000001.InstantTime.ToString());
        //    if (iot0000000001.ReceivTime != null)
        //        dict.Add("@ReceivTime", iot0000000001.ReceivTime.ToString());
        //    if (iot0000000001.StdSum != null)
        //        dict.Add("@StdSum", iot0000000001.StdSum.ToString());
        //    if (iot0000000001.WorkSum != null)
        //        dict.Add("@WorkSum", iot0000000001.WorkSum.ToString());
        //    if (iot0000000001.StdFlow != null)
        //        dict.Add("@StdFlow", iot0000000001.StdFlow.ToString());
        //    if (iot0000000001.WorkFlow != null)
        //        dict.Add("@WorkFlow", iot0000000001.WorkFlow.ToString());
        //    if (iot0000000001.Temperature != null)
        //        dict.Add("@Temperature", iot0000000001.Temperature.ToString());
        //    if (iot0000000001.Pressure != null)
        //        dict.Add("@Pressure", iot0000000001.Pressure.ToString());
        //    if (iot0000000001.FMState != null)
        //        dict.Add("@FMState", iot0000000001.FMState.ToString());
        //    if (iot0000000001.FMStateMsg != null)
        //        dict.Add("@FMStateMsg", iot0000000001.FMStateMsg.ToString());
        //    if (iot0000000001.RTUState != null)
        //        dict.Add("@RTUState", iot0000000001.RTUState.ToString());
        //    if (iot0000000001.RTUStateMsg != null)
        //        dict.Add("@RTUStateMsg", iot0000000001.RTUStateMsg.ToString());
        //    if (iot0000000001.SumTotal != null)
        //        dict.Add("@SumTotal", iot0000000001.SumTotal.ToString());
        //    if (iot0000000001.RemainMoney != null)
        //        dict.Add("@RemainMoney", iot0000000001.RemainMoney.ToString());
        //    if (iot0000000001.RemainVolume != null)
        //        dict.Add("@RemainVolume", iot0000000001.RemainVolume.ToString());
        //    if (iot0000000001.Overdraft != null)
        //        dict.Add("@Overdraft", iot0000000001.Overdraft.ToString());
        //    if (iot0000000001.RemoteChargeMoney != null)
        //        dict.Add("@RemoteChargeMoney", iot0000000001.RemoteChargeMoney.ToString());
        //    if (iot0000000001.RemoteChargeTimes != null)
        //        dict.Add("@RemoteChargeTimes", iot0000000001.RemoteChargeTimes.ToString());
        //    if (iot0000000001.Price != null)
        //        dict.Add("@Price", iot0000000001.Price.ToString());
        //    if (iot0000000001.ValveState != null)
        //        dict.Add("@ValveState", iot0000000001.ValveState.ToString());
        //    if (iot0000000001.ValveStateMsg != null)
        //        dict.Add("@ValveStateMsg", iot0000000001.ValveStateMsg.ToString());
        //    if (iot0000000001.PowerVoltage != null)
        //        dict.Add("@PowerVoltage", iot0000000001.PowerVoltage.ToString());
        //    if (iot0000000001.BatteryVoltage != null)
        //        dict.Add("@BatteryVoltage", iot0000000001.BatteryVoltage.ToString());
        //    if (iot0000000001.Reserve1 != null)
        //        dict.Add("@Reserve1", iot0000000001.Reserve1.ToString());
        //    if (iot0000000001.Reserve2 != null)
        //        dict.Add("@Reserve2", iot0000000001.Reserve2.ToString());
        //    if (iot0000000001.Reserve3 != null)
        //        dict.Add("@Reserve3", iot0000000001.Reserve3.ToString());
        //    if (iot0000000001.Reserve4 != null)
        //        dict.Add("@Reserve4", iot0000000001.Reserve4.ToString());

        //    return dict;
        //}

        ///// <summary>
        ///// 插入
        ///// </summary>
        ///// <param name="iot0000000001"></param>
        ///// <returns>是否成功</returns>
        //public string GetInsertStr(IOT0000000001 iot0000000001, string tableName)
        //{
        //    StringBuilder part1 = new StringBuilder();
        //    StringBuilder part2 = new StringBuilder();

        //    if (iot0000000001.communicateNo != null)
        //    {
        //        part1.Append("communicateNo,");
        //        part2.Append("@communicateNo,");
        //    }
        //    if (iot0000000001.FLMeterNo != null)
        //    {
        //        part1.Append("FLMeterNo,");
        //        part2.Append("@FLMeterNo,");
        //    }
        //    if (iot0000000001.siteNo != null)
        //    {
        //        part1.Append("siteNo,");
        //        part2.Append("@siteNo,");
        //    }
        //    if (iot0000000001.InstantTime != null)
        //    {
        //        part1.Append("InstantTime,");
        //        part2.Append("@InstantTime,");
        //    }
        //    if (iot0000000001.ReceivTime != null)
        //    {
        //        part1.Append("ReceivTime,");
        //        part2.Append("@ReceivTime,");
        //    }
        //    if (iot0000000001.StdSum != null)
        //    {
        //        part1.Append("StdSum,");
        //        part2.Append("@StdSum,");
        //    }
        //    if (iot0000000001.WorkSum != null)
        //    {
        //        part1.Append("WorkSum,");
        //        part2.Append("@WorkSum,");
        //    }
        //    if (iot0000000001.StdFlow != null)
        //    {
        //        part1.Append("StdFlow,");
        //        part2.Append("@StdFlow,");
        //    }
        //    if (iot0000000001.WorkFlow != null)
        //    {
        //        part1.Append("WorkFlow,");
        //        part2.Append("@WorkFlow,");
        //    }
        //    if (iot0000000001.Temperature != null)
        //    {
        //        part1.Append("Temperature,");
        //        part2.Append("@Temperature,");
        //    }
        //    if (iot0000000001.Pressure != null)
        //    {
        //        part1.Append("Pressure,");
        //        part2.Append("@Pressure,");
        //    }
        //    if (iot0000000001.FMState != null)
        //    {
        //        part1.Append("FMState,");
        //        part2.Append("@FMState,");
        //    }
        //    if (iot0000000001.FMStateMsg != null)
        //    {
        //        part1.Append("FMStateMsg,");
        //        part2.Append("@FMStateMsg,");
        //    }
        //    if (iot0000000001.RTUState != null)
        //    {
        //        part1.Append("RTUState,");
        //        part2.Append("@RTUState,");
        //    }
        //    if (iot0000000001.RTUStateMsg != null)
        //    {
        //        part1.Append("RTUStateMsg,");
        //        part2.Append("@RTUStateMsg,");
        //    }
        //    if (iot0000000001.SumTotal != null)
        //    {
        //        part1.Append("SumTotal,");
        //        part2.Append("@SumTotal,");
        //    }
        //    if (iot0000000001.RemainMoney != null)
        //    {
        //        part1.Append("RemainMoney,");
        //        part2.Append("@RemainMoney,");
        //    }
        //    if (iot0000000001.RemainVolume != null)
        //    {
        //        part1.Append("RemainVolume,");
        //        part2.Append("@RemainVolume,");
        //    }
        //    if (iot0000000001.Overdraft != null)
        //    {
        //        part1.Append("Overdraft,");
        //        part2.Append("@Overdraft,");
        //    }
        //    if (iot0000000001.RemoteChargeMoney != null)
        //    {
        //        part1.Append("RemoteChargeMoney,");
        //        part2.Append("@RemoteChargeMoney,");
        //    }
        //    if (iot0000000001.RemoteChargeTimes != null)
        //    {
        //        part1.Append("RemoteChargeTimes,");
        //        part2.Append("@RemoteChargeTimes,");
        //    }
        //    if (iot0000000001.Price != null)
        //    {
        //        part1.Append("Price,");
        //        part2.Append("@Price,");
        //    }
        //    if (iot0000000001.ValveState != null)
        //    {
        //        part1.Append("ValveState,");
        //        part2.Append("@ValveState,");
        //    }
        //    if (iot0000000001.ValveStateMsg != null)
        //    {
        //        part1.Append("ValveStateMsg,");
        //        part2.Append("@ValveStateMsg,");
        //    }
        //    if (iot0000000001.PowerVoltage != null)
        //    {
        //        part1.Append("PowerVoltage,");
        //        part2.Append("@PowerVoltage,");
        //    }
        //    if (iot0000000001.BatteryVoltage != null)
        //    {
        //        part1.Append("BatteryVoltage,");
        //        part2.Append("@BatteryVoltage,");
        //    }
        //    if (iot0000000001.Reserve1 != null)
        //    {
        //        part1.Append("Reserve1,");
        //        part2.Append("@Reserve1,");
        //    }
        //    if (iot0000000001.Reserve2 != null)
        //    {
        //        part1.Append("Reserve2,");
        //        part2.Append("@Reserve2,");
        //    }
        //    if (iot0000000001.Reserve3 != null)
        //    {
        //        part1.Append("Reserve3,");
        //        part2.Append("@Reserve3,");
        //    }
        //    if (iot0000000001.Reserve4 != null)
        //    {
        //        part1.Append("Reserve4,");
        //        part2.Append("@Reserve4,");
        //    }
        //    StringBuilder sql = new StringBuilder();
        //    sql.Append($"insert into {tableName}(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
        //    return sql.ToString();
        //}

    }
}