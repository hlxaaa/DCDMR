using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ChangeMeterRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String oldMeterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String oldCommunicateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? oldCumulant { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? oldSurplus { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? oldOverDraft { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String oldCaliber { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String oldFluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? oldBaseVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String newMeterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String newCommunicateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? newBaseVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String newFluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String newCaliber { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String changeMan { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? changeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? operatorTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? dispatchID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? oldRemainVolume { get; set; }

}
}
