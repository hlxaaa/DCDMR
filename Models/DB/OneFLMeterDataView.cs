using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class OneFLMeterDataView
    {
        public OneFLMeterDataView(){}
        //public OneFLMeterDataView(Req req){
            //if(req.Id != null)
            //Id = Convert.ToInt32(req.Id);
            //if(req.communicateNo != null)
            //communicateNo = req.communicateNo;
            //if(req.FLMeterNo != null)
            //FLMeterNo = Convert.ToInt32(req.FLMeterNo);
            //if(req.siteNo != null)
            //siteNo = req.siteNo;
            //if(req.InstantTime != null)
            //InstantTime = Convert.ToDateTime(req.InstantTime);
            //if(req.ReceivTime != null)
            //ReceivTime = Convert.ToDateTime(req.ReceivTime);
            //if(req.StdSum != null)
            //StdSum = Convert.ToDecimal(req.StdSum);
            //if(req.WorkSum != null)
            //WorkSum = Convert.ToDecimal(req.WorkSum);
            //if(req.StdFlow != null)
            //StdFlow = Convert.ToDecimal(req.StdFlow);
            //if(req.WorkFlow != null)
            //WorkFlow = Convert.ToDecimal(req.WorkFlow);
            //if(req.Temperature != null)
            //Temperature = Convert.ToDecimal(req.Temperature);
            //if(req.Pressure != null)
            //Pressure = Convert.ToDecimal(req.Pressure);
            //if(req.FMState != null)
            //FMState = Convert.ToInt32(req.FMState);
            //if(req.FMStateMsg != null)
            //FMStateMsg = req.FMStateMsg;
            //if(req.RTUState != null)
            //RTUState = Convert.ToInt32(req.RTUState);
            //if(req.RTUStateMsg != null)
            //RTUStateMsg = req.RTUStateMsg;
            //if(req.SumTotal != null)
            //SumTotal = Convert.ToDecimal(req.SumTotal);
            //if(req.RemainMoney != null)
            //RemainMoney = Convert.ToDecimal(req.RemainMoney);
            //if(req.RemainVolume != null)
            //RemainVolume = Convert.ToDecimal(req.RemainVolume);
            //if(req.Overdraft != null)
            //Overdraft = Convert.ToDecimal(req.Overdraft);
            //if(req.RemoteChargeMoney != null)
            //RemoteChargeMoney = Convert.ToDecimal(req.RemoteChargeMoney);
            //if(req.RemoteChargeTimes != null)
            //RemoteChargeTimes = Convert.ToInt32(req.RemoteChargeTimes);
            //if(req.Price != null)
            //Price = Convert.ToDecimal(req.Price);
            //if(req.ValveState != null)
            //ValveState = Convert.ToInt32(req.ValveState);
            //if(req.ValveStateMsg != null)
            //ValveStateMsg = req.ValveStateMsg;
            //if(req.PowerVoltage != null)
            //PowerVoltage = Convert.ToDecimal(req.PowerVoltage);
            //if(req.BatteryVoltage != null)
            //BatteryVoltage = Convert.ToDecimal(req.BatteryVoltage);
            //if(req.Reserve1 != null)
            //Reserve1 = req.Reserve1;
            //if(req.Reserve2 != null)
            //Reserve2 = req.Reserve2;
            //if(req.Reserve3 != null)
            //Reserve3 = req.Reserve3;
            //if(req.Reserve4 != null)
            //Reserve4 = req.Reserve4;
            //if(req.meterNo != null)
            //meterNo = Convert.ToInt32(req.meterNo);
            //if(req.userId != null)
            //userId = Convert.ToInt32(req.userId);
            //if(req.remark != null)
            //remark = req.remark;
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
            //if(req.IsIC != null)
            //IsIC = Convert.ToInt32(req.IsIC);
            //if(req.moneyOrVolume != null)
            //moneyOrVolume = Convert.ToInt32(req.moneyOrVolume);
            //if(req.meterTypeNo != null)
            //meterTypeNo = req.meterTypeNo;
            //if(req.lat != null)
            //lat = req.lat;
            //if(req.lng != null)
            //lng = req.lng;
            //if(req.deviceNo != null)
            //deviceNo = req.deviceNo;
            //if(req.customerName != null)
            //customerName = req.customerName;
            //if(req.address != null)
            //address = req.address;
            //if(req.LoginState != null)
            //LoginState = Convert.ToInt32(req.LoginState);
            //if(req.LoginStateMsg != null)
            //LoginStateMsg = req.LoginStateMsg;
        //}
        /// <summary>
        /// 
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String communicateNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? FLMeterNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String siteNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? InstantTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReceivTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? StdSum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? WorkSum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? StdFlow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? WorkFlow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? Temperature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? Pressure { get; set; }
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
        public Int32? RTUState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String RTUStateMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? SumTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? RemainMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? RemainVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? Overdraft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? RemoteChargeMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? RemoteChargeTimes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? ValveState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String ValveStateMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? PowerVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? BatteryVoltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Reserve1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Reserve2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Reserve3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Reserve4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? meterNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String remark { get; set; }
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
        public Int32? IsIC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32? moneyOrVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String meterTypeNo { get; set; }
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
        public String deviceNo { get; set; }
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
        public Int32? LoginState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String LoginStateMsg { get; set; }

}
}
