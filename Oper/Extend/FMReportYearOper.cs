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
    public partial class FMReportYearOper : SingleTon<FMReportYearOper>
    {
        public string GetInsertStr2(FMReportYear fmreportyear)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (fmreportyear.FlMeterNo != null)
            {
                part1.Append("FlMeterNo,");
                part2.Append($"{fmreportyear.FlMeterNo},");
            }
            if (fmreportyear.year != null)
            {
                part1.Append("year,");
                part2.Append($"{fmreportyear.year},");
            }
            if (fmreportyear.month != null)
            {
                part1.Append("month,");
                part2.Append($"{ fmreportyear.month},");
            }

            if (fmreportyear.stdsum != null)
            {
                part1.Append("stdsum,");
                part2.Append($"{fmreportyear.stdsum},");
            }
            if (fmreportyear.custTypeName != null)
            {
                part1.Append("custTypeName,");
                part2.Append($"'{fmreportyear.custTypeName}',");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into fmreportyear(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="FMReportYear"></param>
        /// <returns></returns>
        public int Add2(FMReportYear model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr2(model) + " select @@identity";
            var dict = new Dictionary<string, string>();
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar2(str, dict, connection, transaction));
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="FMReportYear"></param>
        /// <returns></returns>
        public void Update2(FMReportYear model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
            var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon2(str, dict, connection, transaction);
        }

    }
}
