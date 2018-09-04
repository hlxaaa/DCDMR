using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_ChargeCheckRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CommunicateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CollectorNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operater { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String LastChargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String LastChargeCmd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? LastChargeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ChargeTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CumulantChargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MessageState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ErrorReason { get; set; }

}
}
