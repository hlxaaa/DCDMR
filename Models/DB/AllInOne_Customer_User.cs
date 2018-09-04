using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class AllInOne_Customer_User
    {
        public AllInOne_Customer_User(){}
        //public AllInOne_Customer_User(Req req){
            //if(req.id != null)
            //id = Convert.ToInt32(req.id);
            //if(req.customerId != null)
            //customerId = req.customerId;
            //if(req.userId != null)
            //userId = Convert.ToInt32(req.userId);
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? userId { get; set; }

}
}
