using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_RTUOnLine
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String siteNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? RTUNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CommNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? LoginTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? LogoutTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? LoginState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String LoginStateMsg { get; set; }

}
}
