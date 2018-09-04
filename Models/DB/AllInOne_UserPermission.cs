using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_UserPermission
    {
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? userId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? perId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Boolean? isOpen { get; set; }

}
}
