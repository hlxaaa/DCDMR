using System.Text;
using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_AreaInfoOper : SingleTon<AllInOne_AreaInfoOper>
    {
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<AllInOne_AreaInfo> GetList(AreaReq req, int size, int userId, int lv)
        {
            var search = req.search ?? "";
            var order = req.orderField;
            var desc = Convert.ToBoolean(req.isDesc);
            var index = Convert.ToInt32(req.pageIndex);
            //var size = 5;
            var orderStr = $"order by {order} ";
            if (desc)
                orderStr += " desc ";
            else
                orderStr += " asc ";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };

            var condition = " isDeleted=0  ";
            if (lv != 100)
                condition += $" and createUserId={userId}";
            if (!search.IsNullOrEmpty())
                condition += " and name like @search ";
            return SqlHelper.Instance.GetViewPaging<AllInOne_AreaInfo>("AllInOne_AreaInfo", "select * from AllInOne_AreaInfo ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// ��ȡ�����б�����
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(AreaReq req, int userId, int lv)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };

            var condition = " isDeleted=0  ";
            if (lv != 100)
                condition += $" and createUserId={userId}";
            if (!search.IsNullOrEmpty())
                condition += " and name like @search ";
            var list = SqlHelper.Instance.GetDistinctCount<AllInOne_AreaInfo>("AllInOne_AreaInfo", condition, dict);
            return list.Count;
        }

        /// <summary>
        /// ��ȡ�����ֵ�,δʹ�õ�����
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAreaDict()
        {
            var list = SqlHelper.Instance.GetByCondition<AllInOne_AreaInfo>(" id not in  (select areaId as id from AllInOne_UserInfo where areaId!=0 and isdeleted=0) and isDeleted=0 ");
            return list.ToDictionary(p => p.id, p => p.name);
        }

        /// <summary>
        /// ��ȡ����û������������б�
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetAreaDictByUserId(int userId)
        {
            var list = SqlHelper.Instance.GetByCondition<AllInOne_AreaInfo>($" id not in  (select areaId as id from AllInOne_UserInfo where areaId!=0 and isdeleted=0) and isDeleted=0 and createUserId={userId} ");
            return list.ToDictionary(p => p.id, p => p.name);
        }

        /// <summary>
        /// ������id��ȡ������Ϣ
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public AllInOne_AreaInfo GetById(int areaId)
        {
            return SqlHelper.Instance.GetById<AllInOne_AreaInfo>("AllInOne_AreaInfo", areaId);
        }

    }
}
