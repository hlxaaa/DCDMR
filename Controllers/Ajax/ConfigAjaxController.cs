using Common.Filter.Mvc;
using Common.Helper;
using Common.Result;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;
using HHTDCDMR.Oper.Function;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Web.Configuration;

namespace HHTDCDMR.Controllers.Ajax
{
    public class ConfigAjaxController : AjaxBaseController
    {

        public string DeleteFactory(DeleteFactoryReq req)
        {
            FactoryTypeOper.Instance.DeleteByNo(req.factoryNo);
            var r = new ResultJson("删除成功");
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetAreaList(AreaReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var lv = Convert.ToInt32(Session["lv"]);
            var r = AllFunc.Instance.GetAreaList(req, userId, lv);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 新增区域
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string AddArea(AreaAddReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.AddArea(req, userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新区域
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string UpdateArea(AreaUpdateReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.UpdateArea(req, userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string DeleteArea(AreaDelReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.DeleteArea(req, userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取设备类型列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetMeterTypeList(MeterTypeReq req)
        {
            var r = AllFunc.Instance.GetMeterTypeList(req);
            return JsonConvert.SerializeObject(r);
        }

        [MvcValidate]
        public string GetFactoryList(GetFactoryListReq req)
        {
            var r = AllFunc.Instance.GetFactoryList(req);
            return JsonConvert.SerializeObject(r);
        }

        [MvcValidate]
        public string AddFactory(AddFactoryReq req)
        {
            var f = new FactoryType();
            f.factoryNo = req.factoryNo;
            f.factoryName = req.factoryName;
            f.MarkCode = req.MarkCode;

            if (FactoryTypeOper.Instance.IsNoExist(f))
                throw new Exception("编号已存在");
            if (FactoryTypeOper.Instance.IsNameExist(f))
                throw new Exception("名称已存在");
            if (FactoryTypeOper.Instance.IsCodeExist(f))
                throw new Exception("代码已存在");

            FactoryTypeOper.Instance.Add2(f);
            var r = new ResultJson("添加成功");
            return JsonConvert.SerializeObject(r);
        }

        [MvcValidate]
        public string UpdateFactory(UpdateFactoryReq req)
        {
            var f = new FactoryType();
            f.factoryNo = req.factoryNo;
            f.factoryName = req.factoryName;
            f.MarkCode = req.MarkCode;

            if (FactoryTypeOper.Instance.IsNameExist2(f))
                throw new Exception("名称已存在");
            if (FactoryTypeOper.Instance.IsCodeExist2(f))
                throw new Exception("代码已存在");

            FactoryTypeOper.Instance.Update(f);
            var r = new ResultJson("更新成功");
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取协议列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [MvcValidate]
        public string GetProtocolList(ProtocolListReq req)
        {
            var r = AllFunc.Instance.GetProtocolList(req);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 新增协议
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string AddProtocol(ProtocolAddUpdateReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.AddProtocol(req, userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新所有参数，修改了配置文件，需要重新登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string UpdateAllConfig(Config2 req)
        {
            ResultJson r = new ResultJson
            {
                HttpCode = 200,
                Message = "更新成功,请重新登录"
            };
            Configuration cfa = WebConfigurationManager.OpenWebConfiguration("~");

            cfa.AppSettings.Settings["FLMeterDataRefreshRate"].Value = req.FLMeterDataRefreshRate;
            cfa.Save();
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 新增设备类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string AddMeterType(MeterTypeAddReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.AddMeterType(req, userId);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 更新设备类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string UpdateMeterType(MeterTypeUpdateReq req)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            var r = AllFunc.Instance.UpdateMeterType(req, userId);
            return JsonConvert.SerializeObject(r);
        }
    }
}