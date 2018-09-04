using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class Col_RemoteCharge
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
        public String MessageState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ChargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? BuyTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ChargeContent { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ReOverZeroMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ReSurplusMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ReChargeTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ErrorReason { get; set; }

}
}
