using Common.Helper;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace HHTDCDMR.Models.Extend.Res
{
    public class AllRes
    {

    }

    public class WXToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    public class WXUserList
    {
        public int total { get; set; }
        public int count { get; set; }
        public WXOpenIds data { get; set; }
        public string next_openid { get; set; }
    }

    public class WXOpenIds
    {
        public List<string> openid { get; set; }
    }

    public class UserIndexRes
    {
        public UserIndexRes() { }
        public UserIndexRes(List<AllInOne_UserPermissionView> views)
        {
            var view = views.First();
            id = view.id.ToString();
            name = view.name ?? "";
            account = view.account ?? "";
            pwd = view.pwd ?? "";
            var pers = views.Where(p => p.isOpen == true).Select(p => p.pername).ToArray();
            pername = StringHelper.Instance.ArrJoin(pers);
            areaName = view.areaName;
            if (view.areaId != null)
                areaId = view.areaId.ToString();
            isStaff = (bool)view.isStaff;
            level = view.level;
            phone = view.phone;
        }
        public string phone { get; set; }
        public int? level { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string pwd { get; set; }
        public string account { get; set; }
        public string pername { get; set; }
        public string areaName { get; set; }
        public bool isStaff { get; set; }
        public string areaId { get; set; }
    }

    public class LayoutIndex
    {
        public string activeName { get; set; }
        public List<nav> listNav { get; set; }
    }

    public class nav
    {
        public nav(string n, string c, string u)
        {
            name = n;
            classStr = c;
            url = u;
        }
        public string name { get; set; }
        public string classStr { get; set; }
        public string url { get; set; }
    }

    public class CustomerRes
    {
        public CustomerRes(CustomerInfo c)
        {
            customerNo = c.customerNo;
            customerName = c.customerName;
            customerType = c.customerType.ToString();
            useState = c.useState.ToString();
            telNo = c.telNo;
            mobileNo = c.mobileNo;
            estateNo = c.estateNo;
            defineNo1 = c.defineNo1;
            defineNo2 = c.defineNo2;
            defineNo3 = c.defineNo3;
            address = c.address;
            remark = c.remark;
            buildTime = c.buildTime.ToString();
            editTime = c.editTime.ToString();
            Operator = c.Operator.ToString();
            what1 = "";
            what2 = "";
        }

        public string customerNo { get; set; }
        public string customerName { get; set; }
        public string customerType { get; set; }
        public string useState { get; set; }
        public string what1 { get; set; }
        public string what2 { get; set; }
        public string telNo { get; set; }
        public string mobileNo { get; set; }
        public string estateNo { get; set; }
        public string address { get; set; }
        public string defineNo1 { get; set; }
        public string defineNo2 { get; set; }
        public string defineNo3 { get; set; }
        public string remark { get; set; }
        public string buildTime { get; set; }
        public string editTime { get; set; }
        public string Operator { get; set; }
    }

    public class CustomerViewRes
    {
        public CustomerViewRes(CustomerView view)
        {
            factoryName = view.factoryName ?? "";
            meterTypeName = view.meterTypeName ?? "";
            telNo = view.telNo ?? "";
            mobileNo = view.mobileNo ?? "";
            address = view.address ?? "";
            defineNo1 = view.defineNo1 ?? "";
            defineNo2 = view.defineNo2 ?? "";
            defineNo3 = view.defineNo3 ?? "";
            remark = view.remark ?? "";
            Operator = view.Operator ?? "";
        }

        /// <summary>
        /// 获取客户视图，时间格式转换
        /// </summary>
        /// <param name="view"></param>
        /// <param name="a"></param>
        public CustomerViewRes(CustomerView view, int a)
        {
            if (view.customerNo != null)
                customerNo = view.customerNo.ToString();
            else
                customerNo = "";
            if (view.customerType != null)
            {
                switch (view.customerType)
                {
                    case 0:
                        customerType = "居民";
                        break;
                    case 1:
                        customerType = "公建";
                        break;
                    case 2:
                        customerType = "工业";
                        break;
                    case 3:
                        customerType = "商福";
                        break;

                    default:
                        break;
                }
            }
            else
                customerType = "";
            if (view.contractNo != null)
                contractNo = view.contractNo.ToString();
            else
                contractNo = "";
            if (view.customerName != null)
                customerName = view.customerName.ToString();
            else
                customerName = "";
            if (view.telNo != null)
                telNo = view.telNo.ToString();
            else
                telNo = "";
            if (view.mobileNo != null)
                mobileNo = view.mobileNo.ToString();
            else
                mobileNo = "";
            if (view.certNo != null)
                certNo = view.certNo.ToString();
            else
                certNo = "";
            if (view.estateNo != null)
                estateNo = view.estateNo.ToString();
            else
                estateNo = "";
            if (view.estateName != null)
                estateName = view.estateName.ToString();
            else
                estateName = "";
            if (view.address != null)
                address = view.address.ToString();
            else
                address = "";
            if (view.houseNo != null)
                houseNo = view.houseNo.ToString();
            else
                houseNo = "";
            if (view.cellNo != null)
                cellNo = view.cellNo.ToString();
            else
                cellNo = "";
            if (view.roomNo != null)
                roomNo = view.roomNo.ToString();
            else
                roomNo = "";
            if (view.useState != null)
                useState = view.useState.ToString();
            else
                useState = "";
            if (view.defineNo1 != null)
                defineNo1 = view.defineNo1.ToString();
            else
                defineNo1 = "";
            if (view.defineNo2 != null)
                defineNo2 = view.defineNo2.ToString();
            else
                defineNo2 = "";
            if (view.defineNo3 != null)
                defineNo3 = view.defineNo3.ToString();
            else
                defineNo3 = "";
            if (view.remark != null)
                remark = view.remark.ToString();
            else
                remark = "";
            if (view.payWay != null)
                payWay = view.payWay.ToString();
            else
                payWay = "";
            if (view.bankNo != null)
                bankNo = view.bankNo.ToString();
            else
                bankNo = "";
            if (view.bankAuthNo != null)
                bankAuthNo = view.bankAuthNo.ToString();
            else
                bankAuthNo = "";
            if (view.accountName != null)
                accountName = view.accountName.ToString();
            else
                accountName = "";
            if (view.accountNo != null)
                accountNo = view.accountNo.ToString();
            else
                accountNo = "";
            if (view.bankCheck != null)
                bankCheck = view.bankCheck.ToString();
            else
                bankCheck = "";
            if (view.buildTime != null)
                buildTime = Convert.ToDateTime(view.buildTime).ToString("yyyy-MM-dd HH:mm:ss");
            else
                buildTime = "";
            if (view.editTime != null)
                editTime = Convert.ToDateTime(view.editTime).ToString("yyyy-MM-dd HH:mm:ss");
            else
                editTime = "";
            if (view.Operator != null)
                Operator = view.Operator.ToString();
            else
                Operator = "";
            if (view.taxNo != null)
                taxNo = view.taxNo.ToString();
            else
                taxNo = "";
            if (view.enterpriseNo != null)
                enterpriseNo = view.enterpriseNo.ToString();
            else
                enterpriseNo = "";
            if (view.CustTypeName != null)
                CustTypeName = view.CustTypeName.ToString();
            else
                CustTypeName = "";
            if (view.useStateName != null)
                useStateName = view.useStateName.ToString();
            else
                useStateName = "";
            if (view.payWayName != null)
                payWayName = view.payWayName.ToString();
            else
                payWayName = "";
            if (view.bankCheckName != null)
                bankCheckName = view.bankCheckName.ToString();
            else
                bankCheckName = "";
            if (view.enterpriseNoName != null)
                enterpriseNoName = view.enterpriseNoName.ToString();
            else
                enterpriseNoName = "";
            if (view.bankName != null)
                bankName = view.bankName.ToString();
            else
                bankName = "";
            if (view.factoryNo != null)
                factoryNo = view.factoryNo.ToString();
            else
                factoryNo = "";
            if (view.meterTypeNo != null)
                meterTypeNo = view.meterTypeNo.ToString();
            else
                meterTypeNo = "";
            if (view.factoryName != null)
                factoryName = view.factoryName.ToString();
            else
                factoryName = "";
            if (view.meterTypeName != null)
                meterTypeName = view.meterTypeName.ToString();
            else
                meterTypeName = "";
        }

        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string customerType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String contractNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String telNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String mobileNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String certNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String estateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String estateName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String address { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String houseNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cellNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String roomNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string useState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string payWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankAuthNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String accountName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String accountNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string bankCheck { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string buildTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string editTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taxNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String enterpriseNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CustTypeName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String useStateName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payWayName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankCheckName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String enterpriseNoName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String factoryNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterTypeNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String factoryName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterTypeName { get; set; }

    }

    public class UserPermission
    {

        public UserPermission(AllInOne_UserPermissionView view)
        {
            userId = view.id.ToString();
            type = (int)view.type;
            pername = view.pername;
            if ((bool)view.isOpen)
                classStr = "btn btn-info";
            else
                classStr = "btn btn-noselect";
            perId = view.upId.ToString();
            name = view.name;
        }
        public string userId { get; set; }
        public int type { get; set; }
        public string pername { get; set; }
        public string classStr { get; set; }
        public string perId { get; set; }
        public string name { get; set; }
    }

    public class AreaDetailRes
    {
        public AreaDetailRes(AllInOne_AreaInfo area)
        {
            areaId = area.id;
            name = area.name;
            mapAddress = area.mapAddress;
            lat = area.lat;
            lng = area.lng;
        }
        public int areaId { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lng { set; get; }
        public string mapAddress { get; set; }
    }

    public class Cno_CustNo
    {
        public string communicateNo { get; set; }
        public string customerNo { get; set; }
    }

    public class DeviceRes
    {
        //public DeviceRes(DeviceInfo device) {
        //    factoryNo = device.factoryNo;
        //    ProtocolNo = device.ProtocolNo;
        //    baseVolume = device.baseVolume.ToString();

        //}

        public DeviceRes(DeviceInfo req)
        {
            if (req.IsIC != null)
                IsIC = req.IsIC;
            if (req.deviceNo != null)
                deviceNo = req.deviceNo;
            if (req.meterNo != null)
                meterNo = req.meterNo.ToString();
            if (req.ProtocolNo != null)
                ProtocolNo = req.ProtocolNo;
            if (req.communicateNo != null)
                communicateNo = req.communicateNo;
            else
                communicateNo = "";
            if (req.barCode != null)
                barCode = req.barCode;
            else
                barCode = "";
            if (req.meterTypeNo != null)
                meterTypeNo = req.meterTypeNo;
            else
                meterTypeNo = "";
            if (req.factoryNo != null)
                factoryNo = req.factoryNo;
            else
                factoryNo = "";
            if (req.openState != null)
                openState = req.openState.ToString();
            else
                openState = "";
            if (req.caliber != null)
                caliber = req.caliber;
            else
                caliber = "";
            if (req.baseVolume != null)
                baseVolume = req.baseVolume.ToString();
            else
                baseVolume = "";
            if (req.fluidNo != null)
                fluidNo = req.fluidNo;
            else
                fluidNo = "";
            if (req.remark != null)
                remark = req.remark;
            else
                remark = "";
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            else
                defineNo1 = "";
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            else
                defineNo2 = "";
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            else
                defineNo3 = "";
            if (req.collectorNo != null)
                collectorNo = req.collectorNo;
            else
                collectorNo = "";
            if (req.AlarmTimes != null)
                AlarmTimes = req.AlarmTimes.ToString();
            else
                AlarmTimes = "";
            if (req.AlarmInvTime != null)
                AlarmInvTime = req.AlarmInvTime.ToString();
            else
                AlarmInvTime = "";
            if (req.TempUpper != null)
                TempUpper = req.TempUpper.ToString();
            else
                TempUpper = "";
            if (req.TempLow != null)
                TempLow = req.TempLow.ToString();
            else
                TempLow = "";
            if (req.PressUpper != null)
                PressUpper = req.PressUpper.ToString();
            else
                PressUpper = "";
            if (req.PressLow != null)
                PressLow = req.PressLow.ToString();
            else
                PressLow = "";
            if (req.StdFlowUpper != null)
                StdFlowUpper = req.StdFlowUpper.ToString();
            else
                StdFlowUpper = "";
            if (req.StdFlowLow != null)
                StdFlowLow = req.StdFlowLow.ToString();
            else
                StdFlowLow = "";
            if (req.WorkFlowUpper != null)
                WorkFlowUpper = req.WorkFlowUpper.ToString();
            else
                WorkFlowUpper = "";
            if (req.WorkFlowLow != null)
                WorkFlowLow = req.WorkFlowLow.ToString();
            else
                WorkFlowLow = "";
            if (req.RemainMoneyLow != null)
                RemainMoneyLow = req.RemainMoneyLow.ToString();
            else
                RemainMoneyLow = "";
            if (req.RemainVolumLow != null)
                RemainVolumLow = req.RemainVolumLow.ToString();
            else
                RemainVolumLow = "";
            if (req.OverMoneyUpper != null)
                OverMoneyUpper = req.OverMoneyUpper.ToString();
            else
                OverMoneyUpper = "";
            if (req.OverVolumeUpper != null)
                OverVolumeUpper = req.OverVolumeUpper.ToString();
            else
                OverVolumeUpper = "";
            if (req.DoorAlarm != null)
                DoorAlarm = req.DoorAlarm.ToString();
            else
                DoorAlarm = "";
            if (req.PowerUpper != null)
                PowerUpper = req.PowerUpper.ToString();
            else
                PowerUpper = "";
            if (req.PowerLow != null)
                PowerLow = req.PowerLow.ToString();
            else
                PowerLow = "";
            if (req.BatteryLow != null)
                BatteryLow = req.BatteryLow.ToString();
            else
                BatteryLow = "";
        }

        public DeviceRes(DeviceView req)
        {
            userId = req.userId;
            moneyOrVolume = req.moneyOrVolume;
            isEncrypt = req.isEncrypt;
            if (req.IsIC != null)
                IsIC = req.IsIC;
            if (req.deviceNo != null)
                deviceNo = req.deviceNo;
            if (req.meterNo != null)
                meterNo = req.meterNo.ToString();
            if (req.ProtocolNo != null)
                ProtocolNo = req.ProtocolNo;
            if (req.communicateNo != null)
                communicateNo = req.communicateNo;
            else
                communicateNo = "";
            if (req.barCode != null)
                barCode = req.barCode;
            else
                barCode = "";
            if (req.meterTypeNo != null)
                meterTypeNo = req.meterTypeNo;
            else
                meterTypeNo = "";
            if (req.factoryNo != null)
                factoryNo = req.factoryNo;
            else
                factoryNo = "";
            if (req.openState != null)
                openState = req.openState.ToString();
            else
                openState = "";
            if (req.caliber != null)
                caliber = req.caliber;
            else
                caliber = "";
            if (req.baseVolume != null)
                baseVolume = req.baseVolume.ToString();
            else
                baseVolume = "";
            if (req.fluidNo != null)
                fluidNo = req.fluidNo;
            else
                fluidNo = "";
            if (req.remark != null)
                remark = req.remark;
            else
                remark = "";
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            else
                defineNo1 = "";
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            else
                defineNo2 = "";
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            else
                defineNo3 = "";
            if (req.collectorNo != null)
                collectorNo = req.collectorNo;
            else
                collectorNo = "";
            if (req.AlarmTimes != null)
                AlarmTimes = req.AlarmTimes.ToString();
            else
                AlarmTimes = "";
            if (req.AlarmInvTime != null)
                AlarmInvTime = req.AlarmInvTime.ToString();
            else
                AlarmInvTime = "";


        }
        public int? userId { get; set; }
        public int? moneyOrVolume { get; set; }
        public int? isEncrypt { get; set; }
        public int? IsIC { get; set; }
        public string deviceNo { get; set; }
        public string meterNo { get; set; }
        public String siteNo { get; set; }
        public String communicateNo { get; set; }
        public string CommAddr { get; set; }
        public String ProtocolNo { get; set; }
        public string CommMode { get; set; }
        public string LinkMode { get; set; }
        public String barCode { get; set; }
        public String customerNo { get; set; }
        public String meterTypeNo { get; set; }
        public String factoryNo { get; set; }
        public string openState { get; set; }
        public String caliber { get; set; }
        public String baseVolume { get; set; }
        public String fluidNo { get; set; }
        public String remark { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public String buildTime { get; set; }
        public String editTime { get; set; }
        public String Operator { get; set; }
        public string isConcentrate { get; set; }
        public String collectorNo { get; set; }
        public String MeterType { get; set; }
        public String Volatility { get; set; }
        public string AlarmTimes { get; set; }
        public string AlarmInvTime { get; set; }
        public String TempUpper { get; set; }
        public String TempLow { get; set; }
        public String PressUpper { get; set; }
        public String PressLow { get; set; }
        public String StdFlowUpper { get; set; }
        public String StdFlowLow { get; set; }
        public String WorkFlowUpper { get; set; }
        public String WorkFlowLow { get; set; }
        public String RemainMoneyLow { get; set; }
        public String RemainVolumLow { get; set; }
        public String OverMoneyUpper { get; set; }
        public String OverVolumeUpper { get; set; }
        public string DoorAlarm { get; set; }
        public String PowerUpper { get; set; }
        public String PowerLow { get; set; }
        public String BatteryLow { get; set; }
        public String Image { get; set; }
        public string IsValve { get; set; }
        public string DayFmStart { get; set; }
    }

    public class Config2
    {
        //public Config2() { }

        public Config2()
        {
            var appSettings = ConfigurationManager.AppSettings;
            FLMeterDataRefreshRate = appSettings.Get("FLMeterDataRefreshRate");
        }
        public string FLMeterDataRefreshRate { get; set; }
    }

    public partial class CustomerInfoRes
    {
        public CustomerInfoRes() { }
        public CustomerInfoRes(CustomerWithUser req)
        {
            //var req = list.FirstOrDefault();
            if (req.customerNo != null)
                customerNo = req.customerNo;
            if (req.customerType != null)
                customerType = req.customerType.ToString();
            if (req.contractNo != null)
                contractNo = req.contractNo;
            if (req.customerName != null)
                customerName = req.customerName;
            if (req.telNo != null)
                telNo = req.telNo;
            if (req.mobileNo != null)
                mobileNo = req.mobileNo;
            if (req.certNo != null)
                certNo = req.certNo;
            if (req.estateNo != null)
                estateNo = req.estateNo;
            if (req.estateName != null)
                estateName = req.estateName;
            if (req.address != null)
                address = req.address;
            if (req.houseNo != null)
                houseNo = req.houseNo;
            if (req.cellNo != null)
                cellNo = req.cellNo;
            if (req.roomNo != null)
                roomNo = req.roomNo;
            if (req.useState != null)
                useState = req.useState.ToString();
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            if (req.remark != null)
                remark = req.remark;
            if (req.payWay != null)
                payWay = req.payWay.ToString();
            if (req.bankNo != null)
                bankNo = req.bankNo;
            if (req.bankAuthNo != null)
                bankAuthNo = req.bankAuthNo;
            if (req.accountName != null)
                accountName = req.accountName;
            if (req.accountNo != null)
                accountNo = req.accountNo;
            if (req.bankCheck != null)
                bankCheck = req.bankCheck.ToString();
            if (req.buildTime != null)
                buildTime = req.buildTime.ToString();
            if (req.editTime != null)
                editTime = req.editTime.ToString();
            if (req.Operator != null)
                Operator = req.Operator;
            if (req.taxNo != null)
                taxNo = req.taxNo;
            if (req.enterpriseNo != null)
                enterpriseNo = req.enterpriseNo;
            if (req.CustTypeName != null)
                CustTypeName = req.CustTypeName;
            if (req.useStateName != null)
                useStateName = req.useStateName;
            if (req.payWayName != null)
                payWayName = req.payWayName;
            if (req.bankCheckName != null)
                bankCheckName = req.bankCheckName;
            if (req.enterpriseNoName != null)
                enterpriseNoName = req.enterpriseNoName;
            if (req.bankName != null)
                bankName = req.bankName;
            if (req.factoryNo != null)
                factoryNo = req.factoryNo;
            if (req.meterTypeNo != null)
                meterTypeNo = req.meterTypeNo;
            if (req.factoryName != null)
                factoryName = req.factoryName;
            if (req.meterTypeName != null)
                meterTypeName = req.meterTypeName;
            if (req.userId != null)
                userId = req.userId.ToString();
            if (req.name != null)
                name = req.name;
            if (req.LEVEL != null)
                LEVEL = req.LEVEL.ToString();
            if (req.parentId != null)
                parentId = req.parentId.ToString();
            if (req.isStaff != null)
                isStaff = req.isStaff.ToString();
            if (req.areaId != null)
                areaId = req.areaId.ToString();
            if (req.cId1 != null)
                cId1 = req.cId1;
            if (req.cId2 != null)
                cId2 = req.cId2;
            if (req.cId3 != null)
                cId3 = req.cId3;
            if (req.cId4 != null)
                cId4 = req.cId4;
        }
        public String customerNo { get; set; }
        public string customerType { get; set; }
        public String contractNo { get; set; }
        public String customerName { get; set; }
        public String telNo { get; set; }
        public String mobileNo { get; set; }
        public String certNo { get; set; }
        public String estateNo { get; set; }
        public String estateName { get; set; }
        public String address { get; set; }
        public String houseNo { get; set; }
        public String cellNo { get; set; }
        public String roomNo { get; set; }
        public string useState { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public String remark { get; set; }
        public string payWay { get; set; }
        public String bankNo { get; set; }
        public String bankAuthNo { get; set; }
        public String accountName { get; set; }
        public String accountNo { get; set; }
        public string bankCheck { get; set; }
        public string buildTime { get; set; }
        public string editTime { get; set; }
        public String Operator { get; set; }
        public String taxNo { get; set; }
        public String enterpriseNo { get; set; }
        public String CustTypeName { get; set; }
        public String useStateName { get; set; }
        public String payWayName { get; set; }
        public String bankCheckName { get; set; }
        public String enterpriseNoName { get; set; }
        public String bankName { get; set; }
        public String factoryNo { get; set; }
        public String meterTypeNo { get; set; }
        public String factoryName { get; set; }
        public String meterTypeName { get; set; }
        public string userId { get; set; }
        public String name { get; set; }
        public string LEVEL { get; set; }
        public string parentId { get; set; }
        public string isStaff { get; set; }
        public string areaId { get; set; }
        public String cId1 { get; set; }
        public String cId2 { get; set; }
        public String cId3 { get; set; }
        public String cId4 { get; set; }

    }

    public class DeviceAlarmConfig
    {
        public DeviceAlarmConfig(DeviceInfo req)
        {
            if (req.meterNo != null)
                meterNo = req.meterNo.ToString();
            else
                meterNo = "";
            if (req.StdFlowUpper != null)
                StdFlowUpper = req.StdFlowUpper.ToString();
            else
                StdFlowUpper = "";
            if (req.StdFlowLow != null)
                StdFlowLow = req.StdFlowLow.ToString();
            else
                StdFlowLow = "";
            if (req.WorkFlowUpper != null)
                WorkFlowUpper = req.WorkFlowUpper.ToString();
            else
                WorkFlowUpper = "";
            if (req.WorkFlowLow != null)
                WorkFlowLow = req.WorkFlowLow.ToString();
            else
                WorkFlowLow = "";
            if (req.PressUpper != null)
                PressUpper = req.PressUpper.ToString();
            else
                PressUpper = "";
            if (req.PressLow != null)
                PressLow = req.PressLow.ToString();
            else
                PressLow = "";
            if (req.TempUpper != null)
                TempUpper = req.TempUpper.ToString();
            else
                TempUpper = "";
            if (req.TempLow != null)
                TempLow = req.TempLow.ToString();
            else
                TempLow = "";
            if (req.RemainMoneyLow != null)
                RemainMoneyLow = req.RemainMoneyLow.ToString();
            else
                RemainMoneyLow = "";
            if (req.RemainVolumLow != null)
                RemainVolumLow = req.RemainVolumLow.ToString();
            else
                RemainVolumLow = "";
            if (req.OverMoneyUpper != null)
                OverMoneyUpper = req.OverMoneyUpper.ToString();
            else
                OverMoneyUpper = "";
            if (req.OverVolumeUpper != null)
                OverVolumeUpper = req.OverVolumeUpper.ToString();
            else
                OverVolumeUpper = "";
            if (req.PowerUpper != null)
                PowerUpper = req.PowerUpper.ToString();
            else
                PowerUpper = "";
            if (req.PowerLow != null)
                PowerLow = req.PowerLow.ToString();
            else
                PowerLow = "";
            if (req.BatteryLow != null)
                BatteryLow = req.BatteryLow.ToString();
            else
                BatteryLow = "";
            if (req.DoorAlarm != null)
                DoorAlarm = req.DoorAlarm.ToString();
            else
                DoorAlarm = "";
        }
        public string meterNo { get; set; }
        public string StdFlowUpper { get; set; }
        public string StdFlowLow { get; set; }
        public string WorkFlowUpper { get; set; }
        public string WorkFlowLow { get; set; }
        public string PressUpper { get; set; }
        public string PressLow { get; set; }
        public string TempUpper { get; set; }
        public string TempLow { get; set; }
        public string RemainMoneyLow { get; set; }
        public string RemainVolumLow { get; set; }
        public string OverMoneyUpper { get; set; }
        public string OverVolumeUpper { get; set; }
        public string PowerUpper { get; set; }
        public string PowerLow { get; set; }
        public string BatteryLow { get; set; }
        public string DoorAlarm { get; set; }
    }

    public class DeviceViewRes
    {
        public DeviceViewRes(DeviceView view)
        {
            if (view.ProtocolNo != null)
                ProtocolNo = view.ProtocolNo;
            if (view.deviceNo != null)
                deviceNo = view.deviceNo;
            else
                deviceNo = "";
            if (view.meterNo != null)
                meterNo = view.meterNo.ToString();
            else
                meterNo = "";
            if (view.communicateNo != null)
                communicateNo = view.communicateNo.ToString();
            else
                communicateNo = "";
            if (view.barCode != null)
                barCode = view.barCode.ToString();
            else
                barCode = "";
            if (view.customerNo != null)
                customerNo = view.customerNo.ToString();
            else
                customerNo = "";
            if (view.meterTypeNo != null)
                meterTypeNo = view.meterTypeNo.ToString();
            else
                meterTypeNo = "";
            if (view.factoryNo != null)
                factoryNo = view.factoryNo.ToString();
            else
                factoryNo = "";
            if (view.openState != null)
                openState = view.openState.ToString();
            else
                openState = "";
            if (view.caliber != null)
                caliber = view.caliber.ToString();
            else
                caliber = "";
            if (view.baseVolume != null)
                baseVolume = view.baseVolume.ToString();
            else
                baseVolume = "";
            if (view.fluidNo != null)
                fluidNo = view.fluidNo.ToString();
            else
                fluidNo = "";
            if (view.lat != null)
                lat = view.lat.ToString();
            else
                lat = "";
            if (view.lng != null)
                lng = view.lng.ToString();
            else
                lng = "";
            if (view.remark != null)
                remark = view.remark.ToString();
            else
                remark = "";
            if (view.defineNo1 != null)
                defineNo1 = view.defineNo1.ToString();
            else
                defineNo1 = "";
            if (view.defineNo2 != null)
                defineNo2 = view.defineNo2.ToString();
            else
                defineNo2 = "";
            if (view.defineNo3 != null)
                defineNo3 = view.defineNo3.ToString();
            else
                defineNo3 = "";
            if (view.buildTime != null)
                buildTime = Convert.ToDateTime(view.buildTime).ToString("yyyy-MM-dd HH:mm:ss");
            else
                buildTime = "";
            if (view.editTime != null)
                editTime = Convert.ToDateTime(view.editTime).ToString("yyyy-MM-dd HH:mm:ss");
            else
                editTime = "";
            if (view.Operator != null)
                Operator = view.Operator.ToString();
            else
                Operator = "";
            if (view.collectorNo != null)
                collectorNo = view.collectorNo.ToString();
            else
                collectorNo = "";
            if (view.MeterType != null)
                MeterType = view.MeterType.ToString();
            else
                MeterType = "";
            if (view.isConcentrate != null)
                isConcentrate = view.isConcentrate.ToString();
            else
                isConcentrate = "";
            if (view.factoryName != null)
                factoryName = view.factoryName.ToString();
            else
                factoryName = "";
            if (view.meterTypeName != null)
                meterTypeName = view.meterTypeName.ToString();
            else
                meterTypeName = "";
            if (view.openStateName != null)
                openStateName = view.openStateName.ToString();
            else
                openStateName = "";
            if (view.fluidName != null)
                fluidName = view.fluidName.ToString();
            else
                fluidName = "";
            if (view.customerName != null)
                customerName = view.customerName.ToString();
            else
                customerName = "";
            if (view.address != null)
                address = view.address.ToString();
            else
                address = "";
            if (view.userId != null)
                userId = view.userId.ToString();
            else
                userId = "";
            if (view.userName != null)
                userName = view.userName.ToString();
            else
                userName = "";
            if (view.LEVEL != null)
                LEVEL = view.LEVEL.ToString();
            else
                LEVEL = "";
            if (view.parentId != null)
                parentId = view.parentId.ToString();
            else
                parentId = "";
            if (view.isStaff != null)
                isStaff = view.isStaff.ToString();
            else
                isStaff = "";
            if (view.cId1 != null)
                cId1 = view.cId1.ToString();
            else
                cId1 = "";
            if (view.cId2 != null)
                cId2 = view.cId2.ToString();
            else
                cId2 = "";
            if (view.cId3 != null)
                cId3 = view.cId3.ToString();
            else
                cId3 = "";
            if (view.cId4 != null)
                cId4 = view.cId4.ToString();
            else
                cId4 = "";
        }
        public string ProtocolNo { get; set; }
        public string deviceNo { get; set; }
        public string meterNo { get; set; }
        public string communicateNo { get; set; }
        public string barCode { get; set; }
        public string customerNo { get; set; }
        public string meterTypeNo { get; set; }
        public string factoryNo { get; set; }
        public string openState { get; set; }
        public string caliber { get; set; }
        public string baseVolume { get; set; }
        public string fluidNo { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string remark { get; set; }
        public string defineNo1 { get; set; }
        public string defineNo2 { get; set; }
        public string defineNo3 { get; set; }
        public string buildTime { get; set; }
        public string editTime { get; set; }
        public string Operator { get; set; }
        public string collectorNo { get; set; }
        public string MeterType { get; set; }
        public string isConcentrate { get; set; }
        public string factoryName { get; set; }
        public string meterTypeName { get; set; }
        public string openStateName { get; set; }
        public string fluidName { get; set; }
        public string customerName { get; set; }
        public string address { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string LEVEL { get; set; }
        public string parentId { get; set; }
        public string isStaff { get; set; }
        public string cId1 { get; set; }
        public string cId2 { get; set; }
        public string cId3 { get; set; }
        public string cId4 { get; set; }

    }

    public class OneFLMeterDataViewRes
    {
        public OneFLMeterDataViewRes(OneFLMeterDataView view)
        {
            if (view.RemainMoney != null)
                RemainMoney = view.RemainMoney.ToString();
            else
                RemainMoney = "";
            if (view.RemainVolume != null)
                RemainVolume = view.RemainVolume.ToString();
            else
                RemainVolume = "";
            meterTypeNo = view.meterTypeNo == null ? "" : view.meterTypeNo;
            IsIC = view.IsIC;

            switch (meterTypeNo)
            {
                case "11":
                    RemainMoney = "-";
                    break;
                case "13":
                    RemainVolume = "-";
                    break;
                case "14":
                    if (IsIC == 0)
                    {
                        RemainMoney = "-";
                        RemainVolume = "-";
                        break;
                    }
                    break;
                case "15":
                case "16":
                    if (IsIC == 0)
                    {
                        RemainMoney = "-";
                        RemainVolume = "-";
                        break;
                    }
                    else
                    {
                        if (view.moneyOrVolume == 1)
                            RemainMoney = "-";
                        else
                            RemainVolume = "-";
                    }
                    break;
                default:
                    break;
            }


            if (view.Id != null)
                Id = view.Id.ToString();
            else
                Id = "";
            if (view.communicateNo != null)
                communicateNo = view.communicateNo.ToString();
            else
                communicateNo = "";
            if (view.FLMeterNo != null)
                FLMeterNo = view.FLMeterNo.ToString();
            else
                FLMeterNo = "";
            if (view.siteNo != null)
                siteNo = view.siteNo.ToString();
            else
                siteNo = "";

            if (view.ReceivTime != null)
                ReceivTime = view.ReceivTime.ToString();
            else
                ReceivTime = "";
            if (view.StdSum != null)
                StdSum = view.StdSum.ToString();
            else
                StdSum = "";
            if (view.WorkSum != null)
                WorkSum = view.WorkSum.ToString();
            else
                WorkSum = "";
            if (view.StdFlow != null)
                StdFlow = view.StdFlow.ToString();
            else
                StdFlow = "";
            if (view.WorkFlow != null)
                WorkFlow = view.WorkFlow.ToString();
            else
                WorkFlow = "";
            if (view.Temperature != null)
                Temperature = view.Temperature.ToString();
            else
                Temperature = "";
            if (view.Pressure != null)
                Pressure = view.Pressure.ToString();
            else
                Pressure = "";
            if (view.FMState != null)
                FMState = view.FMState.ToString();
            else
                FMState = "";
            if (view.FMStateMsg != null)
                FMStateMsg = view.FMStateMsg.ToString();
            else
                FMStateMsg = "";
            if (view.RTUState != null)
                RTUState = view.RTUState.ToString();
            else
                RTUState = "";
            if (view.RTUStateMsg != null)
                RTUStateMsg = view.RTUStateMsg.ToString();
            else
                RTUStateMsg = "";
            if (view.SumTotal != null)
                SumTotal = view.SumTotal.ToString();
            else
                SumTotal = "";

            if (view.Overdraft != null)
                Overdraft = view.Overdraft.ToString();
            else
                Overdraft = "";
            if (view.RemoteChargeMoney != null)
                RemoteChargeMoney = view.RemoteChargeMoney.ToString();
            else
                RemoteChargeMoney = "";
            if (view.RemoteChargeTimes != null)
                RemoteChargeTimes = view.RemoteChargeTimes.ToString();
            else
                RemoteChargeTimes = "";
            if (view.Price != null)
                Price = view.Price.ToString();
            else
                Price = "";
            if (view.ValveState != null)
                ValveState = view.ValveState.ToString();
            else
                ValveState = "";
            if (view.ValveStateMsg != null)
                ValveStateMsg = view.ValveStateMsg.ToString();
            else
                ValveStateMsg = "";
            if (view.PowerVoltage != null)
                PowerVoltage = view.PowerVoltage.ToString();
            else
                PowerVoltage = "";
            if (view.BatteryVoltage != null)
                BatteryVoltage = view.BatteryVoltage.ToString();
            else
                BatteryVoltage = "";
            if (view.Reserve1 != null)
                Reserve1 = view.Reserve1.ToString();
            else
                Reserve1 = "";
            if (view.Reserve2 != null)
                Reserve2 = view.Reserve2.ToString();
            else
                Reserve2 = "";
            if (view.Reserve3 != null)
                Reserve3 = view.Reserve3.ToString();
            else
                Reserve3 = "";
            if (view.Reserve4 != null)
                Reserve4 = view.Reserve4.ToString();
            else
                Reserve4 = "";
            if (view.meterNo != null)
                meterNo = view.meterNo.ToString();
            else
                meterNo = "";
            if (view.userId != null)
                userId = view.userId.ToString();
            else
                userId = "";
            if (view.userName != null)
                userName = view.userName.ToString();
            else
                userName = "";
            if (view.LEVEL != null)
                LEVEL = view.LEVEL.ToString();
            else
                LEVEL = "";
            if (view.parentId != null)
                parentId = view.parentId.ToString();
            else
                parentId = "";
            if (view.isStaff != null)
                isStaff = view.isStaff.ToString();
            else
                isStaff = "";
            if (view.cId1 != null)
                cId1 = view.cId1.ToString();
            else
                cId1 = "";
            if (view.cId2 != null)
                cId2 = view.cId2.ToString();
            else
                cId2 = "";
            if (view.cId3 != null)
                cId3 = view.cId3.ToString();
            else
                cId3 = "";
            if (view.cId4 != null)
                cId4 = view.cId4.ToString();
            else
                cId4 = "";
            if (view.deviceNo != null)
                deviceNo = view.deviceNo.ToString();
            else
                deviceNo = "";
            if (view.customerName != null)
                customerName = view.customerName.ToString();
            else
                customerName = "";
            if (view.address != null)
                address = view.address.ToString();
            else
                address = "";

            if (view.InstantTime != null)
                InstantTime = Convert.ToDateTime(view.InstantTime).ToString("yyyy-MM-dd HH:mm:ss");
            else
                InstantTime = "";

            if (view.LoginState != null)
                LoginState = view.LoginState.ToString();
            else
                LoginState = "";
            if (view.LoginStateMsg != null)
                LoginStateMsg = view.LoginStateMsg.ToString();
            else
                LoginStateMsg = "";

            if (StdSum == "0.000")
                StdSum = "-";
            if (WorkSum == "0.000")
                WorkSum = "-";
            if (StdFlow == "0.000")
                StdFlow = "-";
            if (WorkFlow == "0.000")
                WorkFlow = "-";
            if (Temperature == "0.000")
                Temperature = "-";
            if (Pressure == "0.000")
                Pressure = "-";

            if (RemainMoney == "0.000" || RemainMoney == "3752229.000")
                RemainMoney = "-";
            if (RemainVolume == "0.000" || RemainVolume == "3752229.000")
                RemainVolume = "-";

            var server = ConfigurationManager.AppSettings.Get("server");
            if (server != null || server != "")
            {
                if (meterTypeNo == "01")
                {
                    if (StdSum == "0.000" || StdSum == "3752229.000")
                        StdSum = "-";
                    if (WorkSum == "0.000" || WorkSum == "3752229.000")
                        WorkSum = "-";
                    if (StdFlow == "0.000" || StdFlow == "3752229.000")
                        StdFlow = "-";
                    if (WorkFlow == "0.000" || WorkFlow == "3752229.000")
                        WorkFlow = "-";
                    if (Temperature == "0.000" || Temperature == "3752229.000")
                        Temperature = "-";
                    if (Pressure == "0.000" || Pressure == "3752229.000")
                        Pressure = "-";
                    if (RemainMoney == "0.000" || RemainMoney == "3752229.000")
                        RemainMoney = "-";
                    if (RemainVolume == "0.000" || RemainVolume == "3752229.000")
                        RemainVolume = "-";
                }
            }
            else
            {
                if (meterTypeNo == "14")
                {
                    if (StdSum == "0.000" || StdSum == "3752229.000")
                        StdSum = "-";
                    if (WorkSum == "0.000" || WorkSum == "3752229.000")
                        WorkSum = "-";
                    if (StdFlow == "0.000" || StdFlow == "3752229.000")
                        StdFlow = "-";
                    if (WorkFlow == "0.000" || WorkFlow == "3752229.000")
                        WorkFlow = "-";
                    if (Temperature == "0.000" || Temperature == "3752229.000")
                        Temperature = "-";
                    if (Pressure == "0.000" || Pressure == "3752229.000")
                        Pressure = "-";
                    if (RemainMoney == "0.000" || RemainMoney == "3752229.000")
                        RemainMoney = "-";
                    if (RemainVolume == "0.000" || RemainVolume == "3752229.000")
                        RemainVolume = "-";
                }
            }

            remark = view.remark;
        }

        public string remark { get; set; }
        public int? IsIC { get; set; }
        public string meterTypeNo { set; get; }
        public string Id { get; set; }
        public String communicateNo { get; set; }
        public string FLMeterNo { get; set; }
        public String siteNo { get; set; }
        public string InstantTime { get; set; }
        public string ReceivTime { get; set; }
        public string StdSum { get; set; }
        public string WorkSum { get; set; }
        public string StdFlow { get; set; }
        public string WorkFlow { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        public string FMState { get; set; }
        public String FMStateMsg { get; set; }
        public string RTUState { get; set; }
        public String RTUStateMsg { get; set; }
        public string SumTotal { get; set; }
        public string RemainMoney { get; set; }
        public string RemainVolume { get; set; }
        public string Overdraft { get; set; }
        public string RemoteChargeMoney { get; set; }
        public string RemoteChargeTimes { get; set; }
        public string Price { get; set; }
        public string ValveState { get; set; }
        public String ValveStateMsg { get; set; }
        public string PowerVoltage { get; set; }
        public string BatteryVoltage { get; set; }
        public String Reserve1 { get; set; }
        public String Reserve2 { get; set; }
        public String Reserve3 { get; set; }
        public String Reserve4 { get; set; }
        public string meterNo { get; set; }
        public string userId { get; set; }
        public String userName { get; set; }
        public string LEVEL { get; set; }
        public string parentId { get; set; }
        public string isStaff { get; set; }
        public String cId1 { get; set; }
        public String cId2 { get; set; }
        public String cId3 { get; set; }
        public String cId4 { get; set; }
        public String deviceNo { get; set; }
        public String customerName { get; set; }
        public String address { get; set; }
        public string LoginState { get; set; }
        public String LoginStateMsg { get; set; }
    }

    public class DaysRes
    {

    }

    public class UserId_areaName
    {
        public UserId_areaName(List<AllInOne_UserPermissionView> list)
        {
            var view = list.First();
            userId = view.id;
            areaName = view.areaName;
        }
        public int userId { get; set; }
        public string areaName { get; set; }
    }

    public class chargeReport
    {
        public DateTime dt { get; set; }
        public string sumVolume { get; set; }
        public string sumMoney { get; set; }
    }

    [Serializable]
    public class StdSumReport
    {
        public DateTime dt { get; set; }
        public string span { get; set; }
    }

    public class minMaxSS
    {
        public string minSS { get; set; }
        public string maxSS { get; set; }
    }

    public class DeviceAlarmViewRes
    {
        public DeviceAlarmViewRes(DeviceAlarmView req)
        {
            Id = req.Id == null ? "" : req.Id.ToString();
            siteNo = req.siteNo == null ? "" : req.siteNo.ToString();
            communicateNo = req.communicateNo == null ? "" : req.communicateNo.ToString();
            Devid = req.Devid == null ? "" : req.Devid.ToString();
            DevType = req.DevType == null ? "" : req.DevType.ToString();
            DevTypeName = req.DevTypeName == null ? "" : req.DevTypeName.ToString();
            AlarmContent = req.AlarmContent == null ? "" : req.AlarmContent.ToString();
            AlarmTime = req.AlarmTime == null ? "" : ((DateTime)req.AlarmTime).ToString("yyyy-MM-dd HH:mm:ss");
            DealFlag = req.DealFlag == null ? "" : req.DealFlag.ToString();
            DealTime = req.DealTime == null ? "" : ((DateTime)req.DealTime).ToString("yyyy-MM-dd HH:mm:ss");
            DealOperator = req.DealOperator == null ? "" : req.DealOperator.ToString();
            SmsTimes = req.SmsTimes == null ? "" : req.SmsTimes.ToString();
            SmsSendTimes = req.SmsSendTimes == null ? "" : req.SmsSendTimes.ToString();
            SmsInvTime = req.SmsInvTime == null ? "" : req.SmsInvTime.ToString();
            Linkman = req.Linkman == null ? "" : req.Linkman.ToString();
            MobileNo = req.MobileNo == null ? "" : req.MobileNo.ToString();
            meterNo = req.meterNo == null ? "" : req.meterNo.ToString();
            barCode = req.barCode == null ? "" : req.barCode.ToString();
            customerNo = req.customerNo == null ? "" : req.customerNo.ToString();
            deviceNo = req.deviceNo == null ? "" : req.deviceNo.ToString();
            meterTypeNo = req.meterTypeNo == null ? "" : req.meterTypeNo.ToString();
            factoryNo = req.factoryNo == null ? "" : req.factoryNo.ToString();
            openState = req.openState == null ? "" : req.openState.ToString();
            caliber = req.caliber == null ? "" : req.caliber.ToString();
            baseVolume = req.baseVolume == null ? "" : req.baseVolume.ToString();
            fluidNo = req.fluidNo == null ? "" : req.fluidNo.ToString();
            lat = req.lat == null ? "" : req.lat.ToString();
            lng = req.lng == null ? "" : req.lng.ToString();
            remark = req.remark == null ? "" : req.remark.ToString();
            defineNo1 = req.defineNo1 == null ? "" : req.defineNo1.ToString();
            defineNo2 = req.defineNo2 == null ? "" : req.defineNo2.ToString();
            defineNo3 = req.defineNo3 == null ? "" : req.defineNo3.ToString();
            buildTime = req.buildTime == null ? "" : req.buildTime.ToString();
            editTime = req.editTime == null ? "" : req.editTime.ToString();
            Operator = req.Operator == null ? "" : req.Operator.ToString();
            collectorNo = req.collectorNo == null ? "" : req.collectorNo.ToString();
            MeterType = req.MeterType == null ? "" : req.MeterType.ToString();
            isConcentrate = req.isConcentrate == null ? "" : req.isConcentrate.ToString();
            factoryName = req.factoryName == null ? "" : req.factoryName.ToString();
            meterTypeName = req.meterTypeName == null ? "" : req.meterTypeName.ToString();
            openStateName = req.openStateName == null ? "" : req.openStateName.ToString();
            fluidName = req.fluidName == null ? "" : req.fluidName.ToString();
            customerName = req.customerName == null ? "" : req.customerName.ToString();
            address = req.address == null ? "" : req.address.ToString();
            userId = req.userId == null ? "" : req.userId.ToString();
            userName = req.userName == null ? "" : req.userName.ToString();
            LEVEL = req.LEVEL == null ? "" : req.LEVEL.ToString();
            parentId = req.parentId == null ? "" : req.parentId.ToString();
            isStaff = req.isStaff == null ? "" : req.isStaff.ToString();
            cId1 = req.cId1 == null ? "" : req.cId1.ToString();
            cId2 = req.cId2 == null ? "" : req.cId2.ToString();
            cId3 = req.cId3 == null ? "" : req.cId3.ToString();
            cId4 = req.cId4 == null ? "" : req.cId4.ToString();
        }

        public string Id { get; set; }
        public String siteNo { get; set; }
        public String communicateNo { get; set; }
        public string Devid { get; set; }
        public string DevType { get; set; }
        public String DevTypeName { get; set; }
        public String AlarmContent { get; set; }
        public string AlarmTime { get; set; }
        public string DealFlag { get; set; }
        public string DealTime { get; set; }
        public String DealOperator { get; set; }
        public string SmsTimes { get; set; }
        public string SmsSendTimes { get; set; }
        public string SmsInvTime { get; set; }
        public String Linkman { get; set; }
        public String MobileNo { get; set; }
        public string meterNo { get; set; }
        public String barCode { get; set; }
        public String customerNo { get; set; }
        public String deviceNo { get; set; }
        public String meterTypeNo { get; set; }
        public String factoryNo { get; set; }
        public string openState { get; set; }
        public String caliber { get; set; }
        public string baseVolume { get; set; }
        public String fluidNo { get; set; }
        public String lat { get; set; }
        public String lng { get; set; }
        public String remark { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public string buildTime { get; set; }
        public string editTime { get; set; }
        public String Operator { get; set; }
        public String collectorNo { get; set; }
        public String MeterType { get; set; }
        public string isConcentrate { get; set; }
        public String factoryName { get; set; }
        public String meterTypeName { get; set; }
        public String openStateName { get; set; }
        public String fluidName { get; set; }
        public String customerName { get; set; }
        public String address { get; set; }
        public string userId { get; set; }
        public String userName { get; set; }
        public string LEVEL { get; set; }
        public string parentId { get; set; }
        public string isStaff { get; set; }
        public String cId1 { get; set; }
        public String cId2 { get; set; }
        public String cId3 { get; set; }
        public String cId4 { get; set; }
    }

    public class IotLogin
    {
        public string accessToken { get; set; }
        public string tokenType { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
        public string scope { get; set; }
    }

    public class IotGetDevice
    {
        public string app_key { get; set; }
        public string appId { get; set; }
        public string Authorization { get; set; }
        public string gatewayId { get; set; }
        public string nodeType { get; set; }
        public string deviceType { get; set; }
        public string protocolType { get; set; }
        public string pageNo { get; set; }
        public string pageSize { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string status { get; set; }
        public string sort { get; set; }
    }

    public class PieRes
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Counts
    {
        public string counts { get; set; }
    }

    public class UserLv
    {
        public UserLv() { }
        public UserLv(AllInOne_UserInfo user)
        {
            lv = user.level.ToString();
            name = user.name;
            cid = AllInOne_UserInfoOper.Instance.GetLastCId(user);
        }

        public string lv { get; set; }
        public string name { get; set; }
        public string cid { get; set; }
        public List<UserLv> list { set; get; }
    }

    [Serializable]
    public class StdSum4
    {
        public int FLMeterNo { get; set; }
        public decimal StdSum { set; get; }
        public DateTime InstantTime { get; set; }
        public string CustTypeName { get; set; }
    }

    [Serializable]
    public class Sum4
    {
        public int FLMeterNo { get; set; }
        public decimal sum { set; get; }
        public DateTime InstantTime { get; set; }
        public string CustTypeName { get; set; }
    }

    public class temp
    {
        public string instanttime { set; get; }
    }

    public class pieTime
    {
        public decimal value { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
    }

    public class Factory_No
    {
        public string name { get; set; }
        public string no { get; set; }
    }

    public class DevId_Content
    {
        public DevId_Content() { }
        public DevId_Content(AllInOne_AlarmInfo info)
        {
            devid = info.Devid.ToString();
            content = info.AlarmContent;
        }
        public DevId_Content(DeviceAlarmView info)
        {
            devid = info.Devid.ToString();
            content = info.AlarmContent;
        }
        public string devid { get; set; }
        public string content { get; set; }
    }

    public class AlarmItem
    {
        public AlarmItem(DeviceAlarmView info, int id)
        {
            biggestId = id;
            devid = info.Devid;
            content = info.AlarmContent;
        }
        public int biggestId { get; set; }
        public int? devid { get; set; }
        public string content { get; set; }

    }

}