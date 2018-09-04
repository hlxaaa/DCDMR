using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_RightInfoOper : SingleTon<AllInOne_RightInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="allinone_rightinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(AllInOne_RightInfo allinone_rightinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (allinone_rightinfo.RightNo != null)
                dict.Add("@RightNo", allinone_rightinfo.RightNo.ToString());
            if (allinone_rightinfo.SuperNo != null)
                dict.Add("@SuperNo", allinone_rightinfo.SuperNo.ToString());
            if (allinone_rightinfo.rightName != null)
                dict.Add("@rightName", allinone_rightinfo.rightName.ToString());
            if (allinone_rightinfo.rightMark != null)
                dict.Add("@rightMark", allinone_rightinfo.rightMark.ToString());
            if (allinone_rightinfo.rightPolicy != null)
                dict.Add("@rightPolicy", allinone_rightinfo.rightPolicy.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="allinone_rightinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(AllInOne_RightInfo allinone_rightinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (allinone_rightinfo.SuperNo != null)
            {
                part1.Append("SuperNo,");
                part2.Append("@SuperNo,");
            }
            if (allinone_rightinfo.rightName != null)
            {
                part1.Append("rightName,");
                part2.Append("@rightName,");
            }
            if (allinone_rightinfo.rightMark != null)
            {
                part1.Append("rightMark,");
                part2.Append("@rightMark,");
            }
            if (allinone_rightinfo.rightPolicy != null)
            {
                part1.Append("rightPolicy,");
                part2.Append("@rightPolicy,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_rightinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="allinone_rightinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(AllInOne_RightInfo allinone_rightinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update allinone_rightinfo set ");
            if (allinone_rightinfo.SuperNo != null)
                part1.Append("SuperNo = @SuperNo,");
            if (allinone_rightinfo.rightName != null)
                part1.Append("rightName = @rightName,");
            if (allinone_rightinfo.rightMark != null)
                part1.Append("rightMark = @rightMark,");
            if (allinone_rightinfo.rightPolicy != null)
                part1.Append("rightPolicy = @rightPolicy,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where RightNo= @RightNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="AllInOne_RightInfo"></param>
        /// <returns></returns>
        public int Add(AllInOne_RightInfo model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="AllInOne_RightInfo"></param>
        /// <returns></returns>
        public void Update(AllInOne_RightInfo model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
