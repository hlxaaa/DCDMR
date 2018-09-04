using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AreaInfoOper : SingleTon<AreaInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="areainfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AreaInfo areainfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (areainfo.areaNo != null)
                dict.Add("@areaNo", areainfo.areaNo.ToString());
            if (areainfo.areaName != null)
                dict.Add("@areaName", areainfo.areaName.ToString());
            if (areainfo.shortName != null)
                dict.Add("@shortName", areainfo.shortName.ToString());
            if (areainfo.remark != null)
                dict.Add("@remark", areainfo.remark.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="areainfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AreaInfo areainfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (areainfo.areaName != null)
            {
                part1.Append("areaName,");
                part2.Append("@areaName,");
            }
            if (areainfo.shortName != null)
            {
                part1.Append("shortName,");
                part2.Append("@shortName,");
            }
            if (areainfo.remark != null)
            {
                part1.Append("remark,");
                part2.Append("@remark,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into areainfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="areainfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AreaInfo areainfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update areainfo set ");
            if (areainfo.areaName != null)
                part1.Append("areaName = @areaName,");
            if (areainfo.shortName != null)
                part1.Append("shortName = @shortName,");
            if (areainfo.remark != null)
                part1.Append("remark = @remark,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where areaNo= @areaNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AreaInfo"></param>
        /// <returns></returns>
        public int Add(AreaInfo model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AreaInfo"></param>
        /// <returns></returns>
        public void Update(AreaInfo model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
