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
    public partial class AllInOne_Device_AreaOper : SingleTon<AllInOne_Device_AreaOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_device_area"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_Device_Area allinone_device_area)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_device_area.id != null)
                dict.Add("@id", allinone_device_area.id.ToString());
            if (allinone_device_area.deviceId != null)
                dict.Add("@deviceId", allinone_device_area.deviceId.ToString());
            if (allinone_device_area.areaId != null)
                dict.Add("@areaId", allinone_device_area.areaId.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_device_area"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_Device_Area allinone_device_area)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_device_area.deviceId != null)
            {
                part1.Append("deviceId,");
                part2.Append("@deviceId,");
            }
            if (allinone_device_area.areaId != null)
            {
                part1.Append("areaId,");
                part2.Append("@areaId,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_device_area(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_device_area"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_Device_Area allinone_device_area)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_device_area set ");
            if (allinone_device_area.deviceId != null)
                part1.Append("deviceId = @deviceId,");
            if (allinone_device_area.areaId != null)
                part1.Append("areaId = @areaId,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_Device_Area"></param>
        /// <returns></returns>
        public int Add(AllInOne_Device_Area model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_Device_Area"></param>
        /// <returns></returns>
        public void Update(AllInOne_Device_Area model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
