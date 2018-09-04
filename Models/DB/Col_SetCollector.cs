using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_SetCollector
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 CmdId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String SwiftNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CollectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FreezeType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FreezeCycle { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FreezeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String WorkType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String WorkCycle { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String WorkTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevFreezeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CmdState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }

}
}
