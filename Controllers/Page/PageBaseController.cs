using Common.Filter.Mvc;
using Common.Helper;
using Common.Result;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Page
{
    //[MvcException]
    public class PageBaseController : Controller
    {
        protected string hostUrl = "";
        /// <summary>
        /// Action执行前判断
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            hostUrl = "http://" + Request.Url.Host;
            hostUrl += Request.Url.Port.ToString() == "80" ? "" : ":" + this.Request.Url.Port;
            hostUrl += Request.ApplicationPath;
            if (!checkLogin())// 判断是否登录
            {
                filterContext.Result = View("Login");
            }
            else
            {
                //通用变量
                var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
                ViewBag.user = user;
                ViewBag.AlarmCounts = AllFunc.Instance.GetAlarmCount(user);
                ViewBag.lastAlarmId = AllFunc.Instance.GetLastAlarmId(user);
            }
            base.OnActionExecuting(filterContext);

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

        /// <summary>
        /// page错误，这里拦截
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            //JsonResult jsonResult = new JsonResult();

            LogHelper.WriteLog(typeof(MvcExceptionAttribute), filterContext.Exception);

            // 此处进行异常记录，可以记录到数据库或文本，也可以使用其他日志记录组件。
            // 通过filterContext.Exception来获取这个异常。
            filterContext.ExceptionHandled = true;//组织web.config配置customerror处理
            string requestType = filterContext.HttpContext.Request.RequestType.ToString();//获取请求类型
            UrlHelper url = new UrlHelper(filterContext.RequestContext);

            //判断是否为get请求，如果为get请求，跳转指定页面，如果不是返回json

            var seewhat = ConfigurationManager.AppSettings.Get("seewhat");
            if (seewhat != "error")
                filterContext.Result = new RedirectResult(url.Action("error", "Main"));//跳转到新页面
            else
            {
                JsonResult jsonResult = new JsonResult();
                ResultForWeb result = new ResultForWeb();
                result.HttpCode = 500;
                result.Message = filterContext.Exception.Message;
                LogHelper.WriteLog(typeof(MvcExceptionAttribute), filterContext.Exception);
                jsonResult.Data = result;
                jsonResult.ContentType = "application/json";
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = jsonResult;
                filterContext.ExceptionHandled = true;
            }
        }
    }
}