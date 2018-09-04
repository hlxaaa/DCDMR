using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_CopyDataICRF
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
        public String CollectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ReadState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? OcrRead { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? SurplusMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? CurrentMonthTotal { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Last1MonthTotal { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Last2MonthTotal { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Last3MonthTotal { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ReadTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevPower { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DevVersion { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? SurplusCumulant { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? CurrentPrice { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? CumulantUseMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? CumulantChargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String PriceVersion { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? OverZeroMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? BuyTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String StateMsg { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String SendDevState { get; set; }

}
}
