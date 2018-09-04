using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_CtrlCmd
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 CmdId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CollectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CmdType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CmdData { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CmdEnd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CmdState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FeedBackMsg { get; set; }

}
}
