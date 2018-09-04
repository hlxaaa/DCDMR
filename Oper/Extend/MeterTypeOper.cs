using System.Text;
using Common.Extend;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;

namespace DbOpertion.DBoperation
{
    public partial class MeterTypeOper : SingleTon<MeterTypeOper>
    {
        public List<MeterType> GetAllList()
        {
            return SqlHelper.Instance.GetListFromDb<MeterType>("MeterType");
        }

        /// <summary>
        /// 获取设备类型列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<MeterType> GetList(MeterTypeReq req, int size)
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

            var condition = " 1=1  ";
            if (!search.IsNullOrEmpty())
                condition += " and (meterTypeName like @search or meterTypeNo like @search or MarkCode like @search)";
            return SqlHelper.Instance.GvpForCommon<MeterType>("metertypeno", "MeterType", "select * from MeterType ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// 获取设备类型的总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(MeterTypeReq req)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };

            var condition = " 1=1  ";
            if (!search.IsNullOrEmpty())
                condition += " and (meterTypeName like @search or meterTypeNo like @search or MarkCode like @search)";
            var list = SqlHelper.Instance.GdcForCommon<MeterType>("metertypeno", "MeterType", condition, dict);
            return list.Count;
        }

        public string GetInsertStr2(MeterType metertype)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (metertype.meterTypeNo != null)
            {
                part1.Append("meterTypeNo,");
                part2.Append("@meterTypeNo,");
            }

            if (metertype.meterTypeName != null)
            {
                part1.Append("meterTypeName,");
                part2.Append("@meterTypeName,");
            }
            if (metertype.MarkCode != null)
            {
                part1.Append("MarkCode,");
                part2.Append("@MarkCode,");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into metertype(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }

        public void Add2(MeterType model)
        {
            var str = GetInsertStr2(model) + " select @@identity";
            var dict = GetParameters(model);
            SqlHelper.Instance.ExecuteScalar(str, dict);
        }

        public string GetUpdateStr2(MeterType metertype)
        {
            StringBuilder part1 = new StringBuilder();
            part1.Append("update metertype set ");
            if (metertype.meterTypeName != null)
                part1.Append("meterTypeName = @meterTypeName,");
            if (metertype.MarkCode != null)
                part1.Append("MarkCode = @MarkCode,");
            int n = part1.ToString().LastIndexOf(",");
            part1.Remove(n, 1);
            part1.Append(" where meterTypeNo= @meterTypeNo  ");
            return part1.ToString();
        }

        public void Update2(MeterType model)
        {
            var str = GetUpdateStr2(model);
            var dict = GetParameters(model);
            SqlHelper.Instance.ExcuteNon(str, dict);
        }
    }
}
