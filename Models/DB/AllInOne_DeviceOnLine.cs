using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_DeviceOnLine
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
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
