using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class StaffInfo
    {
        /// <summary>
        ///
        /// </summary>
        public String staffNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String loginName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String staffName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Password { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String departNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String duties { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String telNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String certNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String roleNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? buildTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? editTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? SecondAuth { get; set; }

}
}
