using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class FMReportYear
    {
        public FMReportYear(){}
        //public FMReportYear(Req req){
            //if(req.id != null)
            //id = Convert.ToInt32(req.id);
            //if(req.FlMeterNo != null)
            //FlMeterNo = Convert.ToInt32(req.FlMeterNo);
            //if(req.year != null)
            //year = Convert.ToInt32(req.year);
            //if(req.month != null)
            //month = Convert.ToInt32(req.month);
            //if(req.stdsum != null)
            //stdsum = Convert.ToDecimal(req.stdsum);
            //if(req.custTypeName != null)
            //custTypeName = req.custTypeName;
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? FlMeterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? year { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? month { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? stdsum { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String custTypeName { get; set; }

}
}
