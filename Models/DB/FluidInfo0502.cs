using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class FluidInfo0502
    {
        /// <summary>
        ///
        /// </summary>
        public String fluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? MaxValue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? OverDraft { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ALARM { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? FreeDays { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? MeterType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? PriceTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? PriceDate { get; set; }

}
}
