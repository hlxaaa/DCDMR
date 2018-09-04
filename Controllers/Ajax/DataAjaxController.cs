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
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Ajax
{
    public class DataAjaxController : AjaxBaseController
    {
        /// <summary>
        /// 获取实时抄表数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetMeterDataList(MeterDataListReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetMeterDataList(req, user);
            if (r.HttpCode == 200)
            {
                Session["GetMeterDataReq"] = JsonConvert.SerializeObject(req);
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 开关阀，还没用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string ChangeValve(ValveReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.ChangeValve(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取历史数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetHisData(HisReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            req.lastName = req.lastName ?? user.name;
            var r = AllFunc.Instance.GetFMList(req, user);
            if (r.HttpCode == 200)
                Session["HisReq"] = JsonConvert.SerializeObject(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 地图上显示设备，获取信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetDeviceOnMap(DeviceMapReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetDeviceOnMap(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 由设备号获取某个设备的信息
        /// </summary>
        /// <param name="meterNo"></param>
        /// <returns></returns>
        public string GetOneDeviceByMeterNo(int meterNo)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetOneDeviceViewByNo(meterNo, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 由设备号获取某个设备的信息
        /// </summary>
        /// <param name="meterNo"></param>
        /// <returns></returns>
        public string GetDeviceByMeterNo(int meterNo)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetDeviceViewByNo(meterNo, user);
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
            var req = JsonConvert.DeserializeObject<HisReq>(Session["HisReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var list = AllFunc.Instance.GetFMListForExcel(req, user);
            if (list.Count == 0)
                r.Message = "0";
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取历史数据的excel，会触发浏览器下载文件
        /// </summary>
        /// <returns></returns>
        public FileResult GetHisDataExcel()
        {
            var req = JsonConvert.DeserializeObject<HisReq>(Session["HisReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var list = AllFunc.Instance.GetFMListForExcel(req, user);
            if (list.Count == 0)
                return null;
            return ExportExcelForHisData(list);
        }

        /// <summary>
        /// 跟上面的差不多。随意备注一下
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public FileResult ExportExcelForHisData(List<FMModel> list)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("制造号");
            row1.CreateCell(1).SetCellValue("通讯编号");
            row1.CreateCell(2).SetCellValue("总累计量");
            row1.CreateCell(3).SetCellValue("工况总量");
            row1.CreateCell(4).SetCellValue("瞬时流量");
            row1.CreateCell(5).SetCellValue("工况流量");
            row1.CreateCell(6).SetCellValue("温度");
            row1.CreateCell(7).SetCellValue("压力");
            row1.CreateCell(8).SetCellValue("剩余金额");
            row1.CreateCell(9).SetCellValue("剩余气量");
            row1.CreateCell(10).SetCellValue("供电电压");
            row1.CreateCell(11).SetCellValue("表具状态");
            row1.CreateCell(12).SetCellValue("客户名称");
            row1.CreateCell(13).SetCellValue("客户地址");
            row1.CreateCell(14).SetCellValue("采集时间");
            //将数据逐步写入sheet1各个行
            //var list = new List<FMModel>();
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].deviceNo.ToString());
                rowtemp.CreateCell(1).SetCellValue(list[i].communicateNo ?? "");
                rowtemp.CreateCell(2).SetCellValue(list[i].StdSum.ToString());
                rowtemp.CreateCell(3).SetCellValue(list[i].WorkSum.ToString());
                rowtemp.CreateCell(4).SetCellValue(list[i].StdFlow.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].WorkFlow.ToString());
                rowtemp.CreateCell(6).SetCellValue(list[i].Temperature.ToString());
                rowtemp.CreateCell(7).SetCellValue(list[i].Pressure.ToString());
                rowtemp.CreateCell(8).SetCellValue(list[i].RemainMoney.ToString());
                rowtemp.CreateCell(9).SetCellValue(list[i].RemainVolume.ToString());
                rowtemp.CreateCell(10).SetCellValue(list[i].PowerVoltage.ToString());
                rowtemp.CreateCell(11).SetCellValue(list[i].FMStateMsg.ToString());
                rowtemp.CreateCell(12).SetCellValue(list[i].customerName.ToString());
                rowtemp.CreateCell(13).SetCellValue(list[i].address.ToString());
                rowtemp.CreateCell(14).SetCellValue(list[i].InstantTime.ToString());
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string fileName = "历史数据查询" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);

        }

        /// <summary>
        /// 验证这个用户输入的操作密码对不对
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string ConfirmUser(ConfirmUserReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());

            var r = AllFunc.Instance.ConfirmUser2(user, req.pwd);

            return JsonConvert.SerializeObject(r);

        }

        public string ValveOper(ValveOperReq req)
        {
            //return null;
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.ValveOper(user, req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取某年某月有多少天
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetMonthDays(GetDaysReq req)
        {
            var r = AllFunc.Instance.GetDaysHtml(req);
            return JsonConvert.SerializeObject(r);
        }

        [MvcValidate]
        public string ChargeOper(ChargeOperReq req)
        {

            //return null;
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.ChargeOper(user, req);
            return JsonConvert.SerializeObject(r);


        }

        public string GetMeterDataCount()
        {
            ResultJson r = new ResultJson
            {
                HttpCode = 200,
                Message = "1"
            };
            var req = JsonConvert.DeserializeObject<MeterDataListReq>(Session["GetMeterDataReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var list = AllFunc.Instance.GetMeterDataListForExcel(req, user);
            if (list.Count == 0)
                r.Message = "0";
            return JsonConvert.SerializeObject(r);
        }

        public FileResult GetMeterDataExcel()
        {
            var req = JsonConvert.DeserializeObject<MeterDataListReq>(Session["GetMeterDataReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetMeterDataListForExcel(req, user);
            if (r.Count == 0)
                return null;
            return ExportExcelForMeterData(r);
        }

        public FileResult ExportExcelForMeterData(List<OneFLMeterDataViewRes> list)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("是否在线");
            row1.CreateCell(1).SetCellValue("制造号");
            row1.CreateCell(2).SetCellValue("通讯编号");
            row1.CreateCell(3).SetCellValue("客户名称");
            row1.CreateCell(4).SetCellValue("客户地址");
            row1.CreateCell(5).SetCellValue("总累积量");
            row1.CreateCell(6).SetCellValue("工况总量");
            row1.CreateCell(7).SetCellValue("瞬时流量");
            row1.CreateCell(8).SetCellValue("工况流量");
            row1.CreateCell(9).SetCellValue("温度");
            row1.CreateCell(10).SetCellValue("压力");
            row1.CreateCell(11).SetCellValue("剩余金额");
            row1.CreateCell(12).SetCellValue("剩余气量");
            row1.CreateCell(13).SetCellValue("供电电压");
            row1.CreateCell(14).SetCellValue("表具状态");
            row1.CreateCell(15).SetCellValue("阀门状态");
            row1.CreateCell(16).SetCellValue("采集时间");

            //将数据逐步写入sheet1各个行
            //var list = new List<FMModel>();
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].LoginStateMsg);
                rowtemp.CreateCell(1).SetCellValue(list[i].deviceNo);
                rowtemp.CreateCell(2).SetCellValue(list[i].communicateNo);
                rowtemp.CreateCell(3).SetCellValue(list[i].customerName);
                rowtemp.CreateCell(4).SetCellValue(list[i].address);
                rowtemp.CreateCell(5).SetCellValue(list[i].StdSum);
                rowtemp.CreateCell(6).SetCellValue(list[i].WorkSum);
                rowtemp.CreateCell(7).SetCellValue(list[i].StdFlow);
                rowtemp.CreateCell(8).SetCellValue(list[i].WorkFlow);
                rowtemp.CreateCell(9).SetCellValue(list[i].Temperature);
                rowtemp.CreateCell(10).SetCellValue(list[i].Pressure);
                rowtemp.CreateCell(11).SetCellValue(list[i].RemainMoney);
                rowtemp.CreateCell(12).SetCellValue(list[i].RemainVolume);
                rowtemp.CreateCell(13).SetCellValue(list[i].PowerVoltage);
                rowtemp.CreateCell(14).SetCellValue(list[i].FMStateMsg);
                rowtemp.CreateCell(15).SetCellValue(list[i].ValveStateMsg);
                rowtemp.CreateCell(16).SetCellValue(list[i].InstantTime);
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string fileName = "实时数据查询" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);

        }

    }
}