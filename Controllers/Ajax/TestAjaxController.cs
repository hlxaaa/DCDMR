using Common.Helper;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using HHTDCDMR4._5.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Ajax
{
    public class TestAjaxController : Controller
    {
        // GET: TestAjax
        public ActionResult Index()
        {
            return View();
        }

        public void AddNewOnlineDevice(AllReq req)
        {

        }

        public void ErrorToLog()
        {
            var a = 1;
            var c = a / 0;
        }

        public void testManyList(ManyList req)
        {

        }

        public string CheckNewAlarm2(CheckAlarmReq req)
        {
            //var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var user = AllInOne_UserInfoOper.Instance.GetById(1);
            //oldCount = oldCount ?? 0;
            var r = AllFunc.Instance.CheckNewAlarm(user, Convert.ToInt32(req.oldCount));
            return JsonConvert.SerializeObject(r);
        }


        public object Test()
        {
            return 1;
            return CacheFunc.Instance.GetWxToken();
            //var dc = new DevId_Content
            //{
            //    devid = "1",
            //    content = "警报"
            //};
            //AllFunc.Instance.SendWxMsg(dc);
            //return 1;
        }
        public void AddUp()
        {
            var list = SqlHelper.Instance.GetByCondition<AllInOne_UserInfo>(" isdeleted=0 ");
            var userIds = list.Select(p => p.id).ToList();
            var list2 = new List<AllInOne_UserPermission>();
            foreach (var item in userIds)
            {
                list2.Add(new AllInOne_UserPermission { userId = item, perId = 1, isOpen = true });
                list2.Add(new AllInOne_UserPermission { userId = item, perId = 2, isOpen = true });
                list2.Add(new AllInOne_UserPermission { userId = item, perId = 13, isOpen = true });
            }
            foreach (var item in list2)
            {
                AllInOne_UserPermissionOper.Instance.Add(item);
            }
        }


        //public object Test4851()
        //{

        //}


    }
}