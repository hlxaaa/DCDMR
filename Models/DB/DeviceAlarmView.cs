using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class DeviceAlarmView
    {
        public DeviceAlarmView(){}
        //public DeviceAlarmView(Req req){
            //if(req.Id != null)
            //Id = Convert.ToInt32(req.Id);
            //if(req.siteNo != null)
            //siteNo = req.siteNo;
            //if(req.communicateNo != null)
            //communicateNo = req.communicateNo;
            //if(req.Devid != null)
            //Devid = Convert.ToInt32(req.Devid);
            //if(req.DevType != null)
            //DevType = Convert.ToInt32(req.DevType);
            //if(req.DevTypeName != null)
            //DevTypeName = req.DevTypeName;
            //if(req.AlarmContent != null)
            //AlarmContent = req.AlarmContent;
            //if(req.AlarmTime != null)
            //AlarmTime = Convert.ToDateTime(req.AlarmTime);
            //if(req.DealFlag != null)
            //DealFlag = Convert.ToInt32(req.DealFlag);
            //if(req.DealTime != null)
            //DealTime = Convert.ToDateTime(req.DealTime);
            //if(req.DealOperator != null)
            //DealOperator = req.DealOperator;
            //if(req.SmsTimes != null)
            //SmsTimes = Convert.ToInt32(req.SmsTimes);
            //if(req.SmsSendTimes != null)
            //SmsSendTimes = Convert.ToInt32(req.SmsSendTimes);
            //if(req.SmsInvTime != null)
            //SmsInvTime = Convert.ToInt32(req.SmsInvTime);
            //if(req.Linkman != null)
            //Linkman = req.Linkman;
            //if(req.MobileNo != null)
            //MobileNo = req.MobileNo;
            //if(req.meterNo != null)
            //meterNo = Convert.ToInt32(req.meterNo);
            //if(req.barCode != null)
            //barCode = req.barCode;
            //if(req.customerNo != null)
            //customerNo = req.customerNo;
            //if(req.deviceNo != null)
            //deviceNo = req.deviceNo;
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
            //if(req.lat != null)
            //lat = req.lat;
            //if(req.lng != null)
            //lng = req.lng;
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
            //if(req.collectorNo != null)
            //collectorNo = req.collectorNo;
            //if(req.MeterType != null)
            //MeterType = req.MeterType;
            //if(req.isConcentrate != null)
            //isConcentrate = Convert.ToBoolean(req.isConcentrate);
            //if(req.factoryName != null)
            //factoryName = req.factoryName;
            //if(req.meterTypeName != null)
            //meterTypeName = req.meterTypeName;
            //if(req.openStateName != null)
            //openStateName = req.openStateName;
            //if(req.fluidName != null)
            //fluidName = req.fluidName;
            //if(req.customerName != null)
            //customerName = req.customerName;
            //if(req.address != null)
            //address = req.address;
            //if(req.userId != null)
            //userId = Convert.ToInt32(req.userId);
            //if(req.userName != null)
            //userName = req.userName;
            //if(req.LEVEL != null)
            //LEVEL = Convert.ToInt32(req.LEVEL);
            //if(req.parentId != null)
            //parentId = Convert.ToInt32(req.parentId);
            //if(req.isStaff != null)
            //isStaff = Convert.ToBoolean(req.isStaff);
            //if(req.cId1 != null)
            //cId1 = req.cId1;
            //if(req.cId2 != null)
            //cId2 = req.cId2;
            //if(req.cId3 != null)
            //cId3 = req.cId3;
            //if(req.cId4 != null)
            //cId4 = req.cId4;
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
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
        public Int32? Devid { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? DevType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevTypeName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String AlarmContent { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? AlarmTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? DealFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? DealTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DealOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SmsTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SmsSendTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SmsInvTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Linkman { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MobileNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? meterNo { get; set; }
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
        public String deviceNo { get; set; }
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
        public String lat { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String lng { get; set; }
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
        ///
        /// </summary>
        public String collectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Boolean? isConcentrate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String factoryName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterTypeName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String openStateName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String address { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? userId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String userName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? LEVEL { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? parentId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Boolean? isStaff { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId4 { get; set; }

}
}
