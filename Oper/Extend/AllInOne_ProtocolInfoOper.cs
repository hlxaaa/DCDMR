using System.Text;
using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_ProtocolInfoOper : SingleTon<AllInOne_ProtocolInfoOper>
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<AllInOne_ProtocolInfo> GetList(ProtocolListReq req, int size)
        {
            var search = req.search ?? "";
            var order = req.orderField;
            var desc = Convert.ToBoolean(req.isDesc);
            var index = Convert.ToInt32(req.pageIndex);
            //var size = 5;
            var orderStr = $"order by {order} ";
            if (desc)
                orderStr += " desc ";
            else
                orderStr += " asc ";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };

            var condition = " 1=1 ";
            if (!search.IsNullOrEmpty())
                condition += " and (protocolName like @search or protocolNo like @search)";
            return SqlHelper.Instance.GvpForCommon<AllInOne_ProtocolInfo>("protocolno", "AllInOne_ProtocolInfo", "select * from AllInOne_ProtocolInfo ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// 获取区域列表总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(ProtocolListReq req)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };

            var condition = " 1=1 ";
            if (!search.IsNullOrEmpty())
                condition += " and (protocolName like @search or protocolNo like @search)";
            var list = SqlHelper.Instance.GdcForCommon<AllInOne_ProtocolInfo>("protocolno", "AllInOne_ProtocolInfo", condition, dict);
            return list.Count;
        }

        public string GetInsertStr2(AllInOne_ProtocolInfo allinone_protocolinfo)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (allinone_protocolinfo.ProtocolNo != null)
            {
                part1.Append("ProtocolNo,");
                part2.Append("@ProtocolNo,");
            }

            if (allinone_protocolinfo.ProtocolName != null)
            {
                part1.Append("ProtocolName,");
                part2.Append("@ProtocolName,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into allinone_protocolinfo(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }

        public void Add2(AllInOne_ProtocolInfo model)
        {
            var str = GetInsertStr2(model) + " select @@identity";
            var dict = GetParameters(model);
            SqlHelper.Instance.ExecuteScalar(str, dict);
        }
    }
}
