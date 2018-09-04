using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class FeedGasDevUseSet
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 deviceNo { get; set; }
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

}
}
