using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_AlarmInfoOper : SingleTon<AllInOne_AlarmInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_alarminfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_AlarmInfo allinone_alarminfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_alarminfo.Id != null)
                dict.Add("@Id", allinone_alarminfo.Id.ToString());
            if (allinone_alarminfo.siteNo != null)
                dict.Add("@siteNo", allinone_alarminfo.siteNo.ToString());
            if (allinone_alarminfo.RtuNo != null)
                dict.Add("@RtuNo", allinone_alarminfo.RtuNo.ToString());
            if (allinone_alarminfo.Devid != null)
                dict.Add("@Devid", allinone_alarminfo.Devid.ToString());
            if (allinone_alarminfo.DevType != null)
                dict.Add("@DevType", allinone_alarminfo.DevType.ToString());
            if (allinone_alarminfo.DevTypeName != null)
                dict.Add("@DevTypeName", allinone_alarminfo.DevTypeName.ToString());
            if (allinone_alarminfo.AlarmContent != null)
                dict.Add("@AlarmContent", allinone_alarminfo.AlarmContent.ToString());
            if (allinone_alarminfo.AlarmTime != null)
                dict.Add("@AlarmTime", allinone_alarminfo.AlarmTime.ToString());
            if (allinone_alarminfo.DealFlag != null)
                dict.Add("@DealFlag", allinone_alarminfo.DealFlag.ToString());
            if (allinone_alarminfo.DealTime != null)
                dict.Add("@DealTime", allinone_alarminfo.DealTime.ToString());
            if (allinone_alarminfo.DealOperator != null)
                dict.Add("@DealOperator", allinone_alarminfo.DealOperator.ToString());
            if (allinone_alarminfo.SmsTimes != null)
                dict.Add("@SmsTimes", allinone_alarminfo.SmsTimes.ToString());
            if (allinone_alarminfo.SmsSendTimes != null)
                dict.Add("@SmsSendTimes", allinone_alarminfo.SmsSendTimes.ToString());
            if (allinone_alarminfo.SmsInvTime != null)
                dict.Add("@SmsInvTime", allinone_alarminfo.SmsInvTime.ToString());
            if (allinone_alarminfo.Linkman != null)
                dict.Add("@Linkman", allinone_alarminfo.Linkman.ToString());
            if (allinone_alarminfo.MobileNo != null)
                dict.Add("@MobileNo", allinone_alarminfo.MobileNo.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_alarminfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_AlarmInfo allinone_alarminfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (allinone_alarminfo.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (allinone_alarminfo.RtuNo != null)
            {
                part1.Append("RtuNo,");
                part2.Append("@RtuNo,");
            }
            if (allinone_alarminfo.Devid != null)
            {
                part1.Append("Devid,");
                part2.Append("@Devid,");
            }
            if (allinone_alarminfo.DevType != null)
            {
                part1.Append("DevType,");
                part2.Append("@DevType,");
            }
            if (allinone_alarminfo.DevTypeName != null)
            {
                part1.Append("DevTypeName,");
                part2.Append("@DevTypeName,");
            }
            if (allinone_alarminfo.AlarmContent != null)
            {
                part1.Append("AlarmContent,");
                part2.Append("@AlarmContent,");
            }
            if (allinone_alarminfo.AlarmTime != null)
            {
                part1.Append("AlarmTime,");
                part2.Append("@AlarmTime,");
            }
            if (allinone_alarminfo.DealFlag != null)
            {
                part1.Append("DealFlag,");
                part2.Append("@DealFlag,");
            }
            if (allinone_alarminfo.DealTime != null)
            {
                part1.Append("DealTime,");
                part2.Append("@DealTime,");
            }
            if (allinone_alarminfo.DealOperator != null)
            {
                part1.Append("DealOperator,");
                part2.Append("@DealOperator,");
            }
            if (allinone_alarminfo.SmsTimes != null)
            {
                part1.Append("SmsTimes,");
                part2.Append("@SmsTimes,");
            }
            if (allinone_alarminfo.SmsSendTimes != null)
            {
                part1.Append("SmsSendTimes,");
                part2.Append("@SmsSendTimes,");
            }
            if (allinone_alarminfo.SmsInvTime != null)
            {
                part1.Append("SmsInvTime,");
                part2.Append("@SmsInvTime,");
            }
            if (allinone_alarminfo.Linkman != null)
            {
                part1.Append("Linkman,");
                part2.Append("@Linkman,");
            }
            if (allinone_alarminfo.MobileNo != null)
            {
                part1.Append("MobileNo,");
                part2.Append("@MobileNo,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_alarminfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_alarminfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_AlarmInfo allinone_alarminfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_alarminfo set ");
            if (allinone_alarminfo.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (allinone_alarminfo.RtuNo != null)
                part1.Append("RtuNo = @RtuNo,");
            if (allinone_alarminfo.Devid != null)
                part1.Append("Devid = @Devid,");
            if (allinone_alarminfo.DevType != null)
                part1.Append("DevType = @DevType,");
            if (allinone_alarminfo.DevTypeName != null)
                part1.Append("DevTypeName = @DevTypeName,");
            if (allinone_alarminfo.AlarmContent != null)
                part1.Append("AlarmContent = @AlarmContent,");
            if (allinone_alarminfo.AlarmTime != null)
                part1.Append("AlarmTime = @AlarmTime,");
            if (allinone_alarminfo.DealFlag != null)
                part1.Append("DealFlag = @DealFlag,");
            if (allinone_alarminfo.DealTime != null)
                part1.Append("DealTime = @DealTime,");
            if (allinone_alarminfo.DealOperator != null)
                part1.Append("DealOperator = @DealOperator,");
            if (allinone_alarminfo.SmsTimes != null)
                part1.Append("SmsTimes = @SmsTimes,");
            if (allinone_alarminfo.SmsSendTimes != null)
                part1.Append("SmsSendTimes = @SmsSendTimes,");
            if (allinone_alarminfo.SmsInvTime != null)
                part1.Append("SmsInvTime = @SmsInvTime,");
            if (allinone_alarminfo.Linkman != null)
                part1.Append("Linkman = @Linkman,");
            if (allinone_alarminfo.MobileNo != null)
                part1.Append("MobileNo = @MobileNo,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_AlarmInfo"></param>
        /// <returns></returns>
        public int Add(AllInOne_AlarmInfo model)
        {
            var str = GetInsertStr(model) + " select @@identity";
            var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_AlarmInfo"></param>
        /// <returns></returns>
        public void Update(AllInOne_AlarmInfo model)
        {
            var str = GetUpdateStr(model);
            var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict);
        }
    }
}
