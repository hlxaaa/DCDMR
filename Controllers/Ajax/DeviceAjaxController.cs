using Common.Filter.Mvc;
using Common.Helper;
using Common.Result;
using DbOpertion.DBoperation;
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
    public class DeviceAjaxController : AjaxBaseController
    {
        public string GetDeviceExcelCount()
        {
            ResultJson r = new ResultJson
            {
                HttpCode = 200,
                Message = "1"
            };
            var req = JsonConvert.DeserializeObject<MeterReq>(Session["GetMeterListReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var list = AllFunc.Instance.GetMeterListForExcel(req, user);
            if (list.Count == 0)
                r.Message = "0";
            return JsonConvert.SerializeObject(r);
        }

        public FileResult GetDeviceExcel()
        {
            var req = JsonConvert.DeserializeObject<MeterReq>(Session["GetMeterListReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetMeterListForExcel(req, user);
            if (r.Count == 0)
                return null;
            return ExportExcelForDeviceData(r);
        }

        public FileResult ExportExcelForDeviceData(List<DeviceViewRes> list)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("协议");
            row1.CreateCell(1).SetCellValue("制造号");
            row1.CreateCell(2).SetCellValue("通讯编号");
            row1.CreateCell(3).SetCellValue("客户名称");
            row1.CreateCell(4).SetCellValue("地址");
            row1.CreateCell(5).SetCellValue("客户编号");
            row1.CreateCell(6).SetCellValue("表计类型");
            row1.CreateCell(7).SetCellValue("价格类型");
            row1.CreateCell(8).SetCellValue("开户状态");
            row1.CreateCell(9).SetCellValue("最后编辑时间");

            //将数据逐步写入sheet1各个行
            //var list = new List<FMModel>();
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].ProtocolNo ?? "");
                rowtemp.CreateCell(1).SetCellValue(list[i].deviceNo);
                rowtemp.CreateCell(2).SetCellValue(list[i].communicateNo.ToString());
                rowtemp.CreateCell(3).SetCellValue(list[i].customerName.ToString());
                rowtemp.CreateCell(4).SetCellValue(list[i].address.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].customerNo.ToString());
                rowtemp.CreateCell(6).SetCellValue(list[i].meterTypeName.ToString());
                rowtemp.CreateCell(7).SetCellValue(list[i].fluidName.ToString());
                rowtemp.CreateCell(8).SetCellValue(list[i].openStateName.ToString());
                rowtemp.CreateCell(9).SetCellValue(list[i].editTime.ToString());
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string fileName = "设备数据查询" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);

        }

        public string GetCustomerExcelCount()
        {
            ResultJson r = new ResultJson
            {
                HttpCode = 200,
                Message = "1"
            };
            var req = JsonConvert.DeserializeObject<CustomerReq>(Session["GetCustomerListReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var list = AllFunc.Instance.GetCustomerListForExcel(req, user);
            if (list.Count == 0)
                r.Message = "0";
            return JsonConvert.SerializeObject(r);
        }

        public FileResult GetCustomerExcel()
        {
            var req = JsonConvert.DeserializeObject<CustomerReq>(Session["GetCustomerListReq"].ToString());
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetCustomerListForExcel(req, user);
            if (r.Count == 0)
                return null;
            return ExportExcelForCustomerData(r);
        }

        public FileResult ExportExcelForCustomerData(List<CustomerViewRes> list)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("客户编号");
            row1.CreateCell(1).SetCellValue("客户名称");
            row1.CreateCell(2).SetCellValue("客户类型");
            row1.CreateCell(3).SetCellValue("使用状态");
            row1.CreateCell(4).SetCellValue("表计厂家");
            row1.CreateCell(5).SetCellValue("表计类型");
            row1.CreateCell(6).SetCellValue("固定电话");
            row1.CreateCell(7).SetCellValue("移动电话");
            row1.CreateCell(8).SetCellValue("小区名称");
            row1.CreateCell(9).SetCellValue("详细地址");
            row1.CreateCell(10).SetCellValue("自定义编号1");
            row1.CreateCell(11).SetCellValue("自定义编号2");
            row1.CreateCell(12).SetCellValue("自定义编号3");
            row1.CreateCell(13).SetCellValue("备注");
            row1.CreateCell(14).SetCellValue("建档时间");
            row1.CreateCell(15).SetCellValue("最后编辑时间");
            row1.CreateCell(16).SetCellValue("操作员名称");

            //将数据逐步写入sheet1各个行
            //var list = new List<FMModel>();
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].customerNo);
                rowtemp.CreateCell(1).SetCellValue(list[i].customerName);
                rowtemp.CreateCell(2).SetCellValue(list[i].customerType);
                rowtemp.CreateCell(3).SetCellValue(list[i].useStateName);
                rowtemp.CreateCell(4).SetCellValue(list[i].factoryName);
                rowtemp.CreateCell(5).SetCellValue(list[i].meterTypeName);
                rowtemp.CreateCell(6).SetCellValue(list[i].telNo);
                rowtemp.CreateCell(7).SetCellValue(list[i].mobileNo);
                rowtemp.CreateCell(8).SetCellValue(list[i].estateName);
                rowtemp.CreateCell(9).SetCellValue(list[i].address);
                rowtemp.CreateCell(10).SetCellValue(list[i].defineNo1);
                rowtemp.CreateCell(11).SetCellValue(list[i].defineNo2);
                rowtemp.CreateCell(12).SetCellValue(list[i].defineNo3);
                rowtemp.CreateCell(13).SetCellValue(list[i].remark);
                rowtemp.CreateCell(14).SetCellValue(list[i].buildTime);
                rowtemp.CreateCell(15).SetCellValue(list[i].editTime);
                rowtemp.CreateCell(16).SetCellValue(list[i].Operator);
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string fileName = "客户数据查询" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);

        }

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetCustomerList(CustomerReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetCustomerList(req, user);
            if (r.HttpCode == 200)
            {
                Session["GetCustomerListReq"] = JsonConvert.SerializeObject(req);
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]

        public string GetMeterList(MeterReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetMeterList(req, user);
            if (r.HttpCode == 200)
            {
                Session["GetMeterListReq"] = JsonConvert.SerializeObject(req);
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 新增设备
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string AddDevice(DeviceInfoReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.AddDevice(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string UpdateDevice(DeviceUpdateReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.UpdateDevice(req, user);
            return JsonConvert.SerializeObject(r);

            //var userId = Convert.ToInt32(Session["userId"]);
            //var name = Session["userName"].ToString();
            //var r = AllFunc.Instance.AddDevice(req, userId, name);
            //return JsonConvert.SerializeObject(r);
        }

        [MvcValidate]
        public string DeleteDevice(DeviceDelReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.DelDevice(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string AddCustomer(CustomerAddReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var name = Session["userName"].ToString();
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.AddCustomer(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string UpdateCustomer(CustomerUpdateReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.UpdateCustomer(req, user);
            return JsonConvert.SerializeObject(r);
        }

        [MvcValidate]
        public string DeleteCustomer(CustomerDelReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.DelCustomer(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 开户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string Establish(EstablishReq req)
        {
            if (req.meterNo != null && req.meterNo != "")
            {
                if (DeviceInfoOper.Instance.IsDeviceOpen(Convert.ToInt32(req.meterNo)))
                    throw new Exception("此设备已开户");
            }

            if (req.customerNo != null & req.customerNo != "")
            {
                if (CustomerInfoOper.Instance.IsCustomerOpen(req.customerNo))
                    throw new Exception("此用户已开户");
            }

            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var customer = new CustomerInfo(req);
            if (req.customerNo == null)
                customer.customerNo = AllFunc.Instance.GetNewCustomerNo();
            else
                customer.customerNo = req.customerNo;
            var now = DateTime.Now;
            customer.buildTime = now;
            customer.editTime = now;
            customer.Operator = user.name;
            var device = new DeviceInfo(req,user.id);

            if (req.communicateNo != null && req.meterNo == null && SqlHelper.Instance.IsExists2("deviceInfo", "communicateNo", req.communicateNo, "meterNo", "0"))
                throw new Exception("已存在该通讯编号");

            device.buildTime = now;
            if (device.meterNo == null)
                device.meterNo = 0;
            device.lat = req.lat;
            device.lng = req.lng;
            device.editTime = now;
            device.Operator = user.name;
            device.ProtocolNo = req.ProtocolNo;
            ////新奥协议
            //if (device.ProtocolNo == "1001")
            //{
            //    device.CommMode = 1;//通讯模式
            //    device.LinkMode = 1;//连接模式
            //}
            ////标准协议
            //else if (device.ProtocolNo == "1002")
            //{
            //    device.CommMode = 1;//通讯模式
            //    device.LinkMode = 0;//连接模式
            //}
            ////信东协议1 信东协议2
            //else if (device.ProtocolNo == "1003" || device.ProtocolNo == "1004")
            //{
            //    device.CommMode = 1;//通讯模式
            //    device.LinkMode = 0;//连接模式
            //}
            var r = AllFunc.Instance.OpenAccount(user, customer, device, (int)req.userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取未开户的客户
        /// </summary>
        /// <returns></returns>
        public string GetNotOpenCustomer()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetNotOpenCustomers(user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取未开户的设备
        /// </summary>
        /// <returns></returns>
        public string GetNotOpenDevice()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetNotOpenMeters(user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 由设备号获取设备信息
        /// </summary>
        /// <param name="meterNo"></param>
        /// <returns></returns>
        public string GetDeviceByMeterNo(int meterNo)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var device = AllFunc.Instance.GetDeviceByNo(meterNo, user);
            var view = DeviceInfoOper.Instance.GetViewByNo(meterNo, user);
            DeviceRes res = new DeviceRes(view);
            ResultJson<DeviceRes> r = new ResultJson<DeviceRes>
            {
                HttpCode = 200
            };
            r.ListData.Add(res);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新设备的经纬度
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string UpdateLatlng(UpdateLatlngReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.UpdateLatlng(req, user);
            return JsonConvert.SerializeObject(r);
        }

        public string GetDeviceListByCompanyName(string name)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            if (name == "")
                name = user.name;

            var r = AllFunc.Instance.GetDeviceListByCompanyName(name, user);
            return JsonConvert.SerializeObject(r);
        }
    }
}