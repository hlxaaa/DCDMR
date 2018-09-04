using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class RemoteTask
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? taskTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String communicateNo { get; set; }
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
        public Int32? taskType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? chargeVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? totalVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? chargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? totalMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? chargeTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ReceiptNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? chargeType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? valveCtrl { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? endAmount1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? endAmount2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? endAmount3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? price1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeBranchNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargePosNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? taskTimeout { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? taskState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String DestRtuNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FeedBackMsg { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String BeginDates { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String EndDate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String GetDates { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String NoDate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String HourDate { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MinuteDate { get; set; }

}
}
