using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class AllInOne_AreaInfo
    {
        public AllInOne_AreaInfo(){}
        //public AllInOne_AreaInfo(Req req){
            //if(req.id != null)
            //id = Convert.ToInt32(req.id);
            //if(req.name != null)
            //name = req.name;
            //if(req.isDeleted != null)
            //isDeleted = Convert.ToBoolean(req.isDeleted);
            //if(req.lat != null)
            //lat = req.lat;
            //if(req.lng != null)
            //lng = req.lng;
            //if(req.mapAddress != null)
            //mapAddress = req.mapAddress;
            //if(req.createUserId != null)
            //createUserId = Convert.ToInt32(req.createUserId);
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
        public Boolean? isDeleted { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String lat { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String lng { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String mapAddress { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? createUserId { get; set; }

}
}
