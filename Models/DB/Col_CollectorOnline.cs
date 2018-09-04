using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_CollectorOnline
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CollectorNo { get; set; }
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
        public String LoginState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String LoginStateMsg { get; set; }

}
}
