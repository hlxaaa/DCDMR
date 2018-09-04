using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_ProtocolInfoOper : SingleTon<AllInOne_ProtocolInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_protocolinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_ProtocolInfo allinone_protocolinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_protocolinfo.ProtocolNo != null)
                dict.Add("@ProtocolNo", allinone_protocolinfo.ProtocolNo.ToString());
            if (allinone_protocolinfo.ProtocolName != null)
                dict.Add("@ProtocolName", allinone_protocolinfo.ProtocolName.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_protocolinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_ProtocolInfo allinone_protocolinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_protocolinfo.ProtocolName != null)
            {
                part1.Append("ProtocolName,");
                part2.Append("@ProtocolName,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_protocolinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_protocolinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_ProtocolInfo allinone_protocolinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_protocolinfo set ");
            if (allinone_protocolinfo.ProtocolName != null)
                part1.Append("ProtocolName = @ProtocolName,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where ProtocolNo= @ProtocolNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_ProtocolInfo"></param>
        /// <returns></returns>
        public int Add(AllInOne_ProtocolInfo model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_ProtocolInfo"></param>
        /// <returns></returns>
        public void Update(AllInOne_ProtocolInfo model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
