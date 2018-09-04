using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_OperateRecordOper : SingleTon<AllInOne_OperateRecordOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_operaterecord"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_OperateRecord allinone_operaterecord)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_operaterecord.id != null)
                dict.Add("@id", allinone_operaterecord.id.ToString());
            if (allinone_operaterecord.content != null)
                dict.Add("@content", allinone_operaterecord.content.ToString());
            if (allinone_operaterecord.operatorId != null)
                dict.Add("@operatorId", allinone_operaterecord.operatorId.ToString());
            if (allinone_operaterecord.autherId != null)
                dict.Add("@autherId", allinone_operaterecord.autherId.ToString());
            if (allinone_operaterecord.operateTime != null)
                dict.Add("@operateTime", allinone_operaterecord.operateTime.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_operaterecord"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_OperateRecord allinone_operaterecord)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_operaterecord.content != null)
            {
                part1.Append("content,");
                part2.Append("@content,");
            }
            if (allinone_operaterecord.operatorId != null)
            {
                part1.Append("operatorId,");
                part2.Append("@operatorId,");
            }
            if (allinone_operaterecord.autherId != null)
            {
                part1.Append("autherId,");
                part2.Append("@autherId,");
            }
            if (allinone_operaterecord.operateTime != null)
            {
                part1.Append("operateTime,");
                part2.Append("@operateTime,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_operaterecord(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_operaterecord"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_OperateRecord allinone_operaterecord)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_operaterecord set ");
            if (allinone_operaterecord.content != null)
                part1.Append("content = @content,");
            if (allinone_operaterecord.operatorId != null)
                part1.Append("operatorId = @operatorId,");
            if (allinone_operaterecord.autherId != null)
                part1.Append("autherId = @autherId,");
            if (allinone_operaterecord.operateTime != null)
                part1.Append("operateTime = @operateTime,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where id= @id  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_OperateRecord"></param>
        /// <returns></returns>
        public int Add(AllInOne_OperateRecord model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_OperateRecord"></param>
        /// <returns></returns>
        public void Update(AllInOne_OperateRecord model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
