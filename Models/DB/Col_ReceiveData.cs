using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_ReceiveData
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
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
        public String AllCount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ThisCount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dataCmd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dataArea { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ReceiveTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dealFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? dealTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dealMsg { get; set; }

}
}
