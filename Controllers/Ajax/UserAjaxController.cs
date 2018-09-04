using Common.Filter.Mvc;
using Common.Result;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Ajax
{
    public class UserAjaxController : AjaxBaseController
    {
        /// <summary>
        /// 获取子账号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetList(UserGetReq req)
        {
            var lv = Convert.ToInt32(Session["lv"]);
            var id = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.GetSonUser(id, req, lv);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetStaffList(UserGetReq req)
        {
            var lv = Convert.ToInt32(Session["lv"]);
            var id = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.GetStaff(id, req, lv);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取账号个人信息
        /// </summary>
        /// <returns></returns>
        public string GetUserInfo()
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.GetUserInfo(userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新主账号信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string UpdateUserInfo(UpdateUserSelfReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.UpdateUserInfo(req, userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 修改子账号、员工
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string UpdateSonInfo(UpdateUserReq req)
        {
            var id = Convert.ToInt32(Session["userId"]);
            var userId = Convert.ToInt32(req.userId);
            var r = AllFunc.Instance.UpdateSonInfo(req, userId, id);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 删除员工信息（软）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string DeleteSonInfo(string userId)
        {
            var id = Convert.ToInt32(Session["userId"]);
            var uId = Convert.ToInt32(userId);
            var r = AllFunc.Instance.DeleteSonInfo(uId, id);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 由账号id获取账号信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetUserInfoById(UserIdReq req)
        {
            var userId = Convert.ToInt32(req.userId);
            var r = AllFunc.Instance.GetUserInfo(userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string AddStaff(AddStaffReq req)
        {
            //var userId = Convert.ToInt32(Session["userId"]);
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.AddStaff(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 添加子账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string AddSon(AddSonReq req)
        {
            //var userId = Convert.ToInt32(Session["userId"]);
            //var lv = Convert.ToInt32(Session["lv"]);
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());

            var r = AllFunc.Instance.AddSon(req, user);
            return JsonConvert.SerializeObject(r);
        }

        public string UpdateFactoryName(UpdateFactoryNameReq req)
        {
            var r = new ResultJson("更新成功");
            UserInfoOper.Instance.UpdateFactoryName(req.name);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新员工某个权限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string UpdatePermission(UpdatePermissionReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            //var lv = Convert.ToInt32(Session["lv"]);
            var r = AllFunc.Instance.UpdatePermission(req, userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 判断用户是否具有某项操作的权限
        /// </summary>
        /// <param name="pId">操作的perId</param>
        /// <returns></returns>
        public string IsOpenPermission(int pId)
        {
            var pIds = JsonConvert.DeserializeObject<List<int>>(Session["pIds"].ToString());
            ResultJson r = new ResultJson();
            if (pIds.Contains(pId))
            {
                r.HttpCode = 200;
            }
            else
            {
                r.HttpCode = 500;
                r.Message = "您没有权限操作";
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 由名称获取子账号列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetSonSelects(string name)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetSonByName(name, user);
            return JsonConvert.SerializeObject(r);
        }

    }
}