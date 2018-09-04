using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class RtuOnlineRate
    {
        public RtuOnlineRate(){}
        //public RtuOnlineRate(Req req){
            //if(req.ID != null)
            //ID = Convert.ToInt32(req.ID);
            //if(req.RTUNo != null)
            //RTUNo = Convert.ToInt32(req.RTUNo);
            //if(req.FLMeterNo != null)
            //FLMeterNo = Convert.ToInt32(req.FLMeterNo);
            //if(req.acount != null)
            //acount = Convert.ToDecimal(req.acount);
            //if(req.InDate != null)
            //InDate = Convert.ToDateTime(req.InDate);
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 ID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32 RTUNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32 FLMeterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal acount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime InDate { get; set; }

}
}
