using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class DeviceView
    {
        public DeviceView(){}
        //public DeviceView(Req req){
            //if(req.meterNo != null)
            //meterNo = Convert.ToInt32(req.meterNo);
            //if(req.IsIC != null)
            //IsIC = Convert.ToInt32(req.IsIC);
            //if(req.moneyOrVolume != null)
            //moneyOrVolume = Convert.ToInt32(req.moneyOrVolume);
            //if(req.isEncrypt != null)
            //isEncrypt = Convert.ToInt32(req.isEncrypt);
            //if(req.ScadaInvTime != null)
            //ScadaInvTime = Convert.ToInt32(req.ScadaInvTime);
            //if(req.communicateNo != null)
            //communicateNo = req.communicateNo;
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
            //if(req.ProtocolNo != null)
            //ProtocolNo = req.ProtocolNo;
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
            //if(req.deviceNo != null)
            //deviceNo = req.deviceNo;
            //if(req.collectorNo != null)
            //collectorNo = req.collectorNo;
            //if(req.MeterType != null)
            //MeterType = req.MeterType;
            //if(req.isConcentrate != null)
            //isConcentrate = Convert.ToBoolean(req.isConcentrate);
            //if(req.AlarmTimes != null)
            //AlarmTimes = Convert.ToInt32(req.AlarmTimes);
            //if(req.AlarmInvTime != null)
            //AlarmInvTime = Convert.ToInt32(req.AlarmInvTime);
            //if(req.CommMode != null)
            //CommMode = Convert.ToInt32(req.CommMode);
            //if(req.LinkMode != null)
            //LinkMode = Convert.ToInt32(req.LinkMode);
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
            //if(req.LoginState != null)
            //LoginState = Convert.ToInt32(req.LoginState);
            //if(req.FMState != null)
            //FMState = Convert.ToInt32(req.FMState);
            //if(req.FMStateMsg != null)
            //FMStateMsg = req.FMStateMsg;
            //if(req.LoginStateMsg != null)
            //LoginStateMsg = req.LoginStateMsg;
            //if(req.customerType != null)
            //customerType = Convert.ToInt32(req.customerType);
            //if(req.CustTypeName != null)
            //CustTypeName = req.CustTypeName;
        //}
        /// <summary>
        /// 
        /// </summary>
        public Int32 meterNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? IsIC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? moneyOrVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? isEncrypt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? ScadaInvTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String communicateNo { get; set; }
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
        public String ProtocolNo { get; set; }
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
        public String deviceNo { get; set; }
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
        public Int32? AlarmTimes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? AlarmInvTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? CommMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? LinkMode { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        public Int32? LoginState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? FMState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String FMStateMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String LoginStateMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? customerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String CustTypeName { get; set; }

}
}
