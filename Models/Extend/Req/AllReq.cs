using Common.Attribute;
using DbOpertion.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HHTDCDMR.Models.Extend.Req
{
    public class AllReq
    {

    }
    public class UserLoginReq
    {
        [Required]
        public string account { get; set; }
        [PwdValidate]
        public string pwd { get; set; }
    }

    public class UserGetReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
    }

    public class UserIdReq
    {
        [IdNotZeroValidate]
        public string userId { get; set; }
    }

    public class AddStaffReq
    {
        [StringValidate(ErrorMessage = "请填写名称")]
        public string name { get; set; }
        [StringValidate(ErrorMessage = "请填写账号")]
        public string account { get; set; }
        [PwdValidate]
        public string pwd { get; set; }
    }

    public class UpdateFactoryNameReq
    {
        public string name { get; set; }
    }

    public class AddSonReq
    {
        [StringValidate(ErrorMessage = "请填写名称")]
        public string name { get; set; }
        [StringValidate(ErrorMessage = "请填写账号")]
        public string account { get; set; }
        [PwdValidate]
        public string pwd { get; set; }
        public string areaId { get; set; }
    }

    public class UpdateUserReq
    {
        [Required(ErrorMessage = "请填写名称")]
        public string name { get; set; }
        [Required(ErrorMessage = "请填写账号")]
        public string account { get; set; }
        [PwdValidate2(ErrorMessage = "密码应为6~16位英文字母、数字")]
        public string pwd { get; set; }
        public string userId { get; set; }
        [Required(ErrorMessage = "请选择区域")]
        public string areaId { get; set; }
    }

    public class UpdateUserSelfReq
    {
        [Required(ErrorMessage = "userName,请填写名称")]
        public string name { get; set; }
        [Required(ErrorMessage = "userAccount,请填写账号")]
        public string account { get; set; }
        [PwdValidate2(ErrorMessage = "pwd,密码应为6~16位英文字母、数字")]
        public string pwd { get; set; }
        public string userId { get; set; }
        //[Required(ErrorMessage = "areaId,请选择地址")]
        public string areaId { get; set; }
        public string phone { get; set; }
    }

    public class CustomerReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
        public string useState { get; set; }
        public string customerType { get; set; }
        public string estateNo { get; set; }
        public string factoryNo { get; set; }
        public string meterType { get; set; }
        public string operatorName { get; set; }
        public string lastName { get; set; }
    }

    public class ValveReq
    {
        public string id { get; set; }
        public string state { get; set; }
    }

    public class MeterDataListReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
        public string lastName { get; set; }
    }

    public class MeterReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
        public string meterTypeNo { get; set; }
        public string factoryNo { get; set; }
        public string openState { get; set; }
        public string fluidNo { get; set; }
        public string Operator { get; set; }
        public string lastName { get; set; }
    }

    public class HisReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
        [DateTimeValidate2(ErrorMessage = "起始时间错误")]
        public string startTime { get; set; }
        [DateTimeValidate2(ErrorMessage = "终止时间错误")]
        public string endTime { get; set; }
        [Year2(ErrorMessage = "起始时间错误")]
        public string yStart { get; set; }
        [Year2(ErrorMessage = "终止时间错误")]
        public string yEnd { get; set; }
        [YearMonth2Attribute(ErrorMessage = "起始时间错误")]
        public string mStart { get; set; }
        [YearMonth2Attribute(ErrorMessage = "终止时间错误")]
        public string mEnd { get; set; }

        //[IntValidate2]
        //public string meterNo { set; get; }

        //public string year { get; set; }
        //public string month { get; set; }
        //public string day { get; set; }
        public string dateStr { get; set; }
        public string lastName { get; set; }
        public string customerNo { get; set; }
    }

    public class DeviceMapReq
    {
        //[Required]
        public string search { get; set; }

        [IntValidate2]
        public string meterNo { set; get; }
        [IntValidate2]
        public string sonId { get; set; }
    }

    public class DeleteFactoryReq
    {
        public string factoryNo { get; set; }
    }

    public class AreaReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
    }

    public class AreaAddReq
    {
        [StringValidate]
        public string name { set; get; }
        //[LatValidate]
        //public string lat { set; get; }
        //[LngValidate]
        //public string lng { set; get; }
        public string areaId { set; get; }
        //[StringValidate]
        //public string mapAddress { set; get; }
    }

    public class AreaUpdateReq
    {
        [StringValidate]
        public string name { set; get; }
        //[LatValidate]
        //public string lat { set; get; }
        //[LngValidate]
        //public string lng { set; get; }
        [IdNotZeroValidate]
        public string areaId { set; get; }
        //[StringValidate]
        //public string mapAddress { set; get; }
    }

    public class AreaDelReq
    {
        [IdNotZeroValidate]
        public string id { set; get; }
    }

    public class MeterTypeReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
    }

    public class GetFactoryListRes
    {
        public GetFactoryListRes(FactoryType ft)
        {
            factoryNo = ft.factoryNo;
            factoryName = ft.factoryName;
            MarkCode = ft.MarkCode;
        }
        public string factoryNo { get; set; }
        public string factoryName { get; set; }
        public string MarkCode { get; set; }
    }

    public class UpdateFactoryReq
    {
        public string factoryNo { get; set; }
        public string factoryName { get; set; }
        public string MarkCode { get; set; }
    }

    public class AddFactoryReq
    {
        [Required]
        public string factoryNo { get; set; }
        [Required]
        public string factoryName { get; set; }
        [Required]
        public string MarkCode { get; set; }
    }

    public class GetFactoryListReq
    {
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
    }

    public class AlarmListReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
        //public string meterNo { get; set; }
        public string lastName { get; set; }
        public string customerNo { get; set; }
    }

    public class AlarmIdReq
    {
        [IntValidate]
        public string id { get; set; }
    }

    public class DeviceInfoReq
    {
        public int? userId { get; set; }
        public int? isEncrypt { get; set; }
        public int? moneyOrVolume { get; set; }
        //public int? IsIC { get; set; }
        //[Required]
        //public int? userId { get; set; }

        public int? ScadaInvTime { get; set; }
        [Required]
        public int? IsIC { get; set; }
        [Required]
        public string CommMode { get; set; }
        [Required]
        public string LinkMode { get; set; }
        public string deviceNo { get; set; }
        [LatValidate2]
        public string lat { get; set; }
        [LngValidate2]
        public string lng { get; set; }

        //[StringValidate(ErrorMessage = "meterNo,请填写设备号")]
        //public string meterNo { get; set; }
        /// <summary>
        /// 设备厂家
        /// </summary>
        public String factoryNo { get; set; }
        public string openState { get; set; }
        //[DecimalValidate2(ErrorMessage = "表底读数错误")]
        //public string baseVolume { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "设备类型错误")]
        public String meterTypeNo { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "价格类型错误")]
        public String fluidNo { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "通讯编号只能是数字字符串")]
        public String communicateNo { get; set; }
        //[IntString2ValidateAttribute(ErrorMessage = "条码号错误")]
        //public String barCode { get; set; }
        /// <summary>
        /// 口径
        /// </summary>
        public String caliber { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public String remark { get; set; }
        //public string collectorNo { get; set; }
        [DecimalValidate2(ErrorMessage = "温度上限错误")]
        public string TempUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "温度下限错误")]
        public string TempLow { get; set; }
        [DecimalValidate2(ErrorMessage = "压力上限错误")]
        public string PressUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "压力下限错误")]
        public string PressLow { get; set; }
        [DecimalValidate2(ErrorMessage = "标况流量上限错误")]
        public string StdFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "标况流量下限错误")]
        public string StdFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "工况流量上限错误")]
        public string WorkFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "工况流量下限错误")]
        public string WorkFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "剩余金额下限错误")]
        public string RemainMoneyLow { get; set; }
        [DecimalValidate2(ErrorMessage = "剩余气量下限错误")]
        public string RemainVolumLow { get; set; }
        [DecimalValidate2(ErrorMessage = "过零金额上限错误")]
        public string OverMoneyUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "过零气量上限错误")]
        public string OverVolumeUpper { get; set; }
        [IntValidate2(ErrorMessage = "柜门报警开关错误")]
        public string DoorAlarm { get; set; }
        [DecimalValidate2(ErrorMessage = "供电电压上限错误")]
        public string PowerUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "供电电压下限错误")]
        public string PowerLow { get; set; }
        [DecimalValidate2(ErrorMessage = "铅酸电池电压下限")]
        public string BatteryLow { get; set; }
        [IntValidate2(ErrorMessage = "信息发送次数错误")]
        public string AlarmTimes { get; set; }
        [IntValidate2(ErrorMessage = "信息发送间隔错误")]
        public string AlarmInvTime { get; set; }
        public string ProtocolNo { set; get; }
    }

    public class DeviceUpdateReq
    {
        public int? ScadaInvTime { get; set; }
        public int? userId { get; set; }
        public int? isEncrypt { get; set; }
        public int? moneyOrVolume { get; set; }
        //public int? IsIC { get; set; }
        //[Required]
        //public int? userId { get; set; }
        [Required]
        public int? IsIC { get; set; }
        [Required]
        public string CommMode { get; set; }
        [Required]
        public string LinkMode { get; set; }
        public string deviceNo { get; set; }
        [LatValidate2]
        public string lat { get; set; }
        [LngValidate2]
        public string lng { get; set; }
        //[StringValidate(ErrorMessage = "meterNo,请填写设备号")]
        public string meterNo { get; set; }
        /// <summary>
        /// 设备厂家
        /// </summary>
        public String factoryNo { get; set; }
        public string openState { get; set; }
        //[DecimalValidate2(ErrorMessage = "表底读数错误")]
        //public string baseVolume { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "设备类型错误")]
        public String meterTypeNo { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "价格类型错误")]
        public String fluidNo { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "通讯编号错误")]
        public String communicateNo { get; set; }
        //[IntString2ValidateAttribute(ErrorMessage = "条码号错误")]
        //public String barCode { get; set; }
        /// <summary>
        /// 口径
        /// </summary>
        public String caliber { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public String remark { get; set; }
        //public string collectorNo { get; set; }
        [DecimalValidate2(ErrorMessage = "温度上限错误")]
        public string TempUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "温度下限错误")]
        public string TempLow { get; set; }
        [DecimalValidate2(ErrorMessage = "压力上限错误")]
        public string PressUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "压力下限错误")]
        public string PressLow { get; set; }
        [DecimalValidate2(ErrorMessage = "标况流量上限错误")]
        public string StdFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "标况流量下限错误")]
        public string StdFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "工况流量上限错误")]
        public string WorkFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "工况流量下限错误")]
        public string WorkFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "剩余金额下限错误")]
        public string RemainMoneyLow { get; set; }
        [DecimalValidate2(ErrorMessage = "剩余气量下限错误")]
        public string RemainVolumLow { get; set; }
        [DecimalValidate2(ErrorMessage = "过零金额上限错误")]
        public string OverMoneyUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "过零气量上限错误")]
        public string OverVolumeUpper { get; set; }
        [IntValidate2(ErrorMessage = "柜门报警开关错误")]
        public string DoorAlarm { get; set; }
        //[DecimalValidate2(ErrorMessage = "PowerUpper,供电电压上限错误")]
        //public string PowerUpper { get; set; }
        //[DecimalValidate2(ErrorMessage = "PowerLow,供电电压下限错误")]
        //public string PowerLow { get; set; }
        //[DecimalValidate2(ErrorMessage = "BatteryLow,电池电压下限错误")]
        public string BatteryLow { get; set; }
        [IntValidate2(ErrorMessage = "信息发送次数错误")]
        public string AlarmTimes { get; set; }
        [IntValidate2(ErrorMessage = "信息发送间隔错误")]
        public string AlarmInvTime { get; set; }
        public string ProtocolNo { set; get; }
    }

    public class DeviceDelReq
    {
        [Required]
        public string meterNo { get; set; }
    }

    public class CustomerDelReq
    {
        [Required]
        public string customerNo { get; set; }
    }

    public class CustomerAddReq
    {
        [IntString2ValidateAttribute(ErrorMessage = "客户编号错误")]
        public String customerNo { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        [IntValidate(ErrorMessage = "客户类型错误")]
        public string customerType { get; set; }
        /// <summary>
        /// 合同号
        /// </summary>
        [IntString2ValidateAttribute(ErrorMessage = "合同号错误")]
        public String contractNo { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        [IntString2ValidateAttribute(ErrorMessage = "固定电话错误")]
        public String telNo { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        [IntString2ValidateAttribute(ErrorMessage = "移动电话错误")]
        public String mobileNo { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public String certNo { get; set; }
        /// <summary>
        /// 所属小区
        /// </summary>
        [IntValidate2(ErrorMessage = "所属小区错误")]
        public String estateNo { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public String address { get; set; }
        /// <summary>
        /// 楼号
        /// </summary>
        public String houseNo { get; set; }
        /// <summary>
        /// 单元号
        /// </summary>
        public String cellNo { get; set; }
        /// <summary>
        /// 室号
        /// </summary>
        public String roomNo { get; set; }
        /// <summary>
        /// 是否启用（0未启用1一起用2已停用
        /// </summary>
        [IntValidate2(ErrorMessage = "是否启用错误")]
        public string useState { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public String remark { get; set; }





        public string buildTime { get; set; }
        public string editTime { get; set; }
        public String Operator { get; set; }




        /// <summary>
        /// 缴费方式
        /// </summary>
        public string payWay { get; set; }
        public String bankNo { get; set; }
        /// <summary>
        /// 银行授权码
        /// </summary>
        public String bankAuthNo { get; set; }
        public String accountNo { get; set; }
        /// <summary>
        /// 银行账户名
        /// </summary>
        public String accountName { get; set; }
        public string bankCheck { get; set; }

        public String loginName { get; set; }
        public String Password { get; set; }
        public String taxNo { get; set; }
        public String enterpriseNo { get; set; }
    }

    public class CustomerUpdateReq
    {
        [IntString2ValidateAttribute(ErrorMessage = "客户编号错误")]
        public String customerNo { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        [IntValidate(ErrorMessage = "客户类型错误")]
        public string customerType { get; set; }
        /// <summary>
        /// 合同号
        /// </summary>
        [IntString2ValidateAttribute(ErrorMessage = "合同号错误")]
        public String contractNo { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        [IntString2ValidateAttribute(ErrorMessage = "固定电话错误")]
        public String telNo { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        [IntString2ValidateAttribute(ErrorMessage = "移动电话错误")]
        public String mobileNo { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public String certNo { get; set; }
        /// <summary>
        /// 所属小区
        /// </summary>
        [IntValidate2(ErrorMessage = "所属小区错误")]
        public String estateNo { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public String address { get; set; }
        /// <summary>
        /// 楼号
        /// </summary>
        public String houseNo { get; set; }
        /// <summary>
        /// 单元号
        /// </summary>
        public String cellNo { get; set; }
        /// <summary>
        /// 室号
        /// </summary>
        public String roomNo { get; set; }
        /// <summary>
        /// 是否启用（0未启用1一起用2已停用
        /// </summary>
        [IntValidate2(ErrorMessage = "是否启用错误")]
        public string useState { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public String remark { get; set; }





        public string buildTime { get; set; }
        public string editTime { get; set; }
        public String Operator { get; set; }




        /// <summary>
        /// 缴费方式
        /// </summary>
        public string payWay { get; set; }
        public String bankNo { get; set; }
        /// <summary>
        /// 银行授权码
        /// </summary>
        public String bankAuthNo { get; set; }
        public String accountNo { get; set; }
        /// <summary>
        /// 银行账户名
        /// </summary>
        public String accountName { get; set; }
        public string bankCheck { get; set; }

        public String loginName { get; set; }
        public String Password { get; set; }
        public String taxNo { get; set; }
        public String enterpriseNo { get; set; }
    }

    public class UpdatePermissionReq
    {
        [IdNotZeroValidate]
        public string perId { get; set; }
        public bool isOpen { get; set; }
    }

    public class ProtocolListReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
    }

    public class ProtocolAddUpdateReq
    {
        [Required]
        public string no { get; set; }
        [Required]
        public string name { get; set; }
    }

    public class MeterTypeAddReq
    {
        [Required]
        public string no { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string code { get; set; }
    }

    public class MeterTypeUpdateReq
    {
        [Required]
        public string no { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string code { get; set; }
    }

    public class UpdateLatlngReq
    {
        [Required]
        public string meterNo { get; set; }
        [LatValidate]
        public string lat { set; get; }
        [LngValidate]
        public string lng { set; get; }
    }
    public class EstablishReq
    {
        [Required]
        public int? moneyOrVolume { get; set; }
        [Required]
        public int? LinkMode { get; set; }
        [Required]
        public int? CommMode { get; set; }
        public int? isEncrypt { get; set; }
        [Required]
        public int? userId { get; set; }
        [Required]
        public int? IsIC { get; set; }
        public string deviceNo { get; set; }
        /// <summary>
        /// 客户编号，可有可无，有表示是选择了已存在的客户
        /// </summary>
        [IntString2ValidateAttribute(ErrorMessage = "customerNo,客户编号错误")]
        public string customerNo { get; set; }
        public string customerName { get; set; }
        public string address { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "mobileNo,电话错误")]
        public string mobileNo { get; set; }

        public string meterNo { get; set; }
        /// <summary>
        /// 设备厂家
        /// </summary>
        public String factoryNo { get; set; }
        //public string openState { get; set; }
        [DecimalValidate2(ErrorMessage = "baseVolume,表底读数错误")]
        public string baseVolume { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "meterTypeNo,设备类型错误")]
        public String meterTypeNo { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "fluidNo,价格类型错误")]
        public String fluidNo { get; set; }
        [IntString2ValidateAttribute(ErrorMessage = "communicateNo,通讯编号错误")]
        public String communicateNo { get; set; }
        //[IntString2ValidateAttribute(ErrorMessage = "barCode,条码号错误")]
        //public String barCode { get; set; }
        /// <summary>
        /// 口径
        /// </summary>
        public String caliber { get; set; }
        public String defineNo1 { get; set; }
        public String defineNo2 { get; set; }
        public String defineNo3 { get; set; }
        public String remark { get; set; }
        public string collectorNo { get; set; }
        [DecimalValidate2(ErrorMessage = "TempUpper,温度上限错误")]
        public string TempUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "TempLow,温度下限错误")]
        public string TempLow { get; set; }
        [DecimalValidate2(ErrorMessage = "PressUpper,压力上限错误")]
        public string PressUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "PressLow,压力下限错误")]
        public string PressLow { get; set; }
        [DecimalValidate2(ErrorMessage = "StdFlowUpper,标况流量上限错误")]
        public string StdFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "StdFlowLow,标况流量下限错误")]
        public string StdFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "WorkFlowUpper,工况流量上限错误")]
        public string WorkFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "WorkFlowLow,工况流量下限错误")]
        public string WorkFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "RemainMoneyLow,剩余金额下限错误")]
        public string RemainMoneyLow { get; set; }
        [DecimalValidate2(ErrorMessage = "RemainVolumLow,剩余气量下限错误")]
        public string RemainVolumLow { get; set; }
        [DecimalValidate2(ErrorMessage = "OverMoneyUpper,过零金额上限错误")]
        public string OverMoneyUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "OverVolumeUpper,过零气量上限错误")]
        public string OverVolumeUpper { get; set; }
        [IntValidate2(ErrorMessage = "DoorAlarm,柜门报警开关错误")]
        public string DoorAlarm { get; set; }
        [DecimalValidate2(ErrorMessage = "PowerUpper,供电电压上限错误")]
        public string PowerUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "PowerLow,供电电压下限错误")]
        public string PowerLow { get; set; }
        [DecimalValidate2(ErrorMessage = "BatteryLow,电池电压下限错误")]
        public string BatteryLow { get; set; }
        [IntValidate2(ErrorMessage = "AlarmTimes,信息发送次数错误")]
        public string AlarmTimes { get; set; }
        [IntValidate2(ErrorMessage = "AlarmInvTime,信息发送间隔错误")]
        public string AlarmInvTime { get; set; }
        /// <summary>
        /// 协议编号
        /// </summary>
        public string ProtocolNo { set; get; }

        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class OperRecordReq
    {
        //[Required]
        public string search { get; set; }
        [Required]
        public string pageIndex { get; set; }
        [Required]
        public string orderField { get; set; }
        [Required]
        public string isDesc { get; set; }
        public List<string> listSort { get; set; }
    }

    public class UpdateDeviceAlarmConfigReq
    {

        [DecimalValidate2(ErrorMessage = "温度上限错误")]
        public string TempUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "温度下限错误")]
        public string TempLow { get; set; }
        [DecimalValidate2(ErrorMessage = "压力上限错误")]
        public string PressUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "压力下限错误")]
        public string PressLow { get; set; }
        [DecimalValidate2(ErrorMessage = "标况流量上限错误")]
        public string StdFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "标况流量下限错误")]
        public string StdFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "工况流量上限错误")]
        public string WorkFlowUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "工况流量下限错误")]
        public string WorkFlowLow { get; set; }
        [DecimalValidate2(ErrorMessage = "剩余金额下限错误")]
        public string RemainMoneyLow { get; set; }
        [DecimalValidate2(ErrorMessage = "剩余气量下限错误")]
        public string RemainVolumLow { get; set; }
        [DecimalValidate2(ErrorMessage = "过零金额上限错误")]
        public string OverMoneyUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "过零气量上限错误")]
        public string OverVolumeUpper { get; set; }
        [IntValidate2(ErrorMessage = "柜门报警开关错误")]
        public string DoorAlarm { get; set; }
        [DecimalValidate2(ErrorMessage = "供电电压上限错误")]
        public string PowerUpper { get; set; }
        [DecimalValidate2(ErrorMessage = "供电电压下限错误")]
        public string PowerLow { get; set; }
        [DecimalValidate2(ErrorMessage = "电池电压下限错误")]
        public string BatteryLow { get; set; }
        [Required]
        public string meterNo { get; set; }
    }

    public class ConfirmUserReq
    {
        public string pwd { set; get; }
    }

    public class ValveOperReq
    {
        public string pwd { get; set; }
        public string oper { get; set; }
        public string commNo { get; set; }
    }

    public class ChargeOperReq
    {
        public string pwd { get; set; }
        public string commNo { get; set; }
        [DecimalValidate(ErrorMessage = "充值金额出错")]
        public string money { get; set; }
    }

    public class GetDaysReq
    {
        public string year { get; set; }
        public string month { get; set; }
    }
    public class GetChargeReportReq
    {
        [DateTimeValidate2]
        public string date { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public string customerNo { get; set; }
        //[Required]
        public string startTime { get; set; }
        public string lastName { get; set; }
        public string iYear { get; set; }
        public string iMonth { get; set; }
    }



    public class GetStdSumReq
    {
        [DateTimeValidate2]
        public string date { get; set; }
        [Required]
        public string type { get; set; }
        //[Required]
        public string customerNo { get; set; }
        public string startTime { get; set; }
        public string lastName { get; set; }
        public string iYear { get; set; }
        public string iMonth { get; set; }
    }

    public class IOTReq
    {

    }

    public class TestAddNewDeviceReq
    {
        public string deviceNo { get; set; }

    }

    public class WxSendMsgReq
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string topcolor { get; set; }
        public WxSendData data { get; set; }
    }

    public class WxSendData
    {
        public Value_color devid { get; set; }
        public Value_color content { get; set; }
    }

    public class Value_color
    {
        public string value { get; set; }
        public string color { get; set; }
    }

    public class CheckAlarmReq
    {
        public string oldCount { get; set; }
        public int? oldLastId { get; set; }
    }

    public class ManyList
    {
        public List<list1> list1 { get; set; }
        public List<list2> list2 { get; set; }
        public string title { get; set; }
    }

    public class list1
    {
        public string name { get; set; }
        public string sex { get; set; }
    }

    public class list2
    {
        public string str { get; set; }
        public string str2 { get; set; }
    }
}