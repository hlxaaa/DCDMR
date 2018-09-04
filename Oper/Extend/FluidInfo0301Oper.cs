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
        /// ���ݼ۸����ͻ�ȡ�۸����͵���ϸ��¼
        /// </summary>
        /// <param name="fluidNo"></param>
        /// <returns></returns>
        public FluidInfo0301 GetByFluidNo(string fluidNo)
        {
            var list = SqlHelper.Instance.GetByCondition<FluidInfo0301>($" fluidno={fluidNo} ");
            return list.FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ�۸����͵������
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
        /// ��ȡ���еļ۸�����
        /// </summary>
        /// <returns></returns>
        public List<FluidInfo0301> GetList()
        {
            return SqlHelper.Instance.GetByCondition<FluidInfo0301>(" 1=1 ");
        }
    }
}
