using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_CtrlCmd
    {
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
