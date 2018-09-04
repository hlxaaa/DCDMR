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
    public partial class AllInOne_Customer_UserOper : SingleTon<AllInOne_Customer_UserOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_customer_user"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_Customer_User allinone_customer_user)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_customer_user.id != null)
                dict.Add("@id", allinone_customer_user.id.ToString());
            if (allinone_customer_user.customerId != null)
                dict.Add("@customerId", allinone_customer_user.customerId.ToString());
            if (allinone_customer_user.userId != null)
                dict.Add("@userId", allinone_customer_user.userId.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_customer_user"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_Customer_User allinone_customer_user)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_customer_user.customerId != null)
            {
                part1.Append("customerId,");
                part2.Append("@customerId,");
            }
            if (allinone_customer_user.userId != null)
            {
                part1.Append("userId,");
                part2.Append("@userId,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_customer_user(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_customer_user"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_Customer_User allinone_customer_user)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_customer_user set ");
            if (allinone_customer_user.customerId != null)
                part1.Append("customerId = @customerId,");
            if (allinone_customer_user.userId != null)
                part1.Append("userId = @userId,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_Customer_User"></param>
        /// <returns></returns>
        public int Add(AllInOne_Customer_User model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_Customer_User"></param>
        /// <returns></returns>
        public void Update(AllInOne_Customer_User model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
