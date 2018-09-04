using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class SecondAuthRecord
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Detial { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String AuthTo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Auther { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? Authtime { get; set; }

}
}
