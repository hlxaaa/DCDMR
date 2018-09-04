using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Page
{
    public class UserController : PageBaseController
    {
        // GET: User
        /// <summary>
        /// 子账号管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "账号管理"
            };
            var listNav = new List<nav>
            {
                new nav("账号管理", "heading", "/user/userDetail"),
                             new nav("子账号管理", "selected", "/user/index"),
                new nav("个人信息", "", "/user/userDetail"),

                new nav("员工账号", "", "/user/staff")
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            ViewBag.addName = "添加子账号";

            var lv = Convert.ToInt32(Session["lv"]);
            var userId = Convert.ToInt32(Session["userId"]);
            ViewBag.areaDict = AllFunc.Instance.GetAreaList(userId, lv);
            return View();
        }

        /// <summary>
        /// 账号个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDetail(string userId)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "账号管理"
            };
            var listNav = new List<nav>
            {
                new nav("账号管理", "heading", "/user/userDetail"),
                new nav("个人信息", "selected", "/user/userDetail"),
            };
            if (user.level > 98)
                listNav.Add(new nav("子账号管理", "", "/user/index"));
            if (user.level > 97)
                listNav.Add(new nav("员工账号", "", "/user/staff"));

            lay.listNav = listNav;
            ViewBag.lay = lay;
            //ViewBag.areaDict = AllFunc.Instance.GetAreaList();
            var id = Convert.ToInt32(Session["userId"]);
            if (userId == null)
            {
                ViewBag.userId = 0;
                var uir = UserPermissionViewOper.Instance.GetListUIR(id);
                ViewBag.uir = uir;
            }
            else
            {
                ViewBag.userId = userId;
                var uir = UserPermissionViewOper.Instance.GetUIRValidate(Convert.ToInt32(userId), id);
                ViewBag.uir = uir;
            }

            return View();
        }

        /// <summary>
        /// 员工账号管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Staff()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "账号管理"
            };
            var listNav = new List<nav>
            {
                new nav("账号管理", "heading", "/user/userDetail"),
                new nav("员工账号", "selected", "/user/staff"),
                new nav("个人信息", "", "/user/userDetail"),
            };
            if (user.level > 98)
                listNav.Add(new nav("子账号管理", "", "/user/index"));


            lay.listNav = listNav;
            ViewBag.lay = lay;
            ViewBag.addName = "添加新员工";
            return View();
        }

        /// <summary>
        /// 员工权限管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Permission()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "账号管理"
            };
            //var listNav = new List<nav>
            //{
            //    new nav("账号管理", "heading", "/user/userDetail"),
            //    new nav("员工账号", "selected", "/user/staff"),
            //    new nav("子账号管理", "", "/user/index"),
            //    new nav("个人信息", "", "/user/userDetail"),
            //};
            var listNav = new List<nav>
            {
                new nav("账号管理", "heading", "/user/userDetail"),
                new nav("个人信息", "selected", "/user/userDetail"),
            };
            if (user.level > 98)
                listNav.Add(new nav("子账号管理", "", "/user/index"));
            if (user.level > 97)
                listNav.Add(new nav("员工账号", "", "/user/staff"));

            lay.listNav = listNav;
            ViewBag.lay = lay;
            ViewBag.addName = "添加新员工";
            var parentId = Convert.ToInt32(Session["userId"]);
            var id = Convert.ToInt32(Request["staffid"]);
            ViewBag.list = AllFunc.Instance.GetPermissionByUserId(id, parentId);

            return View();
        }

        public ActionResult GuidePage()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "基础信息"
            };
            var listNav = new List<nav>
            {
                new nav("信息管理", "heading", "/device/index"),
                new nav("建点", "selected", "/device/Establish"),
                new nav("设备信息", "", "/device/index"),
                new nav("客户信息", "", "/device/customerList"),
            };


            ViewBag.cno = Request["customerNo"] ?? "0";

            lay.listNav = listNav;
            ViewBag.lay = lay;
            return View();
        }
    }
}