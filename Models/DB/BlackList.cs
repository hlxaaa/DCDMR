using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class BlackList
    {
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? addTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String addReason { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? editTime { get; set; }

}
}
