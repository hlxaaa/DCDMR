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

        public string GetInsertStr2(FMReportDay fmreportday)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (fmreportday.FlMeterNo != null)
            {
                part1.Append("FlMeterNo,");
                part2.Append($"{fmreportday.FlMeterNo},");
            }
            if (fmreportday.year != null)
            {
                part1.Append("year,");
                part2.Append($"{fmreportday.year},");
            }
            if (fmreportday.month != null)
            {
                part1.Append("month,");
                part2.Append($"{ fmreportday.month},");
            }
            if (fmreportday.day != null)
            {
                part1.Append("day,");
                part2.Append($"{fmreportday.day},");
            }
            if (fmreportday.hour != null)
            {
                part1.Append("hour,");
                part2.Append($"{fmreportday.hour},");
            }
            if (fmreportday.stdsum != null)
            {
                part1.Append("stdsum,");
                part2.Append($"{fmreportday.stdsum},");
            }
            if (fmreportday.custTypeName != null)
            {
                part1.Append("custTypeName,");
                part2.Append($"'{fmreportday.custTypeName}',");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into fmreportday(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="FMReportDay"></param>
        /// <returns></returns>
        public int Add2(FMReportDay model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr2(model)+" select @@identity";
            var dict = new Dictionary<string, string>();
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar2(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="FMReportDay"></param>
        /// <returns></returns>
        public void Update2(FMReportDay model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon2(str, dict, connection, transaction);
        }
    }
}
