using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class CopyData
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lastShow { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lastDosage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentShow { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentDosage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? unitPrice { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? printFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? meterState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String copyWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? copyState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? copyTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String copyMan { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? OperateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? isBalance { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }

}
}
