using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class DepartmentInfoOper : SingleTon<DepartmentInfoOper>
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="departmentinfo"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters(DepartmentInfo departmentinfo)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (departmentinfo.departNo != null)
                dict.Add("@departNo", departmentinfo.departNo.ToString());
            if (departmentinfo.departName != null)
                dict.Add("@departName", departmentinfo.departName.ToString());
            if (departmentinfo.defineNo != null)
                dict.Add("@defineNo", departmentinfo.defineNo.ToString());
            if (departmentinfo.remark != null)
                dict.Add("@remark", departmentinfo.remark.ToString());

            return dict;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="departmentinfo"></param>
        /// <returns>是否成功</returns>
        public string GetInsertStr(DepartmentInfo departmentinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();
            
            if (departmentinfo.departName != null)
            {
                part1.Append("departName,");
                part2.Append("@departName,");
            }
            if (departmentinfo.defineNo != null)
            {
                part1.Append("defineNo,");
                part2.Append("@defineNo,");
            }
            if (departmentinfo.remark != null)
            {
                part1.Append("remark,");
                part2.Append("@remark,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into departmentinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length-1)).Append(")");
            return sql.ToString();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="departmentinfo"></param>
        /// <returns>是否成功</returns>
        public string GetUpdateStr(DepartmentInfo departmentinfo)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update departmentinfo set ");
            if (departmentinfo.departName != null)
                part1.Append("departName = @departName,");
            if (departmentinfo.defineNo != null)
                part1.Append("defineNo = @defineNo,");
            if (departmentinfo.remark != null)
                part1.Append("remark = @remark,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where departNo= @departNo  ");
            return part1.ToString();
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="DepartmentInfo"></param>
        /// <returns></returns>
        public int Add(DepartmentInfo model)
        {
            var str = GetInsertStr(model)+" select @@identity";
              var dict = GetParameters(model);
            return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str,dict));
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="DepartmentInfo"></param>
        /// <returns></returns>
        public void Update(DepartmentInfo model)
        {
            var str = GetUpdateStr(model);
              var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str,dict);
        }
    }
}
