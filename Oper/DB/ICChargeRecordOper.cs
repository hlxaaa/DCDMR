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
    public partial class ICChargeRecordOper : SingleTon<ICChargeRecordOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="icchargerecord"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(ICChargeRecord icchargerecord)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (icchargerecord.Id != null)
                dict.Add("@Id", icchargerecord.Id.ToString());
            if (icchargerecord.chargeTime != null)
                dict.Add("@chargeTime", icchargerecord.chargeTime.ToString());
            if (icchargerecord.customerNo != null)
                dict.Add("@customerNo", icchargerecord.customerNo.ToString());
            if (icchargerecord.meterNo != null)
                dict.Add("@meterNo", icchargerecord.meterNo.ToString());
            if (icchargerecord.meterTypeNo != null)
                dict.Add("@meterTypeNo", icchargerecord.meterTypeNo.ToString());
            if (icchargerecord.factoryNo != null)
                dict.Add("@factoryNo", icchargerecord.factoryNo.ToString());
            if (icchargerecord.fluidNo != null)
                dict.Add("@fluidNo", icchargerecord.fluidNo.ToString());
            if (icchargerecord.Price != null)
                dict.Add("@Price", icchargerecord.Price.ToString());
            if (icchargerecord.chargeVolume != null)
                dict.Add("@chargeVolume", icchargerecord.chargeVolume.ToString());
            if (icchargerecord.chargeMoney != null)
                dict.Add("@chargeMoney", icchargerecord.chargeMoney.ToString());
            if (icchargerecord.totalVolume != null)
                dict.Add("@totalVolume", icchargerecord.totalVolume.ToString());
            if (icchargerecord.totalMoney != null)
                dict.Add("@totalMoney", icchargerecord.totalMoney.ToString());
            if (icchargerecord.chargeTimes != null)
                dict.Add("@chargeTimes", icchargerecord.chargeTimes.ToString());
            if (icchargerecord.chargeType != null)
                dict.Add("@chargeType", icchargerecord.chargeType.ToString());
            if (icchargerecord.chargeBranchNo != null)
                dict.Add("@chargeBranchNo", icchargerecord.chargeBranchNo.ToString());
            if (icchargerecord.chargePosNo != null)
                dict.Add("@chargePosNo", icchargerecord.chargePosNo.ToString());
            if (icchargerecord.chargeOperator != null)
                dict.Add("@chargeOperator", icchargerecord.chargeOperator.ToString());
            if (icchargerecord.ICWriteMark != null)
                dict.Add("@ICWriteMark", icchargerecord.ICWriteMark.ToString());
            if (icchargerecord.ReceiptNo != null)
                dict.Add("@ReceiptNo", icchargerecord.ReceiptNo.ToString());
            if (icchargerecord.chargeCheck != null)
                dict.Add("@chargeCheck", icchargerecord.chargeCheck.ToString());
            if (icchargerecord.cycleVolume != null)
                dict.Add("@cycleVolume", icchargerecord.cycleVolume.ToString());
            if (icchargerecord.cycleMoney != null)
                dict.Add("@cycleMoney", icchargerecord.cycleMoney.ToString());
            if (icchargerecord.payId != null)
                dict.Add("@payId", icchargerecord.payId.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="icchargerecord"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(ICChargeRecord icchargerecord)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (icchargerecord.chargeTime != null)
            {
                part1.Append("chargeTime,");
                part2.Append("@chargeTime,");
            }
            if (icchargerecord.customerNo != null)
            {
                part1.Append("customerNo,");
                part2.Append("@customerNo,");
            }
            if (icchargerecord.meterNo != null)
            {
                part1.Append("meterNo,");
                part2.Append("@meterNo,");
            }
            if (icchargerecord.meterTypeNo != null)
            {
                part1.Append("meterTypeNo,");
                part2.Append("@meterTypeNo,");
            }
            if (icchargerecord.factoryNo != null)
            {
                part1.Append("factoryNo,");
                part2.Append("@factoryNo,");
            }
            if (icchargerecord.fluidNo != null)
            {
                part1.Append("fluidNo,");
                part2.Append("@fluidNo,");
            }
            if (icchargerecord.Price != null)
            {
                part1.Append("Price,");
                part2.Append("@Price,");
            }
            if (icchargerecord.chargeVolume != null)
            {
                part1.Append("chargeVolume,");
                part2.Append("@chargeVolume,");
            }
            if (icchargerecord.chargeMoney != null)
            {
                part1.Append("chargeMoney,");
                part2.Append("@chargeMoney,");
            }
            if (icchargerecord.totalVolume != null)
            {
                part1.Append("totalVolume,");
                part2.Append("@totalVolume,");
            }
            if (icchargerecord.totalMoney != null)
            {
                part1.Append("totalMoney,");
                part2.Append("@totalMoney,");
            }
            if (icchargerecord.chargeTimes != null)
            {
                part1.Append("chargeTimes,");
                part2.Append("@chargeTimes,");
            }
            if (icchargerecord.chargeType != null)
            {
                part1.Append("chargeType,");
                part2.Append("@chargeType,");
            }
            if (icchargerecord.chargeBranchNo != null)
            {
                part1.Append("chargeBranchNo,");
                part2.Append("@chargeBranchNo,");
            }
            if (icchargerecord.chargePosNo != null)
            {
                part1.Append("chargePosNo,");
                part2.Append("@chargePosNo,");
            }
            if (icchargerecord.chargeOperator != null)
            {
                part1.Append("chargeOperator,");
                part2.Append("@chargeOperator,");
            }
            if (icchargerecord.ICWriteMark != null)
            {
                part1.Append("ICWriteMark,");
                part2.Append("@ICWriteMark,");
            }
            if (icchargerecord.ReceiptNo != null)
            {
                part1.Append("ReceiptNo,");
                part2.Append("@ReceiptNo,");
            }
            if (icchargerecord.chargeCheck != null)
            {
                part1.Append("chargeCheck,");
                part2.Append("@chargeCheck,");
            }
            if (icchargerecord.cycleVolume != null)
            {
                part1.Append("cycleVolume,");
                part2.Append("@cycleVolume,");
            }
            if (icchargerecord.cycleMoney != null)
            {
                part1.Append("cycleMoney,");
                part2.Append("@cycleMoney,");
            }
            if (icchargerecord.payId != null)
            {
                part1.Append("payId,");
                part2.Append("@payId,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into icchargerecord(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="icchargerecord"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(ICChargeRecord icchargerecord)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update icchargerecord set ");
            if (icchargerecord.chargeTime != null)
                part1.Append("chargeTime = @chargeTime,");
            if (icchargerecord.customerNo != null)
                part1.Append("customerNo = @customerNo,");
            if (icchargerecord.meterNo != null)
                part1.Append("meterNo = @meterNo,");
            if (icchargerecord.meterTypeNo != null)
                part1.Append("meterTypeNo = @meterTypeNo,");
            if (icchargerecord.factoryNo != null)
                part1.Append("factoryNo = @factoryNo,");
            if (icchargerecord.fluidNo != null)
                part1.Append("fluidNo = @fluidNo,");
            if (icchargerecord.Price != null)
                part1.Append("Price = @Price,");
            if (icchargerecord.chargeVolume != null)
                part1.Append("chargeVolume = @chargeVolume,");
            if (icchargerecord.chargeMoney != null)
                part1.Append("chargeMoney = @chargeMoney,");
            if (icchargerecord.totalVolume != null)
                part1.Append("totalVolume = @totalVolume,");
            if (icchargerecord.totalMoney != null)
                part1.Append("totalMoney = @totalMoney,");
            if (icchargerecord.chargeTimes != null)
                part1.Append("chargeTimes = @chargeTimes,");
            if (icchargerecord.chargeType != null)
                part1.Append("chargeType = @chargeType,");
            if (icchargerecord.chargeBranchNo != null)
                part1.Append("chargeBranchNo = @chargeBranchNo,");
            if (icchargerecord.chargePosNo != null)
                part1.Append("chargePosNo = @chargePosNo,");
            if (icchargerecord.chargeOperator != null)
                part1.Append("chargeOperator = @chargeOperator,");
            if (icchargerecord.ICWriteMark != null)
                part1.Append("ICWriteMark = @ICWriteMark,");
            if (icchargerecord.ReceiptNo != null)
                part1.Append("ReceiptNo = @ReceiptNo,");
            if (icchargerecord.chargeCheck != null)
                part1.Append("chargeCheck = @chargeCheck,");
            if (icchargerecord.cycleVolume != null)
                part1.Append("cycleVolume = @cycleVolume,");
            if (icchargerecord.cycleMoney != null)
                part1.Append("cycleMoney = @cycleMoney,");
            if (icchargerecord.payId != null)
                part1.Append("payId = @payId,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="ICChargeRecord"></param>
        /// <returns></returns>
        public int Add(ICChargeRecord model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="ICChargeRecord"></param>
        /// <returns></returns>
        public void Update(ICChargeRecord model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
