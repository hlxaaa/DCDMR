using System;
using Common;
using DbOpertion.Models;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class OpenAccountRecordOper : SingleTon<OpenAccountRecordOper>
    {
        /// <summary>
        /// 新增开户记录
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="device"></param>
        /// <param name="recordId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool AddOpenAccountRecord(CustomerInfo customer, DeviceInfo device, out int recordId, SqlConnection conn, SqlTransaction tran)
        {
            recordId = 0;
            OpenAccountRecord model = new OpenAccountRecord
            {
                customerNo = customer.customerNo,
                customerType = customer.customerType,
                customerName = customer.customerName,
                estateNo = customer.estateNo,
                address = customer.address,

                meterNo = device.meterNo.ToString(),
                meterTypeNo = device.meterTypeNo,
                factoryNo = device.factoryNo,

                Opentime = DateTime.Now,
                OpenType = 0
            };

            recordId = Add(model, conn, tran);
            return true;
        }
    }
}
