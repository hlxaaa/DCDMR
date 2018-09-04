using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_WIMeterCopyData
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CommunicateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ReadState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CollectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? OcrRead { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ReadTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevPower { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevVersion { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CopyWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String StateMsg { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String SendDevState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? thisRead { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String balanceStatus { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? balanceMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? balanceTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? operationTime { get; set; }

}
}
