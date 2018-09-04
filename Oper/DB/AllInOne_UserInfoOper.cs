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
    public partial class AllInOne_UserInfoOper : SingleTon<AllInOne_UserInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_userinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_UserInfo allinone_userinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_userinfo.id != null)
                dict.Add("@id", allinone_userinfo.id.ToString());
            if (allinone_userinfo.name != null)
                dict.Add("@name", allinone_userinfo.name.ToString());
            if (allinone_userinfo.account != null)
                dict.Add("@account", allinone_userinfo.account.ToString());
            if (allinone_userinfo.pwd != null)
                dict.Add("@pwd", allinone_userinfo.pwd.ToString());
            if (allinone_userinfo.level != null)
                dict.Add("@level", allinone_userinfo.level.ToString());
            if (allinone_userinfo.jurisdiction != null)
                dict.Add("@jurisdiction", allinone_userinfo.jurisdiction.ToString());
            if (allinone_userinfo.isDeleted != null)
                dict.Add("@isDeleted", allinone_userinfo.isDeleted.ToString());
            if (allinone_userinfo.parentId != null)
                dict.Add("@parentId", allinone_userinfo.parentId.ToString());
            if (allinone_userinfo.isStaff != null)
                dict.Add("@isStaff", allinone_userinfo.isStaff.ToString());
            if (allinone_userinfo.areaId != null)
                dict.Add("@areaId", allinone_userinfo.areaId.ToString());
            if (allinone_userinfo.cId1 != null)
                dict.Add("@cId1", allinone_userinfo.cId1.ToString());
            if (allinone_userinfo.cId2 != null)
                dict.Add("@cId2", allinone_userinfo.cId2.ToString());
            if (allinone_userinfo.cId3 != null)
                dict.Add("@cId3", allinone_userinfo.cId3.ToString());
            if (allinone_userinfo.cId4 != null)
                dict.Add("@cId4", allinone_userinfo.cId4.ToString());
            if (allinone_userinfo.phone != null)
                dict.Add("@phone", allinone_userinfo.phone.ToString());
            if (allinone_userinfo.openIds != null)
                dict.Add("@openIds", allinone_userinfo.openIds.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_userinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_UserInfo allinone_userinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_userinfo.name != null)
            {
                part1.Append("name,");
                part2.Append("@name,");
            }
            if (allinone_userinfo.account != null)
            {
                part1.Append("account,");
                part2.Append("@account,");
            }
            if (allinone_userinfo.pwd != null)
            {
                part1.Append("pwd,");
                part2.Append("@pwd,");
            }
            if (allinone_userinfo.level != null)
            {
                part1.Append("level,");
                part2.Append("@level,");
            }
            if (allinone_userinfo.jurisdiction != null)
            {
                part1.Append("jurisdiction,");
                part2.Append("@jurisdiction,");
            }
            if (allinone_userinfo.isDeleted != null)
            {
                part1.Append("isDeleted,");
                part2.Append("@isDeleted,");
            }
            if (allinone_userinfo.parentId != null)
            {
                part1.Append("parentId,");
                part2.Append("@parentId,");
            }
            if (allinone_userinfo.isStaff != null)
            {
                part1.Append("isStaff,");
                part2.Append("@isStaff,");
            }
            if (allinone_userinfo.areaId != null)
            {
                part1.Append("areaId,");
                part2.Append("@areaId,");
            }
            if (allinone_userinfo.cId1 != null)
            {
                part1.Append("cId1,");
                part2.Append("@cId1,");
            }
            if (allinone_userinfo.cId2 != null)
            {
                part1.Append("cId2,");
                part2.Append("@cId2,");
            }
            if (allinone_userinfo.cId3 != null)
            {
                part1.Append("cId3,");
                part2.Append("@cId3,");
            }
            if (allinone_userinfo.cId4 != null)
            {
                part1.Append("cId4,");
                part2.Append("@cId4,");
            }
            if (allinone_userinfo.phone != null)
            {
                part1.Append("phone,");
                part2.Append("@phone,");
            }
            if (allinone_userinfo.openIds != null)
            {
                part1.Append("openIds,");
                part2.Append("@openIds,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_userinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_userinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_UserInfo allinone_userinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_userinfo set ");
            if (allinone_userinfo.name != null)
                part1.Append("name = @name,");
            if (allinone_userinfo.account != null)
                part1.Append("account = @account,");
            if (allinone_userinfo.pwd != null)
                part1.Append("pwd = @pwd,");
            if (allinone_userinfo.level != null)
                part1.Append("level = @level,");
            if (allinone_userinfo.jurisdiction != null)
                part1.Append("jurisdiction = @jurisdiction,");
            if (allinone_userinfo.isDeleted != null)
                part1.Append("isDeleted = @isDeleted,");
            if (allinone_userinfo.parentId != null)
                part1.Append("parentId = @parentId,");
            if (allinone_userinfo.isStaff != null)
                part1.Append("isStaff = @isStaff,");
            if (allinone_userinfo.areaId != null)
                part1.Append("areaId = @areaId,");
            if (allinone_userinfo.cId1 != null)
                part1.Append("cId1 = @cId1,");
            if (allinone_userinfo.cId2 != null)
                part1.Append("cId2 = @cId2,");
            if (allinone_userinfo.cId3 != null)
                part1.Append("cId3 = @cId3,");
            if (allinone_userinfo.cId4 != null)
                part1.Append("cId4 = @cId4,");
            if (allinone_userinfo.phone != null)
                part1.Append("phone = @phone,");
            if (allinone_userinfo.openIds != null)
                part1.Append("openIds = @openIds,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_UserInfo"></param>
        /// <returns></returns>
        public int Add(AllInOne_UserInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_UserInfo"></param>
        /// <returns></returns>
        public void Update(AllInOne_UserInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
