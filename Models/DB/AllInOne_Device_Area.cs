using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class AllInOne_Device_Area
    {
        public AllInOne_Device_Area(){}
        //public AllInOne_Device_Area(Req req){
            //if(req.id != null)
            //id = Convert.ToInt32(req.id);
            //if(req.deviceId != null)
            //deviceId = Convert.ToInt32(req.deviceId);
            //if(req.areaId != null)
            //areaId = Convert.ToInt32(req.areaId);
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? deviceId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? areaId { get; set; }

}
}
