using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class SafeCheckItemRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? recordID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ItemNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Barcode { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkContent { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String checkResult { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        //public Byte[]? ImageData { get; set; }-txy
        /// <summary>
        ///
        /// </summary>
        public String checkResultType { get; set; }

}
}
