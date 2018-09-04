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
    public partial class IOT_CopyDataOper : SingleTon<IOT_CopyDataOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="iot_copydata"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(IOT_CopyData iot_copydata)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (iot_copydata.Id != null)
                dict.Add("@Id", iot_copydata.Id.ToString());
            if (iot_copydata.meterNo != null)
                dict.Add("@meterNo", iot_copydata.meterNo.ToString());
            if (iot_copydata.lastShow != null)
                dict.Add("@lastShow", iot_copydata.lastShow.ToString());
            if (iot_copydata.lastDosage != null)
                dict.Add("@lastDosage", iot_copydata.lastDosage.ToString());
            if (iot_copydata.currentShow != null)
                dict.Add("@currentShow", iot_copydata.currentShow.ToString());
            if (iot_copydata.currentDosage != null)
                dict.Add("@currentDosage", iot_copydata.currentDosage.ToString());
            if (iot_copydata.unitPrice != null)
                dict.Add("@unitPrice", iot_copydata.unitPrice.ToString());
            if (iot_copydata.printFlag != null)
                dict.Add("@printFlag", iot_copydata.printFlag.ToString());
            if (iot_copydata.meterState != null)
                dict.Add("@meterState", iot_copydata.meterState.ToString());
            if (iot_copydata.copyWay != null)
                dict.Add("@copyWay", iot_copydata.copyWay.ToString());
            if (iot_copydata.copyState != null)
                dict.Add("@copyState", iot_copydata.copyState.ToString());
            if (iot_copydata.copyTime != null)
                dict.Add("@copyTime", iot_copydata.copyTime.ToString());
            if (iot_copydata.copyMan != null)
                dict.Add("@copyMan", iot_copydata.copyMan.ToString());
            if (iot_copydata.Operator != null)
                dict.Add("@Operator", iot_copydata.Operator.ToString());
            if (iot_copydata.OperateTime != null)
                dict.Add("@OperateTime", iot_copydata.OperateTime.ToString());
            if (iot_copydata.isBalance != null)
                dict.Add("@isBalance", iot_copydata.isBalance.ToString());
            if (iot_copydata.Remark != null)
                dict.Add("@Remark", iot_copydata.Remark.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="iot_copydata"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(IOT_CopyData iot_copydata)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (iot_copydata.meterNo != null)
            {
                part1.Append("meterNo,");
                part2.Append("@meterNo,");
            }
            if (iot_copydata.lastShow != null)
            {
                part1.Append("lastShow,");
                part2.Append("@lastShow,");
            }
            if (iot_copydata.lastDosage != null)
            {
                part1.Append("lastDosage,");
                part2.Append("@lastDosage,");
            }
            if (iot_copydata.currentShow != null)
            {
                part1.Append("currentShow,");
                part2.Append("@currentShow,");
            }
            if (iot_copydata.currentDosage != null)
            {
                part1.Append("currentDosage,");
                part2.Append("@currentDosage,");
            }
            if (iot_copydata.unitPrice != null)
            {
                part1.Append("unitPrice,");
                part2.Append("@unitPrice,");
            }
            if (iot_copydata.printFlag != null)
            {
                part1.Append("printFlag,");
                part2.Append("@printFlag,");
            }
            if (iot_copydata.meterState != null)
            {
                part1.Append("meterState,");
                part2.Append("@meterState,");
            }
            if (iot_copydata.copyWay != null)
            {
                part1.Append("copyWay,");
                part2.Append("@copyWay,");
            }
            if (iot_copydata.copyState != null)
            {
                part1.Append("copyState,");
                part2.Append("@copyState,");
            }
            if (iot_copydata.copyTime != null)
            {
                part1.Append("copyTime,");
                part2.Append("@copyTime,");
            }
            if (iot_copydata.copyMan != null)
            {
                part1.Append("copyMan,");
                part2.Append("@copyMan,");
            }
            if (iot_copydata.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            if (iot_copydata.OperateTime != null)
            {
                part1.Append("OperateTime,");
                part2.Append("@OperateTime,");
            }
            if (iot_copydata.isBalance != null)
            {
                part1.Append("isBalance,");
                part2.Append("@isBalance,");
            }
            if (iot_copydata.Remark != null)
            {
                part1.Append("Remark,");
                part2.Append("@Remark,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into iot_copydata(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="iot_copydata"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(IOT_CopyData iot_copydata)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update iot_copydata set ");
            if (iot_copydata.meterNo != null)
                part1.Append("meterNo = @meterNo,");
            if (iot_copydata.lastShow != null)
                part1.Append("lastShow = @lastShow,");
            if (iot_copydata.lastDosage != null)
                part1.Append("lastDosage = @lastDosage,");
            if (iot_copydata.currentShow != null)
                part1.Append("currentShow = @currentShow,");
            if (iot_copydata.currentDosage != null)
                part1.Append("currentDosage = @currentDosage,");
            if (iot_copydata.unitPrice != null)
                part1.Append("unitPrice = @unitPrice,");
            if (iot_copydata.printFlag != null)
                part1.Append("printFlag = @printFlag,");
            if (iot_copydata.meterState != null)
                part1.Append("meterState = @meterState,");
            if (iot_copydata.copyWay != null)
                part1.Append("copyWay = @copyWay,");
            if (iot_copydata.copyState != null)
                part1.Append("copyState = @copyState,");
            if (iot_copydata.copyTime != null)
                part1.Append("copyTime = @copyTime,");
            if (iot_copydata.copyMan != null)
                part1.Append("copyMan = @copyMan,");
            if (iot_copydata.Operator != null)
                part1.Append("Operator = @Operator,");
            if (iot_copydata.OperateTime != null)
                part1.Append("OperateTime = @OperateTime,");
            if (iot_copydata.isBalance != null)
                part1.Append("isBalance = @isBalance,");
            if (iot_copydata.Remark != null)
                part1.Append("Remark = @Remark,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="IOT_CopyData"></param>
        /// <returns></returns>
        public int Add(IOT_CopyData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="IOT_CopyData"></param>
        /// <returns></returns>
        public void Update(IOT_CopyData model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
