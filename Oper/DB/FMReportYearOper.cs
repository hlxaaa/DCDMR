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
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="fmreportyear"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(FMReportYear fmreportyear)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (fmreportyear.id != null)
                dict.Add("@id", fmreportyear.id.ToString());
            if (fmreportyear.FlMeterNo != null)
                dict.Add("@FlMeterNo", fmreportyear.FlMeterNo.ToString());
            if (fmreportyear.year != null)
                dict.Add("@year", fmreportyear.year.ToString());
            if (fmreportyear.month != null)
                dict.Add("@month", fmreportyear.month.ToString());
            if (fmreportyear.stdsum != null)
                dict.Add("@stdsum", fmreportyear.stdsum.ToString());
            if (fmreportyear.custTypeName != null)
                dict.Add("@custTypeName", fmreportyear.custTypeName.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="fmreportyear"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(FMReportYear fmreportyear)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (fmreportyear.FlMeterNo != null)
            {
                part1.Append("FlMeterNo,");
                part2.Append("@FlMeterNo,");
            }
            if (fmreportyear.year != null)
            {
                part1.Append("year,");
                part2.Append("@year,");
            }
            if (fmreportyear.month != null)
            {
                part1.Append("month,");
                part2.Append("@month,");
            }
            if (fmreportyear.stdsum != null)
            {
                part1.Append("stdsum,");
                part2.Append("@stdsum,");
            }
            if (fmreportyear.custTypeName != null)
            {
                part1.Append("custTypeName,");
                part2.Append("@custTypeName,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into fmreportyear(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fmreportyear"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(FMReportYear fmreportyear)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update fmreportyear set ");
            if (fmreportyear.FlMeterNo != null)
                part1.Append("FlMeterNo = @FlMeterNo,");
            if (fmreportyear.year != null)
                part1.Append("year = @year,");
            if (fmreportyear.month != null)
                part1.Append("month = @month,");
            if (fmreportyear.stdsum != null)
                part1.Append("stdsum = @stdsum,");
            if (fmreportyear.custTypeName != null)
                part1.Append("custTypeName = @custTypeName,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="FMReportYear"></param>
        /// <returns></returns>
        public int Add(FMReportYear model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="FMReportYear"></param>
        /// <returns></returns>
        public void Update(FMReportYear model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
