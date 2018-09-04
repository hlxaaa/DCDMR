using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class CheckResultOper : SingleTon<CheckResultOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="checkresult"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(CheckResult checkresult)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (checkresult.ID != null)
                dict.Add("@ID", checkresult.ID.ToString());
            if (checkresult.Explain != null)
                dict.Add("@Explain", checkresult.Explain.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="checkresult"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(CheckResult checkresult)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (checkresult.Explain != null)
            {
                part1.Append("Explain,");
                part2.Append("@Explain,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into checkresult(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="checkresult"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(CheckResult checkresult)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update checkresult set ");
            if (checkresult.Explain != null)
                part1.Append("Explain = @Explain,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where ID= @ID  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="CheckResult"></param>
        /// <returns></returns>
        public int Add(CheckResult model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="CheckResult"></param>
        /// <returns></returns>
        public void Update(CheckResult model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
