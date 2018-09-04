using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class LadderVolume
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? startTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? endTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? StepVolume1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? StepVolume2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? StepVolume3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? StepVolume4 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? StepVolume5 { get; set; }

}
}
