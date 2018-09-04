using HHTDCDMR.Models.Extend.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Page
{
    public class RecordController : PageBaseController
    {
        // GET: Record
        public ActionResult Index()
        {

            return View();
        }
        /// <summary>
        /// 操作记录视图
        /// </summary>
        /// <returns></returns>
        public ActionResult OperRecord()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "操作记录"
            };
            var listNav = new List<nav>
            {
                new nav("操作记录", "heading", "/record/operRecord"),
                //new nav("区域管理", "selected", "/config"),
                //new nav("设备类型管理", "", "/config/metertype"),
            };
            lay.listNav = listNav;
            ViewBag.lay = lay;
            return View();
        }

    }
}