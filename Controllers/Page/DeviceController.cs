using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Linq;

namespace HHTDCDMR.Controllers.Page
{
    public class DeviceController : PageBaseController
    {
        // GET: Device
        /// <summary>
        /// 设备列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "基础信息"
            };
            var listNav = new List<nav>
            {
                new nav("信息管理", "heading", "/device/index"),
                new nav("设备信息", "selected", "/device/index"),
                new nav("客户信息", "", "/device/customerList"),
                new nav("建点", "", "/device/Establish"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            ViewBag.operatorList = AllInOne_UserInfoOper.Instance.GetMyOpertor(user);
            ViewBag.fluidList = AllFunc.Instance.GetFluidList();
            ViewBag.UserLv = AllFunc.Instance.GetUserSelects(user);
            //ViewBag.pIds = JsonConvert.DeserializeObject<List<int>>(Session["pIds"].ToString());



            return View();
        }

        /// <summary>
        /// 客户列表视图
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerList()
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "基础信息"
            };
            var listNav = new List<nav>
            {
                new nav("信息管理", "heading", "/device/index"),
                new nav("客户信息", "selected", "/device/customerList"),
                new nav("设备信息", "", "/device/index"),
                new nav("建点", "", "/device/Establish"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            ViewBag.UserLv = AllFunc.Instance.GetUserSelects(user);
            ViewBag.estateList = AllFunc.Instance.GetEstateList();
            ViewBag.operatorList = AllInOne_UserInfoOper.Instance.GetMyOpertor(user);
            //ViewBag.pIds = JsonConvert.DeserializeObject<List<int>>(Session["pIds"].ToString());
            return View();
        }

        /// <summary>
        /// 表具信息，更新或新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Device()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "基础信息"
            };
            var listNav = new List<nav>
            {
                new nav("信息管理", "heading", "/device/index"),
                new nav("设备信息", "selected", "/device/index"),
                new nav("建点", "", "/device/Establish"),
                new nav("客户信息", "", "/device/customerList"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            var no = -1;

            var deviceView = new DeviceView { meterNo = 0 };

            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());

            var sons = AllInOne_UserInfoOper.Instance.GetSonAndGrandSon(user);
            //sons.Add(user);
            sons = sons.OrderByDescending(p => p.level).ToList();
            ViewBag.sons = sons;
            ViewBag.meterTypeList = MeterTypeOper.Instance.GetAllList();
            //更新
            if (Request["no"] != null)
            {
                no = Convert.ToInt32(Request["no"]);

                //ViewBag.device = AllFunc.Instance.GetDeviceByNo(no, user);
                deviceView = DeviceInfoOper.Instance.GetViewByNo(no, user);
            }
            //新建
            else
            {
                ViewBag.newId = AllFunc.Instance.GetNewDeviceId();
            }

            //var sons = AllInOne_UserInfoOper.Instance.GetSonAndGrandSon(user);
            //sons.Add(user);
            //sons = sons.OrderByDescending(p => p.level).ToList();
            //ViewBag.sons = sons;

            ViewBag.device = deviceView;

            //ViewBag.pIds = JsonConvert.DeserializeObject<List<int>>(Session["pIds"].ToString());
            ViewBag.no = no;
            ViewBag.caliberList = AllFunc.Instance.GetCaliber();
            ViewBag.fluidNoList = AllFunc.Instance.GetFluidList();

            var fn = new Factory_No();


            var server = ConfigurationManager.AppSettings.Get("server");
            ViewBag.server = server ?? "";
            //if (server == "gt")
            //{
            //    fn.name = "杭州鸿鹄";
            //    fn.no = "03";
            //}
            //else {
            //    fn.name = "上海信东";
            //    fn.no = "08";
            //}
            //ViewBag.fn = fn;
            return View();
        }

        /// <summary>
        /// 客户信息，更新或新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Customer()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "基础信息"
            };
            //ViewBag.pIds = JsonConvert.DeserializeObject<List<int>>(Session["pIds"].ToString());
            var listNav = new List<nav>
            {
                 new nav("信息管理", "heading", "/device/index"),
                   new nav("客户信息", "selected", "/device/customerList"),
                 new nav("设备信息", "", "/device/index"),
                new nav("建点", "", "/device/Establish"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            if (Request["no"] != null)
            {
                var no = Request["no"].ToString();
                var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
                //ViewBag.customer = AllFunc.Instance.GetCustomerByNo(no, user);
                ViewBag.customer = AllFunc.Instance.GetCustomerViewByNo(no, user);
                ViewBag.isAdd = 0;
            }
            else
            {
                ViewBag.customerNo = AllFunc.Instance.GetNewCustomerNo();
                ViewBag.isAdd = 1;
            }
            return View();
        }

        /// <summary>
        /// 建点。建客户、建表、绑定、开户一体
        /// </summary>
        /// <returns></returns>
        public ActionResult Establish()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "基础信息"
            };
            var listNav = new List<nav>
            {
                new nav("信息管理", "heading", "/device/index"),
                new nav("建点", "selected", "/device/Establish"),
                  new nav("客户信息", "", "/device/customerList"),
                new nav("设备信息", "", "/device/index"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            ViewBag.caliberList = AllFunc.Instance.GetCaliber();
            ViewBag.fluidNoList = AllFunc.Instance.GetFluidList();
            ViewBag.meterTypeList = MeterTypeOper.Instance.GetAllList();
            ViewBag.factoryList = FactoryTypeOper.Instance.GetAllList();

            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var sons = AllInOne_UserInfoOper.Instance.GetSonAndGrandSon(user);
            //sons.Add(user);
            sons = sons.OrderByDescending(p => p.level).ToList();
            ViewBag.sons = sons;

            var server = ConfigurationManager.AppSettings.Get("server");
            ViewBag.server = server ?? "";

            return View();
        }

    }
}