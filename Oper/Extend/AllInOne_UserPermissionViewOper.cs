using System.Text;
using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_UserPermissionViewOper : SingleTon<AllInOne_UserPermissionViewOper>
    {
        /// <summary>
        /// ��ȡ���˺ŵ�Ȩ����ͼ
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetSon(int parentId, UserGetReq req, int size)
        {
            var search = req.search;
            var order = req.orderField;
            var desc = Convert.ToBoolean(req.isDesc);
            var index = Convert.ToInt32(req.pageIndex);

            var orderStr = $"order by {order} ";
            if (desc)
                orderStr += " desc ";
            else
                orderStr += " asc ";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };
            var condition = " isdeleted=0 and parentId=" + parentId;
            if (!search.IsNullOrEmpty())
                condition += " and (name like @search or pername like @search) ";

            return SqlHelper.Instance.GetViewPaging<AllInOne_UserPermissionView>("AllInOne_UserPermissionView", "select * from UserPermissionView ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// ��ȡ���˺ŵ�����
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetSonCount(int parentId, UserGetReq req)
        {
            var search = req.search;
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };
            var condition = " isdeleted=0 and parentId=" + parentId;
            if (!search.IsNullOrEmpty())
                condition += " and (name like @search or pername like @search) ";

            var list = SqlHelper.Instance.GetDistinctCount<AllInOne_UserPermissionView>("AllInOne_UserPermissionView", condition, dict);
            return list.Count();
        }

        /// <summary>
        /// ��ȡĳ���û���Ȩ����ͼ
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetByUserId(int userId)
        {
            return SqlHelper.Instance.GetByCondition<AllInOne_UserPermissionView>("  id = " + userId);
        }

        /// <summary>
        /// ��ȡĳ���û������ŵ�Ȩ��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetPerIdsByUserId(int userId)
        {
            var list = SqlHelper.Instance.GetByCondition<AllInOne_UserPermissionView>(" isOpen=1 and  id = " + userId);
            return list.Select(p => (int)p.perId).ToList();
        }

        /// <summary>
        /// ��ȡĳ��Ȩ�޵���ͼ
        /// </summary>
        /// <param name="perId"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetByUPId(int upId)
        {
            return SqlHelper.Instance.GetByCondition<AllInOne_UserPermissionView>("  upId = " + upId);
        }

        /// <summary>
        /// �����û�ʱ�����û�Ȩ�ޱ���������еģ�����Ϊδ����
        /// </summary>
        /// <param name="userId"></param>
        public void AddUpWithUserId(int userId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            string str = "select id from allinone_permission ";
            var list2 = SqlHelper.Instance.ExecuteGetDt<AllInOne_Permission>(str, new Dictionary<string, string>());
            var pIds = list2.Select(p => p.id).Distinct().ToList();
            foreach (var item2 in pIds)
            {
                str += $" insert into allinone_userpermission (userid,perid) values ({userId},{item2}) ";
            }
            SqlHelper.Instance.ExcuteNon(str, new Dictionary<string, string>(), conn, tran);
        }

        /// <summary>
        /// ���������Ȩ�ޣ�ÿ��id��Ҫ�����������Ӷ�Ӧ�ġ���δ��֮ǰ������֮��Ͳ�����(Ȩ�޴�ž������е���Щ�ˣ����ᶯ̬����ʲôȨ��
        /// </summary>
        public void Complete()
        {
            string str = "select id from allinone_userInfo where isstaff=1 and isdeleted=0";

            var list1 = SqlHelper.Instance.ExecuteGetDt<AllInOne_UserInfo>(str, new Dictionary<string, string>());
            str = "select id from allinone_permission";
            var list2 = SqlHelper.Instance.ExecuteGetDt<AllInOne_Permission>(str, new Dictionary<string, string>());
            var userIds = list1.Select(p => p.id).Distinct().ToList();
            var pIds = list2.Select(p => p.id).Distinct().ToList();
            str = "truncate table AllInOne_UserPermission ";
            foreach (var item in userIds)
            {
                foreach (var item2 in pIds)
                {
                    str += $" insert into allinone_userpermission (userid,perid) values ({item},{item2}) ";
                }
            }

            SqlHelper.Instance.ExcuteNon(str);

        }


    }
}
