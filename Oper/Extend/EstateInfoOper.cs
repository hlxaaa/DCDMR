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
    public partial class EstateInfoOper : SingleTon<EstateInfoOper>
    {
        /// <summary>
        /// ��ȡС���б�
        /// </summary>
        /// <returns></returns>
        public List<EstateInfo> GetList()
        {
            return SqlHelper.Instance.GetByCondition<EstateInfo>(" 1=1 ");
        }
    }
}
