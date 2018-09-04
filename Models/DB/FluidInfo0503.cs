using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class FluidInfo0503
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
        public Int32? Alarm { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? FreeDays { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? MeterType { get; set; }

}
}
