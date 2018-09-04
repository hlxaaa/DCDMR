using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class RtuNoInfo
    {
        public RtuNoInfo(){}
        //public RtuNoInfo(Req req){
            //if(req.RtuNo != null)
            //RtuNo = req.RtuNo;
            //if(req.FLMeterNo != null)
            //FLMeterNo = req.FLMeterNo;
            //if(req.Rcount != null)
            //Rcount = Convert.ToInt32(req.Rcount);
        //}
        /// <summary>
        ///
        /// </summary>
        public String RtuNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FLMeterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? Rcount { get; set; }

}
}
