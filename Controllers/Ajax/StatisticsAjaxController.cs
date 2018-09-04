using Common.Filter.Mvc;
using Common.Result;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Ajax
{
    public class StatisticsAjaxController : AjaxBaseController
    {
        #region 充值记录
        /// <summary>
        /// 曲线图，柱形图
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetChargeLine(GetChargeReportReq req)
        {
            //req.date = "2016-08-01";
            //req.customerNo = "0000000681";
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            req.date = req.date ?? DateTime.Now.ToString("yyyy-MM-dd");
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());

            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetChargeLine(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        ///  表格
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetChargeReport(GetChargeReportReq req)
        {
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            req.date = req.date ?? DateTime.Now.ToString("yyyy-MM-dd");

            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());

            req.lastName = req.lastName ?? user.name;

            //return null;
            var r = AllFunc.Instance.GetChargeLine(req);
            if (r.HttpCode == 200)
            {
                Session["gcrReq"] = JsonConvert.SerializeObject(req);
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 圆饼 用户类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetChargePieByType(GetChargeReportReq req)
        {
            //req.date = "2016-08-01";
            //req.customerNo = "0000000681";
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetChargePieByType(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 圆饼，时间分
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetChargePieByTime(GetChargeReportReq req)
        {
            //req.date = "2016-08-01";
            //req.customerNo = "0000000681";
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetChargePieByTime(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取excel中记录的条数
        /// </summary>
        /// <returns></returns>
        public string GetExcelCount()
        {
            ResultJson r = new ResultJson
            {
                HttpCode = 200,
                Message = "1"
            };
            var req = JsonConvert.DeserializeObject<GetChargeReportReq>(Session["gcrReq"].ToString());
            var list = AllFunc.Instance.GetChargeExcelCount(req);
            if (list.Count == 0)
                r.Message = "0";
            return JsonConvert.SerializeObject(r);
        }

        public FileResult GetChargeExcel()
        {
            var req = JsonConvert.DeserializeObject<GetChargeReportReq>(Session["gcrReq"].ToString());
            var list = AllFunc.Instance.GetChargeExcelCount(req);
            if (list.Count == 0)
                return null;
            return ExportExcelForCharge(list);
        }

        /// <summary>
        /// 程序内用
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public FileResult ExportExcelForCharge(List<chargeReport> list)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("时间");
            row1.CreateCell(1).SetCellValue("充值气量");
            //row1.CreateCell(2).SetCellValue("充值金额");

            //将数据逐步写入sheet1各个行
            //var list = new List<FMModel>();
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].dt.ToString());
                rowtemp.CreateCell(1).SetCellValue(list[i].sumVolume.ToString());
                //rowtemp.CreateCell(2).SetCellValue(list[i].sumMoney.ToString());

            }
            MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string fileName = "充值统计查询" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);

        }



        #endregion

        #region 用户使用量
        /// <summary>
        /// 曲线图，柱形图
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetStdSumLine(GetStdSumReq req)
        {
            //req.customerNo = "0000000681";
            //req.date = "2018-03-30";
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            req.date = req.date ?? DateTime.Now.ToString("yyyy-MM-dd");
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetStdSumLine(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 圆饼 用户类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetStdSumPieByType(GetStdSumReq req)
        {
            //req.customerNo = "0000000681";
            //req.date = "2018-03-30";
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetStdSumPieByType(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 圆饼 时间分
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetStdSumPieByTime(GetStdSumReq req)
        {
            //req.customerNo = "0000000681";
            //req.date = "2018-03-30";
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetStdSumPieByTime(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 表格
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetStdSumReport(GetStdSumReq req)
        {
            switch (req.type)
            {
                case "year":
                    req.date = new DateTime(Convert.ToInt32(req.iYear), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr = req.iMonth.Split('-');
                    req.date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            req.date = req.date ?? DateTime.Now.ToString("yyyy-MM-dd");

            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            //return null;
            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetStdSumLine(req);
            if (r.HttpCode == 200)
            {
                Session["gssReq"] = JsonConvert.SerializeObject(req);
            }
            return JsonConvert.SerializeObject(r);
        }

        public string GetExcelCountSSR()
        {
            ResultJson r = new ResultJson
            {
                HttpCode = 200,
                Message = "1"
            };
            var req = JsonConvert.DeserializeObject<GetStdSumReq>(Session["gssReq"].ToString());
            var list = AllFunc.Instance.GetStdSumExcelCount(req);
            if (list.Count == 0)
                r.Message = "0";
            return JsonConvert.SerializeObject(r);
        }

        public FileResult GetChargeExcelSSR()
        {
            var req = JsonConvert.DeserializeObject<GetStdSumReq>(Session["gssReq"].ToString());
            var list = AllFunc.Instance.GetStdSumExcelCount(req);
            if (list.Count == 0)
                return null;
            return ExportExcelForChargeSSR(list);
        }

        public FileResult ExportExcelForChargeSSR(List<StdSumReport> list)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("时间");
            row1.CreateCell(1).SetCellValue("使用气量");
            //row1.CreateCell(2).SetCellValue("充值金额");

            //将数据逐步写入sheet1各个行
            //var list = new List<FMModel>();
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].dt.ToString());
                rowtemp.CreateCell(1).SetCellValue(list[i].span.ToString());
                //rowtemp.CreateCell(2).SetCellValue(list[i].sumMoney.ToString());
            }
            MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string fileName = "用量统计查询" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);

        }
        #endregion

    }
}