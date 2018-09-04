using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class LateFeeParam
    {
        /// <summary>
        ///
        /// </summary>
        public String lateFeeNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String lateFeeName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? interest { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? bonusDays { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? calcType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? limitType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? ratio { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? defLimit { get; set; }

}
}
