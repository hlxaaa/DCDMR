using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class IOT_CopyData
    {
        public IOT_CopyData(){}
        //public IOT_CopyData(Req req){
            //if(req.Id != null)
            //Id = Convert.ToInt32(req.Id);
            //if(req.meterNo != null)
            //meterNo = req.meterNo;
            //if(req.lastShow != null)
            //lastShow = Convert.ToDecimal(req.lastShow);
            //if(req.lastDosage != null)
            //lastDosage = Convert.ToDecimal(req.lastDosage);
            //if(req.currentShow != null)
            //currentShow = Convert.ToDecimal(req.currentShow);
            //if(req.currentDosage != null)
            //currentDosage = Convert.ToDecimal(req.currentDosage);
            //if(req.unitPrice != null)
            //unitPrice = Convert.ToDecimal(req.unitPrice);
            //if(req.printFlag != null)
            //printFlag = Convert.ToInt32(req.printFlag);
            //if(req.meterState != null)
            //meterState = Convert.ToInt32(req.meterState);
            //if(req.copyWay != null)
            //copyWay = req.copyWay;
            //if(req.copyState != null)
            //copyState = Convert.ToInt32(req.copyState);
            //if(req.copyTime != null)
            //copyTime = Convert.ToDateTime(req.copyTime);
            //if(req.copyMan != null)
            //copyMan = req.copyMan;
            //if(req.Operator != null)
            //Operator = req.Operator;
            //if(req.OperateTime != null)
            //OperateTime = Convert.ToDateTime(req.OperateTime);
            //if(req.isBalance != null)
            //isBalance = Convert.ToInt32(req.isBalance);
            //if(req.Remark != null)
            //Remark = req.Remark;
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lastShow { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lastDosage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentShow { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentDosage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? unitPrice { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? printFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? meterState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String copyWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? copyState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? copyTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String copyMan { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? OperateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? isBalance { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }

}
}
