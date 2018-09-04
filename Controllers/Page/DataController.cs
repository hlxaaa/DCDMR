using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Page
{
    public class DataController : PageBaseController
    {
        // GET: Data
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 实时数据
        /// </summary>
        /// <returns></returns>
        public ActionResult MeterData()
        {

            LayoutIndex lay = new LayoutIndex
            {
                activeName = "数据查询"
            };
            var listNav = new List<nav>
            {
                new nav("数据查询", "heading", ""),
                new nav("实时数据", "selected", "/data/meterData"),
                new nav("地图监控", "", "/data/deviceOnMap"),
                new nav("历史数据", "", "/data/hisData"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            var config = AllFunc.Instance.GetConfig();
            ViewBag.time = config.FLMeterDataRefreshRate;
            ViewBag.pIds = JsonConvert.DeserializeObject<List<int>>(Session["pIds"].ToString());
            var server = "";
            server = ConfigurationManager.AppSettings.Get("server");
            ViewBag.server = server ?? "";

            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            ViewBag.UserLv = AllFunc.Instance.GetUserSelects(user);

            return View();
        }

        /// <summary>
        /// 历史数据
        /// </summary>
        /// <returns></returns>
        public ActionResult HisData()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "数据查询"
            };
            var listNav = new List<nav>
            {
                new nav("数据查询", "heading", ""),
                new nav("历史数据", "selected", "/data/hisData"),
                new nav("实时数据", "", "/data/meterData"),
                new nav("地图监控", "", "/data/deviceOnMap"),
            };
            var cno = "0";
            if (Request["cno"] != null)
                cno = Request["cno"];
            ViewBag.cno = cno;
            lay.listNav = listNav;
            ViewBag.lay = lay;
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            //ViewBag.devices = AllFunc.Instance.GetDeviceByUser(user);

            ViewBag.UserLv = AllFunc.Instance.GetUserSelects(user);

            return View();
        }

        /// <summary>
        /// 地图定位
        /// </summary>
        /// <returns></returns>
        public ActionResult DeviceOnMap()
        {
            var config = AllFunc.Instance.GetConfig();
            ViewBag.time = config.FLMeterDataRefreshRate;
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "数据查询"
            };
            var listNav = new List<nav>
            {
                new nav("数据查询", "heading", ""),
                new nav("地图监控", "selected", "/data/deviceOnMap"),
                new nav("实时数据", "", "/data/meterData"),
                new nav("历史数据", "", "/data/hisData"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            ViewBag.fluidList = AllFunc.Instance.GetFluidList();
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var list = AllFunc.Instance.GetSon(user);
            var ids = list.Select(p => p.id).Distinct().ToList();
            var sons = new List<UserId_areaName>();
            foreach (var item in ids)
            {
                var temp = list.Where(p => p.id == item).ToList();
                UserId_areaName u = new UserId_areaName(temp);
                sons.Add(u);
            }

            ViewBag.sons = sons;
            return View();
        }

    }
}