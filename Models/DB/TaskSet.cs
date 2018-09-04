using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class TaskSet
    {
        /// <summary>
        ///
        /// </summary>
        public String TaskNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taskTypeNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taskContent { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String skillNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? skillSn { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Remark { get; set; }

}
}
