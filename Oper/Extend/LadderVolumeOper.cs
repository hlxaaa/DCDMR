using System;
using Common;
using DbOpertion.Models;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class LadderVolumeOper : SingleTon<LadderVolumeOper>
    {
        /// <summary>
        /// 创建用户阶梯用量,用的时候只用最后一个，所以加了没用也没关系。不用事务
        /// </summary>
        /// <returns></returns>
        public bool AddLv(string fluidNo, CustomerInfo customer, SqlConnection conn, SqlTransaction tran)
        {
            var customerNo = customer.customerNo;
            var customerName = customer.customerName;
            var fi = FluidInfo0301Oper.Instance.GetByFluidNo(fluidNo);
            if (fi == null)
                return false;
            LadderVolume lv = new LadderVolume
            {
                customerNo = customerNo,
                customerName = customerName,
                startTime = fi.startTime,
                endTime = Convert.ToDateTime(fi.startTime).AddYears(1)
            };
            var list = FluidInfo0301Oper.Instance.GetStepVolumes(fi);
            lv.StepVolume1 = list[0];
            lv.StepVolume2 = list[1];
            lv.StepVolume3 = list[2];
            lv.StepVolume4 = list[3];
            lv.StepVolume5 = list[4];
            Add(lv, conn, tran);
            return true;
        }
    }
}
