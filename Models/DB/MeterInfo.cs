using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class MeterInfo
    {
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String communicateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String barCode { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterTypeNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String factoryNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? openState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String caliber { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? baseVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? buildTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? editTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Boolean? isConcentrate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String collectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Volatility { get; set; }

}
}
