using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;
using System.Linq;
using System.Configuration;

namespace DbOpertion.DBoperation
{
    public partial class HisOper : SingleTon<HisOper>
    {
        /// <summary>
        /// 获取两个数据表联合的查询字符串
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        public string GetDBUnion(List<int> deviceIds)
        {
            var tbs = new List<string>();
            foreach (var item in deviceIds)
            {
                tbs.Add($"FM{StringHelper.Instance.GetIntStringWithZero(item.ToString(), 10)}");
            }
            var r = "";
            for (int i = 0; i < tbs.Count; i++)
            {
                if (i == 0)
                {
                    r += $" select fm{i}.*,h.customerName,h.address,h.deviceNo from {tbs[i]} as fm{i} LEFT JOIN HHMDeviceView as h on h.meterNo=fm{i}.FLMeterNo";
                }
                else
                {
                    r += $" union select fm{i}.*,h.customerName,h.address,h.deviceNo from {tbs[i]} as fm{i} LEFT JOIN HHMDeviceView as h on h.meterNo=fm{i}.FLMeterNo";
                }
            }
            //return $" select ROW_NUMBER() OVER(order by r.id) rId,r.* from({r})r ";
            return r;
        }

        public string GetDBUnion(List<string> tbs)
        {
            var r = "";
            for (int i = 0; i < tbs.Count; i++)
            {
                if (i == 0)
                {
                    //r += $" select * from {tbs[i]} ";
                    r += $" select fm{i}.*,h.customerName,h.address,h.deviceNo from {tbs[i]} as fm{i} LEFT JOIN HHMDeviceView as h on h.meterNo=fm{i}.FLMeterNo";
                }
                else
                {
                    r += $" union select fm{i}.*,h.customerName,h.address,h.deviceNo from {tbs[i]} as fm{i} LEFT JOIN HHMDeviceView as h on h.meterNo=fm{i}.FLMeterNo";
                }
            }
            //return $" select ROW_NUMBER() OVER(order by r.id) rId,r.* from({r})r ";
            return r;
        }

        /// <summary>
        /// 根据数据表获取其中设备的设备号集合
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public List<int> GetDeviceIds(List<string> tableNames)
        {
            var listR = new List<int>();
            foreach (var name in tableNames)
            {
                var temp = name.Substring(2);
                int r;
                if (int.TryParse(temp, out r))
                {
                    listR.Add(r);
                }
            }
            return listR;
        }

        /// <summary>
        /// 获取历史数据的列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <param name="selectUnion"></param>
        /// <returns></returns>
        public List<FMModel> GetList(HisReq req, int size, string selectUnion)
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

            var condition = $" 1=1 ";
            if (!search.IsNullOrEmpty())
                condition += " and (h.customerName like @search or h.address like @search or h.communicateNo=@search2  or h.deviceNo=@search2) ";

            var name = req.lastName;
            if (name != null)
            {
                var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(name);

                if (cid != null)
                {
                    dict.Add("cid", cid);
                    condition += $" and  (cid1=@cid or cid2=@cid or cid3=@cid or cid4=@cid) ";
                }
                else
                    return new List<FMModel>();
            }

            if (req.startTime == null && req.endTime == null)
            {
                var dateStr = req.dateStr;
                var now = DateTime.Now;
                var year = now.Year;
                var month = now.Month;
                var day = now.Day;
                switch (dateStr)
                {
                    case "year":
                        condition += $" and InstantTime>'{new DateTime(year - 1, 12, 31)}' and InstantTime<'{new DateTime(year + 1, 1, 1)}'";
                        break;
                    case "month":
                        condition += $" and InstantTime>'{new DateTime(year, month, 1)}' and InstantTime<'{new DateTime(year, month, 1).AddMonths(1)}'";
                        break;
                    case "day":
                        condition += $" and InstantTime>'{new DateTime(year, month, day, 23, 59, 59).AddDays(-1)}' and InstantTime<'{new DateTime(year, month, day, 0, 0, 0).AddDays(1)}'";
                        break;
                }
            }
            else
            {
                if (req.startTime != null)
                {
                    condition += $" and InstantTime>'{req.startTime}'";
                }
                if (req.endTime != null)
                {
                    condition += $" and InstantTime<'{req.endTime}'";
                }
            }

            return SqlHelper.Instance.GvpForFMModel<FMModel>(selectUnion, condition, index, size, orderStr, dict);
        }

        public List<FMModel> GetList2(HisReq req, int size, List<string> tableNames)
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
                { "@search", $"%{search}%" },
                  { "@search2", search },
            };

            var condition = $" 1=1 ";
            if (!search.IsNullOrEmpty())
                condition += " and (h.customerName like @search or h.address like @search or h.communicateNo=@search2  or h.deviceNo=@search2) ";

            var name = req.lastName;
            if (name != null)
            {
                var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(name);

                if (cid != null)
                {
                    dict.Add("cid", cid);
                    condition += $" and  (cid1=@cid or cid2=@cid or cid3=@cid or cid4=@cid) ";
                }
                else
                    return new List<FMModel>();
            }

            //if (req.meterNo != null && req.meterNo != "0")
            //    condition += $" and FLMeterNo={req.meterNo} ";-txy 不记得什么作用了

            if (req.startTime == null && req.endTime == null)
            {
                var dateStr = req.dateStr;
                var now = DateTime.Now;
                var year = now.Year;
                var month = now.Month;
                var day = now.Day;
                switch (dateStr)
                {
                    case "year":
                        condition += $" and InstantTime>'{new DateTime(year - 1, 12, 31)}' and InstantTime<'{new DateTime(year + 1, 1, 1)}'";
                        break;
                    case "month":
                        condition += $" and InstantTime>'{new DateTime(year, month, 1)}' and InstantTime<'{new DateTime(year, month, 1).AddMonths(1)}'";
                        break;
                    case "day":
                        condition += $" and InstantTime>'{new DateTime(year, month, day, 23, 59, 59).AddDays(-1)}' and InstantTime<'{new DateTime(year, month, day, 0, 0, 0).AddDays(1)}'";
                        break;
                }
            }
            else
            {
                if (req.startTime != null)
                {
                    condition += $" and InstantTime>'{req.startTime}'";
                }
                if (req.endTime != null)
                {
                    condition += $" and InstantTime<'{req.endTime}'";
                }
            }
            //tableNames = tableNames;
            //var meterNos = GetDeviceIds(tableNames);
            //var idsStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());

            //condition += $" and flmeterNo in ({idsStr})";

            //           string select = $"select top {size} * from allfmdata where rid not in (select top {(index - 1) * size} rid from allfmdata where {condition}) and {condition}";

            //           select = $@"SELECT
            //				top {size} *
            //			FROM
            //				allfmdata AS fm0
            //			LEFT JOIN HHMDeviceView AS h ON h.meterNo = fm0.FLMeterNo where 
            //			{condition} and rid not in (SELECT
            //top {(index - 1) * size}
            //				fm0.rid
            //			FROM
            //				allfmdata AS fm0
            //			LEFT JOIN HHMDeviceView AS h ON h.meterNo = fm0.FLMeterNo where 
            //			{condition})";


            //           return SqlHelper.Instance.ExecuteGetDt2<FMModel>(select, dict);

            //tableNames = GetExistTableByCondition(tableNames, condition, dict);

            var str = $@"create table #Temp
                            (
                            rId int identity(1,1),
                            id int,
                            communicateNo nvarchar(50),
                            StdSum DECIMAL (12,3),
                            WorkSum DECIMAL (12,3),
                            StdFlow DECIMAL (12,3),
                            WorkFlow DECIMAL (12,3),
                            Temperature DECIMAL (12,3),
                            Pressure DECIMAL (12,3),
                            RemainMoney DECIMAL (12,3),
                            RemainVolume DECIMAL (12,3),
                            PowerVoltage DECIMAL (12,3),
                            FMStateMsg nvarchar(50),
                            InstantTime datetime,
                            customerName nvarchar(50),
                            address nvarchar(50),
                            deviceNo nvarchar(50)
                            )
                            ";

            foreach (var item in tableNames)
            {
                //                str += $@"
                //                    INSERT #temp
                //                    SELECT
                //                    DISTINCT
                //					fm0.id,
                //					fm0.communicateNo,
                //					fm0.StdSum,
                //					fm0.WorkSum,
                //					fm0.StdFlow,
                //					fm0.WorkFlow,
                //					fm0.Temperature,
                //					fm0.Pressure,
                //					fm0.RemainMoney,
                //					fm0.RemainVolume,
                //					fm0.PowerVoltage,
                //					fm0.FMStateMsg,
                //					fm0.InstantTime,
                //					h.customerName,
                //					h.address,
                //					h.deviceNo
                //				FROM
                //					{item} AS fm0
                //				LEFT JOIN HHMDeviceView AS h ON h.meterNo = fm0.FLMeterNo where 
                //				{condition}
                //";
                str += $@"
                    INSERT #temp
                    SELECT
                    DISTINCT
					fm0.id,
					fm0.communicateNo,
					fm0.StdSum,
					fm0.WorkSum,
					fm0.StdFlow,
					fm0.WorkFlow,
					fm0.Temperature,
					fm0.Pressure,
					fm0.RemainMoney,
					fm0.RemainVolume,
					fm0.PowerVoltage,
					fm0.FMStateMsg,
					fm0.InstantTime,
					h.customerName,
					h.address,
					h.deviceNo
				FROM
					{item} AS fm0
				LEFT JOIN HHMDeviceView AS h ON h.meterNo = fm0.FLMeterNo where 
				{condition}
";
            }

            //str += $@" select top {size} * from #temp  where rid not in ( select top {(index - 1) * size} rid from #temp  order by id desc)  order by id desc";

            str += $@" select top {size} * from #temp  where rid not in ( select top {(index - 1) * size} rid from #temp  {orderStr})  {orderStr}";

            var list = SqlHelper.Instance.ExecuteGetDt2<FMModel>(str, dict);
            return list;




            //var selectStr = "";

            //for (int i = 0; i < tableNames.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        selectStr += $" select fm{i}.id,fm{i}.communicateNo,fm{i}.StdSum,fm{i}.WorkSum,fm{i}.StdFlow,fm{i}.WorkFlow,fm{i}.Temperature,fm{i}.Pressure,fm{i}.RemainMoney,fm{i}.RemainVolume,fm{i}.PowerVoltage,fm{i}.FMStateMsg,fm{i}.InstantTime,h.customerName,h.address,h.deviceNo from {tableNames[i]} as fm{i} LEFT JOIN HHMDeviceView as h on h.meterNo=fm{i}.FLMeterNo where {condition} ";
            //    }
            //    else
            //    {
            //        selectStr += $" union select fm{i}.id,fm{i}.communicateNo,fm{i}.StdSum,fm{i}.WorkSum,fm{i}.StdFlow,fm{i}.WorkFlow,fm{i}.Temperature,fm{i}.Pressure,fm{i}.RemainMoney,fm{i}.RemainVolume,fm{i}.PowerVoltage,fm{i}.FMStateMsg,fm{i}.InstantTime,h.customerName,h.address,h.deviceNo from {tableNames[i]} as fm{i} LEFT JOIN HHMDeviceView as h on h.meterNo=fm{i}.FLMeterNo where {condition} ";
            //    }
            //}

            //return SqlHelper.Instance.GvpForFMModel2<FMModel>(selectStr, index, size, orderStr, dict);
        }

        public List<FMModel> GetPiece(string tableName, string condition)
        {
            //var selectStr = "";

            ////r += $" select * from {tbs[i]} ";
            //selectStr += $" select fm.id,fm.communicateNo,fm.StdSum,fm.WorkSum,fm.StdFlow,fm.WorkFlow,fm.Temperature,fm.Pressure,fm.RemainMoney,fm.RemainVolume,fm.PowerVoltage,fm.FMStateMsg,fm.InstantTime,h.customerName,h.address,h.deviceNo from {tableName} as fm LEFT JOIN HHMDeviceView as h on h.meterNo=fm.FLMeterNo where {condition} ";

            //return SqlHelper.Instance.GvpForFMModel2<FMModel>(selectStr, index, size, orderStr, dict);
            return null;
        }

        /// <summary>
        /// 历史数据的条数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="selectUnion"></param>
        /// <returns></returns>
        public int GetCount(HisReq req, string selectUnion, List<string> tableNames)
        {
            var search = req.search ?? "";

            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                  { "@search2", search },
            };

            var condition = $" 1=1 ";
            if (!search.IsNullOrEmpty())
                condition += " and (h.customerName like @search or h.address like @search or h.communicateNo=@search2  or h.deviceNo=@search2) ";

            var name = req.lastName;
            if (name != null)
            {
                var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(name);

                if (cid != null)
                {
                    dict.Add("cid", cid);
                    condition += $" and  (cid1=@cid or cid2=@cid or cid3=@cid or cid4=@cid) ";
                }
                else
                    return 0;
            }

            //if (req.meterNo != null && req.meterNo != "0")
            //    condition += $" and FLMeterNo={req.meterNo} ";

            if (req.startTime == null && req.endTime == null)
            {
                var dateStr = req.dateStr;
                var now = DateTime.Now;
                var year = now.Year;
                var month = now.Month;
                var day = now.Day;
                switch (dateStr)
                {
                    case "year":
                        condition += $" and InstantTime>'{new DateTime(year - 1, 12, 31)}' and InstantTime<'{new DateTime(year + 1, 1, 1)}'";
                        break;
                    case "month":
                        condition += $" and InstantTime>'{new DateTime(year, month, 1)}' and InstantTime<'{new DateTime(year, month, 1).AddMonths(1)}'";
                        break;
                    case "day":
                        condition += $" and InstantTime>'{new DateTime(year, month, day, 23, 59, 59).AddDays(-1)}' and InstantTime<'{new DateTime(year, month, day, 0, 0, 0).AddDays(1)}'";
                        break;
                }
            }
            else
            {
                if (req.startTime != null)
                {
                    condition += $" and InstantTime>'{req.startTime}'";
                }
                if (req.endTime != null)
                {
                    condition += $" and InstantTime<'{req.endTime}'";
                }
            }

            //var meterNos = GetDeviceIds(tableNames);
            //var idsStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());

            //condition += $" and flmeterNo in ({idsStr})";

            //        var str = $@"SELECT
            //	 *
            //FROM
            //	allfmdata AS fm0
            //LEFT JOIN HHMDeviceView AS h ON h.meterNo = fm0.FLMeterNo where 
            //{condition}";
            //        var list = SqlHelper.Instance.ExecuteGetDt2<FMModel>(str, dict);

            var list = SqlHelper.Instance.GdcForFMModel<FMModel>(selectUnion, condition, dict);
            return list.Count;
        }

        public minMaxSS GetMinMaxSS(string tableName)
        {
            string str = $@"select * from (select top 1 StdSum as minSS from {tableName} )r1 LEFT JOIN (select top 1 StdSum as maxSS from {tableName} order by StdSum desc)r2 on 1=1";
            var list = SqlHelper.Instance.ExecuteGetDt2<minMaxSS>(str, new Dictionary<string, string>());
            if (list.Count == 0)
                return null;
            else
                return list.First();
        }

        public List<StdSumReport> GetHisDataListForChart(GetStdSumReq req, string tableName)
        {
            var date = Convert.ToDateTime(req.date);
            var type = req.type;
            var mmss = GetMinMaxSS(tableName);
            if (mmss == null)
            {
                var temp = new List<StdSumReport>();
                return CompleteSSR(temp, type, date, req.startTime);
            }
            var min = mmss.minSS;
            var max = mmss.maxSS;

            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;


            var str = "";
            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var newDate = new DateTime(year, i, 1);

                    var select1 = $@"    SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddMonths(1)}'
		                                ORDER BY
			                                instantTime DESC ";
                    var select2 = $@"  SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC";

                    str += $@"   SELECT
                                '{newDate}' as dt,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS span
                                FROM
	                                (
		                             {select1}
	                                ) r1
                                LEFT JOIN (
	                             {select2}
                                ) r2 ON 1 = 1";

                    if (i != 1)
                    {
                        str += $@" union {str}";
                    }
                }
            }
            else if (type == "month")
            {
                var days = DateTime.DaysInMonth(year, month);
                for (int i = 1; i < days + 1; i++)
                {
                    var newDate = new DateTime(year, month, i);
                    if (i == 1)
                    {
                        str += $@"SELECT
                                '{newDate}' as dt,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS span
                                FROM
	                                (
		                                SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddDays(1)}'
		                                ORDER BY
			                                instantTime DESC
	                                ) r1
                                LEFT JOIN (
	                                SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC
                                ) r2 ON 1 = 1";
                    }
                    else
                    {
                        str += $@" union SELECT
                                '{newDate}' as dt,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS span
                                FROM
	                                (
		                                SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddDays(1)}'
		                                ORDER BY
			                                instantTime DESC
	                                ) r1
                                LEFT JOIN (
	                                SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC
                                ) r2 ON 1 = 1";
                    }
                }
            }
            else if (type == "day")
            {
                var hour = Convert.ToInt32(req.startTime);
                for (int i = hour; i < hour + 24; i++)
                {
                    //var newDate = new DateTime(year, month, day, i, 0, 0);
                    var newDate = new DateTime();

                    if (i > 23)
                    {
                        newDate = new DateTime(year, month, day, 0, 0, 0).AddDays(1).AddHours(i - 24);
                        //newDate = new DateTime(year, month, day, i, 0, 0);
                    }
                    else
                        newDate = new DateTime(year, month, day, i, 0, 0);

                    if (i == hour)
                    {
                        str += $@"SELECT
                                '{newDate}' as dt,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS span
                                FROM
	                                (
		                                SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddHours(1)}'
		                                ORDER BY
			                                instantTime DESC
	                                ) r1
                                LEFT JOIN (
	                                SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC
                                ) r2 ON 1 = 1";
                    }
                    else
                    {
                        str += $@" union SELECT
                                '{newDate}' as dt,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS span
                                FROM
	                                (
		                                SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddHours(1)}'
		                                ORDER BY
			                                instantTime DESC
	                                ) r1
                                LEFT JOIN (
	                                SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC
                                ) r2 ON 1 = 1";
                    }
                }
            }

            var list = SqlHelper.Instance.ExecuteGetDt2<StdSumReport>(str, new Dictionary<string, string>());
            return CompleteSSR(list, type, date, req.startTime);
        }

        public List<StdSumReport> GetHisDataListForChartForCompany(GetStdSumReq req)
        {
            var date = Convert.ToDateTime(req.date);

            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);

            if (tableNames.Count == 0)
                return CompleteSSR(new List<StdSumReport>(), req.type, date, req.startTime);
            tableNames = GetExistHisTable(tableNames);
            tableNames = RemoveBadTable(tableNames);



            if (tableNames.Count == 0)
                return CompleteSSR(new List<StdSumReport>(), req.type, date, req.startTime);

            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            //var addStr = "t1.span";
            var str = "";
            //var oj = "";
            var type = req.type;

            var meterNos = GetDeviceIds(tableNames);
            var nosStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());

            switch (type)
            {
                case "year":
                    str = $"select year,month,sum(stdsum) as value from FMReportYear where FlMeterNo  in ({nosStr}) and year={year} GROUP BY year,month";
                    break;
                case "month":
                    str = $"select year,month,day,sum(stdsum) as value from FMReportMonth where FlMeterNo  in ({nosStr}) and year={year} and month={month} GROUP BY year,month,day";
                    break;
                case "day":
                    var hour = Convert.ToInt32(req.startTime);
                    date = date.AddHours(hour + 1);
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                    hour = date.Hour;
                    var date2 = date.AddDays(1).AddHours(hour - 1);
                    var year2 = date2.Year;
                    var month2 = date2.Month;
                    var day2 = date2.Day;
                    var hour2 = date2.Hour;

                    str = $@"SELECT
	                        YEAR,
	                        MONTH,
	                        DAY,
                        hour,
	                        SUM (stdsum) AS
                        VALUE
                        FROM
	                        FMReportDay
                        WHERE
                         FlMeterNo  in ({nosStr}) and  
                        ((year={year} and month={month} and day={day} and hour>={hour}) or (year={year2} and month={month2} and day={day2} and hour<{hour2}))
                        GROUP BY
	                        YEAR,
	                        MONTH,
	                        DAY,hour";
                    break;
            }
            var list2 = SqlHelper.Instance.ExecuteGetDt2<pieTime>(str, new Dictionary<string, string>());
            var list = ConvertToSSR(list2, type, "");


            //return ConvertPieTimeToPieRes(list2, type, req.startTime);
            #region 新SQL
            //            str += $@"
            //create table #temp(
            //name nvarchar(50),
            //valuess DECIMAL(12,2)
            //)
            //";
            //            for (int j = 0; j < tableNames.Count; j++)
            //            {
            //                var mmss = GetMinMaxSS(tableNames[j]);
            //                if (mmss == null)
            //                    continue;
            //                var min = mmss.minSS;
            //                var max = mmss.maxSS;
            //                var strHere = "";
            //                if (type == "year")
            //                {
            //                    for (int i = 1; i < 13; i++)
            //                    {
            //                        var newDate = new DateTime(year, i, 1);

            //                        strHere = $@" 
            //insert #temp
            //SELECT
            //                                '{newDate}' as dt,
            //                                 (
            //                                  isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                                 ) AS span
            //                                FROM
            //                                 (
            //                                  SELECT
            //                                   TOP 1 StdSum
            //                                  FROM
            //                                   {tableNames[j]}
            //                                  WHERE
            //                                   instantTime < '{newDate.AddMonths(1)}'
            //                                  ORDER BY
            //                                   instantTime DESC
            //                                 ) r1
            //                                LEFT JOIN (
            //                                 SELECT
            //                                  TOP 1 StdSum
            //                                 FROM
            //                                  {tableNames[j]}
            //                                 WHERE
            //                                  instantTime <= '{newDate}'
            //                                 ORDER BY
            //                                  InstantTime DESC
            //                                ) r2 ON 1 = 1 ";


            //                        //addStr += $" + isnull(t{j}{i + 1}.span,0) ";
            //                        str += strHere;
            //                    }
            //                }
            //                else if (type == "month")
            //                {
            //                    var days = DateTime.DaysInMonth(year, month);
            //                    for (int i = 1; i < days + 1; i++)
            //                    {
            //                        var newDate = new DateTime(year, month, i);
            //                        strHere = $@" 
            //insert #temp
            //SELECT
            //                                '{newDate}' as dt,
            //                                 (
            //                                  isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                                 ) AS span
            //                                FROM
            //                                 (
            //                                  SELECT
            //                                   TOP 1 StdSum
            //                                  FROM
            //                                   {tableNames[j]}
            //                                  WHERE
            //                                   instantTime < '{newDate.AddDays(1)}'
            //                                  ORDER BY
            //                                   instantTime DESC
            //                                 ) r1
            //                                LEFT JOIN (
            //                                 SELECT
            //                                  TOP 1 StdSum
            //                                 FROM
            //                                  {tableNames[j]}
            //                                 WHERE
            //                                  instantTime <= '{newDate}'
            //                                 ORDER BY
            //                                  InstantTime DESC
            //                                ) r2 ON 1 = 1 ";

            //                        //addStr += $" + isnull(t{j}{i + 1}.span,0) ";
            //                        str += strHere;
            //                    }
            //                }
            //                else if (type == "day")
            //                {
            //                    var hour = Convert.ToInt32(req.startTime);
            //                    for (int i = hour; i < hour + 24; i++)
            //                    {
            //                        //var newDate = new DateTime(year, month, day, i, 0, 0);
            //                        var newDate = new DateTime();

            //                        if (i > 23)
            //                        {
            //                            newDate = new DateTime(year, month, day, 0, 0, 0).AddDays(1).AddHours(i - 24);
            //                            //newDate = new DateTime(year, month, day, i, 0, 0);
            //                        }
            //                        else
            //                            newDate = new DateTime(year, month, day, i, 0, 0);

            //                        strHere = $@" 
            //insert #temp
            //SELECT
            //                                '{newDate.ToString("yyyy/MM/dd HH:00:00")}' as dt,
            //                                 (
            //                                  isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                                 ) AS span
            //                                FROM
            //                                 (
            //                                  SELECT
            //                                   TOP 1 StdSum
            //                                  FROM
            //                                   {tableNames[j]}
            //                                  WHERE
            //                                   instantTime < '{newDate.AddHours(1).ToString("yyyy/MM/dd HH:00:00")}'
            //                                  ORDER BY
            //                                   instantTime DESC
            //                                 ) r1
            //                                LEFT JOIN (
            //                                 SELECT
            //                                  TOP 1 StdSum
            //                                 FROM
            //                                  {tableNames[j]}
            //                                 WHERE
            //                                  instantTime <= '{newDate}'
            //                                 ORDER BY
            //                                  InstantTime DESC
            //                                ) r2 ON 1 = 1 ";


            //                        //addStr += $" + isnull(t{j}{i + 1}.span,0) ";
            //                        str += strHere;

            //                    }
            //                }


            //            }
            //            //GetTempTableStr(date, req.startTime, type);
            //            //str = $@" {GetTempTableStr(date, req.startTime, type)} select t1.dt,({addStr})as span from #Temp t1 {str}";//-txy 加东西
            //            str += $@"
            //select name as dt,sum(valuess) as span from #temp group by name
            //";
            #endregion


            #region 老SQL

            //for (int j = 0; j < tableNames.Count; j++)
            //{
            //    var mmss = GetMinMaxSS(tableNames[j]);
            //    if (mmss == null)
            //        continue;
            //    var min = mmss.minSS;
            //    var max = mmss.maxSS;
            //    var strHere = "";
            //    if (type == "year")
            //    {
            //        for (int i = 1; i < 13; i++)
            //        {
            //            var newDate = new DateTime(year, i, 1);

            //            strHere = $@" left join (SELECT
            //                    '{newDate}' as dt,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS span
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableNames[j]}
            //                      WHERE
            //                       instantTime < '{newDate.AddMonths(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableNames[j]}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1) t{j}{i + 1} on t1.dt=t{j}{i + 1}.dt ";


            //            addStr += $" + isnull(t{j}{i + 1}.span,0) ";
            //            str += strHere;
            //        }
            //    }
            //    else if (type == "month")
            //    {
            //        var days = DateTime.DaysInMonth(year, month);
            //        for (int i = 1; i < days + 1; i++)
            //        {
            //            var newDate = new DateTime(year, month, i);
            //            strHere = $@" left join (SELECT
            //                    '{newDate}' as dt,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS span
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableNames[j]}
            //                      WHERE
            //                       instantTime < '{newDate.AddDays(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableNames[j]}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1) t{j}{i + 1} on t1.dt=t{j}{i + 1}.dt ";

            //            addStr += $" + isnull(t{j}{i + 1}.span,0) ";
            //            str += strHere;
            //        }
            //    }
            //    else if (type == "day")
            //    {
            //        var hour = Convert.ToInt32(req.startTime);
            //        for (int i = hour; i < hour + 24; i++)
            //        {
            //            //var newDate = new DateTime(year, month, day, i, 0, 0);
            //            var newDate = new DateTime();

            //            if (i > 23)
            //            {
            //                newDate = new DateTime(year, month, day, 0, 0, 0).AddDays(1).AddHours(i - 24);
            //                //newDate = new DateTime(year, month, day, i, 0, 0);
            //            }
            //            else
            //                newDate = new DateTime(year, month, day, i, 0, 0);

            //            strHere = $@" left join (SELECT
            //                    '{newDate.ToString("yyyy/MM/dd HH:00:00")}' as dt,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS span
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableNames[j]}
            //                      WHERE
            //                       instantTime < '{newDate.AddHours(1).ToString("yyyy/MM/dd HH:00:00")}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableNames[j]}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1) t{j}{i + 1} on t1.dt=t{j}{i + 1}.dt ";


            //            addStr += $" + isnull(t{j}{i + 1}.span,0) ";
            //            str += strHere;

            //        }
            //    }


            //}
            ////GetTempTableStr(date, req.startTime, type);
            //str = $@" {GetTempTableStr(date, req.startTime, type)} select t1.dt,({addStr})as span from #Temp t1 {str}";//-txy 加东西

            #endregion


            //var list = SqlHelper.Instance.ExecuteGetDt2<StdSumReport>(str, new Dictionary<string, string>());
            return CompleteSSR(list, type, date, req.startTime);
        }

        /// <summary>
        /// 查苏哥报表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<StdSumReport> GetHisDataListForChartForCompany2(GetStdSumReq req)
        {
            var date = Convert.ToDateTime(req.date);

            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);

            if (tableNames.Count == 0)
                return CompleteSSR(new List<StdSumReport>(), req.type, date, req.startTime);
            tableNames = GetExistHisTable(tableNames);
            tableNames = RemoveBadTable(tableNames);

            if (tableNames.Count == 0)
                return CompleteSSR(new List<StdSumReport>(), req.type, date, req.startTime);

            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            //var addStr = "t1.span";
            var str = "";
            //var oj = "";
            var type = req.type;

            var meterNos = GetDeviceIds(tableNames);
            var nosStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());

            switch (type)
            {
                case "year":
                    str = $"select year,month,sum(StdDosageVolume) as value from FMReaoprtYearly f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo  where FlMeterNo  in ({nosStr}) and year={year} GROUP BY year,month";
                    break;
                case "month":
                    str = $"select year,month,day,sum(StdDosageVolume) as value from FMReaoprtMonthly f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo  where FlMeterNo  in ({nosStr}) and year={year} and month={month} GROUP BY year,month,day";
                    break;
                case "day":
                    var hour = Convert.ToInt32(req.startTime);
                    date = date.AddHours(hour + 1);
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                    hour = date.Hour;
                    var date2 = date.AddDays(1).AddHours(hour - 1);
                    var year2 = date2.Year;
                    var month2 = date2.Month;
                    var day2 = date2.Day;
                    var hour2 = date2.Hour;

                    str = $@"SELECT
	                        YEAR,
	                        MONTH,
	                        DAY,
                        hour,
	                        SUM (StdDosageVolume) AS
                        VALUE
                        from FMReaoprtDaily f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo 
                        WHERE
                         FlMeterNo  in ({nosStr}) and  
                        ((year={year} and month={month} and day={day} and hour>={hour}) or (year={year2} and month={month2} and day={day2} and hour<{hour2}))
                        GROUP BY
	                        YEAR,
	                        MONTH,
	                        DAY,hour";
                    break;
            }
            var list2 = SqlHelper.Instance.ExecuteGetDt2<pieTime>(str, new Dictionary<string, string>());
            var list = ConvertToSSR(list2, type, "");

            return CompleteSSR(list, type, date, req.startTime);
        }

        public List<StdSumReport> CompleteSSR(List<StdSumReport> ssr, string type, DateTime date, string hourStr)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var r = new List<StdSumReport>();
            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var temp = new DateTime(year, i, 1);
                    StdSumReport s = new StdSumReport
                    {
                        dt = temp
                    };
                    var tempList = ssr.Where(p => p.dt == temp).ToList();
                    if (tempList.Count > 0)
                        s.span = tempList.First().span;
                    else
                        s.span = "0.000";
                    r.Add(s);
                }
            }
            else if (type == "month")
            {
                var days = DateTime.DaysInMonth(year, month);
                for (int i = 1; i < days + 1; i++)
                {
                    var temp = new DateTime(year, month, i);
                    StdSumReport s = new StdSumReport
                    {
                        dt = temp
                    };
                    var tempList = ssr.Where(p => p.dt == temp).ToList();
                    if (tempList.Count > 0)
                        s.span = tempList.First().span;
                    else
                        s.span = "0.000";
                    r.Add(s);
                }
            }
            else if (type == "day")
            {
                var hour = Convert.ToInt32(hourStr);
                for (int i = hour; i < hour + 24; i++)
                {
                    var iTemp = i > 23 ? i - 24 : i;
                    var temp = new DateTime();
                    if (i > 23)
                        temp = new DateTime(year, month, day, iTemp, 0, 0).AddDays(1);
                    else
                        temp = new DateTime(year, month, day, iTemp, 0, 0);
                    StdSumReport s = new StdSumReport
                    {
                        dt = temp
                    };
                    var tempList = ssr.Where(p => p.dt == temp).ToList();
                    if (tempList.Count > 0)
                        s.span = tempList.First().span;
                    else
                        s.span = "0.000";
                    r.Add(s);
                }
            }
            return r;
        }

        public List<StdSumReport> ConvertToSSR(List<pieTime> list, string type, string hourStr)
        {
            var r = new List<StdSumReport>();

            switch (type)
            {
                case "year":
                    foreach (var item in list)
                    {
                        var year = item.year;
                        var month = item.month;
                        var pr = new StdSumReport();
                        pr.dt = new DateTime(year, month, 1);
                        pr.span = item.value.ToString();
                        r.Add(pr);
                    }
                    break;
                case "month":
                    foreach (var item in list)
                    {
                        var year = item.year;
                        var month = item.month;
                        var day = item.day;
                        var pr = new StdSumReport();
                        pr.dt = new DateTime(year, month, day);
                        pr.span = item.value.ToString();
                        r.Add(pr);
                    }
                    break;
                case "day":
                    foreach (var item in list)
                    {
                        var year = item.year;
                        var month = item.month;
                        var day = item.day;
                        var hourHere = item.hour;
                        var pr = new StdSumReport();
                        pr.dt = new DateTime(year, month, day, hourHere, 0, 0);
                        pr.span = item.value.ToString();
                        r.Add(pr);
                    }

                    break;
                default:
                    break;
            }

            return r;
        }

        public List<PieRes> GetPieListByCustType(GetStdSumReq req, string tableName)
        {

            var r = new List<PieRes>();//民用，公建，工业
            var date = Convert.ToDateTime(req.date);
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var type = req.type;

            var mmss = GetMinMaxSS(tableName);
            //if (mmss == null)
            //{
            //    var temp = new List<StdSumReport>();
            //    return CompleteSSR(temp, type, date);
            //}
            var min = mmss.minSS;
            var max = mmss.maxSS;

            var newDate = new DateTime();
            var newDate2 = new DateTime();

            string str = "";
            switch (type)
            {
                case "year":
                    newDate = new DateTime(year, 1, 1);
                    newDate2 = newDate.AddYears(1);
                    break;
                case "month":
                    newDate = new DateTime(year, month, 1);
                    newDate2 = newDate.AddMonths(1);
                    break;
                case "day":
                    var hour = Convert.ToInt32(req.startTime);
                    newDate = new DateTime(year, month, day, hour, 0, 0);
                    newDate2 = newDate.AddDays(1);
                    break;
            }

            str = $@"SELECT
	'居民' AS name,
	(
		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	) AS value
FROM
	(
		SELECT
			TOP 1 StdSum 
		FROM
			{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
		WHERE
			instantTime < '{newDate2}' and  h.CustTypeName='居民'
		ORDER BY
			instantTime DESC
	) r1
LEFT JOIN (
	SELECT
		TOP 1 StdSum
	FROM
		{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
	WHERE
		instantTime <= '{newDate}' and  h.CustTypeName='居民'
	ORDER BY
		InstantTime DESC
) r2 ON 1 = 1
union 
SELECT
	'公建' AS name,
	(
		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	) AS value
FROM
	(
		SELECT
			TOP 1 StdSum 
		FROM
			{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
		WHERE
			instantTime < '{newDate2}' and  h.CustTypeName='公建'
		ORDER BY
			instantTime DESC
	) r1
LEFT JOIN (
	SELECT
		TOP 1 StdSum
	FROM
		{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
	WHERE
		instantTime <= '{newDate}' and  h.CustTypeName='公建'
	ORDER BY
		InstantTime DESC
) r2 ON 1 = 1
union 
SELECT
	'工业' AS name,
	(
		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	) AS value
FROM
	(
		SELECT
			TOP 1 StdSum 
		FROM
			{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
		WHERE
			instantTime < '{newDate2}' and  h.CustTypeName='工业'
		ORDER BY
			instantTime DESC
	) r1
LEFT JOIN (
	SELECT
		TOP 1 StdSum
	FROM
		{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
	WHERE
		instantTime <= '{newDate}' and  h.CustTypeName='工业'
	ORDER BY
		InstantTime DESC
) r2 ON 1 = 1
union 
SELECT
	'商福' AS name,
	(
		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	) AS value
FROM
	(
		SELECT
			TOP 1 StdSum 
		FROM
			{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
		WHERE
			instantTime < '{newDate2}' and  h.CustTypeName='商福'
		ORDER BY
			instantTime DESC
	) r1
LEFT JOIN (
	SELECT
		TOP 1 StdSum
	FROM
		{tableName} LEFT JOIN HHMDeviceView h ON {tableName}.communicateNo = h.communicateNo
	WHERE
		instantTime <= '{newDate}' and  h.CustTypeName='商福'
	ORDER BY
		InstantTime DESC
) r2 ON 1 = 1";

            r = SqlHelper.Instance.ExecuteGetDt2<PieRes>(str, new Dictionary<string, string>());
            r = CompletePieForType(r);
            return r;
        }

        public List<StdSum4> GetStdSumCache(List<string> tableNames)
        {
            string astr = $@"create table #Temp
                                    (
                                    FLMeterNo nvarchar(50),
                                    StdSum nvarchar(50),
                                    InstantTime nvarchar(50),
                                    CustTypeName nvarchar(50)
                                    ) ";
            //var dt = new DateTime(date.Year, 1, 1);
            foreach (var item in tableNames)
            {
                astr += $@"
                            insert #temp
                            select FLMeterNo,StdSum,InstantTime,CustTypeName from {item} left join HHMDeviceView h ON {item}.FLMeterNo = h.meterNo  
                            ";
            }
            astr += $@"
                             select * from #temp
                            ";

            var list = SqlHelper.Instance.ExecuteGetDt2<StdSum4>(astr, new Dictionary<string, string>());
            return list;
            //var listNo = list.Select(p => p.FLMeterNo).Distinct().ToList();
            //var r = new List<StdSum4>();
            //foreach (var item in listNo)
            //{
            //    var temp = list.Where(p => p.FLMeterNo == item).ToList();
            //    var start = list.OrderBy(p => p.InstantTime).First().InstantTime;
            //    var end = list.OrderByDescending(p => p.InstantTime).First().InstantTime;

            //}

        }

        public List<PieRes> GetPieListByCustTypeForCompany(GetStdSumReq req)
        {
            var date = Convert.ToDateTime(req.date);
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);
            if (tableNames.Count == 0)
                return CompletePieForType(new List<PieRes>());
            tableNames = GetExistHisTable(tableNames);
            tableNames = RemoveBadTable(tableNames);
            if (tableNames.Count == 0)
                return CompletePieForType(new List<PieRes>());
            var r = new List<PieRes>();//民用，公建，工业
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var type = req.type;
            //var newDate = new DateTime();
            //var newDate2 = new DateTime();
            string str = "";
            var meterNos = GetDeviceIds(tableNames);
            var nosStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());
            #region 报表里查询
            switch (type)
            {
                case "year":
                    str = $"select custtypeName name,sum(stdsum) value from FMReportYear where FlMeterNo  in ({nosStr}) and year={year} GROUP BY custTypeName";
                    break;
                case "month":
                    str = $"select custtypeName name,sum(stdsum) value from FMReportMonth where FlMeterNo  in ({nosStr}) and year={year} and month={month} GROUP BY custTypeName";
                    break;
                case "day":
                    var hour = Convert.ToInt32(req.startTime);
                    date = date.AddHours(hour + 1);
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                    hour = date.Hour;
                    //newDate = new DateTime(year, month, day, hour, 0, 0);
                    //newDate2 = newDate.AddDays(1);
                    var date2 = date.AddDays(1).AddHours(hour - 1);
                    var year2 = date2.Year;
                    var month2 = date2.Month;
                    var day2 = date2.Day;
                    var hour2 = date2.Hour;

                    str = $"select custtypeName name,sum(stdsum) value from FMReportDay where FlMeterNo  in ({nosStr}) and ((year={year} and month={month} and day={day} and hour>={hour}) or (year={year2} and month={month2} and day={day2} and hour<{hour2}))GROUP BY custTypeName";
                    break;
            }
            #endregion


            #region 新SQL
            //            str = $@"create table #temp(
            //name nvarchar(50),
            //valuess DECIMAL(12,2)
            //)
            //";

            //            for (int j = 0; j < tableNames.Count; j++)
            //            {
            //                var mmss = GetMinMaxSS(tableNames[j]);
            //                var min = mmss.minSS;
            //                var max = mmss.maxSS;

            //                #region Sql
            //                switch (type)
            //                {
            //                    case "year":
            //                        newDate = new DateTime(year, 1, 1);
            //                        newDate2 = newDate.AddYears(1);
            //                        break;
            //                    case "month":
            //                        newDate = new DateTime(year, month, 1);
            //                        newDate2 = newDate.AddMonths(1);
            //                        break;
            //                    case "day":
            //                        var hour = Convert.ToInt32(req.startTime);
            //                        newDate = new DateTime(year, month, day, hour, 0, 0);
            //                        newDate2 = newDate.AddDays(1);
            //                        break;
            //                }
            //                //addStr += $"+isnull(tt{j}.value,0)";
            //                str += $@"	 insert #temp 
            //SELECT
            //	'居民' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='居民'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='居民'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1
            //insert #temp
            //SELECT
            //	'公建' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='公建'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='公建'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1
            //insert #temp 
            //SELECT
            //	'工业' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='工业'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='工业'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1
            //insert #temp
            //SELECT
            //	'商福' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='商福'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='商福'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1 
            //";
            //            }

            //            //           str = $@"{GetTempTableStrForPieType()} SELECT
            //            //	t1.name,({addStr}) as value
            //            //FROM
            //            //	#temp t1 {str}";
            //            str += $@"
            //select name as name,sum(valuess) as value from #temp group by name
            //";
            //            #endregion
            #endregion

            #region 老SQL


            //            for (int j = 0; j < tableNames.Count; j++)
            //            {
            //                var mmss = GetMinMaxSS(tableNames[j]);
            //                var min = mmss.minSS;
            //                var max = mmss.maxSS;

            //                #region Sql
            //                switch (type)
            //                {
            //                    case "year":
            //                        newDate = new DateTime(year, 1, 1);
            //                        newDate2 = newDate.AddYears(1);
            //                        break;
            //                    case "month":
            //                        newDate = new DateTime(year, month, 1);
            //                        newDate2 = newDate.AddMonths(1);
            //                        break;
            //                    case "day":
            //                        var hour = Convert.ToInt32(req.startTime);
            //                        newDate = new DateTime(year, month, day, hour, 0, 0);
            //                        newDate2 = newDate.AddDays(1);
            //                        break;
            //                }
            //                addStr += $"+isnull(tt{j}.value,0)";
            //                str += $@"	 LEFT JOIN (SELECT
            //	'居民' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='居民'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='居民'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1
            //union 
            //SELECT
            //	'公建' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='公建'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='公建'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1
            //union 
            //SELECT
            //	'工业' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='工业'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='工业'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1
            //union 
            //SELECT
            //	'商福' AS name,
            //	(
            //		isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //	) AS value
            //FROM
            //	(
            //		SELECT
            //			TOP 1 StdSum 
            //		FROM
            //			{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //		WHERE
            //			instantTime < '{newDate2}' and  h.CustTypeName='商福'
            //		ORDER BY
            //			instantTime DESC
            //	) r1
            //LEFT JOIN (
            //	SELECT
            //		TOP 1 StdSum
            //	FROM
            //		{tableNames[j]} LEFT JOIN HHMDeviceView h ON {tableNames[j]}.FLMeterNo = h.meterNo
            //	WHERE
            //		instantTime <= '{newDate}' and  h.CustTypeName='商福'
            //	ORDER BY
            //		InstantTime DESC
            //) r2 ON 1 = 1 ) tt{j} on t1.name=tt{j}.name ";
            //            }

            //            str = $@"{GetTempTableStrForPieType()} SELECT
            //		t1.name,({addStr}) as value
            //	FROM
            //		#temp t1 {str}"; 
            //            #endregion
            #endregion

            r = SqlHelper.Instance.ExecuteGetDt2<PieRes>(str, new Dictionary<string, string>());
            r = CompletePieForType(r);
            return r;
        }

        /// <summary>
        /// 查苏哥的报表，还未使用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PieRes> GetPieListByCustTypeForCompany2(GetStdSumReq req)
        {
            var date = Convert.ToDateTime(req.date);
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);
            if (tableNames.Count == 0)
                return CompletePieForType(new List<PieRes>());
            tableNames = GetExistHisTable(tableNames);
            tableNames = RemoveBadTable(tableNames);
            if (tableNames.Count == 0)
                return CompletePieForType(new List<PieRes>());
            var r = new List<PieRes>();//民用，公建，工业
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var type = req.type;

            string str = "";
            var meterNos = GetDeviceIds(tableNames);
            var nosStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());
            #region 报表里查询
            switch (type)
            {
                case "year":
                    str = $"select  custtypeName name,sum(StdDosageVolume) value from FMReaoprtYearly f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo where FlMeterNo  in ({nosStr}) and year={year} GROUP BY custTypeName";
                    break;
                case "month":
                    str = $"select  custtypeName name,sum(StdDosageVolume) value from FMReaoprtMonthly f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo where FlMeterNo  in ({nosStr}) and year={year} and month={month} GROUP BY custTypeName";
                    break;
                case "day":
                    var hour = Convert.ToInt32(req.startTime);
                    date = date.AddHours(hour + 1);
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                    hour = date.Hour;
                    //newDate = new DateTime(year, month, day, hour, 0, 0);
                    //newDate2 = newDate.AddDays(1);
                    var date2 = date.AddDays(1).AddHours(hour - 1);
                    var year2 = date2.Year;
                    var month2 = date2.Month;
                    var day2 = date2.Day;
                    var hour2 = date2.Hour;

                    str = $"select  custtypeName name,sum(StdDosageVolume) value from FMReaoprtDaily f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo where FlMeterNo  in ({nosStr}) and ((year={year} and month={month} and day={day} and hour>={hour}) or (year={year2} and month={month2} and day={day2} and hour<{hour2}))GROUP BY custTypeName";
                    break;
            }
            #endregion

            r = SqlHelper.Instance.ExecuteGetDt2<PieRes>(str, new Dictionary<string, string>());
            r = CompletePieForType(r);
            return r;
        }

        public List<PieRes> CompletePieForType(List<PieRes> list)
        {
            var r = new List<PieRes>
            {
                new PieRes { name = "居民", value = "0" },
                new PieRes { name = "工业", value = "0" },
                new PieRes { name = "公建", value = "0" },
                new PieRes { name = "商福", value = "0" }
            };

            for (int i = 0; i < r.Count; i++)
            {
                var temp = list.Where(p => p.name == r[i].name).ToList();
                if (temp.Count > 0)
                    r[i].value = temp.First().value;
            }
            return r;

        }

        public List<PieRes> GetPieListByTime(GetStdSumReq req, string tableName)
        {
            var date = Convert.ToDateTime(req.date);
            var type = req.type;
            var mmss = GetMinMaxSS(tableName);
            //if (mmss == null)
            //{
            //    var temp = new List<StdSumReport>();
            //    return CompleteSSR(temp, type, date);
            //}
            var min = mmss.minSS;
            var max = mmss.maxSS;

            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;


            var str = "";

            #region 新SQL
            str += $@"
create table #temp(
name nvarchar(50),
valuess DECIMAL(12,2)
)
";

            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var newDate = new DateTime(year, i, 1);

                    str += $@"
insert #temp
SELECT
                                '{newDate.ToString("yyyy-MM")}' as name,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS value
                                FROM
	                                (
		                                SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddMonths(1)}'
		                                ORDER BY
			                                instantTime DESC
	                                ) r1
                                LEFT JOIN (
	                                SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC
                                ) r2 ON 1 = 1";

                    //else
                    //{
                    //    str += $@" union SELECT
                    //            '{newDate.ToString("yyyy-MM")}' as name,
                    //             (
                    //              isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
                    //             ) AS value
                    //            FROM
                    //             (
                    //              SELECT
                    //               TOP 1 StdSum
                    //              FROM
                    //               {tableName}
                    //              WHERE
                    //               instantTime < '{newDate.AddMonths(1)}'
                    //              ORDER BY
                    //               instantTime DESC
                    //             ) r1
                    //            LEFT JOIN (
                    //             SELECT
                    //              TOP 1 StdSum
                    //             FROM
                    //              {tableName}
                    //             WHERE
                    //              instantTime <= '{newDate}'
                    //             ORDER BY
                    //              InstantTime DESC
                    //            ) r2 ON 1 = 1";
                    //}
                }
            }
            else if (type == "month")
            {
                var days = DateTime.DaysInMonth(year, month);
                for (int i = 1; i < days + 1; i++)
                {
                    var newDate = new DateTime(year, month, i);
                    //if (i == 1)
                    //{
                    str += $@"
                                    insert #temp
                                    SELECT
                                '{newDate.ToString("dd号")}' as name,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS value
                                FROM
	                                (
		                                SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddDays(1)}'
		                                ORDER BY
			                                instantTime DESC
	                                ) r1
                                LEFT JOIN (
	                                SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC
                                ) r2 ON 1 = 1";
                    //}
                    //else
                    //{
                    //    str += $@" union SELECT
                    //            '{newDate.ToString("dd号")}' as name,
                    //             (
                    //              isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
                    //             ) AS value
                    //            FROM
                    //             (
                    //              SELECT
                    //               TOP 1 StdSum
                    //              FROM
                    //               {tableName}
                    //              WHERE
                    //               instantTime < '{newDate.AddDays(1)}'
                    //              ORDER BY
                    //               instantTime DESC
                    //             ) r1
                    //            LEFT JOIN (
                    //             SELECT
                    //              TOP 1 StdSum
                    //             FROM
                    //              {tableName}
                    //             WHERE
                    //              instantTime <= '{newDate}'
                    //             ORDER BY
                    //              InstantTime DESC
                    //            ) r2 ON 1 = 1";
                    //}
                }
            }
            else if (type == "day")
            {

                var hour = Convert.ToInt32(req.startTime);
                for (int i = hour; i < hour + 24; i = i + 2)
                {
                    //var newDate = new DateTime(year, month, day, i, 0, 0);

                    var newDate = new DateTime();

                    if (i > 23)
                    {
                        newDate = new DateTime(year, month, day, 0, 0, 0).AddDays(1).AddHours(i - 24);
                        //newDate = new DateTime(year, month, day, i, 0, 0);
                    }
                    else
                        newDate = new DateTime(year, month, day, i, 0, 0);
                    var lastHour = i + 2;
                    if (lastHour > 24)
                        lastHour -= 24;
                    //if (i == hour)
                    //{
                    str += $@"
insert #temp
SELECT
                                '{i}时-{lastHour}时' as name,
	                                (
		                                isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
	                                ) AS value
                                FROM
	                                (
		                                SELECT
			                                TOP 1 StdSum
		                                FROM
			                                {tableName}
		                                WHERE
			                                instantTime < '{newDate.AddHours(1)}'
		                                ORDER BY
			                                instantTime DESC
	                                ) r1
                                LEFT JOIN (
	                                SELECT
		                                TOP 1 StdSum
	                                FROM
		                                {tableName}
	                                WHERE
		                                instantTime <= '{newDate}'
	                                ORDER BY
		                                InstantTime DESC
                                ) r2 ON 1 = 1";
                    //}
                    //else
                    //{
                    //    str += $@" union SELECT
                    //            '{i}时-{lastHour}时' as name,
                    //             (
                    //              isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
                    //             ) AS value
                    //            FROM
                    //             (
                    //              SELECT
                    //               TOP 1 StdSum
                    //              FROM
                    //               {tableName}
                    //              WHERE
                    //               instantTime < '{newDate.AddHours(1)}'
                    //              ORDER BY
                    //               instantTime DESC
                    //             ) r1
                    //            LEFT JOIN (
                    //             SELECT
                    //              TOP 1 StdSum
                    //             FROM
                    //              {tableName}
                    //             WHERE
                    //              instantTime <= '{newDate}'
                    //             ORDER BY
                    //              InstantTime DESC
                    //            ) r2 ON 1 = 1";
                    //}
                }
            }
            str += $@"
select name,sum(valuess) as value from #temp group by name
";
            #endregion

            #region 老SQL
            //if (type == "year")
            //{
            //    for (int i = 1; i < 13; i++)
            //    {
            //        var newDate = new DateTime(year, i, 1);
            //        if (i == 1)
            //        {
            //            str += $@"SELECT
            //                    '{newDate.ToString("yyyy-MM")}' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableName}
            //                      WHERE
            //                       instantTime < '{newDate.AddMonths(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableName}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1";
            //        }
            //        else
            //        {
            //            str += $@" union SELECT
            //                    '{newDate.ToString("yyyy-MM")}' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableName}
            //                      WHERE
            //                       instantTime < '{newDate.AddMonths(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableName}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1";
            //        }
            //    }
            //}
            //else if (type == "month")
            //{
            //    var days = DateTime.DaysInMonth(year, month);
            //    for (int i = 1; i < days + 1; i++)
            //    {
            //        var newDate = new DateTime(year, month, i);
            //        if (i == 1)
            //        {
            //            str += $@"SELECT
            //                    '{newDate.ToString("dd号")}' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableName}
            //                      WHERE
            //                       instantTime < '{newDate.AddDays(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableName}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1";
            //        }
            //        else
            //        {
            //            str += $@" union SELECT
            //                    '{newDate.ToString("dd号")}' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableName}
            //                      WHERE
            //                       instantTime < '{newDate.AddDays(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableName}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1";
            //        }
            //    }
            //}
            //else if (type == "day")
            //{

            //    var hour = Convert.ToInt32(req.startTime);
            //    for (int i = hour; i < hour + 24; i = i + 2)
            //    {
            //        //var newDate = new DateTime(year, month, day, i, 0, 0);

            //        var newDate = new DateTime();

            //        if (i > 23)
            //        {
            //            newDate = new DateTime(year, month, day, 0, 0, 0).AddDays(1).AddHours(i - 24);
            //            //newDate = new DateTime(year, month, day, i, 0, 0);
            //        }
            //        else
            //            newDate = new DateTime(year, month, day, i, 0, 0);
            //        var lastHour = i + 2;
            //        if (lastHour > 24)
            //            lastHour -= 24;
            //        if (i == hour)
            //        {
            //            str += $@"SELECT
            //                    '{i}时-{lastHour}时' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableName}
            //                      WHERE
            //                       instantTime < '{newDate.AddHours(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableName}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1";
            //        }
            //        else
            //        {
            //            str += $@" union SELECT
            //                    '{i}时-{lastHour}时' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableName}
            //                      WHERE
            //                       instantTime < '{newDate.AddHours(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableName}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1";
            //        }
            //    }
            //} 
            #endregion

            var list = SqlHelper.Instance.ExecuteGetDt2<PieRes>(str, new Dictionary<string, string>());
            return CompletePieForTime(list, type, date, req.startTime);
            //return list;
            //return CompleteSSR(list, type, date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PieRes> GetPieListByTimeForCompany(GetStdSumReq req)
        {
            var date = Convert.ToDateTime(req.date);
            var type = req.type;
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);

            if (tableNames.Count == 0)
                return CompletePieForTime(new List<PieRes>(), type, date, req.startTime);
            tableNames = GetExistHisTable(tableNames);
            if (tableNames.Count == 0)
                return CompletePieForTime(new List<PieRes>(), type, date, req.startTime);

            tableNames = RemoveBadTable(tableNames);
            var meterNos = GetDeviceIds(tableNames);
            var nosStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());

            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            //var addStr = "t1.value";
            var str = "";
            switch (type)
            {
                case "year":
                    str = $"select year,month,sum(stdsum) as value from FMReportYear where FlMeterNo  in ({nosStr}) and year={year} GROUP BY year,month";
                    break;
                case "month":
                    str = $"select year,month,day,sum(stdsum) as value from FMReportMonth where FlMeterNo  in ({nosStr}) and year={year} and month={month} GROUP BY year,month,day";
                    break;
                case "day":
                    var hour = Convert.ToInt32(req.startTime);
                    date = date.AddHours(hour + 1);
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                    hour = date.Hour;
                    var date2 = date.AddDays(1).AddHours(hour - 1);
                    var year2 = date2.Year;
                    var month2 = date2.Month;
                    var day2 = date2.Day;
                    var hour2 = date2.Hour;

                    str = $@"SELECT
	                        YEAR,
	                        MONTH,
	                        DAY,
                        hour,
	                        SUM (stdsum) AS
                        VALUE
                        FROM
	                        FMReportDay
                        WHERE
                         FlMeterNo  in ({nosStr}) and  
                        ((year={year} and month={month} and day={day} and hour>={hour}) or (year={year2} and month={month2} and day={day2} and hour<{hour2}))
                        GROUP BY
	                        YEAR,
	                        MONTH,
	                        DAY,hour";
                    break;
            }

            var list2 = SqlHelper.Instance.ExecuteGetDt2<pieTime>(str, new Dictionary<string, string>());
            return ConvertPieTimeToPieRes(list2, type, req.startTime);
            #region 新SQL
            //            str += $@"
            //                        create table #temp(
            //                        name nvarchar(50),
            //                        valuess DECIMAL(12,2)
            //                        )
            //";
            //            for (int j = 0; j < tableNames.Count; j++)
            //            {
            //                var mmss = GetMinMaxSS(tableNames[j]);
            //                if (mmss == null)
            //                    continue;
            //                var min = mmss.minSS;
            //                var max = mmss.maxSS;
            //                var strHere = "";
            //                if (type == "year")
            //                {
            //                    for (int i = 1; i < 13; i++)
            //                    {
            //                        var newDate = new DateTime(year, i, 1);

            //                        strHere = $@" 
            //insert #temp
            //SELECT
            //                                '{newDate.ToString("yyyy-MM")}' as name,
            //                                 (
            //                                  isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                                 ) AS value
            //                                FROM
            //                                 (
            //                                  SELECT
            //                                   TOP 1 StdSum
            //                                  FROM
            //                                   {tableNames[j]}
            //                                  WHERE
            //                                   instantTime < '{newDate.AddMonths(1)}'
            //                                  ORDER BY
            //                                   instantTime DESC
            //                                 ) r1
            //                                LEFT JOIN (
            //                                 SELECT
            //                                  TOP 1 StdSum
            //                                 FROM
            //                                  {tableNames[j]}
            //                                 WHERE
            //                                  instantTime <= '{newDate}'
            //                                 ORDER BY
            //                                  InstantTime DESC
            //                                ) r2 ON 1 = 1 ";


            //                        //addStr += $" + isnull(t{j}t{i + 1}.value,0) ";
            //                        str += strHere;
            //                    }
            //                }
            //                else if (type == "month")
            //                {
            //                    var days = DateTime.DaysInMonth(year, month);
            //                    for (int i = 1; i < days + 1; i++)
            //                    {
            //                        var newDate = new DateTime(year, month, i);
            //                        strHere = $@" 
            //insert #temp
            //SELECT
            //                                '{newDate.ToString("dd号")}' as name,
            //                                 (
            //                                  isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                                 ) AS value
            //                                FROM
            //                                 (
            //                                  SELECT
            //                                   TOP 1 StdSum
            //                                  FROM
            //                                   {tableNames[j]}
            //                                  WHERE
            //                                   instantTime < '{newDate.AddDays(1)}'
            //                                  ORDER BY
            //                                   instantTime DESC
            //                                 ) r1
            //                                LEFT JOIN (
            //                                 SELECT
            //                                  TOP 1 StdSum
            //                                 FROM
            //                                  {tableNames[j]}
            //                                 WHERE
            //                                  instantTime <= '{newDate}'
            //                                 ORDER BY
            //                                  InstantTime DESC
            //                                ) r2 ON 1 = 1 ";

            //                        addStr += $" + isnull(t{j}t{i + 1}.value,0) ";
            //                        str += strHere;
            //                    }
            //                }
            //                else if (type == "day")
            //                {
            //                    var hour = Convert.ToInt32(req.startTime);
            //                    for (int i = hour; i < hour + 24; i = i + 2)
            //                    {
            //                        //var newDate = new DateTime(year, month, day, i, 0, 0);
            //                        var newDate = new DateTime();

            //                        if (i > 23)
            //                        {
            //                            newDate = new DateTime(year, month, day, 0, 0, 0).AddDays(1).AddHours(i - 24);
            //                            //newDate = new DateTime(year, month, day, i, 0, 0);
            //                        }
            //                        else
            //                            newDate = new DateTime(year, month, day, i, 0, 0);
            //                        var iTemp = i > 23 ? i - 24 : i;

            //                        var lastHour = i + 2;
            //                        if (lastHour > 24)
            //                            lastHour -= 24;
            //                        strHere = $@" 
            //insert #temp
            //SELECT
            //                                '{iTemp}时-{lastHour}时' as name,
            //                                 (
            //                                  isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                                 ) AS value
            //                                FROM
            //                                 (
            //                                  SELECT
            //                                   TOP 1 StdSum
            //                                  FROM
            //                                   {tableNames[j]}
            //                                  WHERE
            //                                   instantTime < '{newDate.AddHours(2)}'
            //                                  ORDER BY
            //                                   instantTime DESC
            //                                 ) r1
            //                                LEFT JOIN (
            //                                 SELECT
            //                                  TOP 1 StdSum
            //                                 FROM
            //                                  {tableNames[j]}
            //                                 WHERE
            //                                  instantTime <= '{newDate}'
            //                                 ORDER BY
            //                                  InstantTime DESC
            //                                ) r2 ON 1 = 1 ";


            //                        //addStr += $" + isnull(t{j}t{i + 1}.value,0) ";
            //                        str += strHere;

            //                    }
            //                }


            //            }


            //            //GetTempTableStr(date, req.startTime, type);
            //            //str = $@" {GetTempTableStrForPie(date, req.startTime, type)} select t1.name,({addStr})as value from #Temp t1 {str}";//-txy 加东西
            //            str += $@"
            //select name,sum(valuess) as value from #temp group by name
            //";
            #endregion

            #region 老SQL

            //for (int j = 0; j < tableNames.Count; j++)
            //{
            //    var mmss = GetMinMaxSS(tableNames[j]);
            //    if (mmss == null)
            //        continue;
            //    var min = mmss.minSS;
            //    var max = mmss.maxSS;
            //    var strHere = "";
            //    if (type == "year")
            //    {
            //        for (int i = 1; i < 13; i++)
            //        {
            //            var newDate = new DateTime(year, i, 1);

            //            strHere = $@" left join (SELECT
            //                    '{newDate.ToString("yyyy-MM")}' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableNames[j]}
            //                      WHERE
            //                       instantTime < '{newDate.AddMonths(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableNames[j]}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1) t{j}t{i + 1} on t1.name=t{j}t{i + 1}.name ";


            //            addStr += $" + isnull(t{j}t{i + 1}.value,0) ";
            //            str += strHere;
            //        }
            //    }
            //    else if (type == "month")
            //    {
            //        var days = DateTime.DaysInMonth(year, month);
            //        for (int i = 1; i < days + 1; i++)
            //        {
            //            var newDate = new DateTime(year, month, i);
            //            strHere = $@" left join (SELECT
            //                    '{newDate.ToString("dd号")}' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableNames[j]}
            //                      WHERE
            //                       instantTime < '{newDate.AddDays(1)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableNames[j]}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1) t{j}t{i + 1} on t1.name=t{j}t{i + 1}.name ";

            //            addStr += $" + isnull(t{j}t{i + 1}.value,0) ";
            //            str += strHere;
            //        }
            //    }
            //    else if (type == "day")
            //    {
            //        var hour = Convert.ToInt32(req.startTime);
            //        for (int i = hour; i < hour + 24; i = i + 2)
            //        {
            //            //var newDate = new DateTime(year, month, day, i, 0, 0);
            //            var newDate = new DateTime();

            //            if (i > 23)
            //            {
            //                newDate = new DateTime(year, month, day, 0, 0, 0).AddDays(1).AddHours(i - 24);
            //                //newDate = new DateTime(year, month, day, i, 0, 0);
            //            }
            //            else
            //                newDate = new DateTime(year, month, day, i, 0, 0);
            //            var iTemp = i > 23 ? i - 24 : i;

            //            var lastHour = i + 2;
            //            if (lastHour > 24)
            //                lastHour -= 24;
            //            strHere = $@" left join (SELECT
            //                    '{iTemp}时-{lastHour}时' as name,
            //                     (
            //                      isnull(r1.StdSum, {max}) - isnull(r2.StdSum, {min})
            //                     ) AS value
            //                    FROM
            //                     (
            //                      SELECT
            //                       TOP 1 StdSum
            //                      FROM
            //                       {tableNames[j]}
            //                      WHERE
            //                       instantTime < '{newDate.AddHours(2)}'
            //                      ORDER BY
            //                       instantTime DESC
            //                     ) r1
            //                    LEFT JOIN (
            //                     SELECT
            //                      TOP 1 StdSum
            //                     FROM
            //                      {tableNames[j]}
            //                     WHERE
            //                      instantTime <= '{newDate}'
            //                     ORDER BY
            //                      InstantTime DESC
            //                    ) r2 ON 1 = 1) t{j}t{i + 1} on t1.name=t{j}t{i + 1}.name ";


            //            addStr += $" + isnull(t{j}t{i + 1}.value,0) ";
            //            str += strHere;

            //        }
            //    }


            //}
            ////GetTempTableStr(date, req.startTime, type);
            //str = $@" {GetTempTableStrForPie(date, req.startTime, type)} select t1.name,({addStr})as value from #Temp t1 {str}";//-txy 加东西

            #endregion

            //var list = SqlHelper.Instance.ExecuteGetDt2<PieRes>(str, new Dictionary<string, string>());
            //return CompletePieForTime(list, type, date, req.startTime);

        }

        /// <summary>
        /// 查苏哥的报表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PieRes> GetPieListByTimeForCompany2(GetStdSumReq req)
        {
            var date = Convert.ToDateTime(req.date);
            var type = req.type;
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);

            if (tableNames.Count == 0)
                return CompletePieForTime(new List<PieRes>(), type, date, req.startTime);
            tableNames = GetExistHisTable(tableNames);
            if (tableNames.Count == 0)
                return CompletePieForTime(new List<PieRes>(), type, date, req.startTime);

            tableNames = RemoveBadTable(tableNames);
            var meterNos = GetDeviceIds(tableNames);
            var nosStr = StringHelper.Instance.ArrJoin(meterNos.ToArray());

            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            //var addStr = "t1.value";
            var str = "";
            switch (type)
            {
                case "year":
                    str = $"select year,month,sum(StdDosageVolume) as value from FMReaoprtYearly f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo  where FlMeterNo  in ({nosStr}) and year={year} GROUP BY year,month";
                    break;
                case "month":
                    str = $"select year,month,day,sum(StdDosageVolume) as value from FMReaoprtMonthly f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo  where FlMeterNo  in ({nosStr}) and year={year} and month={month} GROUP BY year,month,day";
                    break;
                case "day":
                    var hour = Convert.ToInt32(req.startTime);
                    date = date.AddHours(hour + 1);
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                    hour = date.Hour;
                    var date2 = date.AddDays(1).AddHours(hour - 1);
                    var year2 = date2.Year;
                    var month2 = date2.Month;
                    var day2 = date2.Day;
                    var hour2 = date2.Hour;

                    str = $@"SELECT
	                        YEAR,
	                        MONTH,
	                        DAY,
                        hour,
	                        SUM (StdDosageVolume) AS
                        VALUE
                        from FMReaoprtDaily f LEFT JOIN HHMDeviceView h on f.FLMeterNo=h.meterNo 
                        WHERE
                         FlMeterNo  in ({nosStr}) and  
                        ((year={year} and month={month} and day={day} and hour>={hour}) or (year={year2} and month={month2} and day={day2} and hour<{hour2}))
                        GROUP BY
	                        YEAR,
	                        MONTH,
	                        DAY,hour";
                    break;
            }

            var list2 = SqlHelper.Instance.ExecuteGetDt2<pieTime>(str, new Dictionary<string, string>());
            return ConvertPieTimeToPieRes(list2, type, req.startTime);
        }

        public List<PieRes> ConvertPieTimeToPieRes(List<pieTime> list, string type, string hourStr)
        {
            var r = new List<PieRes>();
            switch (type)
            {
                case "year":
                    foreach (var item in list)
                    {
                        var year = item.year;
                        var month = item.month;
                        var pr = new PieRes();
                        pr.name = new DateTime(year, month, 1).ToString("yyyy-MM");
                        pr.value = item.value.ToString();
                        r.Add(pr);
                    }
                    break;
                case "month":
                    foreach (var item in list)
                    {
                        var year = item.year;
                        var month = item.month;
                        var day = item.day;
                        var pr = new PieRes();
                        pr.name = new DateTime(year, month, day).ToString("dd号");
                        pr.value = item.value.ToString();
                        r.Add(pr);
                    }
                    break;
                case "day":
                    foreach (var item in list)
                    {
                        var year = item.year;
                        var month = item.month;
                        var day = item.day;
                        var hourHere = item.hour;
                        var pr = new PieRes();
                        pr.name = new DateTime(year, month, day, hourHere, 0, 0).ToString("dd号hh时");
                        pr.value = item.value.ToString();
                        r.Add(pr);
                    }
                    ////var listName = new List<string>();
                    ////if (hour % 2 == 0)//如果被2整除
                    ////{
                    ////    listName = new List<string> {
                    ////       "0时-2时",
                    ////        "2时-4时",
                    ////        "4时-6时",
                    ////        "6时-8时",
                    ////        "8时-10时",
                    ////        "10时-12时",
                    ////        "12时-14时",
                    ////        "14时-16时",
                    ////        "16时-18时",
                    ////        "18时-20时",
                    ////        "20时-22时",
                    ////        "22时-0时",
                    ////    };
                    ////}
                    ////else
                    ////{
                    ////    listName = new List<string> {
                    ////        "1时-3时",
                    ////        "3时-5时",
                    ////        "5时-7时",
                    ////        "7时-9时",
                    ////        "9时-11时",
                    ////        "11时-13时",
                    ////        "13时-15时",
                    ////        "15时-17时",
                    ////        "17时-19时",
                    ////        "19时-21时",
                    ////        "21时-23时",
                    ////        "23时-1时",
                    ////    };
                    ////}

                    //for (int i = hour; i < hour + 24; i++)
                    //{
                    //    var iTemp = i > 23 ? i - 24 : i;
                    //    var lastHour = i + 2;
                    //    if (lastHour > 24)
                    //        lastHour -= 24;
                    //    //var temp = new DateTime(year, month, day, i, 0, 0);
                    //    PieRes s = new PieRes
                    //    {
                    //        name = $"{iTemp}时-{lastHour}时"
                    //    };
                    //    s.value = "0.000";
                    //    r.Add(s);
                    //}
                    //if (hour % 2 == 0)
                    //{
                    //    foreach (var item in list)
                    //    {
                    //        var year = item.year;
                    //        var month = item.month;
                    //        var day = item.day;
                    //        var hourHere = item.hour;
                    //        var pr = new PieRes();
                    //        pr.name = new DateTime(year, month, day).ToString("dd号");
                    //        pr.value = item.value.ToString();
                    //        r.Add(pr);
                    //    }
                    //}
                    //else {

                    //}
                    ////foreach (var item in list)
                    ////{
                    ////    var year = item.year;
                    ////    var month = item.month;
                    ////    var day = item.day;
                    ////    var hourHere = item.hour;
                    ////    var pr = new PieRes();
                    ////    pr.name = new DateTime(year, month, day).ToString("dd号");
                    ////    pr.value = item.value.ToString();
                    ////    r.Add(pr);
                    ////}
                    break;
                default:
                    break;
            }
            return r;
        }

        public PieRes GetPr(bool isOdd, int hourHere, decimal value)
        {
            var r = new PieRes();
            //if (isOdd)//奇数
            //{
            //    if(hourHere)
            //}
            //else {

            //}

            return r;
        }

        public List<PieRes> CompletePieForTime(List<PieRes> list, string type, DateTime date, string hourStr)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var r = new List<PieRes>();
            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var temp = new DateTime(year, i, 1);
                    PieRes s = new PieRes
                    {
                        name = temp.ToString("yyyy-MM")
                    };
                    var tempList = list.Where(p => p.name == temp.ToString("yyyy-MM")).ToList();
                    if (tempList.Count > 0)
                        s.value = tempList.First().value;
                    else
                        s.value = "0.000";
                    r.Add(s);
                }
            }
            else if (type == "month")
            {
                var days = DateTime.DaysInMonth(year, month);
                for (int i = 1; i < days + 1; i++)
                {
                    var temp = new DateTime(year, month, i);
                    PieRes s = new PieRes
                    {
                        name = temp.ToString("dd号")
                    };
                    var tempList = list.Where(p => p.name == temp.ToString("dd号")).ToList();
                    if (tempList.Count > 0)
                        s.value = tempList.First().value;
                    else
                        s.value = "0.000";
                    r.Add(s);
                }
            }
            else if (type == "day")
            {
                var hour = Convert.ToInt32(hourStr);
                for (int i = hour; i < hour + 24; i++)
                {
                    var iTemp = i > 23 ? i - 24 : i;
                    var lastHour = i + 2;
                    if (lastHour > 24)
                        lastHour -= 24;
                    //var temp = new DateTime(year, month, day, i, 0, 0);
                    PieRes s = new PieRes
                    {
                        name = $"{iTemp}时-{lastHour}时"
                    };
                    var tempList = list.Where(p => p.name == $"{iTemp}时-{lastHour}时").ToList();
                    if (tempList.Count > 0)
                        s.value = tempList.First().value;
                    else
                        s.value = "0.000";
                    r.Add(s);
                }
            }
            return r;
        }

        public List<string> GetExistHisTable(List<string> tableNames)
        {
            var str = @"DECLARE @str VARCHAR(max)
set @str=''";
            foreach (var item in tableNames)
            {
                str += $@" IF  EXISTS (select * from dbo.sysobjects where xtype='U' and Name = '{item}')
BEGIN
 set @str=@str+'{item},'
END ";
            }
            str += "select @str";
            var tablesStr = SqlHelper.Instance.ExecuteScalar2(str);
            var arr = tablesStr.Split(',').ToList();
            arr.RemoveAt(arr.Count() - 1);
            return arr;
        }

        public List<string> GetExistTableByCondition(List<string> tableNames, string condition, Dictionary<string, string> dict)
        {
            var str = @"DECLARE @str VARCHAR(max)
set @str=''";
            foreach (var item in tableNames)
            {
                str += $@" IF  EXISTS (
SELECT		fm0.id
				FROM
					{item} AS fm0
				LEFT JOIN HHMDeviceView AS h ON h.meterNo = fm0.FLMeterNo
				WHERE
					{condition}
)
BEGIN
 set @str=@str+'{item},'
END ";
            }
            str += "select @str";
            var tablesStr = SqlHelper.Instance.ExecuteScalar2(str, dict);
            var arr = tablesStr.Split(',').ToList();
            arr.RemoveAt(arr.Count() - 1);
            return arr;
        }

        /// <summary>
        /// 获取建立缓存表的sql
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hourStr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTempTableStr(DateTime date, string hourStr, string type)
        {
            var r = "";
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            switch (type)
            {
                #region year
                case "year":
                    r = $@"
                        DECLARE @dt1 VARCHAR (50)
                        DECLARE @dt2 VARCHAR (50)
                        DECLARE @dt3 VARCHAR (50)
                        DECLARE @dt4 VARCHAR (50)
                        DECLARE @dt5 VARCHAR (50)
                        DECLARE @dt6 VARCHAR (50)
                        DECLARE @dt7 VARCHAR (50)
                        DECLARE @dt8 VARCHAR (50)
                        DECLARE @dt9 VARCHAR (50)
                        DECLARE @dt10 VARCHAR (50)
                        DECLARE @dt11 VARCHAR (50)
                        DECLARE @dt12 VARCHAR (50)
                        SET @dt1 = '{year}/1/1 0:00:00'
                        SET @dt2 = '{year}/2/1 0:00:00'
                        SET @dt3 = '{year}/3/1 0:00:00'
                        SET @dt4 = '{year}/4/1 0:00:00'
                        SET @dt5 = '{year}/5/1 0:00:00'
                        SET @dt6 = '{year}/6/1 0:00:00'
                        SET @dt7 = '{year}/7/1 0:00:00'
                        SET @dt8 = '{year}/8/1 0:00:00'
                        SET @dt9 = '{year}/9/1 0:00:00'
                        SET @dt10 = '{year}/10/1 0:00:00'
                        SET @dt11 = '{year}/11/1 0:00:00'
                        SET @dt12 = '{year}/12/1 0:00:00' CREATE TABLE #Temp (
	                        dt VARCHAR (50),
	                        span VARCHAR (12)
                        ) INSERT INTO #Temp (dt, span)
                        VALUES
	                        (@dt1, 0),
	                        (@dt2, 0),
	                        (@dt3, 0),
	                        (@dt4, 0),
	                        (@dt5, 0),
	                        (@dt6, 0),
	                        (@dt7, 0),
	                        (@dt8, 0.0),
	                        (@dt9, 0),
	                        (@dt10, 0),
	                        (@dt11, 0),
	                        (@dt12, 0)";
                    break;
                #endregion
                #region month
                case "month":
                    var days = DateTime.DaysInMonth(year, month);
                    for (int i = 0; i < days; i++)
                    {
                        r += $" DECLARE @dt{i + 1} VARCHAR (50) ";
                    }
                    for (int i = 0; i < days; i++)
                    {
                        r += $"	SET @dt{i + 1} = '{year}/{month}/{i + 1} 0:00:00'  ";
                    }
                    r += $@" CREATE TABLE #Temp (
	                        dt VARCHAR (50),
	                        span VARCHAR (12)
                        ) INSERT INTO #Temp (dt, span)
                        VALUES ";
                    for (int i = 0; i < days; i++)
                    {
                        r += $" (@dt{i + 1}, 0),";
                    }
                    r = StringHelper.Instance.RemoveLastOne(r);

                    break;
                #endregion
                #region day
                case "day":
                    for (int i = 0; i < 24; i++)
                    {
                        r += $" DECLARE @dt{i + 1} VARCHAR (50) ";
                    }
                    for (int i = 0; i < 24; i++)
                    {
                        var temp = Convert.ToInt32(hourStr) + i;
                        temp = temp > 23 ? temp - 24 : temp;
                        var dateHere = new DateTime(year, month, day, temp, 0, 0);
                        if ((Convert.ToInt32(hourStr) + i) > 23)
                            dateHere = dateHere.AddDays(1);
                        //r += $"	SET @dt{i + 1} = '{year}/{month}/{day} {temp}:00:00'  ";
                        r += $"	SET @dt{i + 1} = '{dateHere.ToString("yyyy/MM/dd HH:00:00")}'  ";
                    }

                    r += $@"CREATE TABLE #Temp (
	dt VARCHAR (50),
	span VARCHAR (12)
) INSERT INTO #Temp (dt, span)
VALUES
(@dt1, 0),
(@dt2, 0),
(@dt3, 0),
(@dt4, 0),
(@dt5, 0),
(@dt6, 0),
(@dt7, 0),
(@dt8, 0),
(@dt9, 0),
(@dt10, 0),
(@dt11, 0),
(@dt12, 0),
(@dt13, 0),
(@dt14, 0),
(@dt15, 0),
(@dt16, 0),
(@dt17, 0),
(@dt18, 0),
(@dt19, 0),
(@dt20, 0),
(@dt21, 0),
(@dt22, 0),
(@dt23, 0),
(@dt24, 0)";
                    break;
                    #endregion
            }
            return r;
        }

        public string GetTempTableStrForPie(DateTime date, string hourStr, string type)
        {
            var r = "";
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            switch (type)
            {
                case "year":
                    r = $@"
                        DECLARE @dt1 VARCHAR (50)
                        DECLARE @dt2 VARCHAR (50)
                        DECLARE @dt3 VARCHAR (50)
                        DECLARE @dt4 VARCHAR (50)
                        DECLARE @dt5 VARCHAR (50)
                        DECLARE @dt6 VARCHAR (50)
                        DECLARE @dt7 VARCHAR (50)
                        DECLARE @dt8 VARCHAR (50)
                        DECLARE @dt9 VARCHAR (50)
                        DECLARE @dt10 VARCHAR (50)
                        DECLARE @dt11 VARCHAR (50)
                        DECLARE @dt12 VARCHAR (50)
                        SET @dt1 = '{year}-01'
                        SET @dt2 = '{year}-02'
                        SET @dt3 = '{year}-03'
                        SET @dt4 = '{year}-04'
                        SET @dt5 = '{year}-05'
                        SET @dt6 = '{year}-06'
                        SET @dt7 = '{year}-07'
                        SET @dt8 = '{year}-08'
                        SET @dt9 = '{year}-09'
                        SET @dt10 = '{year}-10'
                        SET @dt11 = '{year}-11'
                        SET @dt12 = '{year}-12' CREATE TABLE #Temp (
	                        name VARCHAR (50),
	                        value VARCHAR (12)
                        ) INSERT INTO #Temp (name, value)
                        VALUES
	                        (@dt1, 0),
	                        (@dt2, 0),
	                        (@dt3, 0),
	                        (@dt4, 0),
	                        (@dt5, 0),
	                        (@dt6, 0),
	                        (@dt7, 0),
	                        (@dt8, 0.0),
	                        (@dt9, 0),
	                        (@dt10, 0),
	                        (@dt11, 0),
	                        (@dt12, 0)";
                    break;
                case "month":
                    var days = DateTime.DaysInMonth(year, month);
                    for (int i = 0; i < days; i++)
                    {
                        r += $" DECLARE @dt{i + 1} VARCHAR (50) ";
                    }
                    for (int i = 0; i < days; i++)
                    {
                        r += $"	SET @dt{i + 1} = '{i + 1}号'  ";
                    }
                    r += $@" CREATE TABLE #Temp (
	                        name VARCHAR (50),
	                        value VARCHAR (12)
                        ) INSERT INTO #Temp (name, value)
                        VALUES ";
                    for (int i = 0; i < days; i++)
                    {
                        r += $" (@dt{i + 1}, 0),";
                    }
                    r = StringHelper.Instance.RemoveLastOne(r);

                    break;
                case "day":
                    for (int i = 0; i < 24; i = i + 2)
                    {
                        r += $" DECLARE @dt{i + 1} VARCHAR (50) ";
                    }
                    for (int i = 0; i < 24; i = i + 2)
                    {
                        var temp = Convert.ToInt32(hourStr) + i;
                        temp = temp > 23 ? temp - 24 : temp;
                        var lastHour = temp + 2;
                        if (lastHour > 24)
                            lastHour -= 24;

                        r += $"	SET @dt{i + 1} = '{temp}时-{lastHour}时'  ";
                    }

                    r += $@"CREATE TABLE #Temp (
	name VARCHAR (50),
	value VARCHAR (12)
) INSERT INTO #Temp (name, value)
VALUES
(@dt1, 0),

(@dt3, 0),

(@dt5, 0),

(@dt7, 0),

(@dt9, 0),

(@dt11, 0),

(@dt13, 0),

(@dt15, 0),

(@dt17, 0),

(@dt19, 0),

(@dt21, 0),

(@dt23, 0) ";
                    break;
            }
            return r;
        }

        public string GetTempTableStrForPieType()
        {
            var r = "";
            r = $@" DECLARE @dt1 VARCHAR (50)
                        DECLARE @dt2 VARCHAR (50)
                        DECLARE @dt3 VARCHAR (50)
                        DECLARE @dt4 VARCHAR (50)
                        SET @dt1 = '居民'
                        SET @dt2 = '公建'
                        SET @dt3 = '工业'
                        SET @dt4 = '商福'
                       CREATE TABLE #Temp (
	                        name VARCHAR (50),
	                        value VARCHAR (12)
                        ) INSERT INTO #Temp (name, value)
                        VALUES
	                        (@dt1, 0),
	                        (@dt2, 0),
	                        (@dt3, 0),
	                        (@dt4, 0)";
            return r;
        }

        /// <summary>
        /// 建立假数据用
        /// </summary>
        public void CreateHisTable(string tableName)
        {
            var str = $@"
CREATE TABLE [dbo].[{tableName}] (
[Id] int NOT NULL IDENTITY(1,1) ,
[communicateNo] nvarchar(50) NULL ,
[FLMeterNo] int NULL ,
[siteNo] nvarchar(50) NULL ,
[InstantTime] datetime NULL ,
[ReceivTime] datetime NULL ,
[StdSum] decimal(18,3) NULL ,
[WorkSum] decimal(18,3) NULL ,
[StdFlow] decimal(18,3) NULL ,
[WorkFlow] decimal(18,3) NULL ,
[Temperature] decimal(18,3) NULL ,
[Pressure] decimal(18,3) NULL ,
[FMState] int NULL ,
[FMStateMsg] nvarchar(200) NULL ,
[RTUState] int NULL ,
[RTUStateMsg] nvarchar(200) NULL ,
[SumTotal] decimal(18,3) NULL ,
[RemainMoney] decimal(18,3) NULL ,
[RemainVolume] decimal(18,3) NULL ,
[Overdraft] decimal(18,3) NULL ,
[RemoteChargeMoney] decimal(18,3) NULL ,
[RemoteChargeTimes] int NULL ,
[Price] decimal(18,3) NULL ,
[ValveState] int NULL ,
[ValveStateMsg] nvarchar(200) NULL ,
[PowerVoltage] decimal(18,3) NULL ,
[BatteryVoltage] decimal(18,3) NULL ,
[Reserve1] nvarchar(50) NULL ,
[Reserve2] nvarchar(50) NULL ,
[Reserve3] nvarchar(50) NULL ,
[Reserve4] nvarchar(50) NULL 
)



DBCC CHECKIDENT(N'[dbo].[{tableName}]', RESEED, 60)

IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'{tableName}', 
'COLUMN', N'InstantTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'采集时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'{tableName}'
, @level2type = 'COLUMN', @level2name = N'InstantTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'采集时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'{tableName}'
, @level2type = 'COLUMN', @level2name = N'InstantTime'

SET IDENTITY_INSERT [dbo].[{tableName}] ON";
            var meterNo = DeviceInfoOper.Instance.GetMeterNoByTableName(tableName);
            for (int i = 0; i < RandHelper.Instance.GetRandomInt(60, 90); i++)
            {
                var date = Convert.ToDateTime("2018-03-29 08:00:58.000");
                var std = 280985.024;

                str += $@"
INSERT INTO [dbo].[{tableName}] ([Id], [communicateNo], [FLMeterNo], [siteNo], [InstantTime], [ReceivTime], [StdSum], [WorkSum], [StdFlow], [WorkFlow], [Temperature], [Pressure], [FMState], [FMStateMsg], [RTUState], [RTUStateMsg], [SumTotal], [RemainMoney], [RemainVolume], [Overdraft], [RemoteChargeMoney], [RemoteChargeTimes], [Price], [ValveState], [ValveStateMsg], [PowerVoltage], [BatteryVoltage], [Reserve1], [Reserve2], [Reserve3], [Reserve4]) VALUES (N'{i + 1}', N'18506831972', N'{meterNo}', N'1001', N'{date.AddHours(i).ToString("yyyy-MM-dd HH:mm:ss")}', N'2018-03-30 15:39:30.000', N'{std + i * 5}', N'{std + i * 5}', N'.000', N'.000', N'20.000', N'101.325', N'0', N'', N'0', N'', N'.000', N'.000', N'.000', N'.000', N'.000', N'0', N'1803.000', N'0', N'关阀', N'33.000', N'33.000', N'', N'', N'', N'')
 ";
            }

            str += $@" 
SET IDENTITY_INSERT [dbo].[{tableName}] OFF


ALTER TABLE [dbo].[{tableName}] ADD PRIMARY KEY ([Id])

";

            //var r = str;

            SqlHelper.Instance.ExcuteNon2(str);
        }

        public List<PieRes> TypePie(List<StdSum4> list, DateTime date, string type, string hour)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var r = new List<PieRes>();
            var list1 = list.Where(p => p.CustTypeName == "居民").ToList();
            if (list1.Count != 0)
            {
                var value = 0m;
                var listNo = list1.Select(p => p.FLMeterNo).Distinct().ToList();
                switch (type)
                {
                    case "year":
                        value = PieTypeYear(list1, listNo, year);
                        break;
                    case "month":
                        break;
                    case "day":
                        break;
                    default:
                        break;
                }
                r.Add(new PieRes { name = "居民", value = value.ToString() });
            }
            list1 = list.Where(p => p.CustTypeName == "工业").ToList();
            if (list1.Count != 0)
            {
                var value = 0m;
                var listNo = list1.Select(p => p.FLMeterNo).Distinct().ToList();
                switch (type)
                {
                    case "year":
                        value = PieTypeYear(list1, listNo, year);
                        break;
                    case "month":
                        break;
                    case "day":
                        break;
                    default:
                        break;
                }
                r.Add(new PieRes { name = "工业", value = value.ToString() });
            }
            list1 = list.Where(p => p.CustTypeName == "公建").ToList();
            if (list1.Count != 0)
            {
                var value = 0m;
                var listNo = list1.Select(p => p.FLMeterNo).Distinct().ToList();
                switch (type)
                {
                    case "year":
                        value = PieTypeYear(list1, listNo, year);
                        break;
                    case "month":
                        break;
                    case "day":
                        break;
                    default:
                        break;
                }
                r.Add(new PieRes { name = "公建", value = value.ToString() });
            }
            list1 = list.Where(p => p.CustTypeName == "商福").ToList();
            if (list1.Count != 0)
            {
                var value = 0m;
                var listNo = list1.Select(p => p.FLMeterNo).Distinct().ToList();
                switch (type)
                {
                    case "year":
                        value = PieTypeYear(list1, listNo, year);
                        break;
                    case "month":
                        break;
                    case "day":
                        break;
                    default:
                        break;
                }
                r.Add(new PieRes { name = "商福", value = value.ToString() });
            }
            r = CompletePieForType(r);
            return r;
        }

        public decimal PieTypeYear(List<StdSum4> list1, List<int> listNo, int year)
        {
            var value = 0m;
            foreach (var item in listNo)
            {
                var dt1 = new DateTime(year, 1, 1);
                var dt2 = dt1.AddYears(1);
                var list12 = list1.OrderByDescending(p => p.InstantTime).Where(p => p.InstantTime < dt2).ToList();
                if (list12.Count != 0)
                {
                    var min = list1.OrderBy(p => p.StdSum).Where(p => p.FLMeterNo == item).First().StdSum;
                    var max = list1.OrderByDescending(p => p.StdSum).Where(p => p.FLMeterNo == item).First().StdSum;

                    var list11 = list1.OrderBy(p => p.InstantTime).Where(p => p.InstantTime > dt1).ToList();
                    if (list11.Count == 0)
                        value += list12.First().StdSum - min;
                    else
                        value += list12.First().StdSum - list11.First().StdSum;
                }
            }
            return value;
        }

        public decimal PieTypeMonth(List<StdSum4> list1, List<int> listNo, DateTime date)
        {
            var value = 0m;
            foreach (var item in listNo)
            {
                var min = list1.OrderBy(p => p.StdSum).Where(p => p.FLMeterNo == item).First().StdSum;
                var max = list1.OrderByDescending(p => p.StdSum).Where(p => p.FLMeterNo == item).First().StdSum;
                var dt1 = new DateTime(date.Year, date.Month, 1);
                var dt2 = dt1.AddMonths(1);
                var list12 = list1.OrderByDescending(p => p.InstantTime).Where(p => p.InstantTime < dt2).ToList();
                if (list12.Count != 0)
                {
                    var list11 = list1.OrderBy(p => p.InstantTime).Where(p => p.InstantTime > dt1).ToList();
                    if (list11.Count == 0)
                        value += list12.First().StdSum - min;
                    else
                        value += list12.First().StdSum - list11.First().StdSum;
                }
            }
            return value;
        }

        /// <summary>
        /// 港太的一些历史表，字段不对，不管了
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public List<string> RemoveBadTable(List<string> tableNames)
        {
            var server = ConfigurationManager.AppSettings.Get("server");
            if (server == "gt")
            {
                tableNames.Remove("FM0000000002");
                tableNames.Remove("FM0000000081");
                tableNames.Remove("FM0000000083");
                tableNames.Remove("FM0000000084");
                tableNames.Remove("FM0000000085");
                tableNames.Remove("FM0000000086");
                tableNames.Remove("FM0000000089");
                tableNames.Remove("FM0000000090");
                tableNames.Remove("FM0000000091");
                tableNames.Remove("FM0000000092");
                tableNames.Remove("FM0000000094");
                tableNames.Remove("FM0000000097");
                tableNames.Remove("FM0000000099");
                tableNames.Remove("FM0000000100");
                tableNames.Remove("FM0000000101");
                tableNames.Remove("FM0000000102");
                tableNames.Remove("FM0000000104");
            }
            return tableNames;
        }

        /// <summary>
        /// 将其他表里没有的数据，传进来
        /// </summary>
        public void GetNotHave()
        {
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo("超级管理员", null);
            tableNames = GetExistHisTable(tableNames);
            tableNames = RemoveBadTable(tableNames);
            var meterNos = GetDeviceIds(tableNames);
            var str = $@"create table #temp (
Id nvarchar(50),
communicateNo nvarchar(50),
FLMeterNo nvarchar(50),
siteNo nvarchar(50),
InstantTime nvarchar(50),
ReceivTime nvarchar(50),
StdSum nvarchar(50),
WorkSum nvarchar(50),
StdFlow nvarchar(50),
WorkFlow nvarchar(50),
Temperature nvarchar(50),
Pressure nvarchar(50),
FMState nvarchar(50),
FMStateMsg nvarchar(50),
RTUState nvarchar(50),
RTUStateMsg nvarchar(50),
SumTotal nvarchar(50),
RemainMoney nvarchar(50),
RemainVolume nvarchar(50),
Overdraft nvarchar(50),
RemoteChargeMoney nvarchar(50),
RemoteChargeTimes nvarchar(50),
Price nvarchar(50),
ValveState nvarchar(50),
ValveStateMsg nvarchar(50),
PowerVoltage nvarchar(50),
BatteryVoltage nvarchar(50),
Reserve1 nvarchar(50),
Reserve2 nvarchar(50),
Reserve3 nvarchar(50),
Reserve4 nvarchar(50)
)
";
            for (int i = 0; i < tableNames.Count; i++)
            {
                str += $@"
insert #temp
SELECT * from {tableNames[i]} where id not in (
select id from AllFMData where FLMeterNo={meterNos[i]})
";
            }
            str += $" select * from #temp ";
            var list = SqlHelper.Instance.ExecuteGetDt2<AllFMData>(str, new Dictionary<string, string>());
            if (list.Count > 0)
            {
                var count = list.Count / 1000 + 1;
                for (int i = 0; i < count; i++)
                {
                    var tempList = list.Skip((i) * 1000).Take(1000).ToList();
                    string insert = "";
                    foreach (var item2 in tempList)
                    {
                        insert += AllFMDataOper.Instance.GetInsertStr2(item2);
                    }
                    SqlHelper.Instance.ExcuteNon2(insert);
                }
            }
        }

        #region 201807230947
        public List<FMModel> GetList22(HisReq req, int size, string tableName)
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
                { "@search", $"%{search}%" },
                  { "@search2", search },
            };

            var condition = GetCondition22(req);
            var meterno = DeviceInfoOper.Instance.GetMeterNoByTableName(tableName);
            var device = DeviceInfoOper.Instance.GetByMeterNo(meterno);
            if (device == null)
                return new List<FMModel>();
            var cname = device.customerName;
            var addr = device.address;
            var dno = device.deviceNo;

            var list = SqlHelper.Instance.GetViewPaging2<FMModel>(tableName, $"select * from {tableName}", condition, index, size, orderStr, dict);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].customerName = cname;
                list[i].address = addr;
                list[i].deviceNo = dno;
            }

            return list;
        }

        public int GetCount22(HisReq req, string tableName)
        {
            var search = req.search ?? "";

            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                  { "@search2", search },
            };

            var condition = GetCondition22(req);


            var list = SqlHelper.Instance.GetDistinctCount2<FMModel>(tableName, condition, dict);

            //var list = SqlHelper.Instance.GdcForFMModel<FMModel>(selectUnion, condition, dict);
            return list.Count;
        }

        public string GetCondition22(HisReq req)
        {

            var search = req.search ?? "";
            var condition = $" 1=1 ";
            //if (!search.IsNullOrEmpty())
            //    condition += " and (h.customerName like @search or h.address like @search or h.communicateNo=@search2  or h.deviceNo=@search2) ";

            //if (req.startTime == null && req.endTime == null)
            //{
            //    var dateStr = req.dateStr;
            //    var now = DateTime.Now;
            //    var year = now.Year;
            //    var month = now.Month;
            //    var day = now.Day;
            //    switch (dateStr)
            //    {
            //        case "year":
            //            condition += $" and InstantTime>'{new DateTime(year - 1, 12, 31)}' and InstantTime<'{new DateTime(year + 1, 1, 1)}'";
            //            break;
            //        case "month":
            //            condition += $" and InstantTime>'{new DateTime(year, month, 1)}' and InstantTime<'{new DateTime(year, month, 1).AddMonths(1)}'";
            //            break;
            //        case "day":
            //            condition += $" and InstantTime>'{new DateTime(year, month, day, 23, 59, 59).AddDays(-1)}' and InstantTime<'{new DateTime(year, month, day, 0, 0, 0).AddDays(1)}'";
            //            break;
            //    }
            //}
            //else
            //{

            //var s = new DateTime();
            //var e = new DateTime();

            if (req.startTime != null)
            {
                condition += $" and InstantTime>'{req.startTime}'";
            }
            if (req.endTime != null)
            {
                condition += $" and InstantTime<'{req.endTime}'";
            }
            //}
            return condition;
        }

        #endregion



    }
}
