using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class CopyDataICRF
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
        public Decimal? Cumulant { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? SurplusMoney { get; set; }
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
        public Int32? OverFlowTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? MagAttTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? CardAttTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? MeterState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String StateMessage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? CurrMonthTotal { get; set; }
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
        public Decimal? accPrice { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? sumUseMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? sumRechargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? weekUseVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String VersionNumber { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ChangeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? ChangePrice { get; set; }

}
}
