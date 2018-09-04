using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using System.Data.SqlClient;
using HHTDCDMR.Models.Extend.Req;
using Common.Extend;

namespace DbOpertion.DBoperation
{
    public partial class FactoryTypeOper : SingleTon<FactoryTypeOper>
    {

        public void DeleteByNo(string factoryNo)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@no", factoryNo);
            var str = $"delete from factoryType where factoryNo=@no";
            SqlHelper.Instance.ExcuteNon(str, dict);
        }

        public List<FactoryType> GetAllList()
        {
            var str = "select * from factorytype where factoryNo not in ('02','03','04','05','06','07') ";
            return SqlHelper.Instance.ExecuteGetDt<FactoryType>(str, new Dictionary<string, string>());

        }

        public List<FactoryType> GetFactoryList(GetFactoryListReq req, int size)
        {
            var search = req.search ?? "";
            var order = req.orderField;
            var desc = Convert.ToBoolean(req.isDesc);
            var index = Convert.ToInt32(req.pageIndex);

            var orderStr = $"order by {order} ";
            if (desc)
                orderStr += " desc ";
            else
                orderStr += " asc ";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                 { "@search2", search },
            };

            var fields = "factoryno";
            if (order != fields)
                fields += "," + order;

            var condition = GetCondition(req, search);

            return SqlHelper.Instance.GetMutiView<FactoryType>("factoryno", fields, condition, index, size, orderStr, dict);

            //return SqlHelper.Instance.GetViewPaging<FactoryType>("FactoryType", @"select * from FactoryType ", condition, index, size, orderStr, dict);
        }

        public int GetListCount(GetFactoryListReq req)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                 { "@search2", search },
            };

            var condition = GetCondition(req, search);

            var list = SqlHelper.Instance.GetMutiViewCount<FactoryType>("factoryno", condition, dict);

            //var list = SqlHelper.Instance.GetDistinctCount<FactoryType>("FactoryType", condition, dict);
            return list.Count;
        }

        public string GetCondition(GetFactoryListReq req, string search)
        {
            var condition = $" factoryNo not in ('02','03','04','05','06','07') ";
            if (!search.IsNullOrEmpty())
                //condition += " and (companyName like @search or phoneNumber like @search )";
                condition += " and factoryName like @search ";
            return condition;
        }


        public string GetInsertStr2(FactoryType factorytype)
        {
            StringBuilder part1 = new StringBuilder();
            StringBuilder part2 = new StringBuilder();

            if (factorytype.factoryNo != null)
            {
                part1.Append("factoryNo,");
                part2.Append("@factoryNo,");
            }
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
            sql.Append("insert into factorytype(").Append(part1.ToString().Remove(part1.Length - 1)).Append(") values (").Append(part2.ToString().Remove(part2.Length - 1)).Append(")");
            return sql.ToString();
        }

        public string Add2(FactoryType model, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var str = GetInsertStr2(model) + " select @@identity";
            var dict = GetParameters(model);
            return SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction);
        }

        public bool IsNoExist(FactoryType ft)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@no", ft.factoryNo);
            var str = "select * from factorytype where factoryno=@no";
            var list = SqlHelper.Instance.ExecuteGetDt<FactoryType>(str, dict);
            if (list.Count > 0)
                return true;
            return false;
        }

        public bool IsNameExist(FactoryType ft)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@name", ft.factoryName);
            var str = "select * from factorytype where factoryname=@name";
            var list = SqlHelper.Instance.ExecuteGetDt<FactoryType>(str, dict);
            if (list.Count > 0)
                return true;
            return false;
        }

        public bool IsNameExist2(FactoryType ft)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@name", ft.factoryName);
            dict.Add("@no", ft.factoryNo);
            var str = "select * from factorytype where factoryname=@name and factoryNo!=@no";
            var list = SqlHelper.Instance.ExecuteGetDt<FactoryType>(str, dict);
            if (list.Count > 0)
                return true;
            return false;
        }

        public bool IsCodeExist(FactoryType ft)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@name", ft.MarkCode);
            var str = "select * from factorytype where markcode=@name";
            var list = SqlHelper.Instance.ExecuteGetDt<FactoryType>(str, dict);
            if (list.Count > 0)
                return true;
            return false;
        }

        public bool IsCodeExist2(FactoryType ft)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@name", ft.MarkCode);
            dict.Add("@no", ft.factoryNo);
            var str = "select * from factorytype where markcode=@name and factoryNo!=@no";
            var list = SqlHelper.Instance.ExecuteGetDt<FactoryType>(str, dict);
            if (list.Count > 0)
                return true;
            return false;
        }
    }
}
