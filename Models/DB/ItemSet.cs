using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ItemSet
    {
        /// <summary>
        ///
        /// </summary>
        public String ItemNo { get; set; }
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
        public Decimal? Price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }

}
}
