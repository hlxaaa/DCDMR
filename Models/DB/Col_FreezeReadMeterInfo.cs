using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_FreezeReadMeterInfo
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
        public String ReadState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ImageName { get; set; }
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
        public DateTime? ReadTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String OcrRead { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String OcrState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ThisRead { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String OcrResult { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? OcrTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterNo { get; set; }

}
}
