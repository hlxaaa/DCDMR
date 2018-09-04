using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Page
{
    public class AlarmController : PageBaseController
    {
        /// <summary>
        /// 报警历史记录查看
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "报警信息"
            };
            var listNav = new List<nav>
            {
                new nav("报警信息", "heading", ""),
                new nav("历史记录", "selected", "/alarm/list"),
                //new nav("设备报警值设置", "", "/alarm/deviceConfig"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            ViewBag.UserLv = AllFunc.Instance.GetUserSelects(user);
            //ViewBag.devices = AllFunc.Instance.GetDeviceByUser(user);
            return View();
        }

        /// <summary>
        /// 设备报警值设置
        /// </summary>
        /// <returns></returns>
        public ActionResult DeviceConfig()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "报警信息"
            };
            var listNav = new List<nav>
            {
                new nav("报警信息", "heading", ""),
                new nav("设备报警值设置", "selected", ""),
                new nav("历史记录", "", "/alarm/list"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            var meterNo = Convert.ToInt32(Request["meterNo"]);
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            ViewBag.device = AllFunc.Instance.GetDeviceByNo(meterNo, user);

            return View();
        }

        /// <summary>
        /// 具体报警信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AlarmInfo()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "报警信息"
            };
            var listNav = new List<nav>
            {
                new nav("报警信息", "heading", ""),
                new nav("报警时间轴", "selected", ""),
                new nav("历史记录", "", "/alarm/list"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var meterNo = Convert.ToInt32(Request["meterNo"]);
            var list = AllFunc.Instance.GetAlarmByMeterNo(meterNo,user);
            ViewBag.list = list;
            return View();
        }
    }
}