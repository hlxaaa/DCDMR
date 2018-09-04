using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class FeedGasCheckRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? recordId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkItemNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkItemName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkResult { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? checkData { get; set; }

}
}
