using Common.Filter.Mvc;
using Common.Helper;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTDCDMR.Controllers.Ajax
{
    [MvcValidate]
    public class AlarmAjaxController : AjaxBaseController
    {
        /// <summary>
        /// 更新设备报警配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string UpdateDeviceAlarmConfig(UpdateDeviceAlarmConfigReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.UpdateDeviceAlarmConfig(req, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取报警信息列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetList(AlarmListReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.GetAlarmList(req, user);
            return JsonConvert.SerializeObject(r);
        }

        public string DealAlarm(AlarmIdReq req)
        {
            var id = Convert.ToInt32(req.id);
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.DealAlarm(id, user);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 根据之前最后一个警报id来判断的,用这个了
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string CheckNewAlarm3(CheckAlarmReq req)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            var r = AllFunc.Instance.CheckNewAlarm2(user, (int)req.oldLastId);
            return JsonConvert.SerializeObject(r);
        }



        public string CheckNewAlarm(int oldCount)
        {
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            //oldCount = oldCount ?? 0;
            var r = AllFunc.Instance.CheckNewAlarm(user, oldCount);
            return JsonConvert.SerializeObject(r);
        }

        public string CheckNewAlarm2(CheckAlarmReq req)
        {
            //LogHelper.WriteLog(typeof(AlarmAjaxController), "CheckNewAlarm2 In！");
            var user = JsonConvert.DeserializeObject<AllInOne_UserInfo>(Session["user"].ToString());
            //oldCount = oldCount ?? 0;
            var r = AllFunc.Instance.CheckNewAlarm(user, Convert.ToInt32(req.oldCount));
            //LogHelper.WriteLog(typeof(AlarmAjaxController), "CheckNewAlarm2 Out！");
            return JsonConvert.SerializeObject(r);
        }

     
    }
}