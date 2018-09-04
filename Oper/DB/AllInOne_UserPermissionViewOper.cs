using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_UserPermissionViewOper : SingleTon<AllInOne_UserPermissionViewOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_userpermissionview"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_UserPermissionView allinone_userpermissionview)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_userpermissionview.id != null)
                dict.Add("@id", allinone_userpermissionview.id.ToString());
            if (allinone_userpermissionview.name != null)
                dict.Add("@name", allinone_userpermissionview.name.ToString());
            if (allinone_userpermissionview.account != null)
                dict.Add("@account", allinone_userpermissionview.account.ToString());
            if (allinone_userpermissionview.pwd != null)
                dict.Add("@pwd", allinone_userpermissionview.pwd.ToString());
            if (allinone_userpermissionview.level != null)
                dict.Add("@level", allinone_userpermissionview.level.ToString());
            if (allinone_userpermissionview.jurisdiction != null)
                dict.Add("@jurisdiction", allinone_userpermissionview.jurisdiction.ToString());
            if (allinone_userpermissionview.isDeleted != null)
                dict.Add("@isDeleted", allinone_userpermissionview.isDeleted.ToString());
            if (allinone_userpermissionview.parentId != null)
                dict.Add("@parentId", allinone_userpermissionview.parentId.ToString());
            if (allinone_userpermissionview.isStaff != null)
                dict.Add("@isStaff", allinone_userpermissionview.isStaff.ToString());
            if (allinone_userpermissionview.areaId != null)
                dict.Add("@areaId", allinone_userpermissionview.areaId.ToString());
            if (allinone_userpermissionview.pername != null)
                dict.Add("@pername", allinone_userpermissionview.pername.ToString());
            if (allinone_userpermissionview.areaName != null)
                dict.Add("@areaName", allinone_userpermissionview.areaName.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_userpermissionview"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_UserPermissionView allinone_userpermissionview)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_userpermissionview.name != null)
            {
                part1.Append("name,");
                part2.Append("@name,");
            }
            if (allinone_userpermissionview.account != null)
            {
                part1.Append("account,");
                part2.Append("@account,");
            }
            if (allinone_userpermissionview.pwd != null)
            {
                part1.Append("pwd,");
                part2.Append("@pwd,");
            }
            if (allinone_userpermissionview.level != null)
            {
                part1.Append("level,");
                part2.Append("@level,");
            }
            if (allinone_userpermissionview.jurisdiction != null)
            {
                part1.Append("jurisdiction,");
                part2.Append("@jurisdiction,");
            }
            if (allinone_userpermissionview.isDeleted != null)
            {
                part1.Append("isDeleted,");
                part2.Append("@isDeleted,");
            }
            if (allinone_userpermissionview.parentId != null)
            {
                part1.Append("parentId,");
                part2.Append("@parentId,");
            }
            if (allinone_userpermissionview.isStaff != null)
            {
                part1.Append("isStaff,");
                part2.Append("@isStaff,");
            }
            if (allinone_userpermissionview.areaId != null)
            {
                part1.Append("areaId,");
                part2.Append("@areaId,");
            }
            if (allinone_userpermissionview.pername != null)
            {
                part1.Append("pername,");
                part2.Append("@pername,");
            }
            if (allinone_userpermissionview.areaName != null)
            {
                part1.Append("areaName,");
                part2.Append("@areaName,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_userpermissionview(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_userpermissionview"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_UserPermissionView allinone_userpermissionview)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_userpermissionview set ");
            if (allinone_userpermissionview.name != null)
                part1.Append("name = @name,");
            if (allinone_userpermissionview.account != null)
                part1.Append("account = @account,");
            if (allinone_userpermissionview.pwd != null)
                part1.Append("pwd = @pwd,");
            if (allinone_userpermissionview.level != null)
                part1.Append("level = @level,");
            if (allinone_userpermissionview.jurisdiction != null)
                part1.Append("jurisdiction = @jurisdiction,");
            if (allinone_userpermissionview.isDeleted != null)
                part1.Append("isDeleted = @isDeleted,");
            if (allinone_userpermissionview.parentId != null)
                part1.Append("parentId = @parentId,");
            if (allinone_userpermissionview.isStaff != null)
                part1.Append("isStaff = @isStaff,");
            if (allinone_userpermissionview.areaId != null)
                part1.Append("areaId = @areaId,");
            if (allinone_userpermissionview.pername != null)
                part1.Append("pername = @pername,");
            if (allinone_userpermissionview.areaName != null)
                part1.Append("areaName = @areaName,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_UserPermissionView"></param>
        /// <returns></returns>
        public int Add(AllInOne_UserPermissionView model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_UserPermissionView"></param>
        /// <returns></returns>
        public void Update(AllInOne_UserPermissionView model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
