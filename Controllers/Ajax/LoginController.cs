using Common.Filter.Mvc;
using Common.Helper;
using Common.Result;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using Newtonsoft.Json;
using System.Web.Mvc;


namespace HHTDCDMR.Controllers.Ajax
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [MvcValidate]
        //[MvcException]
        public string Login(UserLoginReq req)
        {
            ResultJson r = new ResultJson
            {
                HttpCode = 200,
                Message = "登录成功"
            };
            var account = req.account;
            var pwd = req.pwd;
            var user = UserInfoOper.Instance.GetByAccount(account);
            if (user == null)
            {
                r.HttpCode = 500;
                r.Message = "没有这个账号";
            }
            else if (user.pwd != MD5Helper.Instance.StrToMD5_UTF8(pwd))
            {
                r.HttpCode = 500;
                r.Message = "密码错误";
            }
            else if (user.isStaff == false && (user.areaId == null || user.areaId == 0))
            {
                r.HttpCode = 500;
                r.Message = "请让管理员为此账号设定区域";
            }
            else
            {
                Session.Timeout = 60;
                Session["userId"] = user.id;
                Session["lv"] = user.level;
                Session["userName"] = user.name;
                Session["areaId"] = user.areaId;
                //var admin = AllInOne_UserInfoOper.Instance.GetAdminName();
                //user.admin = admin;
                Session["user"] = JsonConvert.SerializeObject(user);
                var listPIds = AllInOne_UserPermissionViewOper.Instance.GetPerIdsByUserId(user.id);
                Session["pIds"] = JsonConvert.SerializeObject(listPIds);
                //LogHelper.WriteLog(typeof(LoginController), $"{user.name} 登录系统");
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public string LogOut()
        {
            //LogHelper.WriteLog(typeof(LoginController), $"{Session["userName"]} 退出系统");
            ResultJson r = new ResultJson
            {
                HttpCode = 200
            };
            Session.Abandon();
            return JsonConvert.SerializeObject(r);
        }

    }
}