using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class MultiCopyDataHis
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
        public Decimal? lastRead_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentRead_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentVolume_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? meterState_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lastRead_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentRead_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentVolume_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? meterState_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lastRead_G { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentRead_G { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentVolume_G { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? meterState_G { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? copyState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String copyWay { get; set; }
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
        public Int32? isBalance { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? relativeRead_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? relativeRead_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? relativeRead_G { get; set; }

}
}
