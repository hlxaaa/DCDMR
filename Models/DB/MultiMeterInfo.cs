using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class MultiMeterInfo
    {
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ModbusAddr { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? baseVolume_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? baseVolume_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? baseVolume_G { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidNo_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidNo_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidNo_G { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String barCode_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String BarCode_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String barCode_G { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String communicateNo_W { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String communicateNo_E { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String communicateNo_G { get; set; }

}
}
