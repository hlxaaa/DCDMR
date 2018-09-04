using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ChargeLimit
    {
        /// <summary>
        ///
        /// </summary>
        public String branchNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? IsEffect { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? SumLimit { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? CurrLimit { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? CurrRemain { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? SetTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }

}
}
