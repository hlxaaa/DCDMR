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
    public partial class LadderVolumeOper : SingleTon<LadderVolumeOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="laddervolume"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(LadderVolume laddervolume)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (laddervolume.id != null)
                dict.Add("@id", laddervolume.id.ToString());
            if (laddervolume.customerNo != null)
                dict.Add("@customerNo", laddervolume.customerNo.ToString());
            if (laddervolume.customerName != null)
                dict.Add("@customerName", laddervolume.customerName.ToString());
            if (laddervolume.startTime != null)
                dict.Add("@startTime", laddervolume.startTime.ToString());
            if (laddervolume.endTime != null)
                dict.Add("@endTime", laddervolume.endTime.ToString());
            if (laddervolume.StepVolume1 != null)
                dict.Add("@StepVolume1", laddervolume.StepVolume1.ToString());
            if (laddervolume.StepVolume2 != null)
                dict.Add("@StepVolume2", laddervolume.StepVolume2.ToString());
            if (laddervolume.StepVolume3 != null)
                dict.Add("@StepVolume3", laddervolume.StepVolume3.ToString());
            if (laddervolume.StepVolume4 != null)
                dict.Add("@StepVolume4", laddervolume.StepVolume4.ToString());
            if (laddervolume.StepVolume5 != null)
                dict.Add("@StepVolume5", laddervolume.StepVolume5.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="laddervolume"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(LadderVolume laddervolume)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (laddervolume.customerNo != null)
            {
                part1.Append("customerNo,");
                part2.Append("@customerNo,");
            }
            if (laddervolume.customerName != null)
            {
                part1.Append("customerName,");
                part2.Append("@customerName,");
            }
            if (laddervolume.startTime != null)
            {
                part1.Append("startTime,");
                part2.Append("@startTime,");
            }
            if (laddervolume.endTime != null)
            {
                part1.Append("endTime,");
                part2.Append("@endTime,");
            }
            if (laddervolume.StepVolume1 != null)
            {
                part1.Append("StepVolume1,");
                part2.Append("@StepVolume1,");
            }
            if (laddervolume.StepVolume2 != null)
            {
                part1.Append("StepVolume2,");
                part2.Append("@StepVolume2,");
            }
            if (laddervolume.StepVolume3 != null)
            {
                part1.Append("StepVolume3,");
                part2.Append("@StepVolume3,");
            }
            if (laddervolume.StepVolume4 != null)
            {
                part1.Append("StepVolume4,");
                part2.Append("@StepVolume4,");
            }
            if (laddervolume.StepVolume5 != null)
            {
                part1.Append("StepVolume5,");
                part2.Append("@StepVolume5,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into laddervolume(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="laddervolume"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(LadderVolume laddervolume)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update laddervolume set ");
            if (laddervolume.customerNo != null)
                part1.Append("customerNo = @customerNo,");
            if (laddervolume.customerName != null)
                part1.Append("customerName = @customerName,");
            if (laddervolume.startTime != null)
                part1.Append("startTime = @startTime,");
            if (laddervolume.endTime != null)
                part1.Append("endTime = @endTime,");
            if (laddervolume.StepVolume1 != null)
                part1.Append("StepVolume1 = @StepVolume1,");
            if (laddervolume.StepVolume2 != null)
                part1.Append("StepVolume2 = @StepVolume2,");
            if (laddervolume.StepVolume3 != null)
                part1.Append("StepVolume3 = @StepVolume3,");
            if (laddervolume.StepVolume4 != null)
                part1.Append("StepVolume4 = @StepVolume4,");
            if (laddervolume.StepVolume5 != null)
                part1.Append("StepVolume5 = @StepVolume5,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="LadderVolume"></param>
        /// <returns></returns>
        public int Add(LadderVolume model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="LadderVolume"></param>
        /// <returns></returns>
        public void Update(LadderVolume model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
