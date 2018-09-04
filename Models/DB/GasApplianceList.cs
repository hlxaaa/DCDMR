using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class GasApplianceList
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
        public String devBrand { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String devModel { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String devSource { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }

}
}
