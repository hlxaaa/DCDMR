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
    public partial class FMReportMonthOper : SingleTon<FMReportMonthOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="fmreportmonth"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(FMReportMonth fmreportmonth)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (fmreportmonth.id != null)
                dict.Add("@id", fmreportmonth.id.ToString());
            if (fmreportmonth.FlMeterNo != null)
                dict.Add("@FlMeterNo", fmreportmonth.FlMeterNo.ToString());
            if (fmreportmonth.year != null)
                dict.Add("@year", fmreportmonth.year.ToString());
            if (fmreportmonth.month != null)
                dict.Add("@month", fmreportmonth.month.ToString());
            if (fmreportmonth.day != null)
                dict.Add("@day", fmreportmonth.day.ToString());
            if (fmreportmonth.stdsum != null)
                dict.Add("@stdsum", fmreportmonth.stdsum.ToString());
            if (fmreportmonth.custTypeName != null)
                dict.Add("@custTypeName", fmreportmonth.custTypeName.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="fmreportmonth"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(FMReportMonth fmreportmonth)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (fmreportmonth.FlMeterNo != null)
            {
                part1.Append("FlMeterNo,");
                part2.Append("@FlMeterNo,");
            }
            if (fmreportmonth.year != null)
            {
                part1.Append("year,");
                part2.Append("@year,");
            }
            if (fmreportmonth.month != null)
            {
                part1.Append("month,");
                part2.Append("@month,");
            }
            if (fmreportmonth.day != null)
            {
                part1.Append("day,");
                part2.Append("@day,");
            }
            if (fmreportmonth.stdsum != null)
            {
                part1.Append("stdsum,");
                part2.Append("@stdsum,");
            }
            if (fmreportmonth.custTypeName != null)
            {
                part1.Append("custTypeName,");
                part2.Append("@custTypeName,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into fmreportmonth(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fmreportmonth"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(FMReportMonth fmreportmonth)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update fmreportmonth set ");
            if (fmreportmonth.FlMeterNo != null)
                part1.Append("FlMeterNo = @FlMeterNo,");
            if (fmreportmonth.year != null)
                part1.Append("year = @year,");
            if (fmreportmonth.month != null)
                part1.Append("month = @month,");
            if (fmreportmonth.day != null)
                part1.Append("day = @day,");
            if (fmreportmonth.stdsum != null)
                part1.Append("stdsum = @stdsum,");
            if (fmreportmonth.custTypeName != null)
                part1.Append("custTypeName = @custTypeName,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="FMReportMonth"></param>
        /// <returns></returns>
        public int Add(FMReportMonth model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="FMReportMonth"></param>
        /// <returns></returns>
        public void Update(FMReportMonth model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
