using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Common.Helper;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Res;
using System.Linq;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;

namespace HHTDCDMR.Controllers.Page
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            string str = "select * from userinfo";
            var a = SqlHelper.Instance.ExecuteScalar(str, new Dictionary<string, string>());
            return View();
        }

        public ActionResult TestView()
        {
            return View();
        }

        public float test()
        {

            var d = 3.5m;
            return (float)d;
            
        }

        public void CreateHisTable(string tableName)
        {
            HisOper.Instance.CreateHisTable(tableName);
        }

        public void CreateCharge(string cno, string mno)
        {
            ICChargeRecordOper.Instance.AddData(cno, mno);
        }

        public void SendDataToAll()
        {
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo("超级管理员", null);
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);
            tableNames = HisOper.Instance.RemoveBadTable(tableNames);
            foreach (var item in tableNames)
            {
                string str = $"select * from {item}";
                var list = SqlHelper.Instance.ExecuteGetDt2<AllFMData>(str, new Dictionary<string, string>());
                string insert = "";
                foreach (var item2 in list)
                {
                    insert += AllFMDataOper.Instance.GetInsertStr2(item2);
                }
                SqlHelper.Instance.ExcuteNon2(insert);
            }
        }

        public void test2()
        {
            HisOper.Instance.GetNotHave();
        }

        #region old报表

        public string GetSqlStr()
        {
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo("超级管理员", null);
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);
            tableNames = HisOper.Instance.RemoveBadTable(tableNames);
            var meterNos = HisOper.Instance.GetDeviceIds(tableNames);
            var str = @"declare @no INT
declare @typename nvarchar(10)
declare @stdsum DECIMAL(12,2)
set @stdsum=0
DECLARE @start datetime
DECLARE @end datetime
declare @min DECIMAL(12,2)
DECLARE @max decimal(12,2)
declare @year varchar(10)
declare @month varchar(10)
declare @datestr varchar(20)
declare @datestr2 varchar(20)
declare @temp varchar(20)
set @temp=''
DECLARE @i int 
";
            for (int i = 0; i < tableNames.Count; i++)
            {
                str += $@"
set @no={meterNos[i]}
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from {tableNames[i]} order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from {tableNames[i]} order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from {tableNames[i]} f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
	set @datestr=dateadd(day,-1,@datestr)
select @stdsum =abs(r2.stdsum-r1.stdsum) from 
	(select top 1 stdsum from {tableNames[i]} where instanttime>@datestr)r1,
	(select top 1 stdsum from {tableNames[i]} where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
	set @datestr=dateadd(day,-1,@datestr)
select @stdsum = abs(r2.stdsum-r1.stdsum) from 
	(select top 1 stdsum from {tableNames[i]} where instanttime>@datestr)r1,
	(select top 1 stdsum from {tableNames[i]} where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END
";

            }
            return str;
        }

        public string GetMonthSqlStr()
        {
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo("超级管理员", null);
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);
            tableNames = HisOper.Instance.RemoveBadTable(tableNames);
            var meterNos = HisOper.Instance.GetDeviceIds(tableNames);
            var str = @"declare @no INT
declare @typename nvarchar(10)
declare @stdsum DECIMAL(12,2)
set @stdsum=0
DECLARE @start datetime
DECLARE @end datetime
declare @min DECIMAL(12,2)
DECLARE @max decimal(12,2)
declare @year varchar(10)
declare @month varchar(10)
declare @day varchar(10)
declare @datestr varchar(20)
declare @datestr2 varchar(20)
declare @temp varchar(20)
set @temp=''
DECLARE @i int 
";
            for (int i = 0; i < tableNames.Count; i++)
            {
                str += $@"
set @no={meterNos[i]}
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from {tableNames[i]} order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from {tableNames[i]} order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from {tableNames[i]} f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
	set @datestr=dateadd(day,-1,@datestr)
select @stdsum =abs( r2.stdsum-r1.stdsum) from 
	(select top 1 stdsum from {tableNames[i]} where instanttime>@datestr)r1,
	(select top 1 stdsum from {tableNames[i]} where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
	set @datestr=dateadd(day,-1,@datestr)
select @stdsum = abs(r2.stdsum-r1.stdsum) from 
	(select top 1 stdsum from {tableNames[i]} where instanttime>@datestr)r1,
	(select top 1 stdsum from {tableNames[i]} where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END
";

            }
            return str;
        }

        public string GetDaySqlStr()
        {
            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo("超级管理员", null);
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);
            tableNames = HisOper.Instance.RemoveBadTable(tableNames);
            var meterNos = HisOper.Instance.GetDeviceIds(tableNames);
            var str = @"declare @no INT
declare @typename nvarchar(10)
declare @stdsum DECIMAL(12,2)
set @stdsum=0
DECLARE @start datetime
DECLARE @end datetime
declare @min DECIMAL(12,2)
DECLARE @max decimal(12,2)
declare @year varchar(10)
declare @month varchar(10)
declare @day varchar(10)
declare @hour varchar(10)
declare @datestr varchar(20)
declare @datestr2 varchar(20)
declare @temp varchar(20)
set @temp=''
DECLARE @i int 
";
            for (int i = 0; i < tableNames.Count; i++)
            {
                str += $@"
set @no={meterNos[i]}
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from {tableNames[i]} order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from {tableNames[i]} order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from {tableNames[i]} f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
	set @datestr=dateadd(day,-1,@datestr)
select @stdsum = abs(r2.stdsum-r1.stdsum) from 
	(select top 1 stdsum from {tableNames[i]} where instanttime>@datestr)r1,
	(select top 1 stdsum from {tableNames[i]} where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
	set @datestr=dateadd(day,-1,@datestr)
select @stdsum = abs(r2.stdsum-r1.stdsum) from 
	(select top 1 stdsum from {tableNames[i]} where instanttime>@datestr)r1,
	(select top 1 stdsum from {tableNames[i]} where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end

";

            }
            return str;
        }

        public void UpdateDay()
        {
            var str = @"declare @no INT
declare @typename nvarchar(10)
declare @stdsum DECIMAL(12,2)
set @stdsum=0
DECLARE @start datetime
DECLARE @end datetime
declare @min DECIMAL(12,2)
DECLARE @max decimal(12,2)
declare @year varchar(10)
declare @month varchar(10)
declare @day varchar(10)
declare @hour varchar(10)
declare @datestr varchar(20)
declare @datestr2 varchar(20)
declare @temp varchar(20)
set @temp=''
DECLARE @i int 

set @no=1
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000001 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000001 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000001 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000001 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=98
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000098 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000098 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000098 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000098 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=103
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000103 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000103 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000103 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000103 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=105
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000105 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000105 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000105 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000105 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=106
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000106 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000106 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000106 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000106 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=107
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000107 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000107 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000107 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000107 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=111
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000111 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000111 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000111 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000111 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=112
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000112 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000112 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000112 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000112 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=113
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000113 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000113 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000113 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000113 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=114
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000114 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000114 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000114 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000114 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=115
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000115 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000115 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000115 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000115 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=116
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000116 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000116 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000116 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000116 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=117
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000117 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000117 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000117 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000117 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=118
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000118 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000118 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000118 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000118 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=120
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000120 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000120 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000120 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000120 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=121
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000121 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000121 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000121 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000121 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=122
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000122 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000122 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000122 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000122 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=124
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000124 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000124 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000124 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000124 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=125
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000125 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000125 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000125 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000125 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=126
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000126 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000126 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000126 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000126 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=127
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000127 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000127 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000127 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000127 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=128
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000128 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000128 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000128 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000128 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=129
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000129 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000129 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000129 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000129 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=132
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000132 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000132 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000132 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000132 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=133
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000133 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000133 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000133 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000133 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=134
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000134 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000134 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000134 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000134 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=135
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000135 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000135 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000135 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000135 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=137
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000137 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000137 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000137 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000137 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=139
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000139 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000139 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000139 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000139 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=140
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000140 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000140 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000140 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000140 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=141
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000141 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000141 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000141 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000141 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=142
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000142 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000142 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000142 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000142 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=143
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000143 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000143 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000143 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000143 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=145
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000145 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000145 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000145 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000145 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=147
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000147 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000147 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000147 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000147 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=152
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000152 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000152 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000152 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000152 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=153
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000153 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000153 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000153 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000153 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=154
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000154 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000154 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000154 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000154 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=155
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000155 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000155 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000155 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000155 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=156
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000156 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000156 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000156 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000156 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=157
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000157 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000157 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000157 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000157 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=158
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000158 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000158 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000158 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000158 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=160
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000160 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000160 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000160 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000160 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=161
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000161 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000161 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000161 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000161 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=162
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000162 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000162 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000162 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000162 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=163
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000163 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000163 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000163 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000163 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=164
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000164 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000164 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000164 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000164 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=166
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000166 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000166 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000166 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000166 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=167
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000167 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000167 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000167 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000167 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=168
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000168 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000168 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000168 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000168 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=172
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000172 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000172 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000172 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000172 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=173
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000173 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000173 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000173 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000173 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=174
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000174 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000174 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000174 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000174 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=175
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000175 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000175 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000175 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000175 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=176
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000176 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000176 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000176 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000176 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=177
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000177 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000177 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000177 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000177 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=178
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000178 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000178 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000178 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000178 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=179
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000179 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000179 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000179 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000179 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=180
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000180 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000180 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000180 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000180 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=181
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000181 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000181 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000181 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000181 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=183
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000183 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000183 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000183 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000183 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=185
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000185 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000185 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000185 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000185 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=186
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000186 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000186 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000186 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000186 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=187
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000187 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000187 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000187 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000187 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=188
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000188 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000188 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000188 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000188 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=189
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000189 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000189 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000189 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000189 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=191
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000191 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000191 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000191 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000191 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=192
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000192 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000192 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000192 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000192 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=194
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000194 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000194 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000194 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000194 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=197
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000197 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000197 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000197 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000197 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=198
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000198 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000198 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000198 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000198 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=199
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000199 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000199 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000199 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000199 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=200
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000200 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000200 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000200 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000200 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=201
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000201 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000201 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000201 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000201 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=202
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000202 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000202 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000202 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000202 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=203
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000203 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000203 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000203 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000203 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=204
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000204 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000204 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000204 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000204 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=205
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000205 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000205 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000205 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000205 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=206
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000206 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000206 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000206 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000206 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=208
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000208 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000208 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000208 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000208 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=209
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000209 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000209 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000209 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000209 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=212
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000212 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000212 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000212 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000212 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=214
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000214 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000214 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000214 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000214 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=215
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000215 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000215 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000215 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000215 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=216
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000216 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000216 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000216 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000216 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=217
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000217 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000217 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000217 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000217 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=218
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000218 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000218 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000218 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000218 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=219
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000219 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000219 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000219 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000219 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=220
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000220 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000220 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000220 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000220 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=221
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000221 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000221 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000221 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000221 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=222
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000222 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000222 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000222 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000222 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=223
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000223 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000223 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000223 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000223 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=224
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000224 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000224 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000224 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000224 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=225
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000225 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000225 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000225 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000225 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=226
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000226 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000226 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000226 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000226 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=227
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000227 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000227 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000227 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000227 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=228
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000228 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000228 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000228 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000228 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end


set @no=229
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000229 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000229 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000229 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMReportDay where flmeterno=@no)
BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set @hour = SUBSTRING(CONVERT(varchar(100), @start, 120),12,2)
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
select @datestr,@datestr2 as d2,@end,@start
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr2 order by instanttime desc)r2

insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(hour,1,@datestr2), 120)	
	set @datestr=@temp

	END

END
else 
BEGIN
select @year=year,@month=month,@day=day,@hour=hour from(

select top 1 year year ,month month,day day,hour hour from FMReportDay where  flmeterno=@no order by id desc)r

delete  from FMReportDay where flmeterno=@no and year=@year and month=@month and day=@day and hour=@hour

if(len(@hour)=1)
BEGIN
set @hour='0'+@hour
end

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr=CONVERT(varchar(100), @year+'-'+@month+'-'+@day+'T'+@hour+':00:00',120)
set @datestr2 = Convert(varchar(100), dateadd(hour,1,@datestr),120)
set @i=0
while dateadd(hour,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),6,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),9,2)
set @hour = SUBSTRING( CONVERT(varchar(100), @datestr, 120),12,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000229 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportDay (FLMeterno,year,month,day,hour,stdsum,custtypename) values(@no,@year,@month,@day,@hour,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(hour,1,@datestr2),120)
	set @datestr=@temp
	END
end

";
            SqlHelper.Instance.ExcuteNon2(str);
        }

        public void updatemonth()
        {
            var str = @"declare @no INT
declare @typename nvarchar(10)
declare @stdsum DECIMAL(12,2)
set @stdsum=0
DECLARE @start datetime
DECLARE @end datetime
declare @min DECIMAL(12,2)
DECLARE @max decimal(12,2)
declare @year varchar(10)
declare @month varchar(10)
declare @day varchar(10)
declare @datestr varchar(20)
declare @datestr2 varchar(20)
declare @temp varchar(20)
set @temp=''
DECLARE @i int 

set @no=1
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000001 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000001 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000001 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000001 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=98
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000098 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000098 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000098 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000098 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=103
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000103 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000103 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000103 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000103 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=105
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000105 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000105 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000105 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000105 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=106
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000106 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000106 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000106 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000106 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=107
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000107 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000107 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000107 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000107 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=111
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000111 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000111 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000111 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000111 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=112
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000112 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000112 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000112 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000112 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=113
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000113 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000113 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000113 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000113 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=114
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000114 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000114 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000114 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000114 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=115
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000115 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000115 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000115 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000115 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=116
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000116 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000116 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000116 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000116 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=117
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000117 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000117 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000117 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000117 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=118
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000118 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000118 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000118 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000118 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=120
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000120 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000120 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000120 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000120 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=121
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000121 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000121 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000121 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000121 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=122
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000122 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000122 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000122 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000122 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=124
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000124 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000124 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000124 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000124 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=125
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000125 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000125 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000125 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000125 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=126
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000126 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000126 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000126 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000126 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=127
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000127 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000127 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000127 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000127 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=128
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000128 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000128 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000128 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000128 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=129
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000129 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000129 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000129 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000129 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=132
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000132 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000132 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000132 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000132 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=133
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000133 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000133 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000133 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000133 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=134
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000134 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000134 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000134 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000134 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=135
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000135 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000135 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000135 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000135 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=137
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000137 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000137 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000137 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000137 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=139
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000139 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000139 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000139 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000139 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=140
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000140 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000140 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000140 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000140 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=141
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000141 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000141 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000141 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000141 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=142
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000142 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000142 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000142 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000142 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=143
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000143 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000143 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000143 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000143 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=145
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000145 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000145 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000145 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000145 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=147
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000147 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000147 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000147 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000147 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=152
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000152 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000152 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000152 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000152 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=153
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000153 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000153 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000153 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000153 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=154
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000154 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000154 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000154 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000154 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=155
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000155 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000155 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000155 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000155 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=156
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000156 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000156 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000156 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000156 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=157
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000157 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000157 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000157 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000157 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=158
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000158 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000158 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000158 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000158 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=160
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000160 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000160 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000160 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000160 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=161
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000161 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000161 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000161 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000161 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=162
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000162 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000162 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000162 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000162 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=163
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000163 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000163 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000163 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000163 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=164
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000164 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000164 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000164 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000164 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=166
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000166 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000166 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000166 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000166 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=167
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000167 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000167 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000167 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000167 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=168
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000168 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000168 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000168 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000168 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=172
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000172 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000172 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000172 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000172 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=173
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000173 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000173 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000173 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000173 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=174
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000174 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000174 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000174 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000174 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=175
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000175 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000175 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000175 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000175 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=176
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000176 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000176 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000176 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000176 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=177
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000177 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000177 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000177 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000177 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=178
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000178 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000178 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000178 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000178 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=179
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000179 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000179 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000179 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000179 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=180
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000180 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000180 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000180 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000180 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=181
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000181 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000181 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000181 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000181 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=183
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000183 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000183 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000183 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000183 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=185
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000185 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000185 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000185 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000185 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=186
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000186 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000186 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000186 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000186 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=187
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000187 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000187 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000187 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000187 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=188
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000188 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000188 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000188 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000188 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=189
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000189 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000189 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000189 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000189 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=191
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000191 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000191 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000191 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000191 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=192
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000192 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000192 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000192 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000192 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=194
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000194 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000194 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000194 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000194 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=197
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000197 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000197 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000197 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000197 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=198
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000198 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000198 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000198 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000198 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=199
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000199 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000199 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000199 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000199 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=200
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000200 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000200 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000200 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000200 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=201
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000201 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000201 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000201 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000201 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=202
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000202 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000202 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000202 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000202 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=203
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000203 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000203 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000203 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000203 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=204
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000204 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000204 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000204 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000204 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=205
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000205 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000205 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000205 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000205 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=206
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000206 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000206 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000206 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000206 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=208
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000208 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000208 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000208 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000208 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=209
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000209 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000209 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000209 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000209 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=212
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000212 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000212 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000212 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000212 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=214
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000214 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000214 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000214 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000214 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=215
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000215 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000215 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000215 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000215 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=216
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000216 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000216 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000216 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000216 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=217
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000217 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000217 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000217 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000217 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=218
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000218 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000218 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000218 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000218 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=219
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000219 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000219 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000219 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000219 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=220
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000220 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000220 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000220 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000220 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=221
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000221 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000221 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000221 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000221 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=222
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000222 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000222 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000222 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000222 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=223
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000223 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000223 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000223 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000223 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=224
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000224 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000224 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000224 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000224 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=225
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000225 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000225 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000225 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000225 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=226
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000226 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000226 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000226 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000226 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=227
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000227 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000227 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000227 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000227 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=228
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000228 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000228 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000228 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000228 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END

set @no=229
select @min=stdsum,@start=instanttime from ( select top 1 stdsum ,instanttime from FM0000000229 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from ( select top 1 stdsum,instanttime from FM0000000229 order by id desc)r --select @max,@end
SELECT @typename=h.custtypename from FM0000000229 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno
if not EXISTS (select * from FMreportMonth where flmeterno=@no)
BEGIN


set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @start, 112),7,2)
set  @datestr = @year+@month+@day
set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=	CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)	
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month,@day=day from(

select top 1 year year ,month month,day day from fmreportMonth where  flmeterno=@no order by id desc)r


delete  from FmreportMonth where flmeterno=@no and year=@year and month=@month and day=@day

if(len(@day)=1)
BEGIN
set @day='0'+@day
end

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+@day

set @datestr2 = Convert(varchar(100), dateadd(day,1,@datestr),112)
set @i=0
while dateadd(day,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
set @day=SUBSTRING( CONVERT(varchar(100), @datestr, 112),7,2)

select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000229 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportMonth (FLMeterno,year,month,day,stdsum,custtypename) values(@no,@year,@month,@day,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=CONVERT(varchar(100), dateadd(day,1,@datestr2), 112)
	set @datestr=@temp
	END
END
";
            SqlHelper.Instance.ExcuteNon2(str);
        }

        public void updateYear()
        {
            var str = @"declare @no INT
declare @typename nvarchar(10)
declare @stdsum DECIMAL(12,2)
set @stdsum=0
DECLARE @start datetime
DECLARE @end datetime
declare @min DECIMAL(12,2)
DECLARE @max decimal(12,2)
declare @year varchar(10)
declare @month varchar(10)
declare @datestr varchar(20)
declare @datestr2 varchar(20)
declare @temp varchar(20)
set @temp=''
DECLARE @i int 

set @no=1
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000001 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000001 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000001 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000001 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000001 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=98
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000098 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000098 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000098 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000098 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000098 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=103
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000103 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000103 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000103 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000103 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000103 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=105
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000105 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000105 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000105 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000105 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000105 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=106
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000106 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000106 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000106 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000106 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000106 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=107
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000107 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000107 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000107 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000107 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000107 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=111
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000111 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000111 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000111 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000111 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000111 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=112
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000112 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000112 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000112 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000112 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000112 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=113
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000113 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000113 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000113 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000113 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000113 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=114
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000114 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000114 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000114 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000114 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000114 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=115
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000115 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000115 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000115 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000115 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000115 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=116
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000116 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000116 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000116 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000116 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000116 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=117
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000117 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000117 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000117 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000117 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000117 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=118
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000118 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000118 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000118 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000118 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000118 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=120
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000120 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000120 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000120 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000120 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000120 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=121
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000121 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000121 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000121 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000121 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000121 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=122
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000122 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000122 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000122 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000122 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000122 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=124
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000124 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000124 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000124 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000124 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000124 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=125
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000125 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000125 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000125 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000125 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000125 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=126
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000126 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000126 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000126 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000126 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000126 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=127
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000127 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000127 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000127 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000127 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000127 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=128
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000128 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000128 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000128 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000128 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000128 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=129
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000129 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000129 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000129 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000129 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000129 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=132
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000132 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000132 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000132 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000132 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000132 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=133
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000133 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000133 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000133 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000133 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000133 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=134
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000134 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000134 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000134 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000134 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000134 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=135
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000135 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000135 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000135 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000135 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000135 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=137
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000137 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000137 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000137 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000137 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000137 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=139
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000139 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000139 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000139 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000139 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000139 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=140
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000140 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000140 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000140 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000140 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000140 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=141
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000141 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000141 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000141 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000141 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000141 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=142
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000142 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000142 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000142 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000142 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000142 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=143
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000143 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000143 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000143 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000143 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000143 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=145
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000145 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000145 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000145 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000145 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000145 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=147
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000147 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000147 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000147 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000147 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000147 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=152
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000152 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000152 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000152 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000152 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000152 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=153
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000153 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000153 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000153 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000153 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000153 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=154
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000154 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000154 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000154 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000154 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000154 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=155
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000155 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000155 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000155 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000155 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000155 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=156
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000156 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000156 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000156 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000156 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000156 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=157
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000157 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000157 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000157 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000157 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000157 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=158
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000158 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000158 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000158 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000158 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000158 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=160
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000160 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000160 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000160 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000160 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000160 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=161
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000161 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000161 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000161 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000161 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000161 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=162
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000162 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000162 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000162 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000162 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000162 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=163
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000163 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000163 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000163 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000163 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000163 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=164
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000164 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000164 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000164 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000164 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000164 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=166
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000166 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000166 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000166 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000166 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000166 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=167
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000167 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000167 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000167 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000167 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000167 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=168
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000168 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000168 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000168 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000168 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000168 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=172
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000172 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000172 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000172 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000172 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000172 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=173
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000173 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000173 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000173 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000173 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000173 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=174
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000174 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000174 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000174 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000174 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000174 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=175
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000175 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000175 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000175 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000175 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000175 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=176
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000176 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000176 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000176 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000176 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000176 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=177
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000177 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000177 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000177 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000177 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000177 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=178
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000178 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000178 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000178 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000178 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000178 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=179
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000179 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000179 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000179 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000179 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000179 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=180
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000180 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000180 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000180 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000180 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000180 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=181
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000181 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000181 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000181 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000181 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000181 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=183
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000183 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000183 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000183 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000183 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000183 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=185
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000185 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000185 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000185 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000185 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000185 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=186
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000186 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000186 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000186 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000186 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000186 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=187
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000187 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000187 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000187 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000187 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000187 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=188
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000188 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000188 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000188 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000188 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000188 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=189
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000189 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000189 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000189 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000189 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000189 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=191
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000191 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000191 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000191 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000191 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000191 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=192
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000192 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000192 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000192 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000192 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000192 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=194
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000194 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000194 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000194 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000194 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000194 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=197
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000197 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000197 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000197 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000197 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000197 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=198
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000198 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000198 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000198 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000198 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000198 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=199
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000199 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000199 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000199 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000199 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000199 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=200
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000200 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000200 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000200 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000200 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000200 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=201
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000201 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000201 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000201 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000201 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000201 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=202
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000202 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000202 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000202 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000202 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000202 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=203
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000203 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000203 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000203 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000203 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000203 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=204
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000204 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000204 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000204 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000204 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000204 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=205
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000205 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000205 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000205 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000205 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000205 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=206
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000206 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000206 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000206 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000206 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000206 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=208
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000208 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000208 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000208 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000208 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000208 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=209
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000209 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000209 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000209 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000209 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000209 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=212
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000212 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000212 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000212 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000212 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000212 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=214
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000214 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000214 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000214 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000214 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000214 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=215
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000215 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000215 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000215 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000215 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000215 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=216
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000216 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000216 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000216 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000216 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000216 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=217
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000217 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000217 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000217 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000217 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000217 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=218
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000218 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000218 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000218 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000218 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000218 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=219
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000219 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000219 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000219 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000219 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000219 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=220
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000220 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000220 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000220 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000220 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000220 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=221
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000221 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000221 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000221 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000221 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000221 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=222
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000222 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000222 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000222 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000222 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000222 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=223
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000223 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000223 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000223 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000223 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000223 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=224
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000224 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000224 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000224 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000224 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000224 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=225
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000225 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000225 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000225 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000225 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000225 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=226
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000226 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000226 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000226 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000226 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000226 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=227
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000227 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000227 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000227 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000227 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000227 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=228
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000228 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000228 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000228 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000228 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000228 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END

set @no=229
select @min=stdsum,@start=instanttime from (  select top 1 stdsum ,instanttime from FM0000000229 order by id )r --select @min,@start
select @max=stdsum,@end=instanttime from (   select top 1 stdsum,instanttime from FM0000000229 order by id desc)r --select @max,@end
if not EXISTS (select * from FMreportYear where flmeterno=@no)
BEGIN
SELECT @typename=h.custtypename from FM0000000229 f LEFT JOIN HHMDeviceView h on h.meterno =f.flmeterno

set @year = SUBSTRING( CONVERT(varchar(100), @start, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @start, 112),5,2)
select  @datestr = @year+@month+'01'
set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN

set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @datestr
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr)r1,
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END

END
else 
BEGIN
select @year=year,@month=month from(
select top 1 year year ,month month from fmreportyear where  flmeterno=@no order by id desc)r

delete  from FmreportYear where flmeterno=@no and year=@year and month=@month

if(len(@month)=1)
BEGIN
set @month='0'+@month
END
set @datestr= @year+@month+'01'

set @datestr2 = Convert(varchar(100), dateadd(month,1,@datestr),112)
select @datestr,@datestr2
set @i=0
while dateadd(month,1,@end)>@datestr2
	BEGIN
set @year = SUBSTRING( CONVERT(varchar(100), @datestr, 112),0,5)
set @month = SUBSTRING( CONVERT(varchar(100), @datestr, 112),5,2)
select @stdsum = r2.stdsum-r1.stdsum from 
	(select top 1 stdsum from FM0000000229 where instanttime>@datestr)r1,
	(select top 1 stdsum from FM0000000229 where instanttime<@datestr2 order by instanttime desc)r2
insert FMReportYear (FLMeterno,year,month,stdsum,custtypename) values(@no,@year,@month,@stdsum,@typename)
	set  @temp=@datestr2
	set @datestr2=dateadd(month,1,@datestr2)
	set @datestr=@temp
	END
END
";
            SqlHelper.Instance.ExcuteNon2(str);
        }
        #endregion

        public void AddReportYears()
        {
            //var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo("超级管理员", null);
            var tableNames = DeviceInfoOper.Instance.GetHisTableNames();
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);
            tableNames = HisOper.Instance.RemoveBadTable(tableNames);
            var meterNos = HisOper.Instance.GetDeviceIds(tableNames);

            //var tableName = "FM0000000192";
            //var meterNo = 192;
            for (int i = 0; i < tableNames.Count; i++)
            {
                AddReportYear(tableNames[i], meterNos[i]);
            }

        }

        public void AddReportYear(string tableName, int meterNo)
        {
            string str2 = $"select DISTINCT h.custtypename from {tableName} f left join HHMDeviceView h on h.meterNo=f.flmeterno";
            var type = SqlHelper.Instance.ExecuteScalar2(str2);
            string str = $@"select distinct substring(CONVERT(varchar(100), instanttime, 112) ,0,5)as instanttime from {tableName} order by instanttime";
            var listDate = SqlHelper.Instance.ExecuteGetDt2<temp>(str, new Dictionary<string, string>());
            foreach (var item in listDate)
            {
                var year = Convert.ToInt32(item.instanttime);
                for (int i = 1; i < 13; i++)
                {
                    var dt1 = new DateTime(year, i, 1);
                    var dt2 = dt1.AddMonths(1);

                    str = $@"select * from {tableName} where instanttime>='{dt1}'  and instanttime<='{dt2}' ";
                    var list = SqlHelper.Instance.ExecuteGetDt2<FMModel>(str, new Dictionary<string, string>());
                    if (list.Count > 0)
                    {
                        FMReportYear f = new FMReportYear();
                        f.FlMeterNo = meterNo;
                        f.year = year;
                        f.month = i;
                        f.custTypeName = type;
                        var min = list.Min(p => p.StdSum);
                        var max = list.Max(p => p.StdSum);
                        f.stdsum = max - min;
                        FMReportYearOper.Instance.Add2(f);
                    }
                }
            }
        }

        public void AddReportMonths()
        {
            //var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo("超级管理员", null);
            var tableNames = DeviceInfoOper.Instance.GetHisTableNames();
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);
            tableNames = HisOper.Instance.RemoveBadTable(tableNames);
            var meterNos = HisOper.Instance.GetDeviceIds(tableNames);

            //var tableName = "FM0000000192";
            //var meterNo = 192;
            for (int i = 0; i < tableNames.Count; i++)
            {
                AddReportMonth(tableNames[i], meterNos[i]);
            }

        }

        public void AddReportMonth(string tableName, int meterNo)
        {
            string str2 = $"select DISTINCT h.custtypename from {tableName} f left join HHMDeviceView h on h.meterNo=f.flmeterno";
            var type = SqlHelper.Instance.ExecuteScalar2(str2);
            string str = $@"select distinct substring(CONVERT(varchar(100), instanttime, 112) ,0,7)as instanttime from {tableName} order by instanttime";
            var listDate = SqlHelper.Instance.ExecuteGetDt2<temp>(str, new Dictionary<string, string>());
            foreach (var item in listDate)
            {
                var year = Convert.ToInt32(item.instanttime.Substring(0, 4));
                var month = Convert.ToInt32(item.instanttime.Substring(4, 2));
                var days = DateTime.DaysInMonth(year, month);
                for (int i = 1; i < days + 1; i++)
                {
                    var dt1 = new DateTime(year, month, i);
                    var dt2 = dt1.AddDays(1);

                    str = $@"select * from {tableName} where instanttime>='{dt1}'  and instanttime<='{dt2}' ";
                    var list = SqlHelper.Instance.ExecuteGetDt2<FMModel>(str, new Dictionary<string, string>());
                    if (list.Count > 0)
                    {
                        FMReportMonth f = new FMReportMonth();
                        f.FlMeterNo = meterNo;
                        f.year = year;
                        f.month = month;
                        f.day = i;
                        f.custTypeName = type;
                        var min = list.Min(p => p.StdSum);
                        var max = list.Max(p => p.StdSum);
                        f.stdsum = max - min;
                        FMReportMonthOper.Instance.Add2(f);
                    }
                }
            }
        }

        public void AddReportDays()
        {
            var tableNames = DeviceInfoOper.Instance.GetHisTableNames();
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);
            tableNames = HisOper.Instance.RemoveBadTable(tableNames);
            var meterNos = HisOper.Instance.GetDeviceIds(tableNames);


            for (int i = 0; i < tableNames.Count; i++)
            {
                AddReportDay(tableNames[i], meterNos[i]);
            }

        }

        public void AddReportDay(string tableName, int meterNo)
        {
            string sql = "";
            string str2 = $"select DISTINCT h.custtypename from {tableName} f left join HHMDeviceView h on h.meterNo=f.flmeterno";
            var type = SqlHelper.Instance.ExecuteScalar2(str2);
            string str = $@"select distinct CONVERT(varchar(100), instanttime, 112) as instanttime from {tableName} order by instanttime";
            var listDate = SqlHelper.Instance.ExecuteGetDt2<temp>(str, new Dictionary<string, string>());
            foreach (var item in listDate)
            {
                var year = Convert.ToInt32(item.instanttime.Substring(0, 4));
                var month = Convert.ToInt32(item.instanttime.Substring(4, 2));
                var day = Convert.ToInt32(item.instanttime.Substring(6, 2));

                for (int i = 0; i < 24; i++)
                {
                    var dt1 = new DateTime(year, month, day, i, 0, 0);
                    var dt2 = dt1.AddHours(1);

                    str = $@"select * from {tableName} where instanttime>='{dt1}'  and instanttime<='{dt2}' ";
                    var list = SqlHelper.Instance.ExecuteGetDt2<FMModel>(str, new Dictionary<string, string>());
                    if (list.Count > 0)
                    {
                        FMReportDay f = new FMReportDay();
                        f.FlMeterNo = meterNo;
                        f.year = year;
                        f.month = month;
                        f.day = day;
                        f.hour = i;
                        f.custTypeName = type;
                        var min = list.Min(p => p.StdSum);
                        var max = list.Max(p => p.StdSum);
                        f.stdsum = max - min;
                        //FMReportDayOper.Instance.Add2(f);
                        sql += $@"
                                    {FMReportDayOper.Instance.GetInsertStr2(f)}
                                    ";
                    }
                }
            }
            if (sql != "")
                SqlHelper.Instance.ExcuteNon2(sql);
        }

        public string SendData(string msg)
        {
            if (msg == null)
                msg = "FFFF,100D,0571123456";
            var r = AllFunc.Instance.SendMsg2(msg);
            return JsonConvert.SerializeObject(r);
        }

        public string test66()
        {
            return "cool";
        }
    }
}