using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class DispatchList
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? RegID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? dispathTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Dispatcher { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? EventType { get; set; }
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
        public Int32? customerType { get; set; }
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
        public String TaskNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taskDescribe { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String skillNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? skillSn { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String skillName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? requestEndTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? appointmentTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? actualEndTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String finishDetial { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String custComments { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checker { get; set; }
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
        public Int32? dealState { get; set; }
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
        public Int32? attachFormID1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? attachFormID2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? attachFormID3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? attachFormID4 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DocumentNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? dealTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String receptionist { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Estate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String picturePath { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String submiter { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkResult { get; set; }

}
}
