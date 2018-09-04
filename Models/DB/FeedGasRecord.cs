using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class FeedGasRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? dispatchID { get; set; }
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
        public DateTime? planTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Contact1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String telNo1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Contact2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String telNo2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterFactory { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterModel { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String flowRange { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String inletPressure { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String outletPressure { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String swapConcentration { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String planUsage { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterPressure { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String testPressure { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? testLengthofTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String testEffect { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String testOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String testVerifier { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? regTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }

}
}
