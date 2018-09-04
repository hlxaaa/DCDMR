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
    public partial class FactoryTypeOper : SingleTon<FactoryTypeOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="factorytype"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(FactoryType factorytype)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (factorytype.factoryNo != null)
                dict.Add("@factoryNo", factorytype.factoryNo.ToString());
            if (factorytype.factoryName != null)
                dict.Add("@factoryName", factorytype.factoryName.ToString());
            if (factorytype.MarkCode != null)
                dict.Add("@MarkCode", factorytype.MarkCode.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="factorytype"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(FactoryType factorytype)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (factorytype.factoryName != null)
            {
                part1.Append("factoryName,");
                part2.Append("@factoryName,");
            }
            if (factorytype.MarkCode != null)
            {
                part1.Append("MarkCode,");
                part2.Append("@MarkCode,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into factorytype(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="factorytype"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(FactoryType factorytype)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update factorytype set ");
            if (factorytype.factoryName != null)
                part1.Append("factoryName = @factoryName,");
            if (factorytype.MarkCode != null)
                part1.Append("MarkCode = @MarkCode,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where factoryNo= @factoryNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="FactoryType"></param>
        /// <returns></returns>
        public int Add(FactoryType model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="FactoryType"></param>
        /// <returns></returns>
        public int Update(FactoryType model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            return SqlHelper.Instance.ExcuteNonQuery(str, dict, connection, transaction);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="factorytype"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParametersItem(FactoryType factorytype,int i)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (factorytype.factoryNo != null)
                dict.Add("@factoryNo" + i, factorytype.factoryNo.ToString());
            if (factorytype.factoryName != null)
                dict.Add("@factoryName" + i, factorytype.factoryName.ToString());
            if (factorytype.MarkCode != null)
                dict.Add("@MarkCode" + i, factorytype.MarkCode.ToString());

            return dict;
        }        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="factorytype"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStrItem(FactoryType factorytype, int i)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update factorytype set ");
            if (factorytype.factoryName != null)
                part1.Append($"factoryName = @factoryName{i},");
            if (factorytype.MarkCode != null)
                part1.Append($"MarkCode = @MarkCode{i},");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append($" where factoryNo= @factoryNo{i}  ");
            return part1.ToString();
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="FactoryType"></param>
        /// <returns></returns>
        public void UpdateList(List<FactoryType> list, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = "";
            var dict = new Dictionary<string,string>();
            for(int i=0;i<list.Count;i++)
            {
            var tempDict=GetParametersItem(list[i],i);
            foreach(var item in tempDict)
            {
            dict.Add(item.Key,item.Value);
            }
            str+=GetUpdateStrItem(list[i],i);
            }
            SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);
        }
    }
}
