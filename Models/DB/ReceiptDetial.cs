using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ReceiptDetial
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
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? customerType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Estate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String address { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String custDefNo1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String custDefNo2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String custDefNo3 { get; set; }
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
        public String caliber { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterDefNo1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterDefNo2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterDefNo3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payContent { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? payWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? PayMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? payLateMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? surplus { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }
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
        public Int32? payType { get; set; }

}
}
