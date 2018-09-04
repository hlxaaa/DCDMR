using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class FMReaoprtYearly
    {
        public FMReaoprtYearly(){}
        //public FMReaoprtYearly(Req req){
            //if(req.Id != null)
            //Id = Convert.ToInt32(req.Id);
            //if(req.RtuNo != null)
            //RtuNo = Convert.ToInt32(req.RtuNo);
            //if(req.FLMeterNo != null)
            //FLMeterNo = Convert.ToInt32(req.FLMeterNo);
            //if(req.siteNo != null)
            //siteNo = req.siteNo;
            //if(req.Year != null)
            //Year = Convert.ToInt32(req.Year);
            //if(req.Month != null)
            //Month = Convert.ToInt32(req.Month);
            //if(req.StdSum != null)
            //StdSum = Convert.ToDecimal(req.StdSum);
            //if(req.WorkSum != null)
            //WorkSum = Convert.ToDecimal(req.WorkSum);
            //if(req.StdDosageVolume != null)
            //StdDosageVolume = Convert.ToDecimal(req.StdDosageVolume);
            //if(req.SumTotal != null)
            //SumTotal = Convert.ToDecimal(req.SumTotal);
            //if(req.RemainMoney != null)
            //RemainMoney = Convert.ToDecimal(req.RemainMoney);
            //if(req.RemainVolume != null)
            //RemainVolume = Convert.ToDecimal(req.RemainVolume);
            //if(req.Overdraft != null)
            //Overdraft = Convert.ToDecimal(req.Overdraft);
            //if(req.MaxStdFlow != null)
            //MaxStdFlow = Convert.ToDecimal(req.MaxStdFlow);
            //if(req.MinStdFlow != null)
            //MinStdFlow = Convert.ToDecimal(req.MinStdFlow);
            //if(req.AvgStdFlow != null)
            //AvgStdFlow = Convert.ToDecimal(req.AvgStdFlow);
            //if(req.MaxStdFlowTime != null)
            //MaxStdFlowTime = Convert.ToDateTime(req.MaxStdFlowTime);
            //if(req.MinStdFlowTime != null)
            //MinStdFlowTime = Convert.ToDateTime(req.MinStdFlowTime);
            //if(req.MaxTemperature != null)
            //MaxTemperature = Convert.ToDecimal(req.MaxTemperature);
            //if(req.MinTemperature != null)
            //MinTemperature = Convert.ToDecimal(req.MinTemperature);
            //if(req.AvgTemperature != null)
            //AvgTemperature = Convert.ToDecimal(req.AvgTemperature);
            //if(req.MaxTempTime != null)
            //MaxTempTime = Convert.ToDateTime(req.MaxTempTime);
            //if(req.MinTempTime != null)
            //MinTempTime = Convert.ToDateTime(req.MinTempTime);
            //if(req.MaxPress != null)
            //MaxPress = Convert.ToDecimal(req.MaxPress);
            //if(req.MinPress != null)
            //MinPress = Convert.ToDecimal(req.MinPress);
            //if(req.AvgPress != null)
            //AvgPress = Convert.ToDecimal(req.AvgPress);
            //if(req.MaxPressTime != null)
            //MaxPressTime = Convert.ToDateTime(req.MaxPressTime);
            //if(req.MinPressTime != null)
            //MinPressTime = Convert.ToDateTime(req.MinPressTime);
            //if(req.ReportTime != null)
            //ReportTime = Convert.ToDateTime(req.ReportTime);
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
        public Int32? RtuNo { get; set; }
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
        public Int32? Year { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? Month { get; set; }
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
        public Decimal? StdDosageVolume { get; set; }
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
        public Decimal? MaxStdFlow { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? MinStdFlow { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? AvgStdFlow { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MaxStdFlowTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MinStdFlowTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? MaxTemperature { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? MinTemperature { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? AvgTemperature { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MaxTempTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MinTempTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? MaxPress { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? MinPress { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? AvgPress { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MaxPressTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MinPressTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ReportTime { get; set; }
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
