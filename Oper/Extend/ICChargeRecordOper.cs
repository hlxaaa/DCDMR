using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;
using System.Linq;

namespace DbOpertion.DBoperation
{
    public partial class ICChargeRecordOper : SingleTon<ICChargeRecordOper>
    {

        /// <summary>
        /// 曲线图，柱形图。表格
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<chargeReport> GetLineList(GetChargeReportReq req)
        {
            var date = Convert.ToDateTime(req.date);
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var type = req.type;
            var str = "";
            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var newDate = new DateTime(year, i, 1);
                    if (i == 1)
                    {
                        str = $"select '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' as dt,isnull(sum(chargeVolume),0)as sumVolume,isnull(sum(chargeMoney),0)as sumMoney from ICChargeRecord where chargeTime<'{newDate.AddMonths(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                    else
                    {
                        str += $" union select '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' as dt,isnull(sum(chargeVolume),0)as sumVolume,isnull(sum(chargeMoney),0)as sumMoney from ICChargeRecord where chargeTime<'{newDate.AddMonths(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
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
                        str = $"select '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' as dt,isnull(sum(chargeVolume),0)as sumVolume,isnull(sum(chargeMoney),0)as sumMoney from ICChargeRecord where chargeTime<'{newDate.AddDays(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                    else
                    {
                        str += $" union select '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' as dt,isnull(sum(chargeVolume),0)as sumVolume,isnull(sum(chargeMoney),0)as sumMoney from ICChargeRecord where chargeTime<'{newDate.AddDays(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                }
            }
            else if (type == "day")
            {
                var hour = Convert.ToInt32(req.startTime);

                for (int i = hour; i < hour + 24; i++)
                {
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
                        str = $"select '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' as dt,isnull(sum(chargeVolume),0)as sumVolume,isnull(sum(chargeMoney),0)as sumMoney from ICChargeRecord where chargeTime<'{newDate.AddHours(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                    else
                    {
                        str += $" union select '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' as dt,isnull(sum(chargeVolume),0)as sumVolume,isnull(sum(chargeMoney),0)as sumMoney from ICChargeRecord where chargeTime<'{newDate.AddHours(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                }
            }

            var list = SqlHelper.Instance.ExecuteGetDt<chargeReport>(str, new Dictionary<string, string>());
            return list;
        }

        /// <summary>
        /// 大范围统计，进来lastName就一定有了，不判断了
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<chargeReport> GetLineListForCompany(GetChargeReportReq req)
        {
            var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(req.lastName);

            var date = Convert.ToDateTime(req.date);
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var type = req.type;
            var str = "";
            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var newDate = new DateTime(year, i, 1);
                    if (i == 1)
                    {
                        str = $@"SELECT
	                                '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' AS dt,
	                                isnull(SUM(chargeVolume), 0) AS sumVolume
                                ,isnull(sum(chargeMoney),0)as sumMoney
                                FROM
	                                ICChargeRecordCustTypeView
                                WHERE
	                                chargeTime < '{newDate.AddMonths(1)}'
                                AND chargeTime >= '{newDate}'
                                AND (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                    else
                    {
                        str += $@" union SELECT
	                                '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' AS dt,
	                                isnull(SUM(chargeVolume), 0) AS sumVolume
                                ,isnull(sum(chargeMoney),0)as sumMoney
                                FROM
	                                ICChargeRecordCustTypeView
                                WHERE
	                                chargeTime < '{newDate.AddMonths(1)}'
                                AND chargeTime >= '{newDate}'
                                AND (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
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
                        str = $@"SELECT
	                                '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' AS dt,
	                                isnull(SUM(chargeVolume), 0) AS sumVolume
                                ,isnull(sum(chargeMoney),0)as sumMoney
                                FROM
	                                ICChargeRecordCustTypeView
                                WHERE
	                                chargeTime < '{newDate.AddDays(1)}'
                                AND chargeTime >= '{newDate}'
                                AND (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                    else
                    {
                        str += $@" union SELECT
	                                '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' AS dt,
	                                isnull(SUM(chargeVolume), 0) AS sumVolume
                                ,isnull(sum(chargeMoney),0)as sumMoney
                                FROM
	                                ICChargeRecordCustTypeView
                                WHERE
	                                chargeTime < '{newDate.AddDays(1)}'
                                AND chargeTime >= '{newDate}'
                                AND (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                }
            }
            else if (type == "day")
            {
                var hour = Convert.ToInt32(req.startTime);

                for (int i = hour; i < hour + 24; i++)
                {
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
                        str = $@"SELECT
	                                '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' AS dt,
	                                isnull(SUM(chargeVolume), 0) AS sumVolume
                                ,isnull(sum(chargeMoney),0)as sumMoney
                                FROM
	                                ICChargeRecordCustTypeView
                                WHERE
	                                chargeTime < '{newDate.AddHours(1)}'
                                AND chargeTime >= '{newDate}'
                                AND (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                    else
                    {
                        str += $@" union SELECT
	                                '{newDate.ToString("yyyy-MM-dd HH:mm:ss")}' AS dt,
	                                isnull(SUM(chargeVolume), 0) AS sumVolume
                                ,isnull(sum(chargeMoney),0)as sumMoney
                                FROM
	                                ICChargeRecordCustTypeView
                                WHERE
	                                chargeTime < '{newDate.AddHours(1)}'
                                AND chargeTime >= '{newDate}'
                                AND (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                }
            }

            var list = SqlHelper.Instance.ExecuteGetDt<chargeReport>(str, new Dictionary<string, string>());
            return list;
        }

        /// <summary>
        /// 圆饼 用户类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PieRes> GetPieListByCustType(GetChargeReportReq req)
        {
            var r = new List<PieRes>();//民用，公建，工业
            var date = Convert.ToDateTime(req.date);
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var type = req.type;

            string str = "";
            switch (type)
            {
                case "year":
                    var newDate = new DateTime(year, 1, 1);
                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
(select * from ICChargeRecordCustTypeView where chargeTime>'{newDate}' and  chargeTime<'{newDate.AddYears(1)}'and customerNo='{customerNo}')r
GROUP BY r.CustTypeName";
                    break;
                case "month":
                    var newDate2 = new DateTime(year, month, 1);
                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
(select * from ICChargeRecordCustTypeView where chargeTime>'{newDate2}' and  chargeTime<'{newDate2.AddMonths(1)}'and customerNo='{customerNo}')r
GROUP BY r.CustTypeName";
                    break;
                case "day":
                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
(select * from ICChargeRecordCustTypeView where chargeTime>'{date}' and  chargeTime<'{date.AddDays(1)}'and customerNo='{customerNo}')r
GROUP BY r.CustTypeName";
                    break;
            }
            r = SqlHelper.Instance.ExecuteGetDt<PieRes>(str, new Dictionary<string, string>());
            r = CompleteListForType(r);
            return r;
        }

        public List<PieRes> GetPieListByCustTypeForCompany(GetChargeReportReq req)
        {
            var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(req.lastName);
            var r = new List<PieRes>();//民用，公建，工业
            var date = Convert.ToDateTime(req.date);
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            var type = req.type;

            string str = "";
            switch (type)
            {
                case "year":
                    var newDate = new DateTime(year, 1, 1);
                    //                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
                    //(select * from ICChargeRecordCustTypeView where chargeTime>'{newDate}' and  chargeTime<'{newDate.AddYears(1)}'and customerNo='{customerNo}')r
                    //GROUP BY r.CustTypeName";
                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
                            (select * from ICChargeRecordCustTypeView where chargeTime>'{newDate}' and  chargeTime<'{newDate.AddYears(1)}'and (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}') ";
                    if (customerNo != "0")
                        str += $" and customerNo='{customerNo}' ";

                    str += @")r
                            GROUP BY r.CustTypeName";

                    break;
                case "month":
                    var newDate2 = new DateTime(year, month, 1);
                    //                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
                    //(select * from ICChargeRecordCustTypeView where chargeTime>'{newDate2}' and  chargeTime<'{newDate2.AddMonths(1)}'and customerNo='{customerNo}')r
                    //GROUP BY r.CustTypeName";
                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
                            (select * from ICChargeRecordCustTypeView where chargeTime>'{newDate2}' and  chargeTime<'{newDate2.AddMonths(1)}'and (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}')";
                    if (customerNo != "0")
                        str += $" and customerNo='{customerNo}' ";
                    str += @" )r
                            GROUP BY r.CustTypeName";
                    break;
                case "day":
                    //                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
                    //(select * from ICChargeRecordCustTypeView where chargeTime>'{date}' and  chargeTime<'{date.AddDays(1)}'and customerNo='{customerNo}')r
                    //GROUP BY r.CustTypeName";
                    str = $@"select r.CustTypeName as name,sum(r.chargeVolume)as value from 
                            (select * from ICChargeRecordCustTypeView where chargeTime>'{date}' and  chargeTime<'{date.AddDays(1)}'and (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}')";
                    if (customerNo != "0")
                        str += $" and customerNo='{customerNo}' ";
                    str += @" )r
                            GROUP BY r.CustTypeName";
                    break;
            }
            r = SqlHelper.Instance.ExecuteGetDt<PieRes>(str, new Dictionary<string, string>());
            r = CompleteListForType(r);
            return r;
        }

        /// <summary>
        /// 圆饼 时间分
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PieRes> GetPieListByTime(GetChargeReportReq req)
        {
            var date = Convert.ToDateTime(req.date);
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var type = req.type;
            var str = "";
            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var newDate = new DateTime(year, i, 1);
                    if (i == 1)
                    {
                        str = $"select '{newDate.ToString("yyyy-MM")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecord where chargeTime<'{newDate.AddMonths(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                    else
                    {
                        str += $" union select '{newDate.ToString("yyyy-MM")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecord where chargeTime<'{newDate.AddMonths(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
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
                        str = $"select '{newDate.ToString("dd号")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecord where chargeTime<'{newDate.AddDays(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                    else
                    {
                        str += $" union select '{newDate.ToString("dd号")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecord where chargeTime<'{newDate.AddDays(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                }
            }
            else if (type == "day")
            {
                var hour = Convert.ToInt32(req.startTime);

                for (int i = hour; i < hour + 24; i = i + 2)
                {
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
                    if (i == hour)
                    {
                        str = $"select '{i}时-{lastHour}时' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecord where chargeTime<'{newDate.AddHours(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                    else
                    {
                        str += $" union select '{i}时-{lastHour}时' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecord where chargeTime<'{newDate.AddHours(1)}' and  chargeTime>='{newDate}' and customerNo='{customerNo}'";
                    }
                }
            }

            var list = SqlHelper.Instance.ExecuteGetDt<PieRes>(str, new Dictionary<string, string>());
            return list;
        }

        public List<PieRes> GetPieListByTimeForCompany(GetChargeReportReq req)
        {
            var cid = AllInOne_UserInfoOper.Instance.GetLastCIdByName(req.lastName);
            var date = Convert.ToDateTime(req.date);
            var customerNo = req.customerNo;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var type = req.type;
            var str = "";
            if (type == "year")
            {
                for (int i = 1; i < 13; i++)
                {
                    var newDate = new DateTime(year, i, 1);
                    if (i == 1)
                    {
                        str = $"select '{newDate.ToString("yyyy-MM")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecordCustTypeView where chargeTime<'{newDate.AddMonths(1)}' and  chargeTime>='{newDate}' and  (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                    else
                    {
                        str += $" union select '{newDate.ToString("yyyy-MM")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecordCustTypeView where chargeTime<'{newDate.AddMonths(1)}' and  chargeTime>='{newDate}' and  (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
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
                        str = $"select '{newDate.ToString("dd号")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecordCustTypeView where chargeTime<'{newDate.AddDays(1)}' and  chargeTime>='{newDate}' and  (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                    else
                    {
                        str += $" union select '{newDate.ToString("dd号")}' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecordCustTypeView where chargeTime<'{newDate.AddDays(1)}' and  chargeTime>='{newDate}' and  (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                }
            }
            else if (type == "day")
            {
                var hour = Convert.ToInt32(req.startTime);

                for (int i = hour; i < hour + 24; i = i + 2)
                {
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
                    if (i == hour)
                    {
                        str = $"select '{i}时-{lastHour}时' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecordCustTypeView where chargeTime<'{newDate.AddHours(1)}' and  chargeTime>='{newDate}' and  (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                    else
                    {
                        str += $" union select '{i}时-{lastHour}时' as name,isnull(sum(chargeVolume),0)as value from ICChargeRecordCustTypeView where chargeTime<'{newDate.AddHours(1)}' and  chargeTime>='{newDate}' and  (cId1='{cid}' or cId2='{cid}' or cId3='{cid}' or cId4='{cid}' )";
                        if (customerNo != "0")
                            str += $" and customerNo='{customerNo}' ";
                    }
                }
            }

            var list = SqlHelper.Instance.ExecuteGetDt<PieRes>(str, new Dictionary<string, string>());
            return list;
        }

        public List<PieRes> CompleteListForType(List<PieRes> list)
        {
            var r = new List<PieRes> {
                new PieRes{ name="居民",value="0"},
                new PieRes{ name="公建",value="0"},
                new PieRes{ name="工业",value="0"}
            };
            for (int i = 0; i < r.Count; i++)
            {
                var temp = list.Where(p => p.name == r[i].name).ToList();
                if (temp.Count > 0)
                {
                    r[i].value = temp.First().value;
                }
            }
            return r;
        }

        /// <summary>
        /// 添加假数据
        /// </summary>
        /// <param name="cno"></param>
        /// <param name="mno"></param>
        public void AddData(string cno, string mno)
        {
            var str = "";
            var date = new DateTime(2018, 3, 30, 1, 1, 1);
            for (int i = 0; i < 20; i++)
            {
                str += $@"  INSERT INTO [dbo].[ICChargeRecord] (
	                        [chargeTime],
	                        [customerNo],
	                        [meterNo],
	                        [meterTypeNo],
	                        [factoryNo],
	                        [fluidNo],
	                        [Price],
	                        [chargeVolume],
	                        [chargeMoney],
	                        [totalVolume],
	                        [totalMoney],
	                        [chargeTimes],
	                        [chargeType],
	                        [chargeBranchNo],
	                        [chargePosNo],
	                        [chargeOperator],
	                        [ICWriteMark],
	                        [ReceiptNo],
	                        [chargeCheck],
	                        [cycleVolume],
	                        [cycleMoney],
	                        [payId]
                        )
                        VALUES
	                        (
		                        N'{date.AddHours(i)}',
		                        N'{cno}',
		                        N'{mno}',
		                        N'02',
		                        N'03',
		                        N'0000000001',
		                        N'2.500',
		                        N'{100 + 6 * i}',
		                        N'250.000',
		                        N'100.000',
		                        N'250.000',
		                        N'1',
		                        N'0',
		                        N'0000000000',
		                        N'0000000000',
		                        N'管理员',
		                        N'1',
		                        N'1',
		                        NULL,
		                        N'.000',
		                        NULL,
		                        NULL
	                        )   ";


            }
            SqlHelper.Instance.ExcuteNon(str);
        }

    }
}
