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
    public partial class OpenAccountRecordOper : SingleTon<OpenAccountRecordOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="openaccountrecord"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(OpenAccountRecord openaccountrecord)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (openaccountrecord.Id != null)
                dict.Add("@Id", openaccountrecord.Id.ToString());
            if (openaccountrecord.customerNo != null)
                dict.Add("@customerNo", openaccountrecord.customerNo.ToString());
            if (openaccountrecord.customerType != null)
                dict.Add("@customerType", openaccountrecord.customerType.ToString());
            if (openaccountrecord.customerName != null)
                dict.Add("@customerName", openaccountrecord.customerName.ToString());
            if (openaccountrecord.estateNo != null)
                dict.Add("@estateNo", openaccountrecord.estateNo.ToString());
            if (openaccountrecord.address != null)
                dict.Add("@address", openaccountrecord.address.ToString());
            if (openaccountrecord.meterNo != null)
                dict.Add("@meterNo", openaccountrecord.meterNo.ToString());
            if (openaccountrecord.meterTypeNo != null)
                dict.Add("@meterTypeNo", openaccountrecord.meterTypeNo.ToString());
            if (openaccountrecord.factoryNo != null)
                dict.Add("@factoryNo", openaccountrecord.factoryNo.ToString());
            if (openaccountrecord.OpenType != null)
                dict.Add("@OpenType", openaccountrecord.OpenType.ToString());
            if (openaccountrecord.Opentime != null)
                dict.Add("@Opentime", openaccountrecord.Opentime.ToString());
            if (openaccountrecord.Operator != null)
                dict.Add("@Operator", openaccountrecord.Operator.ToString());
            if (openaccountrecord.BranchNo != null)
                dict.Add("@BranchNo", openaccountrecord.BranchNo.ToString());
            if (openaccountrecord.PosNo != null)
                dict.Add("@PosNo", openaccountrecord.PosNo.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="openaccountrecord"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(OpenAccountRecord openaccountrecord)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (openaccountrecord.customerNo != null)
            {
                part1.Append("customerNo,");
                part2.Append("@customerNo,");
            }
            if (openaccountrecord.customerType != null)
            {
                part1.Append("customerType,");
                part2.Append("@customerType,");
            }
            if (openaccountrecord.customerName != null)
            {
                part1.Append("customerName,");
                part2.Append("@customerName,");
            }
            if (openaccountrecord.estateNo != null)
            {
                part1.Append("estateNo,");
                part2.Append("@estateNo,");
            }
            if (openaccountrecord.address != null)
            {
                part1.Append("address,");
                part2.Append("@address,");
            }
            if (openaccountrecord.meterNo != null)
            {
                part1.Append("meterNo,");
                part2.Append("@meterNo,");
            }
            if (openaccountrecord.meterTypeNo != null)
            {
                part1.Append("meterTypeNo,");
                part2.Append("@meterTypeNo,");
            }
            if (openaccountrecord.factoryNo != null)
            {
                part1.Append("factoryNo,");
                part2.Append("@factoryNo,");
            }
            if (openaccountrecord.OpenType != null)
            {
                part1.Append("OpenType,");
                part2.Append("@OpenType,");
            }
            if (openaccountrecord.Opentime != null)
            {
                part1.Append("Opentime,");
                part2.Append("@Opentime,");
            }
            if (openaccountrecord.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            if (openaccountrecord.BranchNo != null)
            {
                part1.Append("BranchNo,");
                part2.Append("@BranchNo,");
            }
            if (openaccountrecord.PosNo != null)
            {
                part1.Append("PosNo,");
                part2.Append("@PosNo,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into openaccountrecord(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="openaccountrecord"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(OpenAccountRecord openaccountrecord)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update openaccountrecord set ");
            if (openaccountrecord.customerNo != null)
                part1.Append("customerNo = @customerNo,");
            if (openaccountrecord.customerType != null)
                part1.Append("customerType = @customerType,");
            if (openaccountrecord.customerName != null)
                part1.Append("customerName = @customerName,");
            if (openaccountrecord.estateNo != null)
                part1.Append("estateNo = @estateNo,");
            if (openaccountrecord.address != null)
                part1.Append("address = @address,");
            if (openaccountrecord.meterNo != null)
                part1.Append("meterNo = @meterNo,");
            if (openaccountrecord.meterTypeNo != null)
                part1.Append("meterTypeNo = @meterTypeNo,");
            if (openaccountrecord.factoryNo != null)
                part1.Append("factoryNo = @factoryNo,");
            if (openaccountrecord.OpenType != null)
                part1.Append("OpenType = @OpenType,");
            if (openaccountrecord.Opentime != null)
                part1.Append("Opentime = @Opentime,");
            if (openaccountrecord.Operator != null)
                part1.Append("Operator = @Operator,");
            if (openaccountrecord.BranchNo != null)
                part1.Append("BranchNo = @BranchNo,");
            if (openaccountrecord.PosNo != null)
                part1.Append("PosNo = @PosNo,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="OpenAccountRecord"></param>
        /// <returns></returns>
        public int Add(OpenAccountRecord model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model) + " select @@identity";
            var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="OpenAccountRecord"></param>
        /// <returns></returns>
        public void Update(OpenAccountRecord model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
            var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
