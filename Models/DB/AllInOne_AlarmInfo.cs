using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_AlarmInfo
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// 
        public string communicateNo { get; set; }
        public String siteNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? RtuNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? Devid { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? DevType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevTypeName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String AlarmContent { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? AlarmTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? DealFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? DealTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DealOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SmsTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SmsSendTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SmsInvTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Linkman { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MobileNo { get; set; }

    }
}
