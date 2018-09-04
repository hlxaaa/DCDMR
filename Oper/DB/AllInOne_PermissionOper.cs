using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_PermissionOper : SingleTon<AllInOne_PermissionOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_permission"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_Permission allinone_permission)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_permission.id != null)
                dict.Add("@id", allinone_permission.id.ToString());
            if (allinone_permission.name != null)
                dict.Add("@name", allinone_permission.name.ToString());
            if (allinone_permission.type != null)
                dict.Add("@type", allinone_permission.type.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_permission"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_Permission allinone_permission)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (allinone_permission.name != null)
            {
                part1.Append("name,");
                part2.Append("@name,");
            }
            if (allinone_permission.type != null)
            {
                part1.Append("type,");
                part2.Append("@type,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_permission(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_permission"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_Permission allinone_permission)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_permission set ");
            if (allinone_permission.name != null)
                part1.Append("name = @name,");
            if (allinone_permission.type != null)
                part1.Append("type = @type,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_Permission"></param>
        /// <returns></returns>
        public int Add(AllInOne_Permission model)
        {
            var str = GetInsertStr(model) + " select @@identity";
            var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_Permission"></param>
        /// <returns></returns>
        public void Update(AllInOne_Permission model)
        {
            var str = GetUpdateStr(model);
            var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict);
        }
    }
}
