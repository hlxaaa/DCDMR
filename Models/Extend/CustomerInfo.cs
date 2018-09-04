using HHTDCDMR.Models.Extend.Req;
using System;

namespace DbOpertion.Models
{
    public partial class CustomerInfo
    {
        public CustomerInfo(EstablishReq req)
        {
            if (req.customerNo != null)
                customerNo = req.customerNo;
            if (req.customerName != null)
                customerName = req.customerName;
            if (req.mobileNo != null)
                mobileNo = req.mobileNo;
            if (req.address != null)
                address = req.address;
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            if (req.remark != null)
                remark = req.remark;
        }

        public CustomerInfo(CustomerUpdateReq req)
        {
            if (req.customerNo != null)
                customerNo = req.customerNo;
            if (req.customerType != null)
                customerType = Convert.ToInt32(req.customerType);
            if (req.contractNo != null)
                contractNo = req.contractNo;
            if (req.customerName != null)
                customerName = req.customerName;
            if (req.telNo != null)
                telNo = req.telNo;
            if (req.mobileNo != null)
                mobileNo = req.mobileNo;
            if (req.certNo != null)
                certNo = req.certNo;
            if (req.estateNo != null)
                estateNo = req.estateNo;
            if (req.address != null)
                address = req.address;
            if (req.houseNo != null)
                houseNo = req.houseNo;
            if (req.cellNo != null)
                cellNo = req.cellNo;
            if (req.roomNo != null)
                roomNo = req.roomNo;
            if (req.useState != null)
                useState = Convert.ToInt32(req.useState);
            if (req.defineNo1 != null)
                defineNo1 = req.defineNo1;
            if (req.defineNo2 != null)
                defineNo2 = req.defineNo2;
            if (req.defineNo3 != null)
                defineNo3 = req.defineNo3;
            if (req.remark != null)
                remark = req.remark;
            if (req.payWay != null)
                payWay = Convert.ToInt32(req.payWay);
            if (req.bankNo != null)
                bankNo = req.bankNo;
            if (req.bankAuthNo != null)
                bankAuthNo = req.bankAuthNo;
            if (req.accountNo != null)
                accountNo = req.accountNo;
            if (req.accountName != null)
                accountName = req.accountName;
            if (req.bankCheck != null)
                bankCheck = Convert.ToInt32(req.bankCheck);
            if (req.buildTime != null)
                buildTime = Convert.ToDateTime(req.buildTime);
            if (req.editTime != null)
                editTime = Convert.ToDateTime(req.editTime);
            if (req.Operator != null)
                Operator = req.Operator;
            if (req.loginName != null)
                loginName = req.loginName;
            if (req.Password != null)
                Password = req.Password;
            if (req.taxNo != null)
                taxNo = req.taxNo;
            if (req.enterpriseNo != null)
                enterpriseNo = req.enterpriseNo;
        }
    }
}
