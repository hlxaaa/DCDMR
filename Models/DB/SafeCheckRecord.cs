using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class SafeCheckRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? dispatchID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? customerType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? arriveTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? completeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String isAdd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkResult { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String telNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DocumentNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String OtherSituation { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ModifyNotice { get; set; }

}
}
