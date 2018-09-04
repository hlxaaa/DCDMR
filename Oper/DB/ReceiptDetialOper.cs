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
    public partial class ReceiptDetialOper : SingleTon<ReceiptDetialOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="receiptdetial"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(ReceiptDetial receiptdetial)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (receiptdetial.Id != null)
                dict.Add("@Id", receiptdetial.Id.ToString());
            if (receiptdetial.customerNo != null)
                dict.Add("@customerNo", receiptdetial.customerNo.ToString());
            if (receiptdetial.customerName != null)
                dict.Add("@customerName", receiptdetial.customerName.ToString());
            if (receiptdetial.customerType != null)
                dict.Add("@customerType", receiptdetial.customerType.ToString());
            if (receiptdetial.Estate != null)
                dict.Add("@Estate", receiptdetial.Estate.ToString());
            if (receiptdetial.address != null)
                dict.Add("@address", receiptdetial.address.ToString());
            if (receiptdetial.custDefNo1 != null)
                dict.Add("@custDefNo1", receiptdetial.custDefNo1.ToString());
            if (receiptdetial.custDefNo2 != null)
                dict.Add("@custDefNo2", receiptdetial.custDefNo2.ToString());
            if (receiptdetial.custDefNo3 != null)
                dict.Add("@custDefNo3", receiptdetial.custDefNo3.ToString());
            if (receiptdetial.meterNo != null)
                dict.Add("@meterNo", receiptdetial.meterNo.ToString());
            if (receiptdetial.meterTypeNo != null)
                dict.Add("@meterTypeNo", receiptdetial.meterTypeNo.ToString());
            if (receiptdetial.factoryNo != null)
                dict.Add("@factoryNo", receiptdetial.factoryNo.ToString());
            if (receiptdetial.caliber != null)
                dict.Add("@caliber", receiptdetial.caliber.ToString());
            if (receiptdetial.fluidNo != null)
                dict.Add("@fluidNo", receiptdetial.fluidNo.ToString());
            if (receiptdetial.meterDefNo1 != null)
                dict.Add("@meterDefNo1", receiptdetial.meterDefNo1.ToString());
            if (receiptdetial.meterDefNo2 != null)
                dict.Add("@meterDefNo2", receiptdetial.meterDefNo2.ToString());
            if (receiptdetial.meterDefNo3 != null)
                dict.Add("@meterDefNo3", receiptdetial.meterDefNo3.ToString());
            if (receiptdetial.payContent != null)
                dict.Add("@payContent", receiptdetial.payContent.ToString());
            if (receiptdetial.payWay != null)
                dict.Add("@payWay", receiptdetial.payWay.ToString());
            if (receiptdetial.PayMoney != null)
                dict.Add("@PayMoney", receiptdetial.PayMoney.ToString());
            if (receiptdetial.payLateMoney != null)
                dict.Add("@payLateMoney", receiptdetial.payLateMoney.ToString());
            if (receiptdetial.Price != null)
                dict.Add("@Price", receiptdetial.Price.ToString());
            if (receiptdetial.surplus != null)
                dict.Add("@surplus", receiptdetial.surplus.ToString());
            if (receiptdetial.remark != null)
                dict.Add("@remark", receiptdetial.remark.ToString());
            if (receiptdetial.payOperator != null)
                dict.Add("@payOperator", receiptdetial.payOperator.ToString());
            if (receiptdetial.payTime != null)
                dict.Add("@payTime", receiptdetial.payTime.ToString());
            if (receiptdetial.payBranchNo != null)
                dict.Add("@payBranchNo", receiptdetial.payBranchNo.ToString());
            if (receiptdetial.payPosNo != null)
                dict.Add("@payPosNo", receiptdetial.payPosNo.ToString());
            if (receiptdetial.payType != null)
                dict.Add("@payType", receiptdetial.payType.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="receiptdetial"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(ReceiptDetial receiptdetial)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (receiptdetial.customerNo != null)
            {
                part1.Append("customerNo,");
                part2.Append("@customerNo,");
            }
            if (receiptdetial.customerName != null)
            {
                part1.Append("customerName,");
                part2.Append("@customerName,");
            }
            if (receiptdetial.customerType != null)
            {
                part1.Append("customerType,");
                part2.Append("@customerType,");
            }
            if (receiptdetial.Estate != null)
            {
                part1.Append("Estate,");
                part2.Append("@Estate,");
            }
            if (receiptdetial.address != null)
            {
                part1.Append("address,");
                part2.Append("@address,");
            }
            if (receiptdetial.custDefNo1 != null)
            {
                part1.Append("custDefNo1,");
                part2.Append("@custDefNo1,");
            }
            if (receiptdetial.custDefNo2 != null)
            {
                part1.Append("custDefNo2,");
                part2.Append("@custDefNo2,");
            }
            if (receiptdetial.custDefNo3 != null)
            {
                part1.Append("custDefNo3,");
                part2.Append("@custDefNo3,");
            }
            if (receiptdetial.meterNo != null)
            {
                part1.Append("meterNo,");
                part2.Append("@meterNo,");
            }
            if (receiptdetial.meterTypeNo != null)
            {
                part1.Append("meterTypeNo,");
                part2.Append("@meterTypeNo,");
            }
            if (receiptdetial.factoryNo != null)
            {
                part1.Append("factoryNo,");
                part2.Append("@factoryNo,");
            }
            if (receiptdetial.caliber != null)
            {
                part1.Append("caliber,");
                part2.Append("@caliber,");
            }
            if (receiptdetial.fluidNo != null)
            {
                part1.Append("fluidNo,");
                part2.Append("@fluidNo,");
            }
            if (receiptdetial.meterDefNo1 != null)
            {
                part1.Append("meterDefNo1,");
                part2.Append("@meterDefNo1,");
            }
            if (receiptdetial.meterDefNo2 != null)
            {
                part1.Append("meterDefNo2,");
                part2.Append("@meterDefNo2,");
            }
            if (receiptdetial.meterDefNo3 != null)
            {
                part1.Append("meterDefNo3,");
                part2.Append("@meterDefNo3,");
            }
            if (receiptdetial.payContent != null)
            {
                part1.Append("payContent,");
                part2.Append("@payContent,");
            }
            if (receiptdetial.payWay != null)
            {
                part1.Append("payWay,");
                part2.Append("@payWay,");
            }
            if (receiptdetial.PayMoney != null)
            {
                part1.Append("PayMoney,");
                part2.Append("@PayMoney,");
            }
            if (receiptdetial.payLateMoney != null)
            {
                part1.Append("payLateMoney,");
                part2.Append("@payLateMoney,");
            }
            if (receiptdetial.Price != null)
            {
                part1.Append("Price,");
                part2.Append("@Price,");
            }
            if (receiptdetial.surplus != null)
            {
                part1.Append("surplus,");
                part2.Append("@surplus,");
            }
            if (receiptdetial.remark != null)
            {
                part1.Append("remark,");
                part2.Append("@remark,");
            }
            if (receiptdetial.payOperator != null)
            {
                part1.Append("payOperator,");
                part2.Append("@payOperator,");
            }
            if (receiptdetial.payTime != null)
            {
                part1.Append("payTime,");
                part2.Append("@payTime,");
            }
            if (receiptdetial.payBranchNo != null)
            {
                part1.Append("payBranchNo,");
                part2.Append("@payBranchNo,");
            }
            if (receiptdetial.payPosNo != null)
            {
                part1.Append("payPosNo,");
                part2.Append("@payPosNo,");
            }
            if (receiptdetial.payType != null)
            {
                part1.Append("payType,");
                part2.Append("@payType,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into receiptdetial(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="receiptdetial"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(ReceiptDetial receiptdetial)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update receiptdetial set ");
            if (receiptdetial.customerNo != null)
                part1.Append("customerNo = @customerNo,");
            if (receiptdetial.customerName != null)
                part1.Append("customerName = @customerName,");
            if (receiptdetial.customerType != null)
                part1.Append("customerType = @customerType,");
            if (receiptdetial.Estate != null)
                part1.Append("Estate = @Estate,");
            if (receiptdetial.address != null)
                part1.Append("address = @address,");
            if (receiptdetial.custDefNo1 != null)
                part1.Append("custDefNo1 = @custDefNo1,");
            if (receiptdetial.custDefNo2 != null)
                part1.Append("custDefNo2 = @custDefNo2,");
            if (receiptdetial.custDefNo3 != null)
                part1.Append("custDefNo3 = @custDefNo3,");
            if (receiptdetial.meterNo != null)
                part1.Append("meterNo = @meterNo,");
            if (receiptdetial.meterTypeNo != null)
                part1.Append("meterTypeNo = @meterTypeNo,");
            if (receiptdetial.factoryNo != null)
                part1.Append("factoryNo = @factoryNo,");
            if (receiptdetial.caliber != null)
                part1.Append("caliber = @caliber,");
            if (receiptdetial.fluidNo != null)
                part1.Append("fluidNo = @fluidNo,");
            if (receiptdetial.meterDefNo1 != null)
                part1.Append("meterDefNo1 = @meterDefNo1,");
            if (receiptdetial.meterDefNo2 != null)
                part1.Append("meterDefNo2 = @meterDefNo2,");
            if (receiptdetial.meterDefNo3 != null)
                part1.Append("meterDefNo3 = @meterDefNo3,");
            if (receiptdetial.payContent != null)
                part1.Append("payContent = @payContent,");
            if (receiptdetial.payWay != null)
                part1.Append("payWay = @payWay,");
            if (receiptdetial.PayMoney != null)
                part1.Append("PayMoney = @PayMoney,");
            if (receiptdetial.payLateMoney != null)
                part1.Append("payLateMoney = @payLateMoney,");
            if (receiptdetial.Price != null)
                part1.Append("Price = @Price,");
            if (receiptdetial.surplus != null)
                part1.Append("surplus = @surplus,");
            if (receiptdetial.remark != null)
                part1.Append("remark = @remark,");
            if (receiptdetial.payOperator != null)
                part1.Append("payOperator = @payOperator,");
            if (receiptdetial.payTime != null)
                part1.Append("payTime = @payTime,");
            if (receiptdetial.payBranchNo != null)
                part1.Append("payBranchNo = @payBranchNo,");
            if (receiptdetial.payPosNo != null)
                part1.Append("payPosNo = @payPosNo,");
            if (receiptdetial.payType != null)
                part1.Append("payType = @payType,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where Id= @Id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="ReceiptDetial"></param>
        /// <returns></returns>
        public int Add(ReceiptDetial model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="ReceiptDetial"></param>
        /// <returns></returns>
        public void Update(ReceiptDetial model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
