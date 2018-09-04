using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class SecondAuthSet
    {
        /// <summary>
        ///
        /// </summary>
        public String MarkCode { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MarkName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? IsEnable { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Threshold { get; set; }

}
}
