using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_UserPermissionOper : SingleTon<AllInOne_UserPermissionOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_userpermission"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_UserPermission allinone_userpermission)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_userpermission.id != null)
                dict.Add("@id", allinone_userpermission.id.ToString());
            if (allinone_userpermission.userId != null)
                dict.Add("@userId", allinone_userpermission.userId.ToString());
            if (allinone_userpermission.perId != null)
                dict.Add("@perId", allinone_userpermission.perId.ToString());
            if (allinone_userpermission.isOpen != null)
                dict.Add("@isOpen", allinone_userpermission.isOpen.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_userpermission"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_UserPermission allinone_userpermission)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_userpermission.userId != null)
            {
                part1.Append("userId,");
                part2.Append("@userId,");
            }
            if (allinone_userpermission.perId != null)
            {
                part1.Append("perId,");
                part2.Append("@perId,");
            }
            if (allinone_userpermission.isOpen != null)
            {
                part1.Append("isOpen,");
                part2.Append("@isOpen,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_userpermission(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_userpermission"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_UserPermission allinone_userpermission)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_userpermission set ");
            if (allinone_userpermission.userId != null)
                part1.Append("userId = @userId,");
            if (allinone_userpermission.perId != null)
                part1.Append("perId = @perId,");
            if (allinone_userpermission.isOpen != null)
                part1.Append("isOpen = @isOpen,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_UserPermission"></param>
        /// <returns></returns>
        public int Add(AllInOne_UserPermission model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_UserPermission"></param>
        /// <returns></returns>
        public void Update(AllInOne_UserPermission model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
