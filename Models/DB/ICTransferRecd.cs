using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class ICTransferRecd
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CustIDFrom { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterIDFrom { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FluidIDFrom { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CustIDTo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MeterIDTo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FluidIDTo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? TranMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String UserID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String NodeID { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String UserName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? TranTime { get; set; }

}
}
