using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class FluidInfo0303
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
        public Decimal? currentPrice { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? persetPrice { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? persetTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? alarmVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? maxOverDraft { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? testSelf { get; set; }

}
}
