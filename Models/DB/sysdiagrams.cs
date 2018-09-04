using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class sysdiagrams
    {
        /// <summary>
        ///
        /// </summary>
        public String name { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32 principal_id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32 diagram_id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? version { get; set; }
        /// <summary>
        ///
        /// </summary>
        //public Byte[]? definition { get; set; }-txy

}
}
