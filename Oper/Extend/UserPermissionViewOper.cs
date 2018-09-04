using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;

namespace DbOpertion.DBoperation
{
    public partial class UserPermissionViewOper : SingleTon<UserPermissionViewOper>
    {
        /// <summary>
        /// 获取子账号，非员工
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetSon(int parentId, UserGetReq req, int size)
        {
            var search = req.search ?? "";
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
            var condition = " isStaff=0 and isdeleted=0 and parentId=" + parentId;
            if (!search.IsNullOrEmpty())
                condition += " and (name like @search ) ";

            return SqlHelper.Instance.GetViewPaging<AllInOne_UserPermissionView>("AllInOne_UserPermissionView", "select * from AllInOne_UserPermissionView ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// 获取子账号，非员工
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetStaff(int parentId, UserGetReq req, int size)
        {
            var search = req.search ?? "";
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
            var condition = "  isStaff=1 and isdeleted=0 and parentId=" + parentId;
            if (!search.IsNullOrEmpty())
                condition += " and (name like @search or pername like @search) ";

            return SqlHelper.Instance.GetViewCommon<AllInOne_UserPermissionView>("id", "AllInOne_UserPermissionView", condition, index, size, orderStr, order, dict);
        }

        /// <summary>
        /// 获取子账号的总数
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetSonCount(int parentId, UserGetReq req)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };
            var condition = " isStaff=0 and isdeleted=0 and parentId=" + parentId;
            if (!search.IsNullOrEmpty())
                condition += " and (name like @search ) ";

            var list = SqlHelper.Instance.GetDistinctCount<AllInOne_UserPermissionView>("AllInOne_UserPermissionView", condition, dict);
            return list.Count();
        }

        /// <summary>
        /// 获取员工账号的总数
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetStaffCount(int parentId, UserGetReq req)
        {
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" }
            };
            var condition = " isStaff=1 and isdeleted=0 and parentId=" + parentId;
            if (!search.IsNullOrEmpty())
                condition += " and (name like @search or pername like @search) ";

            var list = SqlHelper.Instance.GetDistinctCount<AllInOne_UserPermissionView>("AllInOne_UserPermissionView", condition, dict);
            return list.Count();
        }

        /// <summary>
        /// 获取某个用户的权限视图
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetByUserId(int userId)
        {
            return SqlHelper.Instance.GetByCondition<AllInOne_UserPermissionView>(" id = " + userId);
        }

        /// <summary>
        /// 获取某个用户的权限视图
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetByUserIdAndParentId(int userId, int parentId)
        {
            return SqlHelper.Instance.GetByCondition<AllInOne_UserPermissionView>($" id ={userId} and parentId={parentId}");
        }

        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserIndexRes GetListUIR(int userId)
        {
            var r = new UserIndexRes();
            var list = GetByUserId(userId);
            if (list.Count == 0)
                r = null;
            else
                r = new UserIndexRes(list);
            return r;
        }

        /// <summary>
        /// 需要验证是不是自己的子账号或者子员工
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserIndexRes GetUIRValidate(int userId, int parentId)
        {
            var r = new UserIndexRes();
            var list = GetByUserIdAndParentId(userId, parentId);
            if (list.Count == 0)
                r = null;
            else
                r = new UserIndexRes(list);
            return r;
        }

        public bool IsSendMailOpen(int devid)
        {
            var str = $@"select * from AllInOne_UserPermission where userId in(
select userId from DeviceView where meterNo = {devid}) and perId = 13";
            var list = SqlHelper.Instance.ExecuteGetDt<AllInOne_UserPermission>(str, new Dictionary<string, string>());
            if (list.Count == 0)
                return false;
            if ((bool)list.First().isOpen)
                return true;
            return false;
        }

    }
}
