using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class AllInOne_OperateRecordView
    {
        public AllInOne_OperateRecordView() { }
        //public AllInOne_OperateRecordView(Req req){
        //if(req.id != null)
        //id = Convert.ToInt32(req.id);
        //if(req.content != null)
        //content = req.content;
        //if(req.operatorId != null)
        //operatorId = req.operatorId;
        //if(req.autherId != null)
        //autherId = req.autherId;
        //if(req.operateTime != null)
        //operateTime = Convert.ToDateTime(req.operateTime);
        //if(req.operatorName != null)
        //operatorName = req.operatorName;
        //if(req.name != null)
        //name = req.name;
        //if(req.level != null)
        //level = Convert.ToInt32(req.level);
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
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String content { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String operatorId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String autherId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? operateTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String operatorName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String name { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? level { get; set; }
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

    }
}
