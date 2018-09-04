using System;

namespace DbOpertion.Models
{
    [Serializable]
    public partial class ICChargeRecordCustTypeView
    {
        public ICChargeRecordCustTypeView(){}
        //public ICChargeRecordCustTypeView(Req req){
            //if(req.Id != null)
            //Id = Convert.ToInt32(req.Id);
            //if(req.chargeTime != null)
            //chargeTime = Convert.ToDateTime(req.chargeTime);
            //if(req.customerNo != null)
            //customerNo = req.customerNo;
            //if(req.meterNo != null)
            //meterNo = req.meterNo;
            //if(req.meterTypeNo != null)
            //meterTypeNo = req.meterTypeNo;
            //if(req.factoryNo != null)
            //factoryNo = req.factoryNo;
            //if(req.fluidNo != null)
            //fluidNo = req.fluidNo;
            //if(req.Price != null)
            //Price = Convert.ToDecimal(req.Price);
            //if(req.chargeVolume != null)
            //chargeVolume = Convert.ToDecimal(req.chargeVolume);
            //if(req.chargeMoney != null)
            //chargeMoney = Convert.ToDecimal(req.chargeMoney);
            //if(req.totalVolume != null)
            //totalVolume = Convert.ToDecimal(req.totalVolume);
            //if(req.totalMoney != null)
            //totalMoney = Convert.ToDecimal(req.totalMoney);
            //if(req.chargeTimes != null)
            //chargeTimes = Convert.ToInt32(req.chargeTimes);
            //if(req.chargeType != null)
            //chargeType = Convert.ToInt32(req.chargeType);
            //if(req.chargeBranchNo != null)
            //chargeBranchNo = req.chargeBranchNo;
            //if(req.chargePosNo != null)
            //chargePosNo = req.chargePosNo;
            //if(req.chargeOperator != null)
            //chargeOperator = req.chargeOperator;
            //if(req.ICWriteMark != null)
            //ICWriteMark = Convert.ToInt32(req.ICWriteMark);
            //if(req.ReceiptNo != null)
            //ReceiptNo = Convert.ToInt32(req.ReceiptNo);
            //if(req.chargeCheck != null)
            //chargeCheck = req.chargeCheck;
            //if(req.cycleVolume != null)
            //cycleVolume = Convert.ToDecimal(req.cycleVolume);
            //if(req.cycleMoney != null)
            //cycleMoney = Convert.ToDecimal(req.cycleMoney);
            //if(req.payId != null)
            //payId = req.payId;
            //if(req.customerType != null)
            //customerType = Convert.ToInt32(req.customerType);
            //if(req.CustTypeName != null)
            //CustTypeName = req.CustTypeName;
        //}
        /// <summary>
        ///
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? chargeTime { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String customerNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String meterTypeNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String factoryNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String fluidNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? chargeVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? chargeMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? totalVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? totalMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? chargeTimes { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? chargeType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeBranchNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargePosNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeOperator { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ICWriteMark { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? ReceiptNo { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String chargeCheck { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? cycleVolume { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Decimal? cycleMoney { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String payId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Int32? customerType { get; set; }
        /// <summary>
        ///
        /// </summary>
        public String CustTypeName { get; set; }

}
}
