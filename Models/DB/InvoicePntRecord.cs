using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class InvoicePntRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ReceiptNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String InvCode { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String InvNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? InvDate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? InvMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? IsCancel { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String IndustryCode { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Volume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String custType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? isRed { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taxNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankAccountNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankAccountName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? PntTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String PntBranchNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String PntPosNo { get; set; }

}
}
