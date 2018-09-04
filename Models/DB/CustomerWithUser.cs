using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class CustomerWithUser
    {
        public CustomerWithUser(){}
        //public CustomerWithUser(Req req){
            //if(req.customerNo != null)
            //customerNo = req.customerNo;
            //if(req.customerType != null)
            //customerType = Convert.ToInt32(req.customerType);
            //if(req.contractNo != null)
            //contractNo = req.contractNo;
            //if(req.customerName != null)
            //customerName = req.customerName;
            //if(req.telNo != null)
            //telNo = req.telNo;
            //if(req.mobileNo != null)
            //mobileNo = req.mobileNo;
            //if(req.certNo != null)
            //certNo = req.certNo;
            //if(req.estateNo != null)
            //estateNo = req.estateNo;
            //if(req.estateName != null)
            //estateName = req.estateName;
            //if(req.address != null)
            //address = req.address;
            //if(req.houseNo != null)
            //houseNo = req.houseNo;
            //if(req.cellNo != null)
            //cellNo = req.cellNo;
            //if(req.roomNo != null)
            //roomNo = req.roomNo;
            //if(req.useState != null)
            //useState = Convert.ToInt32(req.useState);
            //if(req.defineNo1 != null)
            //defineNo1 = req.defineNo1;
            //if(req.defineNo2 != null)
            //defineNo2 = req.defineNo2;
            //if(req.defineNo3 != null)
            //defineNo3 = req.defineNo3;
            //if(req.remark != null)
            //remark = req.remark;
            //if(req.payWay != null)
            //payWay = Convert.ToInt32(req.payWay);
            //if(req.bankNo != null)
            //bankNo = req.bankNo;
            //if(req.bankAuthNo != null)
            //bankAuthNo = req.bankAuthNo;
            //if(req.accountName != null)
            //accountName = req.accountName;
            //if(req.accountNo != null)
            //accountNo = req.accountNo;
            //if(req.bankCheck != null)
            //bankCheck = Convert.ToInt32(req.bankCheck);
            //if(req.buildTime != null)
            //buildTime = Convert.ToDateTime(req.buildTime);
            //if(req.editTime != null)
            //editTime = Convert.ToDateTime(req.editTime);
            //if(req.Operator != null)
            //Operator = req.Operator;
            //if(req.taxNo != null)
            //taxNo = req.taxNo;
            //if(req.enterpriseNo != null)
            //enterpriseNo = req.enterpriseNo;
            //if(req.CustTypeName != null)
            //CustTypeName = req.CustTypeName;
            //if(req.useStateName != null)
            //useStateName = req.useStateName;
            //if(req.payWayName != null)
            //payWayName = req.payWayName;
            //if(req.bankCheckName != null)
            //bankCheckName = req.bankCheckName;
            //if(req.enterpriseNoName != null)
            //enterpriseNoName = req.enterpriseNoName;
            //if(req.bankName != null)
            //bankName = req.bankName;
            //if(req.factoryNo != null)
            //factoryNo = req.factoryNo;
            //if(req.meterTypeNo != null)
            //meterTypeNo = req.meterTypeNo;
            //if(req.factoryName != null)
            //factoryName = req.factoryName;
            //if(req.meterTypeName != null)
            //meterTypeName = req.meterTypeName;
            //if(req.userId != null)
            //userId = Convert.ToInt32(req.userId);
            //if(req.name != null)
            //name = req.name;
            //if(req.LEVEL != null)
            //LEVEL = Convert.ToInt32(req.LEVEL);
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
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? customerType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String contractNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String telNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String mobileNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String certNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String estateNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String estateName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String address { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String houseNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String cellNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String roomNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? useState { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo1 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo2 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String defineNo3 { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String remark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? payWay { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankAuthNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String accountName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String accountNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? bankCheck { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? buildTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? editTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String taxNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String enterpriseNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CustTypeName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String useStateName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payWayName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankCheckName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String enterpriseNoName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String bankName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String factoryNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterTypeNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String factoryName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterTypeName { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? userId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String name { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? LEVEL { get; set; }
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
