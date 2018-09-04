using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_DeviceOnLineOper : SingleTon<AllInOne_DeviceOnLineOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_deviceonline"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_DeviceOnLine allinone_deviceonline)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_deviceonline.Id != null)
                dict.Add("@Id", allinone_deviceonline.Id.ToString());
            if (allinone_deviceonline.CommNo != null)
                dict.Add("@CommNo", allinone_deviceonline.CommNo.ToString());
            if (allinone_deviceonline.LoginTime != null)
                dict.Add("@LoginTime", allinone_deviceonline.LoginTime.ToString());
            if (allinone_deviceonline.LogoutTime != null)
                dict.Add("@LogoutTime", allinone_deviceonline.LogoutTime.ToString());
            if (allinone_deviceonline.LoginState != null)
                dict.Add("@LoginState", allinone_deviceonline.LoginState.ToString());
            if (allinone_deviceonline.LoginStateMsg != null)
                dict.Add("@LoginStateMsg", allinone_deviceonline.LoginStateMsg.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_deviceonline"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_DeviceOnLine allinone_deviceonline)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_deviceonline.CommNo != null)
            {
                part1.Append("CommNo,");
                part2.Append("@CommNo,");
            }
            if (allinone_deviceonline.LoginTime != null)
            {
                part1.Append("LoginTime,");
                part2.Append("@LoginTime,");
            }
            if (allinone_deviceonline.LogoutTime != null)
            {
                part1.Append("LogoutTime,");
                part2.Append("@LogoutTime,");
            }
            if (allinone_deviceonline.LoginState != null)
            {
                part1.Append("LoginState,");
                part2.Append("@LoginState,");
            }
            if (allinone_deviceonline.LoginStateMsg != null)
            {
                part1.Append("LoginStateMsg,");
                part2.Append("@LoginStateMsg,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_deviceonline(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_deviceonline"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_DeviceOnLine allinone_deviceonline)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_deviceonline set ");
            if (allinone_deviceonline.CommNo != null)
                part1.Append("CommNo = @CommNo,");
            if (allinone_deviceonline.LoginTime != null)
                part1.Append("LoginTime = @LoginTime,");
            if (allinone_deviceonline.LogoutTime != null)
                part1.Append("LogoutTime = @LogoutTime,");
            if (allinone_deviceonline.LoginState != null)
                part1.Append("LoginState = @LoginState,");
            if (allinone_deviceonline.LoginStateMsg != null)
                part1.Append("LoginStateMsg = @LoginStateMsg,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_DeviceOnLine"></param>
        /// <returns></returns>
        public int Add(AllInOne_DeviceOnLine model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_DeviceOnLine"></param>
        /// <returns></returns>
        public void Update(AllInOne_DeviceOnLine model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
