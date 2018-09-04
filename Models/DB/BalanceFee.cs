using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class BalanceFee
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String accPeriod { get; set; }
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
        public Decimal? lastRead { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentRead { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? currentVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? unitPrice { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? payableAmount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? payableDate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? lateFeeEnable { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lateRate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? bonusDays { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? calcType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? limitType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? ratio { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? defLimit { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? lateDays { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? lateFee { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? actualAmount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? payState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? payWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String copyWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? withholdingState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String balanceOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? balanceTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? payTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payBranchNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payPosNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ReceiptNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? copytime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String copyMan { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? copyId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payId { get; set; }

}
}
