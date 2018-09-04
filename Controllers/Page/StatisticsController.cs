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

namespace HHTDCDMR.Controllers.Page
{
    public class StatisticsController : PageBaseController
    {
        // 统计
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChargeReport()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "数据分析"
            };
            var listNav = new List<nav>
            {
                new nav("统计报表", "heading", ""),
                new nav("充值统计", "selected", "/statistics/chargeReport"),
                   new nav("用量统计", "", "/statistics/StdSumReport"),
            };
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            //ViewBag.custList = AllFunc.Instance.GetCustomerListByUser(user);
            lay.listNav = listNav;
            ViewBag.lay = lay;

           
            ViewBag.UserLv = AllFunc.Instance.GetUserSelects(user);

            return View();
        }

        public ActionResult StdSumReport()
        {
            LayoutIndex lay = new LayoutIndex
            {
                activeName = "数据分析"
            };
            var listNav = new List<nav>
            {
                new nav("统计报表", "heading", ""),
                new nav("用量统计", "selected", "/statistics/StdSumReport"),
                new nav("充值统计", "", "/statistics/chargeReport"),
            };
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            ViewBag.UserLv = AllFunc.Instance.GetUserSelects(user);

            //ViewBag.custList = AllFunc.Instance.GetCustomerListByUser(user);
            lay.listNav = listNav;
            ViewBag.lay = lay;
            return View();
        }

        public ActionResult PrintPage()
        {
            var printType = Request["printType"].ToString();
            switch (printType)
            {
                case "charge":
                    var req = JsonConvert.DeserializeObject<GetChargeReportReq>(Session["gcrReq"].ToString());
                    switch (req.type)
                    {
                        case "year":
                            ViewBag.printTitle = "充值记录年报表";
                            break;
                        case "month":
                            ViewBag.printTitle = "充值记录月报表";
                            break;
                        case "day":
                            ViewBag.printTitle = "充值记录日报表";
                            break;
                        default:
                            break;
                    }
                    //ViewBag.arr1 = new List<string> { "时间", "充值气量", "充值金额" };
                    ViewBag.arr1 = new List<string> { "时间", "充值气量" };
                    ViewBag.arr2 = AllFunc.Instance.GetChargeForPrint(req);
                    break;
                case "StdSum":
                    var req2 = JsonConvert.DeserializeObject<GetStdSumReq>(Session["gssReq"].ToString());
                    switch (req2.type)
                    {
                        case "year":
                            ViewBag.printTitle = "用气记录年报表";
                            break;
                        case "month":
                            ViewBag.printTitle = "用气记录月报表";
                            break;
                        case "day":
                            ViewBag.printTitle = "用气记录日报表";
                            break;
                        default:
                            break;
                    }
                    ViewBag.arr1 = new List<string> { "时间", "使用气量" };
                    ViewBag.arr2 = AllFunc.Instance.GetStdSumForPrint(req2);
                    break;
                default:
                    break;
            }
            return View();
        }
    }
}