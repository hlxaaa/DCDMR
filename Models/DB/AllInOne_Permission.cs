using System;

namespace DbOpertion.Models
{
    [Serializable]
    public class AllInOne_Permission
    {
        public AllInOne_Permission(){}
        //public AllInOne_Permission(Req req){
            //if(req.id != null)
            //id = Convert.ToInt32(req.id);
            //if(req.name != null)
            //name = req.name;
            //if(req.type != null)
            //type = Convert.ToInt32(req.type);
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String name { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? type { get; set; }

}
}
