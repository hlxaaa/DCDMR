using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ItemList
    {
        /// <summary>
        ///
        /// </summary>
        public String BillID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ItemNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32 BillType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ItemName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ItemModel { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Unit { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Amount { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Money { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }

}
}
