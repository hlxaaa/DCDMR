using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_SendData
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
        public String DataToSend { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? DealFlag { get; set; }
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
        public DateTime? Timeout { get; set; }

}
}
