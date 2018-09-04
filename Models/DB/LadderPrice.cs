using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class LadderPrice
    {
        /// <summary>
        ///
        /// </summary>
        public String fluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32 ladderNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? minValue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? maxValue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? payType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? startMonth { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? intervalMonth { get; set; }

}
}
