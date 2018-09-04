using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_siteInfoOper : SingleTon<AllInOne_siteInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_siteinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_siteInfo allinone_siteinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_siteinfo.siteNo != null)
                dict.Add("@siteNo", allinone_siteinfo.siteNo.ToString());
            if (allinone_siteinfo.siteName != null)
                dict.Add("@siteName", allinone_siteinfo.siteName.ToString());
            if (allinone_siteinfo.connectString != null)
                dict.Add("@connectString", allinone_siteinfo.connectString.ToString());
            if (allinone_siteinfo.Remark != null)
                dict.Add("@Remark", allinone_siteinfo.Remark.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_siteinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_siteInfo allinone_siteinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_siteinfo.siteName != null)
            {
                part1.Append("siteName,");
                part2.Append("@siteName,");
            }
            if (allinone_siteinfo.connectString != null)
            {
                part1.Append("connectString,");
                part2.Append("@connectString,");
            }
            if (allinone_siteinfo.Remark != null)
            {
                part1.Append("Remark,");
                part2.Append("@Remark,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_siteinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_siteinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_siteInfo allinone_siteinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_siteinfo set ");
            if (allinone_siteinfo.siteName != null)
                part1.Append("siteName = @siteName,");
            if (allinone_siteinfo.connectString != null)
                part1.Append("connectString = @connectString,");
            if (allinone_siteinfo.Remark != null)
                part1.Append("Remark = @Remark,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where siteNo= @siteNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_siteInfo"></param>
        /// <returns></returns>
        public int Add(AllInOne_siteInfo model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_siteInfo"></param>
        /// <returns></returns>
        public void Update(AllInOne_siteInfo model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
