using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_CollectorBind
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CommunicateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CollectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CommCmd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CmdResult { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }

}
}
