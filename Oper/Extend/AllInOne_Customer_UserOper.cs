using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_Customer_UserOper : SingleTon<AllInOne_Customer_UserOper>
    {
        /// <summary>
        /// 登录账户和新增的客户建立联系
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="user"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void AddCU(CustomerInfo customer, AllInOne_UserInfo user, SqlConnection conn=null, SqlTransaction tran=null)
        {
            AllInOne_Customer_User acu = new AllInOne_Customer_User();
            acu.customerId = customer.customerNo;
            acu.userId = user.id;
            Add(acu, conn, tran);
        }
    }
}
