using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class TMReaoprtYearly
    {
        public TMReaoprtYearly(){}
        //public TMReaoprtYearly(Req req){
            //if(req.Id != null)
            //Id = Convert.ToInt32(req.Id);
            //if(req.TransmitterNo != null)
            //TransmitterNo = Convert.ToInt32(req.TransmitterNo);
            //if(req.siteNo != null)
            //siteNo = req.siteNo;
            //if(req.RTUNo != null)
            //RTUNo = Convert.ToInt32(req.RTUNo);
            //if(req.Year != null)
            //Year = Convert.ToInt32(req.Year);
            //if(req.Month != null)
            //Month = Convert.ToInt32(req.Month);
            //if(req.MaxPValue != null)
            //MaxPValue = Convert.ToDecimal(req.MaxPValue);
            //if(req.MinPValue != null)
            //MinPValue = Convert.ToDecimal(req.MinPValue);
            //if(req.AvgPValue != null)
            //AvgPValue = Convert.ToDecimal(req.AvgPValue);
            //if(req.MaxValueTime != null)
            //MaxValueTime = Convert.ToDateTime(req.MaxValueTime);
            //if(req.MinValueTime != null)
            //MinValueTime = Convert.ToDateTime(req.MinValueTime);
            //if(req.ReportTime != null)
            //ReportTime = Convert.ToDateTime(req.ReportTime);
            //if(req.Reserve1 != null)
            //Reserve1 = req.Reserve1;
            //if(req.Reserve2 != null)
            //Reserve2 = req.Reserve2;
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? TransmitterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String siteNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? RTUNo { get; set; }
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
        public Decimal? MaxPValue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? MinPValue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? AvgPValue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MaxValueTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? MinValueTime { get; set; }
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

}
}
