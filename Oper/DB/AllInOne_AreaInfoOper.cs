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
    public partial class AllInOne_AreaInfoOper : SingleTon<AllInOne_AreaInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_areainfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_AreaInfo allinone_areainfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_areainfo.id != null)
                dict.Add("@id", allinone_areainfo.id.ToString());
            if (allinone_areainfo.name != null)
                dict.Add("@name", allinone_areainfo.name.ToString());
            if (allinone_areainfo.isDeleted != null)
                dict.Add("@isDeleted", allinone_areainfo.isDeleted.ToString());
            if (allinone_areainfo.lat != null)
                dict.Add("@lat", allinone_areainfo.lat.ToString());
            if (allinone_areainfo.lng != null)
                dict.Add("@lng", allinone_areainfo.lng.ToString());
            if (allinone_areainfo.mapAddress != null)
                dict.Add("@mapAddress", allinone_areainfo.mapAddress.ToString());
            if (allinone_areainfo.createUserId != null)
                dict.Add("@createUserId", allinone_areainfo.createUserId.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_areainfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_AreaInfo allinone_areainfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_areainfo.name != null)
            {
                part1.Append("name,");
                part2.Append("@name,");
            }
            if (allinone_areainfo.isDeleted != null)
            {
                part1.Append("isDeleted,");
                part2.Append("@isDeleted,");
            }
            if (allinone_areainfo.lat != null)
            {
                part1.Append("lat,");
                part2.Append("@lat,");
            }
            if (allinone_areainfo.lng != null)
            {
                part1.Append("lng,");
                part2.Append("@lng,");
            }
            if (allinone_areainfo.mapAddress != null)
            {
                part1.Append("mapAddress,");
                part2.Append("@mapAddress,");
            }
            if (allinone_areainfo.createUserId != null)
            {
                part1.Append("createUserId,");
                part2.Append("@createUserId,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_areainfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_areainfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_AreaInfo allinone_areainfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_areainfo set ");
            if (allinone_areainfo.name != null)
                part1.Append("name = @name,");
            if (allinone_areainfo.isDeleted != null)
                part1.Append("isDeleted = @isDeleted,");
            if (allinone_areainfo.lat != null)
                part1.Append("lat = @lat,");
            if (allinone_areainfo.lng != null)
                part1.Append("lng = @lng,");
            if (allinone_areainfo.mapAddress != null)
                part1.Append("mapAddress = @mapAddress,");
            if (allinone_areainfo.createUserId != null)
                part1.Append("createUserId = @createUserId,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_AreaInfo"></param>
        /// <returns></returns>
        public int Add(AllInOne_AreaInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_AreaInfo"></param>
        /// <returns></returns>
        public void Update(AllInOne_AreaInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
