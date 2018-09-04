using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;

namespace DbOpertion.DBoperation
{
    public partial class FluidInfo0301Oper : SingleTon<FluidInfo0301Oper>
    {
        /// <summary>
        /// 根据价格类型获取价格类型的详细记录
        /// </summary>
        /// <param name="fluidNo"></param>
        /// <returns></returns>
        public FluidInfo0301 GetByFluidNo(string fluidNo)
        {
            var list = SqlHelper.Instance.GetByCondition<FluidInfo0301>($" fluidno={fluidNo} ");
            return list.FirstOrDefault();
        }

        /// <summary>
        /// 获取价格类型的五阶梯
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public List<decimal> GetStepVolumes(FluidInfo0301 fi)
        {
            var r = new List<decimal>
            {
                (decimal)fi.endAmount1,
                (decimal)(fi.endAmount2 - fi.endAmount1),
                (decimal)(fi.endAmount3 - fi.endAmount2),
                (decimal)(fi.endAmount4 - fi.endAmount3),
                (decimal)(fi.endAmount5 - fi.endAmount4)
            };
            return r;
        }

        /// <summary>
        /// 获取所有的价格类型
        /// </summary>
        /// <returns></returns>
        public List<FluidInfo0301> GetList()
        {
            return SqlHelper.Instance.GetByCondition<FluidInfo0301>(" 1=1 ");
        }
    }
}
