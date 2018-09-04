using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_RTUOnLineOper : SingleTon<AllInOne_RTUOnLineOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_rtuonline"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_RTUOnLine allinone_rtuonline)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_rtuonline.Id != null)
                dict.Add("@Id", allinone_rtuonline.Id.ToString());
            if (allinone_rtuonline.siteNo != null)
                dict.Add("@siteNo", allinone_rtuonline.siteNo.ToString());
            if (allinone_rtuonline.RTUNo != null)
                dict.Add("@RTUNo", allinone_rtuonline.RTUNo.ToString());
            if (allinone_rtuonline.CommNo != null)
                dict.Add("@CommNo", allinone_rtuonline.CommNo.ToString());
            if (allinone_rtuonline.LoginTime != null)
                dict.Add("@LoginTime", allinone_rtuonline.LoginTime.ToString());
            if (allinone_rtuonline.LogoutTime != null)
                dict.Add("@LogoutTime", allinone_rtuonline.LogoutTime.ToString());
            if (allinone_rtuonline.LoginState != null)
                dict.Add("@LoginState", allinone_rtuonline.LoginState.ToString());
            if (allinone_rtuonline.LoginStateMsg != null)
                dict.Add("@LoginStateMsg", allinone_rtuonline.LoginStateMsg.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_rtuonline"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_RTUOnLine allinone_rtuonline)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_rtuonline.siteNo != null)
            {
                part1.Append("siteNo,");
                part2.Append("@siteNo,");
            }
            if (allinone_rtuonline.RTUNo != null)
            {
                part1.Append("RTUNo,");
                part2.Append("@RTUNo,");
            }
            if (allinone_rtuonline.CommNo != null)
            {
                part1.Append("CommNo,");
                part2.Append("@CommNo,");
            }
            if (allinone_rtuonline.LoginTime != null)
            {
                part1.Append("LoginTime,");
                part2.Append("@LoginTime,");
            }
            if (allinone_rtuonline.LogoutTime != null)
            {
                part1.Append("LogoutTime,");
                part2.Append("@LogoutTime,");
            }
            if (allinone_rtuonline.LoginState != null)
            {
                part1.Append("LoginState,");
                part2.Append("@LoginState,");
            }
            if (allinone_rtuonline.LoginStateMsg != null)
            {
                part1.Append("LoginStateMsg,");
                part2.Append("@LoginStateMsg,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_rtuonline(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_rtuonline"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_RTUOnLine allinone_rtuonline)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_rtuonline set ");
            if (allinone_rtuonline.siteNo != null)
                part1.Append("siteNo = @siteNo,");
            if (allinone_rtuonline.RTUNo != null)
                part1.Append("RTUNo = @RTUNo,");
            if (allinone_rtuonline.CommNo != null)
                part1.Append("CommNo = @CommNo,");
            if (allinone_rtuonline.LoginTime != null)
                part1.Append("LoginTime = @LoginTime,");
            if (allinone_rtuonline.LogoutTime != null)
                part1.Append("LogoutTime = @LogoutTime,");
            if (allinone_rtuonline.LoginState != null)
                part1.Append("LoginState = @LoginState,");
            if (allinone_rtuonline.LoginStateMsg != null)
                part1.Append("LoginStateMsg = @LoginStateMsg,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_RTUOnLine"></param>
        /// <returns></returns>
        public int Add(AllInOne_RTUOnLine model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_RTUOnLine"></param>
        /// <returns></returns>
        public void Update(AllInOne_RTUOnLine model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
