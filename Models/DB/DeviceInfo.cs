using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class DeviceInfo
    {
        public DeviceInfo(){}
        //public DeviceInfo(Req req){
            //if(req.meterNo != null)
            //meterNo = Convert.ToInt32(req.meterNo);
            //if(req.siteNo != null)
            //siteNo = req.siteNo;
            //if(req.communicateNo != null)
            //communicateNo = req.communicateNo;
            //if(req.CommAddr != null)
            //CommAddr = Convert.ToInt32(req.CommAddr);
            //if(req.ProtocolNo != null)
            //ProtocolNo = req.ProtocolNo;
            //if(req.CommMode != null)
            //CommMode = Convert.ToInt32(req.CommMode);
            //if(req.LinkMode != null)
            //LinkMode = Convert.ToInt32(req.LinkMode);
            //if(req.barCode != null)
            //barCode = req.barCode;
            //if(req.customerNo != null)
            //customerNo = req.customerNo;
            //if(req.meterTypeNo != null)
            //meterTypeNo = req.meterTypeNo;
            //if(req.factoryNo != null)
            //factoryNo = req.factoryNo;
            //if(req.openState != null)
            //openState = Convert.ToInt32(req.openState);
            //if(req.caliber != null)
            //caliber = req.caliber;
            //if(req.baseVolume != null)
            //baseVolume = Convert.ToDecimal(req.baseVolume);
            //if(req.fluidNo != null)
            //fluidNo = req.fluidNo;
            //if(req.remark != null)
            //remark = req.remark;
            //if(req.defineNo1 != null)
            //defineNo1 = req.defineNo1;
            //if(req.defineNo2 != null)
            //defineNo2 = req.defineNo2;
            //if(req.defineNo3 != null)
            //defineNo3 = req.defineNo3;
            //if(req.buildTime != null)
            //buildTime = Convert.ToDateTime(req.buildTime);
            //if(req.editTime != null)
            //editTime = Convert.ToDateTime(req.editTime);
            //if(req.Operator != null)
            //Operator = req.Operator;
            //if(req.isConcentrate != null)
            //isConcentrate = Convert.ToBoolean(req.isConcentrate);
            //if(req.collectorNo != null)
            //collectorNo = req.collectorNo;
            //if(req.MeterType != null)
            //MeterType = req.MeterType;
            //if(req.Volatility != null)
            //Volatility = req.Volatility;
            //if(req.AlarmTimes != null)
            //AlarmTimes = Convert.ToInt32(req.AlarmTimes);
            //if(req.AlarmInvTime != null)
            //AlarmInvTime = Convert.ToInt32(req.AlarmInvTime);
            //if(req.TempUpper != null)
            //TempUpper = Convert.ToDecimal(req.TempUpper);
            //if(req.TempLow != null)
            //TempLow = Convert.ToDecimal(req.TempLow);
            //if(req.PressUpper != null)
            //PressUpper = Convert.ToDecimal(req.PressUpper);
            //if(req.PressLow != null)
            //PressLow = Convert.ToDecimal(req.PressLow);
            //if(req.StdFlowUpper != null)
            //StdFlowUpper = Convert.ToDecimal(req.StdFlowUpper);
            //if(req.StdFlowLow != null)
            //StdFlowLow = Convert.ToDecimal(req.StdFlowLow);
            //if(req.WorkFlowUpper != null)
            //WorkFlowUpper = Convert.ToDecimal(req.WorkFlowUpper);
            //if(req.WorkFlowLow != null)
            //WorkFlowLow = Convert.ToDecimal(req.WorkFlowLow);
            //if(req.RemainMoneyLow != null)
            //RemainMoneyLow = Convert.ToDecimal(req.RemainMoneyLow);
            //if(req.RemainVolumLow != null)
            //RemainVolumLow = Convert.ToDecimal(req.RemainVolumLow);
            //if(req.OverMoneyUpper != null)
            //OverMoneyUpper = Convert.ToDecimal(req.OverMoneyUpper);
            //if(req.OverVolumeUpper != null)
            //OverVolumeUpper = Convert.ToDecimal(req.OverVolumeUpper);
            //if(req.DoorAlarm != null)
            //DoorAlarm = Convert.ToInt32(req.DoorAlarm);
            //if(req.PowerUpper != null)
            //PowerUpper = Convert.ToDecimal(req.PowerUpper);
            //if(req.PowerLow != null)
            //PowerLow = Convert.ToDecimal(req.PowerLow);
            //if(req.BatteryLow != null)
            //BatteryLow = Convert.ToDecimal(req.BatteryLow);
            //if(req.Image != null)
            //Image = req.Image;
            //if(req.IsValve != null)
            //IsValve = Convert.ToInt32(req.IsValve);
            //if(req.DayFmStart != null)
            //DayFmStart = Convert.ToInt32(req.DayFmStart);
            //if(req.lat != null)
            //lat = req.lat;
            //if(req.lng != null)
            //lng = req.lng;
            //if(req.deviceNo != null)
            //deviceNo = req.deviceNo;
            //if(req.fluidType != null)
            //fluidType = req.fluidType;
            //if(req.IsIC != null)
            //IsIC = Convert.ToInt32(req.IsIC);
            //if(req.ScadaInvTime != null)
            //ScadaInvTime = Convert.ToInt32(req.ScadaInvTime);
            //if(req.isEncrypt != null)
            //isEncrypt = Convert.ToInt32(req.isEncrypt);
            //if(req.moneyOrVolume != null)
            //moneyOrVolume = Convert.ToInt32(req.moneyOrVolume);
        //}
        /// <summary>
        /// 
        /// </summary>
        public Int32 meterNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String siteNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String communicateNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? CommAddr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String ProtocolNo { get; set; }
        /// <summary>
        /// 0主动 1被动
        /// </summary>
        public Int32? CommMode { get; set; }
        /// <summary>
        /// 1短连接  0长连接
        /// </summary>
        public Int32? LinkMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String barCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String meterTypeNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String factoryNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? openState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String caliber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? baseVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String fluidNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String remark { get; set; }
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
        public DateTime? buildTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? editTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        /// 是否集中？
        /// </summary>
        public Boolean? isConcentrate { get; set; }
        /// <summary>
        /// 集中器编号
        /// </summary>
        public String collectorNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String MeterType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Volatility { get; set; }
        /// <summary>
        /// 信息发送次数
        /// </summary>
        public Int32? AlarmTimes { get; set; }
        /// <summary>
        /// 信息发送间隔
        /// </summary>
        public Int32? AlarmInvTime { get; set; }
        /// <summary>
        /// 温度上限
        /// </summary>
        public Decimal? TempUpper { get; set; }
        /// <summary>
        /// 温度下限
        /// </summary>
        public Decimal? TempLow { get; set; }
        /// <summary>
        /// 压力上限
        /// </summary>
        public Decimal? PressUpper { get; set; }
        /// <summary>
        /// 压力下限
        /// </summary>
        public Decimal? PressLow { get; set; }
        /// <summary>
        /// 标况流量上限
        /// </summary>
        public Decimal? StdFlowUpper { get; set; }
        /// <summary>
        /// 标况流量下限
        /// </summary>
        public Decimal? StdFlowLow { get; set; }
        /// <summary>
        /// 工况流量上限
        /// </summary>
        public Decimal? WorkFlowUpper { get; set; }
        /// <summary>
        /// 工况流量下限
        /// </summary>
        public Decimal? WorkFlowLow { get; set; }
        /// <summary>
        /// 剩余金额下限
        /// </summary>
        public Decimal? RemainMoneyLow { get; set; }
        /// <summary>
        /// 剩余气量下限
        /// </summary>
        public Decimal? RemainVolumLow { get; set; }
        /// <summary>
        /// 过零金额上限
        /// </summary>
        public Decimal? OverMoneyUpper { get; set; }
        /// <summary>
        /// 过零气量下限
        /// </summary>
        public Decimal? OverVolumeUpper { get; set; }
        /// <summary>
        /// 柜门报警开关（0不报警1报警）
        /// </summary>
        public Int32? DoorAlarm { get; set; }
        /// <summary>
        /// 供电电压上限
        /// </summary>
        public Decimal? PowerUpper { get; set; }
        /// <summary>
        /// 供电电压下限
        /// </summary>
        public Decimal? PowerLow { get; set; }
        /// <summary>
        /// 铅酸电池电压下限
        /// </summary>
        public Decimal? BatteryLow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? IsValve { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? DayFmStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String lat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String lng { get; set; }
        /// <summary>
        /// 信东设备号，15位。字母数字
        /// </summary>
        public String deviceNo { get; set; }
        /// <summary>
        /// 价格类型标志0x11:阶梯价格 0x22:分时价格 0x33:定时价格
        /// </summary>
        public String fluidType { get; set; }
        /// <summary>
        /// 是否使用IC卡(一体机一定用，鸿鹄rtu，信东rtu，可选）
        /// </summary>
        public Int32? IsIC { get; set; }
        /// <summary>
        /// 上位机主动抄表时间间隔（毫秒）
        /// </summary>
        public Int32? ScadaInvTime { get; set; }
        /// <summary>
        /// 是否加密(大概是用于远程充值的)
        /// </summary>
        public Int32? isEncrypt { get; set; }
        /// <summary>
        /// 0计费 1计量
        /// </summary>
        public Int32? moneyOrVolume { get; set; }

}
}
