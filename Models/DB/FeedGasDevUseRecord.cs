using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class FeedGasDevUseRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? recordId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? deviceNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String devName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String devModel { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String devFactory { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String designUsage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? operatorTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }

}
}
