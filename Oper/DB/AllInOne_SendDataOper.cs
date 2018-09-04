using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_SendDataOper : SingleTon<AllInOne_SendDataOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_senddata"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_SendData allinone_senddata)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_senddata.Id != null)
                dict.Add("@Id", allinone_senddata.Id.ToString());
            if (allinone_senddata.CommNo != null)
                dict.Add("@CommNo", allinone_senddata.CommNo.ToString());
            if (allinone_senddata.DataToSend != null)
                dict.Add("@DataToSend", allinone_senddata.DataToSend.ToString());
            if (allinone_senddata.DealFlag != null)
                dict.Add("@DealFlag", allinone_senddata.DealFlag.ToString());
            if (allinone_senddata.DealMessage != null)
                dict.Add("@DealMessage", allinone_senddata.DealMessage.ToString());
            if (allinone_senddata.SendTime != null)
                dict.Add("@SendTime", allinone_senddata.SendTime.ToString());
            if (allinone_senddata.Timeout != null)
                dict.Add("@Timeout", allinone_senddata.Timeout.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_senddata"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_SendData allinone_senddata)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_senddata.CommNo != null)
            {
                part1.Append("CommNo,");
                part2.Append("@CommNo,");
            }
            if (allinone_senddata.DataToSend != null)
            {
                part1.Append("DataToSend,");
                part2.Append("@DataToSend,");
            }
            if (allinone_senddata.DealFlag != null)
            {
                part1.Append("DealFlag,");
                part2.Append("@DealFlag,");
            }
            if (allinone_senddata.DealMessage != null)
            {
                part1.Append("DealMessage,");
                part2.Append("@DealMessage,");
            }
            if (allinone_senddata.SendTime != null)
            {
                part1.Append("SendTime,");
                part2.Append("@SendTime,");
            }
            if (allinone_senddata.Timeout != null)
            {
                part1.Append("Timeout,");
                part2.Append("@Timeout,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_senddata(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_senddata"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_SendData allinone_senddata)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_senddata set ");
            if (allinone_senddata.CommNo != null)
                part1.Append("CommNo = @CommNo,");
            if (allinone_senddata.DataToSend != null)
                part1.Append("DataToSend = @DataToSend,");
            if (allinone_senddata.DealFlag != null)
                part1.Append("DealFlag = @DealFlag,");
            if (allinone_senddata.DealMessage != null)
                part1.Append("DealMessage = @DealMessage,");
            if (allinone_senddata.SendTime != null)
                part1.Append("SendTime = @SendTime,");
            if (allinone_senddata.Timeout != null)
                part1.Append("Timeout = @Timeout,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_SendData"></param>
        /// <returns></returns>
        public int Add(AllInOne_SendData model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_SendData"></param>
        /// <returns></returns>
        public void Update(AllInOne_SendData model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
