using HHTDCDMR.Models.Extend.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Page
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "配置"
            };
            var listNav = new List<nav>
            {
                new nav("配置管理", "heading", ""),
                new nav("区域管理", "selected", "/config"),
                new nav("设备类型管理", "", "/config/metertype"),
                //new nav("员工账号", "", "/user/staff"),
                //new nav("账号信息", "", "/user/userDetail"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            return View();
        }

        public ActionResult OrderList()
        {
            return View();
        }

        public ActionResult OrderDetail() {
            return View();
        }

        public ActionResult UserDetail() {
            return View();
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}