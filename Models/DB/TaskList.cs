using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class TaskList
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? RegTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? EventType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String tellNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taskTypeNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taskDescribe { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String receptionist { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dealDetial { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? dealTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Checker { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkDetial { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? checkTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Confirmor { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ConfirmDetial { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ConfirmTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? IsNeedReview { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? IsReviewEnd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ReviewNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? DealState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? IsEnd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String abendReason { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Estate { get; set; }

}
}
