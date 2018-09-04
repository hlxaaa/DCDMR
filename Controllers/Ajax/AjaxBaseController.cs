using Common.Filter.Mvc;
using Common.Result;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Ajax
{
    [MvcException]
    public class AjaxBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!checkLogin())// 判断是否登录
            {
                ResultForWeb r = new ResultForWeb();
                r.HttpCode = 300;
                r.Message = "";
                r.data = "{}";
                JsonResult jr = new JsonResult();
                jr.ContentType = "application/json";
                jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                jr.Data = r;
                filterContext.Result = jr;
            }
        }

        /// <summary>
        /// 判断是否登录
        /// </summary>
        public bool checkLogin()
        {
            //return true;
            var a = Session["userId"];
            if (a == null)
            {
                return false;
            }
            return true;
        }
    }
}