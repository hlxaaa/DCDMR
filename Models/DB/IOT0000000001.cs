using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class IOT0000000001
    {
        public IOT0000000001(){}
        //public IOT0000000001(Req req){
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

}
}
