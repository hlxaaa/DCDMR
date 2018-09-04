using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class FluidInfo0301Oper : SingleTon<FluidInfo0301Oper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="fluidinfo0301"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(FluidInfo0301 fluidinfo0301)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (fluidinfo0301.id != null)
                dict.Add("@id", fluidinfo0301.id.ToString());
            if (fluidinfo0301.fluidNo != null)
                dict.Add("@fluidNo", fluidinfo0301.fluidNo.ToString());
            if (fluidinfo0301.fluidName != null)
                dict.Add("@fluidName", fluidinfo0301.fluidName.ToString());
            if (fluidinfo0301.price1 != null)
                dict.Add("@price1", fluidinfo0301.price1.ToString());
            if (fluidinfo0301.price2 != null)
                dict.Add("@price2", fluidinfo0301.price2.ToString());
            if (fluidinfo0301.price3 != null)
                dict.Add("@price3", fluidinfo0301.price3.ToString());
            if (fluidinfo0301.price4 != null)
                dict.Add("@price4", fluidinfo0301.price4.ToString());
            if (fluidinfo0301.price5 != null)
                dict.Add("@price5", fluidinfo0301.price5.ToString());
            if (fluidinfo0301.endAmount1 != null)
                dict.Add("@endAmount1", fluidinfo0301.endAmount1.ToString());
            if (fluidinfo0301.endAmount2 != null)
                dict.Add("@endAmount2", fluidinfo0301.endAmount2.ToString());
            if (fluidinfo0301.endAmount3 != null)
                dict.Add("@endAmount3", fluidinfo0301.endAmount3.ToString());
            if (fluidinfo0301.endAmount4 != null)
                dict.Add("@endAmount4", fluidinfo0301.endAmount4.ToString());
            if (fluidinfo0301.endAmount5 != null)
                dict.Add("@endAmount5", fluidinfo0301.endAmount5.ToString());
            if (fluidinfo0301.alarmVolume != null)
                dict.Add("@alarmVolume", fluidinfo0301.alarmVolume.ToString());
            if (fluidinfo0301.maxOverdraft != null)
                dict.Add("@maxOverdraft", fluidinfo0301.maxOverdraft.ToString());
            if (fluidinfo0301.maxFlow != null)
                dict.Add("@maxFlow", fluidinfo0301.maxFlow.ToString());
            if (fluidinfo0301.testValve != null)
                dict.Add("@testValve", fluidinfo0301.testValve.ToString());
            if (fluidinfo0301.devType != null)
                dict.Add("@devType", fluidinfo0301.devType.ToString());
            if (fluidinfo0301.intervalMonth != null)
                dict.Add("@intervalMonth", fluidinfo0301.intervalMonth.ToString());
            if (fluidinfo0301.startTime != null)
                dict.Add("@startTime", fluidinfo0301.startTime.ToString());
            if (fluidinfo0301.monthStage != null)
                dict.Add("@monthStage", fluidinfo0301.monthStage.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="fluidinfo0301"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(FluidInfo0301 fluidinfo0301)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (fluidinfo0301.fluidNo != null)
            {
                part1.Append("fluidNo,");
                part2.Append("@fluidNo,");
            }
            if (fluidinfo0301.fluidName != null)
            {
                part1.Append("fluidName,");
                part2.Append("@fluidName,");
            }
            if (fluidinfo0301.price1 != null)
            {
                part1.Append("price1,");
                part2.Append("@price1,");
            }
            if (fluidinfo0301.price2 != null)
            {
                part1.Append("price2,");
                part2.Append("@price2,");
            }
            if (fluidinfo0301.price3 != null)
            {
                part1.Append("price3,");
                part2.Append("@price3,");
            }
            if (fluidinfo0301.price4 != null)
            {
                part1.Append("price4,");
                part2.Append("@price4,");
            }
            if (fluidinfo0301.price5 != null)
            {
                part1.Append("price5,");
                part2.Append("@price5,");
            }
            if (fluidinfo0301.endAmount1 != null)
            {
                part1.Append("endAmount1,");
                part2.Append("@endAmount1,");
            }
            if (fluidinfo0301.endAmount2 != null)
            {
                part1.Append("endAmount2,");
                part2.Append("@endAmount2,");
            }
            if (fluidinfo0301.endAmount3 != null)
            {
                part1.Append("endAmount3,");
                part2.Append("@endAmount3,");
            }
            if (fluidinfo0301.endAmount4 != null)
            {
                part1.Append("endAmount4,");
                part2.Append("@endAmount4,");
            }
            if (fluidinfo0301.endAmount5 != null)
            {
                part1.Append("endAmount5,");
                part2.Append("@endAmount5,");
            }
            if (fluidinfo0301.alarmVolume != null)
            {
                part1.Append("alarmVolume,");
                part2.Append("@alarmVolume,");
            }
            if (fluidinfo0301.maxOverdraft != null)
            {
                part1.Append("maxOverdraft,");
                part2.Append("@maxOverdraft,");
            }
            if (fluidinfo0301.maxFlow != null)
            {
                part1.Append("maxFlow,");
                part2.Append("@maxFlow,");
            }
            if (fluidinfo0301.testValve != null)
            {
                part1.Append("testValve,");
                part2.Append("@testValve,");
            }
            if (fluidinfo0301.devType != null)
            {
                part1.Append("devType,");
                part2.Append("@devType,");
            }
            if (fluidinfo0301.intervalMonth != null)
            {
                part1.Append("intervalMonth,");
                part2.Append("@intervalMonth,");
            }
            if (fluidinfo0301.startTime != null)
            {
                part1.Append("startTime,");
                part2.Append("@startTime,");
            }
            if (fluidinfo0301.monthStage != null)
            {
                part1.Append("monthStage,");
                part2.Append("@monthStage,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into fluidinfo0301(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fluidinfo0301"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(FluidInfo0301 fluidinfo0301)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update fluidinfo0301 set ");
            if (fluidinfo0301.fluidNo != null)
                part1.Append("fluidNo = @fluidNo,");
            if (fluidinfo0301.fluidName != null)
                part1.Append("fluidName = @fluidName,");
            if (fluidinfo0301.price1 != null)
                part1.Append("price1 = @price1,");
            if (fluidinfo0301.price2 != null)
                part1.Append("price2 = @price2,");
            if (fluidinfo0301.price3 != null)
                part1.Append("price3 = @price3,");
            if (fluidinfo0301.price4 != null)
                part1.Append("price4 = @price4,");
            if (fluidinfo0301.price5 != null)
                part1.Append("price5 = @price5,");
            if (fluidinfo0301.endAmount1 != null)
                part1.Append("endAmount1 = @endAmount1,");
            if (fluidinfo0301.endAmount2 != null)
                part1.Append("endAmount2 = @endAmount2,");
            if (fluidinfo0301.endAmount3 != null)
                part1.Append("endAmount3 = @endAmount3,");
            if (fluidinfo0301.endAmount4 != null)
                part1.Append("endAmount4 = @endAmount4,");
            if (fluidinfo0301.endAmount5 != null)
                part1.Append("endAmount5 = @endAmount5,");
            if (fluidinfo0301.alarmVolume != null)
                part1.Append("alarmVolume = @alarmVolume,");
            if (fluidinfo0301.maxOverdraft != null)
                part1.Append("maxOverdraft = @maxOverdraft,");
            if (fluidinfo0301.maxFlow != null)
                part1.Append("maxFlow = @maxFlow,");
            if (fluidinfo0301.testValve != null)
                part1.Append("testValve = @testValve,");
            if (fluidinfo0301.devType != null)
                part1.Append("devType = @devType,");
            if (fluidinfo0301.intervalMonth != null)
                part1.Append("intervalMonth = @intervalMonth,");
            if (fluidinfo0301.startTime != null)
                part1.Append("startTime = @startTime,");
            if (fluidinfo0301.monthStage != null)
                part1.Append("monthStage = @monthStage,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="FluidInfo0301"></param>
        /// <returns></returns>
        public int Add(FluidInfo0301 model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="FluidInfo0301"></param>
        /// <returns></returns>
        public void Update(FluidInfo0301 model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
