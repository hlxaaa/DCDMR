using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ICChargeRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? chargeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
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
        public String fluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? chargeVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? chargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? totalVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? totalMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? chargeTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? chargeType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeBranchNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargePosNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ICWriteMark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ReceiptNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeCheck { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? cycleVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? cycleMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payId { get; set; }

}
}
