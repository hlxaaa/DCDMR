using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Page
{
    public class ConfigController : PageBaseController
    {
        public ActionResult FactoryList()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "配置"
            };
            var listNav = new List<nav>
            {
                new nav("配置管理", "heading", ""),
                new nav("厂家管理", "selected", "/config/FactoryList"),
                new nav("区域管理", "", "/config/index")
            };
            if (user.level == 100)
            {
                //listNav.Add(new nav("设备类型管理", "", "/config/metertype"));
                //listNav.Add(new nav("协议管理", "", "/config/protocol"));
                listNav.Add(new nav("参数设置", "", "/config/config"));
            }



            lay.listNav = listNav;
            ViewBag.lay = lay;
            return View();

            //return View();
        }

        /// <summary>
        /// 区域列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "配置"
            };
            var listNav = new List<nav>
            {
                new nav("配置管理", "heading", ""),
                new nav("区域管理", "selected", "/config/index")
            };
            if (user.level == 100)
            {
                //listNav.Add(new nav("设备类型管理", "", "/config/metertype"));
                //listNav.Add(new nav("协议管理", "", "/config/protocol"));
                listNav.Add(new nav("厂家管理", "", "/config/FactoryList"));
                listNav.Add(new nav("参数设置", "", "/config/config"));
            }

            lay.listNav = listNav;
            ViewBag.lay = lay;
            return View();
        }

        /// <summary>
        /// 配置文件参数设置视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "配置"
            };
            var listNav = new List<nav>
            {
                new nav("配置管理", "heading", ""),
                new nav("参数设置", "selected", "/config/config"),
                new nav("厂家管理", "", "/config/FactoryList"),
                //new nav("协议管理", "", "/config/protocol"),
                new nav("区域管理", "", "/config/index"),
                //new nav("设备类型管理", "", "/config/metertype"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;

            ViewBag.config = new Config2();


            return View();
        }

        /// <summary>
        /// 设备坐标设置视图
        /// </summary>
        /// <returns></returns>
        public ActionResult DeviceLatlng()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "基础信息"
            };
            var listNav = new List<nav>
            {
                new nav("信息管理", "heading", "/device/index"),
                new nav("坐标设置", "selected", ""),
                new nav("用户和表", "", "/device/index"),
                new nav("建点", "", "/device/Establish"),
                //new nav("子账号管理", "selected", "/user"),
                //new nav("员工账号", "", "/user/staff"),
                //new nav("账号信息", "", "/user/userDetail"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            if (Request["meterNo"] != null)
            {
                var meterNo = Convert.ToInt32(Request["meterNo"]);
                var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
                ViewBag.device = AllFunc.Instance.GetDeviceByNo(meterNo, user);
                ViewBag.devices = AllFunc.Instance.GetDeviceByUser(user);
            }
            else
            {
                //ViewBag.area = new AllInOne_AreaInfo();
            }

            return View();
        }


        /// <summary>
        /// 设备类型列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MeterType()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "配置"
            };
            var listNav = new List<nav>
            {
                new nav("配置管理", "heading", ""),
                new nav("设备类型管理", "selected", "/config/metertype"),
                new nav("区域管理", "", "/config/index"),
                 new nav("协议管理", "", "/config/protocol"),
                 new nav("参数设置", "", "/config/config"),
                //new nav("账号信息", "", "/user/userDetail"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            return View();
        }

        /// <summary>
        /// 协议管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Protocol()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "配置"
            };
            var listNav = new List<nav>
            {
                new nav("配置管理", "heading", ""),
                   new nav("协议管理", "selected", "/config/protocol"),
                new nav("区域管理", "", "/config/index"),
                new nav("设备类型管理", "", "/config/metertype"),
                 new nav("参数设置", "", "/config/config"),

            };
            lay.listNav = listNav;
            ViewBag.lay = lay;


            return View();
        }
    }
}