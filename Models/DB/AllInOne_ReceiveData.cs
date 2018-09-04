using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_ReceiveData
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CommNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MSTAISEQ { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String MSTA { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ISEQ { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String FSEQ { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String ctrlCode { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String sysType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String devType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dataCmd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dataArea { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ReceiveTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? dealFlag { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? dealTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String dealMsg { get; set; }

}
}
