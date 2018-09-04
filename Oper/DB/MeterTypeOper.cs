using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class MeterTypeOper : SingleTon<MeterTypeOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="metertype"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(MeterType metertype)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (metertype.meterTypeNo != null)
                dict.Add("@meterTypeNo", metertype.meterTypeNo.ToString());
            if (metertype.meterTypeName != null)
                dict.Add("@meterTypeName", metertype.meterTypeName.ToString());
            if (metertype.MarkCode != null)
                dict.Add("@MarkCode", metertype.MarkCode.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="metertype"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(MeterType metertype)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (metertype.meterTypeName != null)
            {
                part1.Append("meterTypeName,");
                part2.Append("@meterTypeName,");
            }
            if (metertype.MarkCode != null)
            {
                part1.Append("MarkCode,");
                part2.Append("@MarkCode,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into metertype(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="metertype"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(MeterType metertype)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update metertype set ");
            if (metertype.meterTypeName != null)
                part1.Append("meterTypeName = @meterTypeName,");
            if (metertype.MarkCode != null)
                part1.Append("MarkCode = @MarkCode,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where meterTypeNo= @meterTypeNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="MeterType"></param>
        /// <returns></returns>
        public int Add(MeterType model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="MeterType"></param>
        /// <returns></returns>
        public void Update(MeterType model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
