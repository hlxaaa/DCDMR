using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class ChargeLimitOper : SingleTon<ChargeLimitOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="chargelimit"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(ChargeLimit chargelimit)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (chargelimit.branchNo != null)
                dict.Add("@branchNo", chargelimit.branchNo.ToString());
            if (chargelimit.IsEffect != null)
                dict.Add("@IsEffect", chargelimit.IsEffect.ToString());
            if (chargelimit.SumLimit != null)
                dict.Add("@SumLimit", chargelimit.SumLimit.ToString());
            if (chargelimit.CurrLimit != null)
                dict.Add("@CurrLimit", chargelimit.CurrLimit.ToString());
            if (chargelimit.CurrRemain != null)
                dict.Add("@CurrRemain", chargelimit.CurrRemain.ToString());
            if (chargelimit.SetTime != null)
                dict.Add("@SetTime", chargelimit.SetTime.ToString());
            if (chargelimit.Operator != null)
                dict.Add("@Operator", chargelimit.Operator.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="chargelimit"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(ChargeLimit chargelimit)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (chargelimit.IsEffect != null)
            {
                part1.Append("IsEffect,");
                part2.Append("@IsEffect,");
            }
            if (chargelimit.SumLimit != null)
            {
                part1.Append("SumLimit,");
                part2.Append("@SumLimit,");
            }
            if (chargelimit.CurrLimit != null)
            {
                part1.Append("CurrLimit,");
                part2.Append("@CurrLimit,");
            }
            if (chargelimit.CurrRemain != null)
            {
                part1.Append("CurrRemain,");
                part2.Append("@CurrRemain,");
            }
            if (chargelimit.SetTime != null)
            {
                part1.Append("SetTime,");
                part2.Append("@SetTime,");
            }
            if (chargelimit.Operator != null)
            {
                part1.Append("Operator,");
                part2.Append("@Operator,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into chargelimit(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="chargelimit"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(ChargeLimit chargelimit)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update chargelimit set ");
            if (chargelimit.IsEffect != null)
                part1.Append("IsEffect = @IsEffect,");
            if (chargelimit.SumLimit != null)
                part1.Append("SumLimit = @SumLimit,");
            if (chargelimit.CurrLimit != null)
                part1.Append("CurrLimit = @CurrLimit,");
            if (chargelimit.CurrRemain != null)
                part1.Append("CurrRemain = @CurrRemain,");
            if (chargelimit.SetTime != null)
                part1.Append("SetTime = @SetTime,");
            if (chargelimit.Operator != null)
                part1.Append("Operator = @Operator,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where branchNo= @branchNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="ChargeLimit"></param>
        /// <returns></returns>
        public int Add(ChargeLimit model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="ChargeLimit"></param>
        /// <returns></returns>
        public void Update(ChargeLimit model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
