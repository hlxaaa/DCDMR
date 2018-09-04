using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_CtrlCmdOper : SingleTon<AllInOne_CtrlCmdOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_ctrlcmd"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_CtrlCmd allinone_ctrlcmd)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_ctrlcmd.Id != null)
                dict.Add("@Id", allinone_ctrlcmd.Id.ToString());
            if (allinone_ctrlcmd.siteNo != null)
                dict.Add("@siteNo", allinone_ctrlcmd.siteNo.ToString());
            if (allinone_ctrlcmd.CmdType != null)
                dict.Add("@CmdType", allinone_ctrlcmd.CmdType.ToString());
            if (allinone_ctrlcmd.RTUNo != null)
                dict.Add("@RTUNo", allinone_ctrlcmd.RTUNo.ToString());
            if (allinone_ctrlcmd.CommNo != null)
                dict.Add("@CommNo", allinone_ctrlcmd.CommNo.ToString());
            if (allinone_ctrlcmd.MeterNo != null)
                dict.Add("@MeterNo", allinone_ctrlcmd.MeterNo.ToString());
            if (allinone_ctrlcmd.FSEQ != null)
                dict.Add("@FSEQ", allinone_ctrlcmd.FSEQ.ToString());
            if (allinone_ctrlcmd.Operator != null)
                dict.Add("@Operator", allinone_ctrlcmd.Operator.ToString());
            if (allinone_ctrlcmd.CreateTime != null)
                dict.Add("@CreateTime", allinone_ctrlcmd.CreateTime.ToString());
            if (allinone_ctrlcmd.Timeout != null)
                dict.Add("@Timeout", allinone_ctrlcmd.Timeout.ToString());
            if (allinone_ctrlcmd.CmdState != null)
                dict.Add("@CmdState", allinone_ctrlcmd.CmdState.ToString());
            if (allinone_ctrlcmd.FeedBackMsg != null)
                dict.Add("@FeedBackMsg", allinone_ctrlcmd.FeedBackMsg.ToString());
            if (allinone_ctrlcmd.SetIPAddr != null)
                dict.Add("@SetIPAddr", allinone_ctrlcmd.SetIPAddr.ToString());
            if (allinone_ctrlcmd.SetPortNum != null)
                dict.Add("@SetPortNum", allinone_ctrlcmd.SetPortNum.ToString());
            if (allinone_ctrlcmd.Set485Inv != null)
                dict.Add("@Set485Inv", allinone_ctrlcmd.Set485Inv.ToString());
            if (allinone_ctrlcmd.SetGPRSInv != null)
                dict.Add("@SetGPRSInv", allinone_ctrlcmd.SetGPRSInv.ToString());
            if (allinone_ctrlcmd.UploadTime1 != null)
                dict.Add("@UploadTime1", allinone_ctrlcmd.UploadTime1.ToString());
            if (allinone_ctrlcmd.UploadTime2 != null)
                dict.Add("@UploadTime2", allinone_ctrlcmd.UploadTime2.ToString());
            if (allinone_ctrlcmd.UploadTime3 != null)
                dict.Add("@UploadTime3", allinone_ctrlcmd.UploadTime3.ToString());
            if (allinone_ctrlcmd.MeterNum != null)
                dict.Add("@MeterNum", allinone_ctrlcmd.MeterNum.ToString());
            if (allinone_ctrlcmd.ProtocotSet != null)
                dict.Add("@ProtocotSet", allinone_ctrlcmd.ProtocotSet.ToString());
            if (allinone_ctrlcmd.ComAddrSet != null)
                dict.Add("@ComAddrSet", allinone_ctrlcmd.ComAddrSet.ToString());
            if (allinone_ctrlcmd.BaudRateSet != null)
                dict.Add("@BaudRateSet", allinone_ctrlcmd.BaudRateSet.ToString());
            if (allinone_ctrlcmd.BuyTimes != null)
                dict.Add("@BuyTimes", allinone_ctrlcmd.BuyTimes.ToString());
            if (allinone_ctrlcmd.BuyMoney != null)
                dict.Add("@BuyMoney", allinone_ctrlcmd.BuyMoney.ToString());
            if (allinone_ctrlcmd.SetPrice != null)
                dict.Add("@SetPrice", allinone_ctrlcmd.SetPrice.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_ctrlcmd"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_CtrlCmd allinone_ctrlcmd)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_ctrlcmd.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (allinone_ctrlcmd.CmdType != null)
            {
                part1.Append("CmdType,");
                part2.Append("@CmdType,");
            }
            if (allinone_ctrlcmd.RTUNo != null)
            {
                part1.Append("RTUNo,");
                part2.Append("@RTUNo,");
            }
            if (allinone_ctrlcmd.CommNo != null)
            {
                part1.Append("CommNo,");
                part2.Append("@CommNo,");
            }
            if (allinone_ctrlcmd.MeterNo != null)
            {
                part1.Append("MeterNo,");
                part2.Append("@MeterNo,");
            }
            if (allinone_ctrlcmd.FSEQ != null)
            {
                part1.Append("FSEQ,");
                part2.Append("@FSEQ,");
            }
            if (allinone_ctrlcmd.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            if (allinone_ctrlcmd.CreateTime != null)
            {
                part1.Append("CreateTime,");
                part2.Append("@CreateTime,");
            }
            if (allinone_ctrlcmd.Timeout != null)
            {
                part1.Append("Timeout,");
                part2.Append("@Timeout,");
            }
            if (allinone_ctrlcmd.CmdState != null)
            {
                part1.Append("CmdState,");
                part2.Append("@CmdState,");
            }
            if (allinone_ctrlcmd.FeedBackMsg != null)
            {
                part1.Append("FeedBackMsg,");
                part2.Append("@FeedBackMsg,");
            }
            if (allinone_ctrlcmd.SetIPAddr != null)
            {
                part1.Append("SetIPAddr,");
                part2.Append("@SetIPAddr,");
            }
            if (allinone_ctrlcmd.SetPortNum != null)
            {
                part1.Append("SetPortNum,");
                part2.Append("@SetPortNum,");
            }
            if (allinone_ctrlcmd.Set485Inv != null)
            {
                part1.Append("Set485Inv,");
                part2.Append("@Set485Inv,");
            }
            if (allinone_ctrlcmd.SetGPRSInv != null)
            {
                part1.Append("SetGPRSInv,");
                part2.Append("@SetGPRSInv,");
            }
            if (allinone_ctrlcmd.UploadTime1 != null)
            {
                part1.Append("UploadTime1,");
                part2.Append("@UploadTime1,");
            }
            if (allinone_ctrlcmd.UploadTime2 != null)
            {
                part1.Append("UploadTime2,");
                part2.Append("@UploadTime2,");
            }
            if (allinone_ctrlcmd.UploadTime3 != null)
            {
                part1.Append("UploadTime3,");
                part2.Append("@UploadTime3,");
            }
            if (allinone_ctrlcmd.MeterNum != null)
            {
                part1.Append("MeterNum,");
                part2.Append("@MeterNum,");
            }
            if (allinone_ctrlcmd.ProtocotSet != null)
            {
                part1.Append("ProtocotSet,");
                part2.Append("@ProtocotSet,");
            }
            if (allinone_ctrlcmd.ComAddrSet != null)
            {
                part1.Append("ComAddrSet,");
                part2.Append("@ComAddrSet,");
            }
            if (allinone_ctrlcmd.BaudRateSet != null)
            {
                part1.Append("BaudRateSet,");
                part2.Append("@BaudRateSet,");
            }
            if (allinone_ctrlcmd.BuyTimes != null)
            {
                part1.Append("BuyTimes,");
                part2.Append("@BuyTimes,");
            }
            if (allinone_ctrlcmd.BuyMoney != null)
            {
                part1.Append("BuyMoney,");
                part2.Append("@BuyMoney,");
            }
            if (allinone_ctrlcmd.SetPrice != null)
            {
                part1.Append("SetPrice,");
                part2.Append("@SetPrice,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_ctrlcmd(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_ctrlcmd"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_CtrlCmd allinone_ctrlcmd)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_ctrlcmd set ");
            if (allinone_ctrlcmd.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (allinone_ctrlcmd.CmdType != null)
                part1.Append("CmdType = @CmdType,");
            if (allinone_ctrlcmd.RTUNo != null)
                part1.Append("RTUNo = @RTUNo,");
            if (allinone_ctrlcmd.CommNo != null)
                part1.Append("CommNo = @CommNo,");
            if (allinone_ctrlcmd.MeterNo != null)
                part1.Append("MeterNo = @MeterNo,");
            if (allinone_ctrlcmd.FSEQ != null)
                part1.Append("FSEQ = @FSEQ,");
            if (allinone_ctrlcmd.Operator != null)
                part1.Append("Operator = @Operator,");
            if (allinone_ctrlcmd.CreateTime != null)
                part1.Append("CreateTime = @CreateTime,");
            if (allinone_ctrlcmd.Timeout != null)
                part1.Append("Timeout = @Timeout,");
            if (allinone_ctrlcmd.CmdState != null)
                part1.Append("CmdState = @CmdState,");
            if (allinone_ctrlcmd.FeedBackMsg != null)
                part1.Append("FeedBackMsg = @FeedBackMsg,");
            if (allinone_ctrlcmd.SetIPAddr != null)
                part1.Append("SetIPAddr = @SetIPAddr,");
            if (allinone_ctrlcmd.SetPortNum != null)
                part1.Append("SetPortNum = @SetPortNum,");
            if (allinone_ctrlcmd.Set485Inv != null)
                part1.Append("Set485Inv = @Set485Inv,");
            if (allinone_ctrlcmd.SetGPRSInv != null)
                part1.Append("SetGPRSInv = @SetGPRSInv,");
            if (allinone_ctrlcmd.UploadTime1 != null)
                part1.Append("UploadTime1 = @UploadTime1,");
            if (allinone_ctrlcmd.UploadTime2 != null)
                part1.Append("UploadTime2 = @UploadTime2,");
            if (allinone_ctrlcmd.UploadTime3 != null)
                part1.Append("UploadTime3 = @UploadTime3,");
            if (allinone_ctrlcmd.MeterNum != null)
                part1.Append("MeterNum = @MeterNum,");
            if (allinone_ctrlcmd.ProtocotSet != null)
                part1.Append("ProtocotSet = @ProtocotSet,");
            if (allinone_ctrlcmd.ComAddrSet != null)
                part1.Append("ComAddrSet = @ComAddrSet,");
            if (allinone_ctrlcmd.BaudRateSet != null)
                part1.Append("BaudRateSet = @BaudRateSet,");
            if (allinone_ctrlcmd.BuyTimes != null)
                part1.Append("BuyTimes = @BuyTimes,");
            if (allinone_ctrlcmd.BuyMoney != null)
                part1.Append("BuyMoney = @BuyMoney,");
            if (allinone_ctrlcmd.SetPrice != null)
                part1.Append("SetPrice = @SetPrice,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_CtrlCmd"></param>
        /// <returns></returns>
        public int Add(AllInOne_CtrlCmd model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_CtrlCmd"></param>
        /// <returns></returns>
        public void Update(AllInOne_CtrlCmd model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
