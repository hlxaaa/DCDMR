using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_OperateRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String content { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String operatorId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String autherId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? operateTime { get; set; }

}
}
