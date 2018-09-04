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
    public partial class IOT_CtrlCmdOper : SingleTon<IOT_CtrlCmdOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="iot_ctrlcmd"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(IOT_CtrlCmd iot_ctrlcmd)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (iot_ctrlcmd.Id != null)
                dict.Add("@Id", iot_ctrlcmd.Id.ToString());
            if (iot_ctrlcmd.siteNo != null)
                dict.Add("@siteNo", iot_ctrlcmd.siteNo.ToString());
            if (iot_ctrlcmd.CmdType != null)
                dict.Add("@CmdType", iot_ctrlcmd.CmdType.ToString());
            if (iot_ctrlcmd.RTUNo != null)
                dict.Add("@RTUNo", iot_ctrlcmd.RTUNo.ToString());
            if (iot_ctrlcmd.CommNo != null)
                dict.Add("@CommNo", iot_ctrlcmd.CommNo.ToString());
            if (iot_ctrlcmd.MeterNo != null)
                dict.Add("@MeterNo", iot_ctrlcmd.MeterNo.ToString());
            if (iot_ctrlcmd.FSEQ != null)
                dict.Add("@FSEQ", iot_ctrlcmd.FSEQ.ToString());
            if (iot_ctrlcmd.Operator != null)
                dict.Add("@Operator", iot_ctrlcmd.Operator.ToString());
            if (iot_ctrlcmd.CreateTime != null)
                dict.Add("@CreateTime", iot_ctrlcmd.CreateTime.ToString());
            if (iot_ctrlcmd.Timeout != null)
                dict.Add("@Timeout", iot_ctrlcmd.Timeout.ToString());
            if (iot_ctrlcmd.CmdState != null)
                dict.Add("@CmdState", iot_ctrlcmd.CmdState.ToString());
            if (iot_ctrlcmd.FeedBackMsg != null)
                dict.Add("@FeedBackMsg", iot_ctrlcmd.FeedBackMsg.ToString());
            if (iot_ctrlcmd.SetIPAddr != null)
                dict.Add("@SetIPAddr", iot_ctrlcmd.SetIPAddr.ToString());
            if (iot_ctrlcmd.SetPortNum != null)
                dict.Add("@SetPortNum", iot_ctrlcmd.SetPortNum.ToString());
            if (iot_ctrlcmd.Set485Inv != null)
                dict.Add("@Set485Inv", iot_ctrlcmd.Set485Inv.ToString());
            if (iot_ctrlcmd.SetGPRSInv != null)
                dict.Add("@SetGPRSInv", iot_ctrlcmd.SetGPRSInv.ToString());
            if (iot_ctrlcmd.UploadTime1 != null)
                dict.Add("@UploadTime1", iot_ctrlcmd.UploadTime1.ToString());
            if (iot_ctrlcmd.UploadTime2 != null)
                dict.Add("@UploadTime2", iot_ctrlcmd.UploadTime2.ToString());
            if (iot_ctrlcmd.UploadTime3 != null)
                dict.Add("@UploadTime3", iot_ctrlcmd.UploadTime3.ToString());
            if (iot_ctrlcmd.MeterNum != null)
                dict.Add("@MeterNum", iot_ctrlcmd.MeterNum.ToString());
            if (iot_ctrlcmd.ProtocotSet != null)
                dict.Add("@ProtocotSet", iot_ctrlcmd.ProtocotSet.ToString());
            if (iot_ctrlcmd.ComAddrSet != null)
                dict.Add("@ComAddrSet", iot_ctrlcmd.ComAddrSet.ToString());
            if (iot_ctrlcmd.BaudRateSet != null)
                dict.Add("@BaudRateSet", iot_ctrlcmd.BaudRateSet.ToString());
            if (iot_ctrlcmd.BuyTimes != null)
                dict.Add("@BuyTimes", iot_ctrlcmd.BuyTimes.ToString());
            if (iot_ctrlcmd.BuyMoney != null)
                dict.Add("@BuyMoney", iot_ctrlcmd.BuyMoney.ToString());
            if (iot_ctrlcmd.SetPrice != null)
                dict.Add("@SetPrice", iot_ctrlcmd.SetPrice.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="iot_ctrlcmd"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(IOT_CtrlCmd iot_ctrlcmd)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (iot_ctrlcmd.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (iot_ctrlcmd.CmdType != null)
            {
                part1.Append("CmdType,");
                part2.Append("@CmdType,");
            }
            if (iot_ctrlcmd.RTUNo != null)
            {
                part1.Append("RTUNo,");
                part2.Append("@RTUNo,");
            }
            if (iot_ctrlcmd.CommNo != null)
            {
                part1.Append("CommNo,");
                part2.Append("@CommNo,");
            }
            if (iot_ctrlcmd.MeterNo != null)
            {
                part1.Append("MeterNo,");
                part2.Append("@MeterNo,");
            }
            if (iot_ctrlcmd.FSEQ != null)
            {
                part1.Append("FSEQ,");
                part2.Append("@FSEQ,");
            }
            if (iot_ctrlcmd.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            if (iot_ctrlcmd.CreateTime != null)
            {
                part1.Append("CreateTime,");
                part2.Append("@CreateTime,");
            }
            if (iot_ctrlcmd.Timeout != null)
            {
                part1.Append("Timeout,");
                part2.Append("@Timeout,");
            }
            if (iot_ctrlcmd.CmdState != null)
            {
                part1.Append("CmdState,");
                part2.Append("@CmdState,");
            }
            if (iot_ctrlcmd.FeedBackMsg != null)
            {
                part1.Append("FeedBackMsg,");
                part2.Append("@FeedBackMsg,");
            }
            if (iot_ctrlcmd.SetIPAddr != null)
            {
                part1.Append("SetIPAddr,");
                part2.Append("@SetIPAddr,");
            }
            if (iot_ctrlcmd.SetPortNum != null)
            {
                part1.Append("SetPortNum,");
                part2.Append("@SetPortNum,");
            }
            if (iot_ctrlcmd.Set485Inv != null)
            {
                part1.Append("Set485Inv,");
                part2.Append("@Set485Inv,");
            }
            if (iot_ctrlcmd.SetGPRSInv != null)
            {
                part1.Append("SetGPRSInv,");
                part2.Append("@SetGPRSInv,");
            }
            if (iot_ctrlcmd.UploadTime1 != null)
            {
                part1.Append("UploadTime1,");
                part2.Append("@UploadTime1,");
            }
            if (iot_ctrlcmd.UploadTime2 != null)
            {
                part1.Append("UploadTime2,");
                part2.Append("@UploadTime2,");
            }
            if (iot_ctrlcmd.UploadTime3 != null)
            {
                part1.Append("UploadTime3,");
                part2.Append("@UploadTime3,");
            }
            if (iot_ctrlcmd.MeterNum != null)
            {
                part1.Append("MeterNum,");
                part2.Append("@MeterNum,");
            }
            if (iot_ctrlcmd.ProtocotSet != null)
            {
                part1.Append("ProtocotSet,");
                part2.Append("@ProtocotSet,");
            }
            if (iot_ctrlcmd.ComAddrSet != null)
            {
                part1.Append("ComAddrSet,");
                part2.Append("@ComAddrSet,");
            }
            if (iot_ctrlcmd.BaudRateSet != null)
            {
                part1.Append("BaudRateSet,");
                part2.Append("@BaudRateSet,");
            }
            if (iot_ctrlcmd.BuyTimes != null)
            {
                part1.Append("BuyTimes,");
                part2.Append("@BuyTimes,");
            }
            if (iot_ctrlcmd.BuyMoney != null)
            {
                part1.Append("BuyMoney,");
                part2.Append("@BuyMoney,");
            }
            if (iot_ctrlcmd.SetPrice != null)
            {
                part1.Append("SetPrice,");
                part2.Append("@SetPrice,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into iot_ctrlcmd(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="iot_ctrlcmd"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(IOT_CtrlCmd iot_ctrlcmd)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update iot_ctrlcmd set ");
            if (iot_ctrlcmd.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (iot_ctrlcmd.CmdType != null)
                part1.Append("CmdType = @CmdType,");
            if (iot_ctrlcmd.RTUNo != null)
                part1.Append("RTUNo = @RTUNo,");
            if (iot_ctrlcmd.CommNo != null)
                part1.Append("CommNo = @CommNo,");
            if (iot_ctrlcmd.MeterNo != null)
                part1.Append("MeterNo = @MeterNo,");
            if (iot_ctrlcmd.FSEQ != null)
                part1.Append("FSEQ = @FSEQ,");
            if (iot_ctrlcmd.Operator != null)
                part1.Append("Operator = @Operator,");
            if (iot_ctrlcmd.CreateTime != null)
                part1.Append("CreateTime = @CreateTime,");
            if (iot_ctrlcmd.Timeout != null)
                part1.Append("Timeout = @Timeout,");
            if (iot_ctrlcmd.CmdState != null)
                part1.Append("CmdState = @CmdState,");
            if (iot_ctrlcmd.FeedBackMsg != null)
                part1.Append("FeedBackMsg = @FeedBackMsg,");
            if (iot_ctrlcmd.SetIPAddr != null)
                part1.Append("SetIPAddr = @SetIPAddr,");
            if (iot_ctrlcmd.SetPortNum != null)
                part1.Append("SetPortNum = @SetPortNum,");
            if (iot_ctrlcmd.Set485Inv != null)
                part1.Append("Set485Inv = @Set485Inv,");
            if (iot_ctrlcmd.SetGPRSInv != null)
                part1.Append("SetGPRSInv = @SetGPRSInv,");
            if (iot_ctrlcmd.UploadTime1 != null)
                part1.Append("UploadTime1 = @UploadTime1,");
            if (iot_ctrlcmd.UploadTime2 != null)
                part1.Append("UploadTime2 = @UploadTime2,");
            if (iot_ctrlcmd.UploadTime3 != null)
                part1.Append("UploadTime3 = @UploadTime3,");
            if (iot_ctrlcmd.MeterNum != null)
                part1.Append("MeterNum = @MeterNum,");
            if (iot_ctrlcmd.ProtocotSet != null)
                part1.Append("ProtocotSet = @ProtocotSet,");
            if (iot_ctrlcmd.ComAddrSet != null)
                part1.Append("ComAddrSet = @ComAddrSet,");
            if (iot_ctrlcmd.BaudRateSet != null)
                part1.Append("BaudRateSet = @BaudRateSet,");
            if (iot_ctrlcmd.BuyTimes != null)
                part1.Append("BuyTimes = @BuyTimes,");
            if (iot_ctrlcmd.BuyMoney != null)
                part1.Append("BuyMoney = @BuyMoney,");
            if (iot_ctrlcmd.SetPrice != null)
                part1.Append("SetPrice = @SetPrice,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="IOT_CtrlCmd"></param>
        /// <returns></returns>
        public int Add(IOT_CtrlCmd model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="IOT_CtrlCmd"></param>
        /// <returns></returns>
        public void Update(IOT_CtrlCmd model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
