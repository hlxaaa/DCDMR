using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class CopyDataICFM
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
        public String customerNo { get; set; }
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
        public Decimal? currentMoney { get; set; }
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
        public DateTime? operateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DataFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DataSource { get; set; }

}
}
