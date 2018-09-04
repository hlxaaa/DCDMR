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
    public partial class FMReportDayOper : SingleTon<FMReportDayOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="fmreportday"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(FMReportDay fmreportday)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (fmreportday.id != null)
                dict.Add("@id", fmreportday.id.ToString());
            if (fmreportday.FlMeterNo != null)
                dict.Add("@FlMeterNo", fmreportday.FlMeterNo.ToString());
            if (fmreportday.year != null)
                dict.Add("@year", fmreportday.year.ToString());
            if (fmreportday.month != null)
                dict.Add("@month", fmreportday.month.ToString());
            if (fmreportday.day != null)
                dict.Add("@day", fmreportday.day.ToString());
            if (fmreportday.hour != null)
                dict.Add("@hour", fmreportday.hour.ToString());
            if (fmreportday.stdsum != null)
                dict.Add("@stdsum", fmreportday.stdsum.ToString());
            if (fmreportday.custTypeName != null)
                dict.Add("@custTypeName", fmreportday.custTypeName.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="fmreportday"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(FMReportDay fmreportday)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (fmreportday.FlMeterNo != null)
            {
                part1.Append("FlMeterNo,");
                part2.Append("@FlMeterNo,");
            }
            if (fmreportday.year != null)
            {
                part1.Append("year,");
                part2.Append("@year,");
            }
            if (fmreportday.month != null)
            {
                part1.Append("month,");
                part2.Append("@month,");
            }
            if (fmreportday.day != null)
            {
                part1.Append("day,");
                part2.Append("@day,");
            }
            if (fmreportday.hour != null)
            {
                part1.Append("hour,");
                part2.Append("@hour,");
            }
            if (fmreportday.stdsum != null)
            {
                part1.Append("stdsum,");
                part2.Append("@stdsum,");
            }
            if (fmreportday.custTypeName != null)
            {
                part1.Append("custTypeName,");
                part2.Append("@custTypeName,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into fmreportday(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fmreportday"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(FMReportDay fmreportday)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update fmreportday set ");
            if (fmreportday.FlMeterNo != null)
                part1.Append("FlMeterNo = @FlMeterNo,");
            if (fmreportday.year != null)
                part1.Append("year = @year,");
            if (fmreportday.month != null)
                part1.Append("month = @month,");
            if (fmreportday.day != null)
                part1.Append("day = @day,");
            if (fmreportday.hour != null)
                part1.Append("hour = @hour,");
            if (fmreportday.stdsum != null)
                part1.Append("stdsum = @stdsum,");
            if (fmreportday.custTypeName != null)
                part1.Append("custTypeName = @custTypeName,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="FMReportDay"></param>
        /// <returns></returns>
        public int Add(FMReportDay model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="FMReportDay"></param>
        /// <returns></returns>
        public void Update(FMReportDay model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
