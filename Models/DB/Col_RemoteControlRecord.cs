using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_RemoteControlRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
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
        public String Operater { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MessageState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterValve { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ErrorReason { get; set; }

}
}
