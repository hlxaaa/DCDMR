using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_SendData
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? CmdId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String SwiftNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String AllCount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ThisCount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CollectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DataToSend { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DealFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DealMessage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? SendTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? TimeOut { get; set; }

}
}
