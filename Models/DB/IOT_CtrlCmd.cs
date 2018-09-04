using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class IOT_CtrlCmd
    {
        public IOT_CtrlCmd(){}
        //public IOT_CtrlCmd(Req req){
            //if(req.Id != null)
            //Id = Convert.ToInt32(req.Id);
            //if(req.siteNo != null)
            //siteNo = req.siteNo;
            //if(req.CmdType != null)
            //CmdType = req.CmdType;
            //if(req.RTUNo != null)
            //RTUNo = Convert.ToInt32(req.RTUNo);
            //if(req.CommNo != null)
            //CommNo = req.CommNo;
            //if(req.MeterNo != null)
            //MeterNo = req.MeterNo;
            //if(req.FSEQ != null)
            //FSEQ = Convert.ToInt32(req.FSEQ);
            //if(req.Operator != null)
            //Operator = req.Operator;
            //if(req.CreateTime != null)
            //CreateTime = Convert.ToDateTime(req.CreateTime);
            //if(req.Timeout != null)
            //Timeout = Convert.ToDateTime(req.Timeout);
            //if(req.CmdState != null)
            //CmdState = Convert.ToInt32(req.CmdState);
            //if(req.FeedBackMsg != null)
            //FeedBackMsg = req.FeedBackMsg;
            //if(req.SetIPAddr != null)
            //SetIPAddr = req.SetIPAddr;
            //if(req.SetPortNum != null)
            //SetPortNum = Convert.ToInt32(req.SetPortNum);
            //if(req.Set485Inv != null)
            //Set485Inv = Convert.ToInt32(req.Set485Inv);
            //if(req.SetGPRSInv != null)
            //SetGPRSInv = Convert.ToInt32(req.SetGPRSInv);
            //if(req.UploadTime1 != null)
            //UploadTime1 = req.UploadTime1;
            //if(req.UploadTime2 != null)
            //UploadTime2 = req.UploadTime2;
            //if(req.UploadTime3 != null)
            //UploadTime3 = req.UploadTime3;
            //if(req.MeterNum != null)
            //MeterNum = Convert.ToInt32(req.MeterNum);
            //if(req.ProtocotSet != null)
            //ProtocotSet = req.ProtocotSet;
            //if(req.ComAddrSet != null)
            //ComAddrSet = req.ComAddrSet;
            //if(req.BaudRateSet != null)
            //BaudRateSet = req.BaudRateSet;
            //if(req.BuyTimes != null)
            //BuyTimes = Convert.ToInt32(req.BuyTimes);
            //if(req.BuyMoney != null)
            //BuyMoney = Convert.ToDecimal(req.BuyMoney);
            //if(req.SetPrice != null)
            //SetPrice = Convert.ToDecimal(req.SetPrice);
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String siteNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CmdType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? RTUNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CommNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? FSEQ { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? Timeout { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? CmdState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FeedBackMsg { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String SetIPAddr { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SetPortNum { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? Set485Inv { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SetGPRSInv { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String UploadTime1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String UploadTime2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String UploadTime3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? MeterNum { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ProtocotSet { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ComAddrSet { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String BaudRateSet { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? BuyTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? BuyMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? SetPrice { get; set; }

}
}
