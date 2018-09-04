using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class AllInOne_UserInfo
    {
        public AllInOne_UserInfo(){}
        //public AllInOne_UserInfo(Req req){
            //if(req.id != null)
            //id = Convert.ToInt32(req.id);
            //if(req.name != null)
            //name = req.name;
            //if(req.account != null)
            //account = req.account;
            //if(req.pwd != null)
            //pwd = req.pwd;
            //if(req.level != null)
            //level = Convert.ToInt32(req.level);
            //if(req.jurisdiction != null)
            //jurisdiction = req.jurisdiction;
            //if(req.isDeleted != null)
            //isDeleted = Convert.ToBoolean(req.isDeleted);
            //if(req.parentId != null)
            //parentId = Convert.ToInt32(req.parentId);
            //if(req.isStaff != null)
            //isStaff = Convert.ToBoolean(req.isStaff);
            //if(req.areaId != null)
            //areaId = Convert.ToInt32(req.areaId);
            //if(req.cId1 != null)
            //cId1 = req.cId1;
            //if(req.cId2 != null)
            //cId2 = req.cId2;
            //if(req.cId3 != null)
            //cId3 = req.cId3;
            //if(req.cId4 != null)
            //cId4 = req.cId4;
            //if(req.phone != null)
            //phone = req.phone;
            //if(req.openIds != null)
            //openIds = req.openIds;
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
        public String account { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String pwd { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? level { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String jurisdiction { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Boolean? isDeleted { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? parentId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Boolean? isStaff { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? areaId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cId4 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String phone { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String openIds { get; set; }

}
}
