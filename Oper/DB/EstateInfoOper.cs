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
    public partial class EstateInfoOper : SingleTon<EstateInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="estateinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(EstateInfo estateinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (estateinfo.estateNo != null)
                dict.Add("@estateNo", estateinfo.estateNo.ToString());
            if (estateinfo.estateName != null)
                dict.Add("@estateName", estateinfo.estateName.ToString());
            if (estateinfo.stationNo != null)
                dict.Add("@stationNo", estateinfo.stationNo.ToString());
            if (estateinfo.areaNo != null)
                dict.Add("@areaNo", estateinfo.areaNo.ToString());
            if (estateinfo.remark != null)
                dict.Add("@remark", estateinfo.remark.ToString());
            if (estateinfo.shortName != null)
                dict.Add("@shortName", estateinfo.shortName.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="estateinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(EstateInfo estateinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (estateinfo.estateName != null)
            {
                part1.Append("estateName,");
                part2.Append("@estateName,");
            }
            if (estateinfo.stationNo != null)
            {
                part1.Append("stationNo,");
                part2.Append("@stationNo,");
            }
            if (estateinfo.areaNo != null)
            {
                part1.Append("areaNo,");
                part2.Append("@areaNo,");
            }
            if (estateinfo.remark != null)
            {
                part1.Append("remark,");
                part2.Append("@remark,");
            }
            if (estateinfo.shortName != null)
            {
                part1.Append("shortName,");
                part2.Append("@shortName,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into estateinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="estateinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(EstateInfo estateinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update estateinfo set ");
            if (estateinfo.estateName != null)
                part1.Append("estateName = @estateName,");
            if (estateinfo.stationNo != null)
                part1.Append("stationNo = @stationNo,");
            if (estateinfo.areaNo != null)
                part1.Append("areaNo = @areaNo,");
            if (estateinfo.remark != null)
                part1.Append("remark = @remark,");
            if (estateinfo.shortName != null)
                part1.Append("shortName = @shortName,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where estateNo= @estateNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="EstateInfo"></param>
        /// <returns></returns>
        public int Add(EstateInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="EstateInfo"></param>
        /// <returns></returns>
        public void Update(EstateInfo model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
