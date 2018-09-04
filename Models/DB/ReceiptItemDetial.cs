using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ReceiptItemDetial
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
        public String items { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Unit { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Quantity { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? subTotal { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }

}
}
