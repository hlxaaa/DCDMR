using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class TMReaoprtDaily
    {
        public TMReaoprtDaily(){}
        //public TMReaoprtDaily(Req req){
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
            //if(req.Day != null)
            //Day = Convert.ToInt32(req.Day);
            //if(req.Hour != null)
            //Hour = Convert.ToInt32(req.Hour);
            //if(req.PValue != null)
            //PValue = Convert.ToDecimal(req.PValue);
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
        public Int32? Day { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? Hour { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? PValue { get; set; }
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
