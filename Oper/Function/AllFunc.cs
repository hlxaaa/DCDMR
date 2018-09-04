using Common;
using Common.Result;
using Common.Extend;
using Common.Helper;
using DbOpertion.DBoperation;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;
using HHTDCDMR.Models.Extend.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using AliyunHelper.SendMail;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using HHTDCDMR4._5.Oper.Function;

namespace HHTDCDMR.Oper.Function
{
    public class AllFunc : SingleTon<AllFunc>
    {
        public string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

        /// <summary>
        /// 历史数据库的连接字符串
        /// </summary>
        public string connHisStr = ConfigurationManager.ConnectionStrings["ConnString2"].ConnectionString;

        #region ResultJson

        public ResultJson<GetFactoryListRes> GetFactoryList(GetFactoryListReq req)
        {
            var size = 10;
            var pages = 0;
            var res = new List<GetFactoryListRes>();
            var r = new ResultJson<GetFactoryListRes>
            {
                HttpCode = 200
            };
            var list2 = FactoryTypeOper.Instance.GetFactoryList(req, size);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                foreach (var item in list2)
                {
                    var temp = new GetFactoryListRes(item);
                    r.ListData.Add(temp);
                }

                var count = FactoryTypeOper.Instance.GetListCount(req);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;

                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// user获取子账号
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<UserIndexRes> GetSonUser(int parentId, UserGetReq req, int lv)
        {
            var size = 5;
            var pages = 0;
            var r = new ResultJson<UserIndexRes>();
            var res = new List<UserIndexRes>();
            if (lv == 97)
            {
                r.ListData = res;
            }
            else
            {
                var list2 = UserPermissionViewOper.Instance.GetSon(parentId, req, size);
                if (list2.Count == 0)
                    r.ListData = res;
                else
                {
                    var ids = list2.Select(p => p.id).Distinct().ToList();
                    foreach (var item in ids)
                    {
                        var temp = list2.Where(p => p.id == item).ToList();
                        UserIndexRes r2 = new UserIndexRes(temp);
                        res.Add(r2);
                    }
                    r.ListData = res;
                    var count = UserPermissionViewOper.Instance.GetSonCount(parentId, req);
                    pages = count / size;
                    //总页数
                    pages = pages * size == count ? pages : pages + 1;
                    r.pages = pages;
                    req.listSort = ClearListSorts(req.listSort);
                    var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                    r.sort = sort;
                    r.index = Convert.ToInt32(req.pageIndex);
                }
            }
            return r;
        }

        /// <summary>
        /// 获取员工账号
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="req"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public ResultJson<UserIndexRes> GetStaff(int parentId, UserGetReq req, int lv)
        {
            var size = 5;
            var pages = 0;
            var r = new ResultJson<UserIndexRes>();
            var res = new List<UserIndexRes>();
            if (lv == 97)
            {
                r.ListData = res;
            }
            else
            {
                var list2 = UserPermissionViewOper.Instance.GetStaff(parentId, req, size);
                if (list2.Count == 0)
                    r.ListData = res;
                else
                {
                    var ids = list2.Select(p => p.id).Distinct().ToList();
                    foreach (var item in ids)
                    {
                        var temp = list2.Where(p => p.id == item).ToList();
                        UserIndexRes r2 = new UserIndexRes(temp);
                        res.Add(r2);
                    }
                    r.ListData = res;
                    var count = UserPermissionViewOper.Instance.GetStaffCount(parentId, req);
                    pages = count / size;
                    //总页数
                    pages = pages * size == count ? pages : pages + 1;
                    r.pages = pages;
                    req.listSort = ClearListSorts(req.listSort);
                    var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                    r.sort = sort;
                    r.index = Convert.ToInt32(req.pageIndex);
                }
            }
            return r;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson<UserIndexRes> GetUserInfo(int userId)
        {
            var r = new ResultJson<UserIndexRes>();
            var uir = UserPermissionViewOper.Instance.GetListUIR(userId);
            if (uir != null)
                r.ListData.Add(uir);
            return r;
        }

        /// <summary>
        /// 主账号更新自己的信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson UpdateUserInfo(UpdateUserSelfReq req, int userId)
        {
            var r = new ResultJson();
            var name = req.name;
            var account = req.account;
            var pwd = req.pwd;

            if (SqlHelper.Instance.IsExists("AllInOne_UserInfo", "account", account, userId))
                throw new Exception("已存在该账号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "已存在该账号";
            //}
            else
            {
                AllInOne_UserInfo user = new AllInOne_UserInfo();
                user.id = userId;
                user.name = name;
                user.account = account;
                if (req.phone != null)
                    user.phone = req.phone;
                //if (req.areaId != null)
                //    user.areaId = Convert.ToInt32(req.areaId);
                if (!pwd.IsNullOrEmpty())
                    user.pwd = MD5Helper.Instance.StrToMD5_UTF8(pwd);
                AllInOne_UserInfoOper.Instance.Update(user);
            }
            return r;
        }

        /// <summary>
        /// 更新自己的子账号、员工信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ResultJson UpdateSonInfo(UpdateUserReq req, int userId, int parentId)
        {
            var r = new ResultJson
            {
                Message = "更新成功"
            };
            var name = req.name;
            var account = req.account;
            var pwd = req.pwd;
            if (SqlHelper.Instance.IsExists("AllInOne_UserInfo", "account", account, userId))
                throw new Exception("已存在该账号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "已存在该账号";
            //}
            else
            {
                var view = AllInOne_UserPermissionViewOper.Instance.GetByUserId(userId);
                if (view.Count == 0)
                    throw new Exception("不存在该账号");
                //{
                //    r.HttpCode = 500;
                //    r.Message = "不存在该账号";
                //}
                else if (view.First().parentId != parentId)
                    throw new Exception("没有权限修改该账号");
                //{
                //    r.HttpCode = 500;
                //    r.Message = "没有权限修改该账号";
                //}
                else
                {
                    AllInOne_UserInfo user = new AllInOne_UserInfo();
                    user.id = userId;
                    user.name = name;
                    user.account = account;
                    if (req.areaId != null)
                        user.areaId = Convert.ToInt32(req.areaId);
                    if (!pwd.IsNullOrEmpty())
                        user.pwd = MD5Helper.Instance.StrToMD5_UTF8(pwd);
                    var sourceAreaId = view.First().areaId;
                    SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction($"changeSonArea_{user.id}");
                    try
                    {
                        AllInOne_Device_AreaOper.Instance.UpdateAreaId((int)sourceAreaId, (int)user.areaId, conn, tran);
                        AllInOne_UserInfoOper.Instance.Update(user, conn, tran);

                        tran.Commit();
                        conn.Close();
                        AddOperateRecord("改变子账号区域", user.id);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        conn.Close();
                        r.HttpCode = 500;
                        r.Message = ex.Message;
                    }

                }
            }
            return r;
        }

        /// <summary>
        /// 删除子账号信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ResultJson DeleteSonInfo(int userId, int parentId)
        {
            var r = new ResultJson();

            var view = AllInOne_UserPermissionViewOper.Instance.GetByUserId(userId);
            if (view.Count == 0)
                throw new Exception("不存在该账号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "不存在该账号";
            //}
            else if (view.First().parentId != parentId)
                throw new Exception("没有权限修改该账号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "没有权限修改该账号";
            //}
            else
            {
                AllInOne_UserInfo user = new AllInOne_UserInfo();
                user.id = userId;
                user.isDeleted = true;
                AllInOne_UserInfoOper.Instance.Update(user);
            }
            return r;
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="req"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ResultJson AddStaff(AddStaffReq req, AllInOne_UserInfo pUser)
        {
            var r = new ResultJson
            {
                Message = "添加成功"
            };
            var account = req.account;
            if (SqlHelper.Instance.IsExists("AllInOne_UserInfo", "account", account, 0))
                throw new Exception("已存在该账号");
            else
            {
                AllInOne_UserInfo user = new AllInOne_UserInfo
                {
                    name = req.name,
                    account = account,
                    pwd = MD5Helper.Instance.StrToMD5_UTF8(req.pwd),
                    parentId = pUser.id,
                    level = 97,
                    isStaff = true
                };
                user = AllInOne_UserInfoOper.Instance.CompleteCIds(user, pUser);



                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction($"AddNewStaff_{user.id}");
                try
                {
                    var userId = AllInOne_UserInfoOper.Instance.Add(user, conn, tran);
                    AllInOne_UserPermissionViewOper.Instance.AddUpWithUserId(userId, conn, tran);
                    AddOperateRecord("新增员工", pUser.id, conn, tran);

                    tran.Commit();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    conn.Close();
                    r.HttpCode = 500;
                    r.Message = ex.Message;
                }

            }
            return r;
        }

        /// <summary>
        /// 添加子账号
        /// </summary>
        /// <param name="req"></param>
        /// <param name="parentId"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public ResultJson AddSon(AddSonReq req, AllInOne_UserInfo pUser)
        {
            var r = new ResultJson
            {
                Message = "添加成功"
            };
            var account = req.account;
            if (SqlHelper.Instance.IsExists("AllInOne_UserInfo", "account", account, 0))
                throw new Exception("已存在该账号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else if (SqlHelper.Instance.IsExists("AllInOne_UserInfo", "name", req.name, 0))
                throw new Exception("已存在该公司名称");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                AllInOne_UserInfo user = new AllInOne_UserInfo
                {
                    name = req.name,
                    account = account,
                    pwd = MD5Helper.Instance.StrToMD5_UTF8(req.pwd),
                    parentId = pUser.id,
                    level = pUser.level - 1,
                    isStaff = false,
                    areaId = Convert.ToInt32(req.areaId)
                };
                user = AllInOne_UserInfoOper.Instance.CompleteCIds(user, pUser);


                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction($"AddNewSon_{user.id}");
                try
                {
                    var userId = AllInOne_UserInfoOper.Instance.Add(user, conn, tran);
                    AllInOne_UserPermissionViewOper.Instance.AddUpWithUserId(userId, conn, tran);
                    AddOperateRecord("新增子账号", pUser.id, conn, tran);

                    tran.Commit();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    conn.Close();
                    r.HttpCode = 500;
                    r.Message = ex.Message;
                }

            }
            return r;
        }

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        public ResultJson<CustomerViewRes> GetCustomerList(CustomerReq req, AllInOne_UserInfo user)
        {
            var size = 20;
            var pages = 0;
            var res = new List<CustomerViewRes>();
            var r = new ResultJson<CustomerViewRes>();
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var list2 = CustomerInfoOper.Instance.GetList(req, size, lastCId);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                foreach (var item in list2)
                {
                    CustomerViewRes d = new CustomerViewRes(item, 1);
                    res.Add(d);
                }

                r.ListData = res;
                var count = CustomerInfoOper.Instance.GetCount(req, lastCId);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 获取报警记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<DeviceAlarmViewRes> GetAlarmList(AlarmListReq req, AllInOne_UserInfo user)
        {
            var size = 20;
            var pages = 0;
            var res = new List<DeviceAlarmViewRes>();
            var r = new ResultJson<DeviceAlarmViewRes>();
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var list2 = AllInOne_AlarmInfoOper.Instance.GetList(req, size, lastCId);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                foreach (var item in list2)
                {
                    DeviceAlarmViewRes davr = new DeviceAlarmViewRes(item);
                    res.Add(davr);
                }
                r.ListData = res;
                var count = AllInOne_AlarmInfoOper.Instance.GetCount(req, lastCId);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 将报警设为已处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson DealAlarm(int id, AllInOne_UserInfo user)
        {
            var r = new ResultJson
            {
                Message = "已处理该警报"
            };
            AllInOne_AlarmInfo m = new AllInOne_AlarmInfo
            {
                Id = id,
                DealFlag = 1,
                DealTime = DateTime.Now,
                DealOperator = user.name
            };
            AllInOne_AlarmInfoOper.Instance.Update(m);
            return r;
        }

        /// <summary>
        /// 获取实时抄表数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<OneFLMeterDataViewRes> GetMeterDataList(MeterDataListReq req, AllInOne_UserInfo user)
        {
            var size = 20;
            var pages = 0;
            var res = new List<OneFLMeterDataViewRes>();
            var r = new ResultJson<OneFLMeterDataViewRes>();
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var list2 = AllInOne_FLMeterDataOper.Instance.GetList(req, size, lastCId);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                foreach (var item in list2)
                {
                    OneFLMeterDataViewRes d = new OneFLMeterDataViewRes(item);
                    res.Add(d);
                }
                r.ListData = res;
                var count = AllInOne_FLMeterDataOper.Instance.GetCount(req, lastCId);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;

                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 开关阀（暂时不用
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson ChangeValve(ValveReq req, AllInOne_UserInfo user)
        {
            ResultJson r = new ResultJson();
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var rr = AllInOne_FLMeterDataOper.Instance.ChangeValve(req, lastCId);
            if (rr == 2)
                throw new Exception("您没有权限操作");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else if (rr == 1)
            {
                r.Message = "开阀成功";
                AddOperateRecord($"设备id{req.id}开阀", user.id);
            }
            else
            {
                r.Message = "关阀成功";
                AddOperateRecord($"设备id{req.id}关阀", user.id);
            }

            return r;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<DeviceViewRes> GetMeterList(MeterReq req, AllInOne_UserInfo user)
        {
            var size = 20;
            var pages = 0;
            var res = new List<DeviceViewRes>();
            var r = new ResultJson<DeviceViewRes>();
            //var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var list2 = DeviceInfoOper.Instance.GetList(req, size, user);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                foreach (var item in list2)
                {
                    DeviceViewRes d = new DeviceViewRes(item);
                    res.Add(d);
                }

                r.ListData = res;
                var count = DeviceInfoOper.Instance.GetCount(req, user);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        public ResultJson<AllInOne_AreaInfo> GetAreaList(AreaReq req, int userId, int lv)
        {
            var size = 5;
            var pages = 0;
            var res = new List<AllInOne_AreaInfo>();
            var r = new ResultJson<AllInOne_AreaInfo>();
            var list2 = AllInOne_AreaInfoOper.Instance.GetList(req, size, userId, lv);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                r.ListData = list2;
                var count = AllInOne_AreaInfoOper.Instance.GetCount(req, userId, lv);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 新增区域，属于自己的id下的区域
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson AddArea(AreaAddReq req, int userId)
        {
            var r = new ResultJson
            {
                Message = "已添加"
            };
            var name = req.name;
            if (SqlHelper.Instance.IsExists("AllInOne_AreaInfo", "name", name, 0))
                throw new Exception("已存在该区域");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                AllInOne_AreaInfo a = new AllInOne_AreaInfo
                {
                    name = name,
                    createUserId = userId
                };
                AllInOne_AreaInfoOper.Instance.Add(a);
                AddOperateRecord("添加区域", userId);
            }
            return r;
        }

        /// <summary>
        /// 更新区域
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson UpdateArea(AreaUpdateReq req, int userId)
        {
            var r = new ResultJson
            {
                Message = "已更新"
            };
            var name = req.name;
            var id = Convert.ToInt32(req.areaId);
            if (SqlHelper.Instance.IsExists("AllInOne_AreaInfo", "name", name, id))
                throw new Exception("存在该区域");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                AllInOne_AreaInfo a = new AllInOne_AreaInfo
                {
                    name = req.name,
                    id = id
                };
                AllInOne_AreaInfoOper.Instance.Update(a);
                AddOperateRecord("更新区域", userId);
            }
            return r;
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson DeleteArea(AreaDelReq req, int userId)
        {
            var r = new ResultJson
            {
                Message = "已删除"
            };
            var id = Convert.ToInt32(req.id);
            AllInOne_AreaInfo a = new AllInOne_AreaInfo();
            a.id = id;
            a.isDeleted = true;
            AllInOne_AreaInfoOper.Instance.Update(a);
            AddOperateRecord("删除区域", userId);
            return r;
        }

        /// <summary>
        /// 获取设备类型列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<MeterType> GetMeterTypeList(MeterTypeReq req)
        {
            var size = 20;
            var pages = 0;
            var res = new List<MeterType>();
            var r = new ResultJson<MeterType>();
            var list2 = MeterTypeOper.Instance.GetList(req, size);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                r.ListData = list2;
                var count = MeterTypeOper.Instance.GetCount(req);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 新增设备类型
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson AddMeterType(MeterTypeAddReq req, int userId)
        {
            var r = new ResultJson
            {
                Message = "已添加"
            };
            var name = req.name;
            var no = req.no;
            if (SqlHelper.Instance.IsExists2("MeterType", "meterTypeNo", no, "meterTypeNo", "0"))
                throw new Exception("已存在该设备号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else if (SqlHelper.Instance.IsExists2("MeterType", "markcode", req.code, "meterTypeNo", "0"))
                throw new Exception("已存在该代码");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                MeterType a = new MeterType
                {
                    meterTypeName = name,
                    meterTypeNo = no,
                    MarkCode = req.code
                };
                MeterTypeOper.Instance.Add2(a);
                AddOperateRecord("添加设备类型", userId);
            }
            return r;
        }

        /// <summary>
        /// 更新设备类型
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson UpdateMeterType(MeterTypeUpdateReq req, int userId)
        {
            var r = new ResultJson
            {
                Message = "更新成功"
            };
            var name = req.name;
            var no = req.no;
            MeterType a = new MeterType
            {
                meterTypeName = name,
                meterTypeNo = no,
                MarkCode = req.code
            };
            MeterTypeOper.Instance.Update2(a);
            AddOperateRecord($"更新设备({no})类型", userId);
            return r;
        }

        /// <summary>
        /// 更新员工的某个权限
        /// </summary>
        /// <param name="req"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ResultJson UpdatePermission(UpdatePermissionReq req, int parentId)
        {
            ResultJson r = new ResultJson
            {
                Message = "更新成功"
            };
            var perId = Convert.ToInt32(req.perId);
            var list = AllInOne_UserPermissionViewOper.Instance.GetByUPId(perId);
            if (list.Count == 0)
                throw new Exception("权限id不存在");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                var view = list.First();
                if (view.parentId != parentId)
                    throw new Exception("没有操作权限");
                //{
                //    r.HttpCode = 500;
                //    r.Message = "";
                //}
                else
                {
                    AllInOne_UserPermission au = new AllInOne_UserPermission();
                    au.id = perId;
                    au.isOpen = Convert.ToBoolean(req.isOpen);
                    AllInOne_UserPermissionOper.Instance.Update(au);
                    AddOperateRecord("更新员工权限", parentId);
                }
            }

            return r;
        }

        /// <summary>
        /// 通过设备号获取设备视图
        /// </summary>
        /// <param name="no"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<DeviceView> GetDeviceViewByNo(int no, AllInOne_UserInfo user)
        {
            var r = new ResultJson<DeviceView>();
            r.ListData = new List<DeviceView>
            {
                DeviceInfoOper.Instance.GetViewByNo(no, user)
            };
            return r;
        }

        /// <summary>
        /// oneFLMeterView那种的单个
        /// </summary>
        /// <param name="no"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<OneFLMeterDataView> GetOneDeviceViewByNo(int no, AllInOne_UserInfo user)
        {
            var r = new ResultJson<OneFLMeterDataView>();
            r.ListData = new List<OneFLMeterDataView>
            {
                DeviceInfoOper.Instance.GetOneFLMeterViewViewByNo(no, user)
            };
            return r;
        }

        /// <summary>
        /// 新增设备
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultJson AddDevice(DeviceInfoReq req, AllInOne_UserInfo user)
        {
            var r = new ResultJson
            {
                Message = "已添加"
            };

            if (req.communicateNo != null && SqlHelper.Instance.IsExists2("deviceInfo", "communicateNo", req.communicateNo, "meterNo", "0"))
                throw new Exception("已存在该通讯编号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                DeviceInfo d = new DeviceInfo(req, user.id)
                {
                    CommAddr = 1,
                    buildTime = DateTime.Now,
                    editTime = DateTime.Now,
                    //lat = "31.141455",
                    //lng = "121.330457",
                    Operator = user.name
                };

                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction($"openAccount_{user.id}");
                try
                {
                    var id = DeviceInfoOper.Instance.Add(d, conn, tran);

                    var userId = (int)req.userId;
                    var userHere = AllInOne_UserInfoOper.Instance.GetById(userId);
                    var ada = new AllInOne_Device_Area
                    {
                        deviceId = id,
                        areaId = userHere.areaId
                    };
                    AllInOne_Device_AreaOper.Instance.Add(ada, conn, tran);
                    AddOperateRecord($"添加设备{id}", user.id, conn, tran);
                    tran.Commit();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    conn.Close();
                    r.HttpCode = 500;
                    r.Message = ex.Message;
                }



                //AllInOne_Device_AreaOper.Instance.AddDeviceWithArea(user, id);

            }
            return r;
        }

        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultJson UpdateDevice(DeviceUpdateReq req, AllInOne_UserInfo user)
        {
            var r = new ResultJson();
            r.Message = "已更新";


            if (req.communicateNo != null && SqlHelper.Instance.IsExists2("deviceInfo", "communicateNo", req.communicateNo, "meterNo", req.meterNo))
                throw new Exception("communicateNo,已存在该通讯编号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                DeviceInfo d = new DeviceInfo(req, user.id)
                {
                    editTime = DateTime.Now,
                    Operator = user.name
                };

                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction($"openAccount_{user.id}");
                try
                {

                    DeviceInfoOper.Instance.Update(d, conn, tran);
                    var userId = (int)req.userId;
                    var userHere = AllInOne_UserInfoOper.Instance.GetById(userId);
                    var ada = AllInOne_Device_AreaOper.Instance.GetByMeterNo(d.meterNo);
                    if (ada == null)
                        throw new Exception("不存在这个账号");
                    ada.areaId = userHere.areaId;
                    //var ada = new AllInOne_Device_Area
                    //{
                    //    deviceId = d.meterNo,
                    //    areaId = userHere.areaId
                    //};
                    AllInOne_Device_AreaOper.Instance.Update(ada, conn, tran);
                    if (d.customerNo != null)
                    {
                        var cu = new AllInOne_Customer_User
                        {
                            customerId = d.customerNo,
                            userId = userId
                        };
                        AllInOne_Customer_UserOper.Instance.Update(cu, conn, tran);
                    }

                    AddOperateRecord($"更新设备{req.meterNo}", user.id, conn, tran);

                    tran.Commit();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    conn.Close();
                    r.HttpCode = 500;
                    r.Message = ex.Message;
                }
            }
            return r;
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson DelDevice(DeviceDelReq req, AllInOne_UserInfo user)
        {
            var r = new ResultJson();
            r.Message = "已删除";
            var meterNo = Convert.ToInt32(req.meterNo);
            switch (DeviceInfoOper.Instance.DeleteByMeterNo(meterNo, user))
            {
                case 2:
                    throw new Exception("设备已开户，无法删除");
                    //r.HttpCode = 500;
                    //r.Message = "";
                    break;
                case 1:
                    AddOperateRecord($"删除设备{req.meterNo}", user.id);
                    break;
                case 0:
                    throw new Exception("不存在设备或您没有操作权限");
                    //r.HttpCode = 500;
                    //r.Message = "";
                    break;
            }
            return r;



            //DeviceInfoOper.Instance.Update(d);

        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson DelCustomer(CustomerDelReq req, AllInOne_UserInfo user)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "已删除";
            var customerNo = req.customerNo;
            switch (CustomerInfoOper.Instance.DeleteByCustomerNo(customerNo, user))
            {
                case 2:
                    throw new Exception("已开户，无法删除");
                    //r.HttpCode = 500;
                    //r.Message = "";
                    break;
                case 1:
                    AddOperateRecord($"删除客户{req.customerNo}", user.id);
                    break;
                case 0:
                    throw new Exception("不存在此客户或您没有操作权限");
                    //r.HttpCode = 500;
                    //r.Message = "";
                    break;
            }
            return r;
        }

        /// <summary>
        /// 更新设备的报警上下限
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson UpdateDeviceAlarmConfig(UpdateDeviceAlarmConfigReq req, AllInOne_UserInfo user)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "更新成功";

            var vStr = ValidateDeviceAlarmConfigReq(req);
            if (vStr != "")
                throw new Exception(vStr);
            //{
            //    r.HttpCode = 500;
            //    r.Message = vStr;
            //    return r;
            //}

            if (!DeviceInfoOper.Instance.ConfirmUser(Convert.ToInt32(req.meterNo), user))
                throw new Exception("没有权限操作此表");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //    return r;
            //}

            DeviceInfo d = new DeviceInfo(req)
            {
                editTime = DateTime.Now,
                Operator = user.name
            };

            DeviceInfoOper.Instance.Update(d);
            AddOperateRecord($"更新设备报警值{req.meterNo}", user.id);
            return r;
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultJson AddCustomer(CustomerAddReq req, AllInOne_UserInfo pUser)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "已添加";

            if (SqlHelper.Instance.IsExists2("customerinfo", "customerNo", req.customerNo, "customerNo", "0"))
                throw new Exception("meterNo,已存在该设备号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}

            else
            {
                CustomerInfo c = new CustomerInfo(req);
                c.buildTime = DateTime.Now;
                c.editTime = DateTime.Now;
                c.Operator = pUser.name;
                c.useState = 0;
                CustomerInfoOper.Instance.Add(c);
                AllInOne_Customer_UserOper.Instance.AddCU(c, pUser);
                AddOperateRecord($"添加客户{c.customerNo}", pUser.id);
            }
            return r;
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="req"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public ResultJson UpdateCustomer(CustomerUpdateReq req, AllInOne_UserInfo pUser)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "已添加";

            if (SqlHelper.Instance.IsExists2("customerinfo", "customerNo", req.customerNo, "customerNo", req.customerNo))
                throw new Exception("meterNo,已存在该设备号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}

            else
            {
                CustomerInfo c = new CustomerInfo(req)
                {
                    editTime = DateTime.Now,
                    Operator = pUser.name
                };
                if (req.useState != null)
                    c.useState = Convert.ToInt32(req.useState);
                CustomerInfoOper.Instance.Update(c);
                AddOperateRecord("添加客户", pUser.id);
            }
            return r;
        }

        /// <summary>
        /// 开户不涉及金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson<Cno_CustNo> OpenAccount(AllInOne_UserInfo user, CustomerInfo customer, DeviceInfo device, int userId)
        {
            var r = new ResultJson<Cno_CustNo>();
            //r.HttpCode = 200;
            r.Message = "开户成功";

            var meterno = 0;

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction($"openAccount_{user.id}");
            try
            {
                //绑定用户信息
                DeviceInfoOper.Instance.BindInfo(user, customer, device, conn, tran, out meterno);

                var userHere = AllInOne_UserInfoOper.Instance.GetById(userId);
                //设备、客户关联某个userId
                var ada = new AllInOne_Device_Area
                {
                    deviceId = meterno,
                    areaId = userHere.areaId
                };
                AllInOne_Device_AreaOper.Instance.Add(ada, conn, tran);
                var cu = new AllInOne_Customer_User
                {
                    customerId = customer.customerNo,
                    userId = userId
                };
                AllInOne_Customer_UserOper.Instance.Add(cu, conn, tran);

                //创建用户阶梯用量
                var flag = LadderVolumeOper.Instance.AddLv(device.fluidNo, customer, conn, tran);
                if (!flag)
                {
                    tran.Rollback();
                    conn.Close();
                    r.HttpCode = 500;
                    r.Message = "开户失败";
                    return r;
                }
                ////收退款明细？
                //ReceiptDetialOper.Instance.AddRd(customer,device,)
                //充值记录

                //开户记录
                int OpenRecId;
                OpenAccountRecordOper.Instance.AddOpenAccountRecord(customer, device, out OpenRecId, conn, tran);
                //营业点配额处理？

                //判断实时数据表里有没有这个表，没有就加一条初始的实时数据
                var meterCount = 0;
                if (device.communicateNo != null)
                {
                    meterCount = AllInOne_FLMeterDataOper.Instance.IsNewCommNo(device.communicateNo);
                    if (meterCount == 0)
                    {
                        var flm = new AllInOne_FLMeterData();
                        flm.communicateNo = device.communicateNo;
                        flm.FLMeterNo = meterno;
                        flm.InstantTime = DateTime.Now;
                        flm.ReceivTime = DateTime.Now.AddSeconds(1);
                        flm.StdSum = 0;
                        flm.WorkSum = 0;
                        flm.StdFlow = 0;
                        flm.WorkFlow = 0;
                        flm.Temperature = 0;
                        flm.Pressure = 0;
                        flm.FMState = 0;
                        flm.FMStateMsg = "";
                        flm.RTUState = 0;
                        flm.RTUStateMsg = "";
                        flm.SumTotal = 0;
                        flm.RemainMoney = 0;
                        flm.RemainVolume = 0;
                        flm.Overdraft = 0;
                        flm.RemoteChargeMoney = 0;
                        flm.RemoteChargeTimes = 0;
                        flm.Price = 0;
                        flm.ValveState = 0;
                        flm.ValveStateMsg = "阀关";
                        flm.PowerVoltage = 0;
                        flm.BatteryVoltage = 0;
                        AllInOne_FLMeterDataOper.Instance.Add(flm, conn, tran);
                    }
                }


                tran.Commit();
                conn.Close();
                AddOperateRecord("开户", user.id);
                r.ListData.Add(new Cno_CustNo { communicateNo = device.communicateNo, customerNo = customer.customerNo });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                conn.Close();
                r.HttpCode = 500;
                r.Message = ex.Message;
            }
            return r;

        }

        /// <summary>
        /// 获取协议列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<AllInOne_ProtocolInfo> GetProtocolList(ProtocolListReq req)
        {
            var size = 5;
            var pages = 0;
            var res = new List<AllInOne_ProtocolInfo>();
            var r = new ResultJson<AllInOne_ProtocolInfo>();
            var list2 = AllInOne_ProtocolInfoOper.Instance.GetList(req, size);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                r.ListData = list2;
                var count = AllInOne_ProtocolInfoOper.Instance.GetCount(req);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 新增协议
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson AddProtocol(ProtocolAddUpdateReq req, int userId)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "已添加";
            var name = req.name;
            var protocolNo = req.no;
            if (SqlHelper.Instance.IsExists2("AllInOne_ProtocolInfo", "protocolNo", protocolNo, "protocolNo", "0"))
                throw new Exception("已存在该协议号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                AllInOne_ProtocolInfo a = new AllInOne_ProtocolInfo();
                a.ProtocolName = name;
                a.ProtocolNo = protocolNo;
                AllInOne_ProtocolInfoOper.Instance.Add2(a);
                AddOperateRecord("添加协议", userId);
            }
            return r;
        }

        /// <summary>
        /// 更新协议
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultJson UpdateProtocol(ProtocolAddUpdateReq req, int userId)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "已更新";
            var name = req.name;
            var no = req.no;
            if (SqlHelper.Instance.IsExists2("AllInOne_ProtocolInfo", "protocolNo", no, "protocolNo", no))
                throw new Exception("已存在该协议号");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            else
            {
                AllInOne_ProtocolInfo a = new AllInOne_ProtocolInfo();
                a.ProtocolName = req.name;
                a.ProtocolNo = no;
                AllInOne_ProtocolInfoOper.Instance.Update(a);
                AddOperateRecord("更新区域", userId);
            }
            return r;
        }

        /// <summary>
        /// 获取未绑定的设备
        /// </summary>
        /// <returns></returns>
        public ResultJson<DeviceView> GetNotOpenMeters(AllInOne_UserInfo user)
        {
            var r = new ResultJson<DeviceView>();
            //r.HttpCode = 200;
            //var list = new List<DeviceInfo>();
            //var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var list = DeviceInfoOper.Instance.GetNotOpen(user);
            r.ListData = list;
            return r;
        }

        /// <summary>
        /// 获取未绑定的客户
        /// </summary>
        /// <returns></returns>
        public ResultJson<CustomerView> GetNotOpenCustomers(AllInOne_UserInfo user)
        {
            var r = new ResultJson<CustomerView>();
            //r.HttpCode = 200;
            //var list = new List<DeviceInfo>();
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var list = CustomerInfoOper.Instance.GetNotOpen(lastCId);
            r.ListData = list;
            return r;
        }

        /// <summary>
        /// 获取操作记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<AllInOne_OperateRecordView> GetOperRecordList(OperRecordReq req, AllInOne_UserInfo user)
        {
            var size = 20;
            var pages = 0;
            var res = new List<AllInOne_OperateRecordView>();
            var r = new ResultJson<AllInOne_OperateRecordView>();
            var list2 = AllInOne_OperateRecordOper.Instance.GetList(req, size, user);
            if (list2.Count == 0)
                r.ListData = res;
            else
            {
                //foreach (var item in list2)
                //{
                //    CustomerViewRes r2 = new CustomerViewRes(item);
                //    res.Add(r2);
                //}
                r.ListData = list2;
                var count = AllInOne_OperateRecordOper.Instance.GetCount(req, user);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 获取历史数据列表，单用户版
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<FMModel> GetFMList(HisReq req, AllInOne_UserInfo user)
        {
            var size = 20;
            var pages = 0;
            var res = new List<FMModel>();
            var r = new ResultJson<FMModel>();

            //这三组只会传一组过来
            var startTime = req.startTime;
            var endTime = req.endTime;
            var yStart = req.yStart;
            var yEnd = req.yEnd;
            var mStart = req.mStart;
            var mEnd = req.mEnd;
            if (startTime != null && endTime != null)
            {
                if (Convert.ToDateTime(endTime) < Convert.ToDateTime(startTime))
                    throw new Exception("终止时间不能早于起始时间");
            }
            else if (yStart != null && yEnd != null)
            {
                if (Convert.ToInt32(yEnd) < Convert.ToInt32(yStart))
                    throw new Exception("终止时间不能早于起始时间");
            }
            else if (mStart != null && mEnd != null)
            {
                var arr1 = mStart.Split('-');
                var arr2 = mEnd.Split('-');
                var yearEnd = Convert.ToInt32(arr2[0]);
                var yearStart = Convert.ToInt32(arr1[0]);
                if (yearEnd < yearStart)
                    throw new Exception("终止时间不能早于起始时间");
                if (yearStart == yearEnd)
                {
                    var monthEnd = Convert.ToInt32(arr2[1]);
                    var monthStart = Convert.ToInt32(arr1[1]);
                    if (monthEnd < monthStart)
                        throw new Exception("终止时间不能早于起始时间");
                }
            }

            switch (req.dateStr)
            {
                case "year":
                    req.startTime = new DateTime(Convert.ToInt32(yStart), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    req.endTime = new DateTime(Convert.ToInt32(yEnd), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "month":
                    var arr1 = mStart.Split('-');
                    var arr2 = mEnd.Split('-');
                    var yearEnd = Convert.ToInt32(arr2[0]);
                    var yearStart = Convert.ToInt32(arr1[0]);
                    var monthEnd = Convert.ToInt32(arr2[1]);
                    var monthStart = Convert.ToInt32(arr1[1]);
                    req.startTime = new DateTime(yearStart, monthStart, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    req.endTime = new DateTime(yearEnd, monthEnd, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }

            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);
            if (tableNames.Count == 0)
                r.ListData = res;
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);

            tableNames = HisOper.Instance.RemoveBadTable(tableNames);

            if (tableNames.Count == 0)
                r.ListData = res;
            else
            {
                //var selectUnion = HisOper.Instance.GetDBUnion(tableNames);
                var tableName = tableNames.First();
                r.ListData = HisOper.Instance.GetList22(req, size, tableName);
                var count = HisOper.Instance.GetCount22(req, tableName);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<FMModel> GetFMList2(HisReq req, AllInOne_UserInfo user)
        {
            var size = 20;
            var pages = 0;
            var res = new List<FMModel>();
            var r = new ResultJson<FMModel>();

            var startTime = req.startTime;
            var endTime = req.endTime;
            if (startTime != null && endTime != null)
            {
                if (Convert.ToDateTime(endTime) < Convert.ToDateTime(startTime))
                    throw new Exception("终止时间不能早于起始时间");
                //{
                //    r.HttpCode = 500;
                //    r.Message = "";
                //    return r;
                //}
            }

            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);
            tableNames = HisOper.Instance.GetExistHisTable(tableNames);

            tableNames = HisOper.Instance.RemoveBadTable(tableNames);

            if (tableNames.Count == 0)
                r.ListData = res;
            else
            {
                var selectUnion = HisOper.Instance.GetDBUnion(tableNames);
                r.ListData = HisOper.Instance.GetList2(req, size, tableNames);
                var count = HisOper.Instance.GetCount(req, selectUnion, tableNames);
                pages = count / size;
                //总页数
                pages = pages * size == count ? pages : pages + 1;
                r.pages = pages;
                req.listSort = ClearListSorts(req.listSort);
                var sort = req.listSort ?? new List<string> { "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting", "th-sort sorting" };
                r.sort = sort;
                r.index = Convert.ToInt32(req.pageIndex);
            }
            return r;
            //var str = HisOper.Instance.GetDBUnion(sameIds);
        }

        /// <summary>
        /// 获取地图上的设备-txying
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<OneFLMeterDataView> GetDeviceOnMap(DeviceMapReq req, AllInOne_UserInfo user)
        {
            var res = new List<OneFLMeterDataView>();
            var r = new ResultJson<OneFLMeterDataView>();
            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var list = DeviceInfoOper.Instance.GetDeviceOnMap(req, lastCId);
            var lats = list.Where(p => p.lat != null && p.lng != null && p.lat != "" && p.lng != "").Select(p => Convert.ToDecimal(p.lat)).ToList();
            if (lats.Count > 0)
            {
                var lat1 = lats.OrderBy(p => p).First();
                var lat2 = lats.OrderByDescending(p => p).First();

                var lngs = list.Where(p => p.lat != null && p.lng != null && p.lat != "" && p.lng != "").Select(p => Convert.ToDecimal(p.lng)).ToList();
                var lng1 = lngs.OrderBy(p => p).First();
                var lng2 = lngs.OrderByDescending(p => p).First();
                r.lat = ((lat1 + lat2) / 2).ToString();
                r.lng = ((lng1 + lng2) / 2).ToString();
                var latSpan = lat2 - lat1;
                var lngSpan = lng2 - lng1;

                var latKM = 111.136m * latSpan;
                var lngKM = 111.136m * lngSpan;
                var l1 = GetMapLevelForLat(latKM);
                var l2 = GetMapLevelForLng(lngKM);
                r.level = l1 > l2 ? l2 : l1;
                //r.level -= 1;
                r.level = r.level == 1 ? 1 : r.level - 1;
            }
            r.ListData = list;
            return r;
        }

        /// <summary>
        /// 更新设备的坐标经纬度
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson UpdateLatlng(UpdateLatlngReq req, AllInOne_UserInfo user)
        {
            ResultJson r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "更新成功";
            if (!DeviceInfoOper.Instance.ConfirmUser(Convert.ToInt32(req.meterNo), user))
                throw new Exception("没有权限操作");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //    return r;
            //}

            DeviceInfo d = new DeviceInfo();
            d.meterNo = Convert.ToInt32(req.meterNo);
            d.lat = req.lat;
            d.lng = req.lng;
            DeviceInfoOper.Instance.Update(d);
            return r;

        }

        /// <summary>
        /// 验证这个用户输入的操作密码对不对
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public ResultJson ConfirmUser2(AllInOne_UserInfo user, string pwd)
        {
            ResultJson r = new ResultJson();
            //r.HttpCode = 200;
            r.Message = "验证成功";
            if (user.pwd != MD5Helper.Instance.StrToMD5_UTF8(pwd))
                throw new Exception("验证失败");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            return r;
        }

        /// <summary>
        /// 远程 开关阀
        /// </summary>
        /// <param name="user"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson ValveOper(AllInOne_UserInfo user, ValveOperReq req)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;
            if (ConfirmUser(user, req.pwd))
            {
                var strOpen = $"FFFF,100D,{req.commNo}";
                var strClose = $"FFFF,100E,{req.commNo}";

                if (req.oper == "开阀")
                    r = SendMsg2(strOpen);
                else
                    r = SendMsg2(strClose);

            }
            else
                throw new Exception("密码不正确或无权限操作");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}

            return r;
        }

        /// <summary>
        /// 远程充值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson ChargeOper(AllInOne_UserInfo user, ChargeOperReq req)
        {
            var r = new ResultJson();
            //r.HttpCode = 200;

            //int times = DeviceInfoOper.Instance.GetChargeTimesByCommNo(req.commNo);
            int times = 1;
            if (ConfirmUser(user, req.pwd))
            {
                var str = $"FFFF,1101,{req.commNo},{req.money},{times}";
                //return SendMsg2(str);
            }
            else
                throw new Exception("密码不正确或无权限操作");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}

            return r;
        }

        /// <summary>
        /// 不用了。获取一个月的n天option的html
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<string> GetDaysHtml(GetDaysReq req)
        {
            var r = new ResultJson<string>();
            //r.HttpCode = 200;
            var year = Convert.ToInt32(req.year);
            var month = Convert.ToInt32(req.month);
            var days = DateTime.DaysInMonth(year, month);
            var h = "";
            for (int i = 1; i < days + 1; i++)
            {
                h += $"<option value=\"{ i}\">{i}</option>";
            }
            r.ListData.Add(h);
            return r;
        }

        /// <summary>
        /// 获取充值数据的列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<chargeReport> GetChargeLine(GetChargeReportReq req)
        {
            var r = new ResultJson<chargeReport>();
            var list = new List<chargeReport>();
            if (req.lastName != null)
            {
                list = ICChargeRecordOper.Instance.GetLineListForCompany(req);
            }
            else
                list = ICChargeRecordOper.Instance.GetLineList(req);
            if (list.Count == 0)
                throw new Exception("没有数据");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //    return r;
            //}
            var date = Convert.ToDateTime(req.date);

            r.level = DateTime.DaysInMonth(date.Year, date.Month);
            r.ListData = list;
            return r;
        }

        /// <summary>
        /// 获取用量的列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<StdSumReport> GetStdSumLine(GetStdSumReq req)
        {
            var r = new ResultJson<StdSumReport>();
            //r.HttpCode = 200;
            var list = new List<StdSumReport>();
            //if (req.lastName != null)
            list = HisOper.Instance.GetHisDataListForChartForCompany(req);
            //else
            //    list = HisOper.Instance.GetHisDataListForChart(req, "FM0000000001");
            if (list.Count == 0)
                throw new Exception("没有数据");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //    return r;
            //}
            var date = Convert.ToDateTime(req.date);

            r.level = DateTime.DaysInMonth(date.Year, date.Month);
            r.ListData = list;
            return r;
        }

        /// <summary>
        /// 类型  充值记录饼图
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<PieRes> GetChargePieByType(GetChargeReportReq req)
        {
            var r = new ResultJson<PieRes>();
            var list = new List<PieRes>();
            if (req.lastName != null)
                list = ICChargeRecordOper.Instance.GetPieListByCustTypeForCompany(req);
            else
                list = ICChargeRecordOper.Instance.GetPieListByCustType(req);
            r.ListData = list;
            //r.HttpCode = 200;
            return r;
        }

        /// <summary>
        /// 时间 充值记录饼图
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<PieRes> GetChargePieByTime(GetChargeReportReq req)
        {
            var r = new ResultJson<PieRes>();
            var list = new List<PieRes>();
            if (req.lastName != null)
                list = ICChargeRecordOper.Instance.GetPieListByTimeForCompany(req);
            else
                list = ICChargeRecordOper.Instance.GetPieListByTime(req);
            r.ListData = list;
            //r.HttpCode = 200;
            return r;
        }

        /// <summary>
        /// 客户类型 用量记录饼图
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<PieRes> GetStdSumPieByType(GetStdSumReq req)
        {
            var r = new ResultJson<PieRes>();
            var list = new List<PieRes>();
            //if (req.lastName != null)
            list = HisOper.Instance.GetPieListByCustTypeForCompany(req);
            //else
            //    list = HisOper.Instance.GetPieListByCustType(req, "FM0000000001");
            //var list=
            r.ListData = list;
            //r.HttpCode = 200;
            return r;
        }

        /// <summary>
        /// 时间 用量记录饼图
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultJson<PieRes> GetStdSumPieByTime(GetStdSumReq req)
        {
            var r = new ResultJson<PieRes>();
            var list = new List<PieRes>();
            //if (req.lastName != null)
            list = HisOper.Instance.GetPieListByTimeForCompany(req);
            //else
            //    list = HisOper.Instance.GetPieListByTime(req, "FM0000000001");
            r.ListData = list;
            //r.HttpCode = 200;
            return r;
        }

        /// <summary>
        /// 根据名字获取子账号的信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<AllInOne_UserInfo> GetSonByName(string name, AllInOne_UserInfo user)
        {
            var r = new ResultJson<AllInOne_UserInfo>();
            //r.HttpCode = 200;
            r.ListData = AllInOne_UserInfoOper.Instance.GetSonsByFatherName(name, user);
            return r;
        }

        /// <summary>
        /// 根据公司名字获取所有设备
        /// </summary>
        /// <param name="name"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultJson<DeviceView> GetDeviceListByCompanyName(string name, AllInOne_UserInfo user)
        {
            var r = new ResultJson<DeviceView>();
            //r.HttpCode = 200;

            if (name == null)
            {
                var list = GetDeviceByUser(user);
                r.ListData = list;
            }
            else
            {
                r.ListData = DeviceInfoOper.Instance.GetDeviceViewByCompanyName(name, user);
            }
            return r;
        }

        /// <summary>
        /// 查询数据库有没有新的报警信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldCount"></param>
        /// <returns></returns>
        public ResultJson<DevId_Content> CheckNewAlarm(AllInOne_UserInfo user, int oldCount)
        {
            var r = new ResultJson<DevId_Content>();
            //r.HttpCode = 200;
            var counts = AllInOne_AlarmInfoOper.Instance.GetSafeAndAlarmCount(user);
            var count = 0;
            if (counts.Count > 0)
            {
                if (counts[0] != null)
                    count += Convert.ToInt32(counts[0].counts);
                if (counts[1] != null)
                    count += Convert.ToInt32(counts[1].counts);

                if (count > oldCount)
                {
                    var span = count - oldCount;
                    var list = AllInOne_AlarmInfoOper.Instance.GetNewAlarmMeterNos(user, span);

                    foreach (var item in list)
                    {
                        r.ListData.Add(new DevId_Content(item));
                    }

                    var ids = r.ListData.Select(p => p.devid).Distinct().ToList();
                    if (user.phone != null && user.phone != "")
                    {
                        foreach (var item in ids)
                        {
                            var temp = r.ListData.Where(p => p.devid == item).First();
                            var phones = new List<string>();
                            phones.Add(user.phone);
                            //发送短信
                            SendMail(phones, temp);
                            //调微信api
                            SendWxMsg(temp);
                        }
                    }


                    //if (list.Count > 0)
                    //{
                    //    r.ListData = list.Select(p => (int)p.Devid).Distinct().ToList();
                    //}
                }
            }
            return r;
        }

        /// <summary>
        /// 查询有没有新的未处理警报
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldLastId"></param>
        /// <returns></returns>
        public ResultJson<AlarmItem> CheckNewAlarm2(AllInOne_UserInfo user, int oldLastId)
        {
            var r = new ResultJson<AlarmItem>();

            var list = AllInOne_AlarmInfoOper.Instance.GetNewAlarm(user, oldLastId);
            if (list.Count == 0)
                return r;
            var maxId = list.Max(p => p.Id);
            foreach (var item in list)
            {
                r.ListData.Add(new AlarmItem(item, maxId));
            }
            var devids = r.ListData.Select(p => p.devid).Distinct().ToList();
            if (user.phone != null && user.phone != "")
            {
                foreach (var devid in devids)
                {
                    var temp = r.ListData.Where(p => p.devid == devid).First();

                    //发送短信
                    if (UserPermissionViewOper.Instance.IsSendMailOpen((int)devid))
                    {
                        var phones = new List<string>();
                        phones.Add(user.phone);
                        SendMail(phones, temp);
                    }
                    //调微信api
                    SendWxMsg(temp);
                }
            }
            return r;

            //r.HttpCode = 200;
            //var counts = AllInOne_AlarmInfoOper.Instance.GetSafeAndAlarmCount(user);
            //var count = 0;
            //if (counts.Count > 0)
            //{
            //    if (counts[0] != null)
            //        count += Convert.ToInt32(counts[0].counts);
            //    if (counts[1] != null)
            //        count += Convert.ToInt32(counts[1].counts);

            //    if (count > oldLastId)
            //    {
            //        var span = count - oldLastId;
            //        var list = AllInOne_AlarmInfoOper.Instance.GetNewAlarmMeterNos(user, span);

            //        foreach (var item in list)
            //        {
            //            r.ListData.Add(new DevId_Content(item));
            //        }

            //        var ids = r.ListData.Select(p => p.devid).Distinct().ToList();
            //        if (user.phone != null && user.phone != "")
            //        {
            //            foreach (var item in ids)
            //            {
            //                var temp = r.ListData.Where(p => p.devid == item).First();
            //                var phones = new List<string>();
            //                phones.Add(user.phone);
            //                //发送短信
            //                SendMail(phones, temp);
            //                //调微信api
            //                SendWxMsg(temp);
            //            }
            //        }


            //        //if (list.Count > 0)
            //        //{
            //        //    r.ListData = list.Select(p => (int)p.Devid).Distinct().ToList();
            //        //}
            //    }
            //}

        }

        /// <summary>
        /// 发送命令给苏背背那边
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ResultJson SendMsg2(string msg)
        {
            var r = new ResultJson();

            //TCP_Pack_ClientHere.TCP_Pack_ClientHere t = new TCP_Pack_ClientHere.TCP_Pack_ClientHere();
            //t.Connect();

            var r1 = TCP_Pack_ClientHere.TCP_Pack_ClientHere.Instance.Connect();
            System.Threading.Thread.Sleep(1000);
            var r2 = TCP_Pack_ClientHere.TCP_Pack_ClientHere.Instance.Send(msg);
            int i = 0;
            while (true)
            {
                i++;
                if (string.IsNullOrEmpty(TCP_Pack_ClientHere.TCP_Pack_ClientHere.Instance.str))
                {
                    System.Threading.Thread.Sleep(200);
                }
                else
                {
                    break;
                }
                if (i == 100)
                {
                    break;
                }
            }

            var r3 = TCP_Pack_ClientHere.TCP_Pack_ClientHere.Instance.Close();
            if (TCP_Pack_ClientHere.TCP_Pack_ClientHere.Instance.str == "01")
            {
                r.HttpCode = 200;
                r.Message = "发送成功";
            }
            else
                throw new Exception("发送失败");
            //{
            //    r.HttpCode = 500;
            //    r.Message = "";
            //}
            return r;
        }
        #endregion

        #region 其他

        /// <summary>
        /// 给手机发送警报短信
        /// </summary>
        /// <param name="phones"></param>
        /// <param name="dc"></param>
        public void SendMail(List<string> phones, DevId_Content dc)
        {
            Enum_SendEmailCode SendEmail;
            SendEmail = Enum_SendEmailCode.UserRegistrationVerificationCode;
            var phone = StringHelper.Instance.ArrJoin(phones.ToArray());
            var paras = JsonConvert.SerializeObject(dc);
            SendSmsResponse Email = AliyunHelper.SendMail.SendMail.Instance.SendAlarm(phone, paras, SendEmail);
            var r = Email.Code;
        }

        public void SendMail(List<string> phones, AlarmItem dc)
        {
            Enum_SendEmailCode SendEmail;
            SendEmail = Enum_SendEmailCode.UserRegistrationVerificationCode;
            var phone = StringHelper.Instance.ArrJoin(phones.ToArray());
            var paras = JsonConvert.SerializeObject(dc);
            SendSmsResponse Email = AliyunHelper.SendMail.SendMail.Instance.SendAlarm(phone, paras, SendEmail);
            var r = Email.Code;
        }

        /// <summary>
        /// 给微信发送警报信息
        /// </summary>
        /// <param name="dc"></param>
        public void SendWxMsg(DevId_Content dc)
        {
            var openIds = GetOpenIds();
            var tid = ConfigurationManager.AppSettings["wxMsgTemplateId"];
            foreach (var item in openIds)
            {
                var token = CacheFunc.Instance.GetWxToken();
                var url = $"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={token}";

                var req = new WxSendMsgReq();
                req.touser = item;
                //req.template_id = "LFZiN0acVrU2DGKj0ivoesBVpNhCyJuyxEl6zZDqNKA";
                req.template_id = tid;
                req.topcolor = "#FF0000";
                req.data = new WxSendData
                {
                    devid = new Value_color { value = dc.devid, color = "#173177" },
                    content = new Value_color { value = dc.content, color = "#173177" }
                };
                var postData = JsonConvert.SerializeObject(req);
                HttpPost(url, postData);
            }


        }

        public void SendWxMsg(AlarmItem dc)
        {
            var openIds = GetOpenIds();

            foreach (var item in openIds)
            {
                var token = CacheFunc.Instance.GetWxToken();
                var url = $"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={token}";

                var req = new WxSendMsgReq();
                req.touser = item;
                req.template_id = "LFZiN0acVrU2DGKj0ivoesBVpNhCyJuyxEl6zZDqNKA";
                req.topcolor = "#FF0000";
                req.data = new WxSendData
                {
                    devid = new Value_color { value = dc.devid.ToString(), color = "#173177" },
                    content = new Value_color { value = dc.content, color = "#173177" }
                };
                var postData = JsonConvert.SerializeObject(req);
                HttpPost(url, postData);
            }


        }

        /// <summary>
        /// 获取微信服务号下所有关注的人的OpenId
        /// </summary>
        /// <returns></returns>
        public List<string> GetOpenIds()
        {
            var token = CacheFunc.Instance.GetWxToken();
            string url = $"https://api.weixin.qq.com/cgi-bin/user/get?access_token={token}&next_openid=";
            var r = HttpGet(url);
            return JsonConvert.DeserializeObject<WXUserList>(r).data.openid;
        }

        /// <summary>
        /// 获取微信服务号的token字符串
        /// </summary>
        /// <returns></returns>
        public string GetWXToken()
        {
            var id = ConfigurationManager.AppSettings["wxMsgAppId"];
            var secret = ConfigurationManager.AppSettings["wxMsgAppSecret"];
            //string id = "wx3f74b9822c798200";
            //string secret = "924183a4ef55dac2f2cf042d662e4f23";
            return GetWXToken(id, secret).access_token;
        }

        /// <summary>
        /// 获取微信服务号的token模型
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public WXToken GetWXToken(string appId, string secret)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={secret}";
            var r = HttpGet(url);
            return JsonConvert.DeserializeObject<WXToken>(r);
        }

        /// <summary>
        /// get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string HttpGet(string url)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/json";
            httpRequest.Referer = null;
            httpRequest.AllowAutoRedirect = true;
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            httpRequest.Accept = "*/*";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            Stream receiveStream = httpResponse.GetResponseStream();

            string result = string.Empty;
            using (StreamReader sr = new StreamReader(receiveStream))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">数据model的字符串</param>
        /// <returns></returns>
        public string HttpPost(string url, string postData)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Referer = null;
            httpRequest.AllowAutoRedirect = true;
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            httpRequest.Accept = "*/*";

            Stream requestStem = httpRequest.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStem);
            sw.Write(postData);
            sw.Close();

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            Stream receiveStream = httpResponse.GetResponseStream();

            string result = string.Empty;
            using (StreamReader sr = new StreamReader(receiveStream))
            {
                result = sr.ReadToEnd();
            }

            return "";
        }

        /// <summary>
        /// 为Excel获取用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<CustomerViewRes> GetCustomerListForExcel(CustomerReq req, AllInOne_UserInfo user)
        {
            var size = 999999;
            var res = new List<CustomerViewRes>();

            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var list2 = CustomerInfoOper.Instance.GetList(req, size, lastCId);
            if (list2.Count == 0)
                return res;
            else
            {
                foreach (var item in list2)
                {
                    CustomerViewRes d = new CustomerViewRes(item, 1);
                    res.Add(d);
                }
            }
            return res;
        }

        /// <summary>
        /// 为Excel获取实时数据信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<OneFLMeterDataViewRes> GetMeterDataListForExcel(MeterDataListReq req, AllInOne_UserInfo user)
        {
            var size = 999999;

            var res = new List<OneFLMeterDataViewRes>();

            var lastCId = AllInOne_UserInfoOper.Instance.DeviceGetLastCID(user);
            var list2 = AllInOne_FLMeterDataOper.Instance.GetList(req, size, lastCId);
            if (list2.Count == 0)
                return res;
            else
            {
                foreach (var item in list2)
                {
                    OneFLMeterDataViewRes d = new OneFLMeterDataViewRes(item);
                    res.Add(d);
                }

            }
            return res;
        }

        /// <summary>
        /// 为Excel获取设备信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DeviceViewRes> GetMeterListForExcel(MeterReq req, AllInOne_UserInfo user)
        {
            var size = 999999;
            //var pages = 0;
            var res = new List<DeviceViewRes>();

            //var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var list2 = DeviceInfoOper.Instance.GetList(req, size, user);
            if (list2.Count == 0)
                return res;
            else
            {
                foreach (var item in list2)
                {
                    DeviceViewRes d = new DeviceViewRes(item);
                    res.Add(d);
                }
            }
            return res;
        }

        /// <summary>
        /// 获取未使用的区域字典,信东获取所有的，其他获取自己创建的区域
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAreaList(int userId, int lv)
        {
            return AllInOne_AreaInfoOper.Instance.GetAreaDictByUserId(userId);
        }

        /// <summary>
        /// 根据区域id获取区域信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public AllInOne_AreaInfo GetAreaById(int areaId)
        {
            return AllInOne_AreaInfoOper.Instance.GetById(areaId);
        }

        /// <summary>
        /// 获取某个员工的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<UserPermission> GetPermissionByUserId(int userId, int parentId)
        {
            var list = AllInOne_UserPermissionViewOper.Instance.GetByUserId(userId);
            if (list.Count == 0)
                return new List<UserPermission>();
            if (list.First().parentId != parentId)
                return new List<UserPermission>();
            var r = new List<UserPermission>();
            foreach (var item in list)
            {
                UserPermission up = new UserPermission(item);
                r.Add(up);
            }

            return r;
        }

        /// <summary>
        /// 获取新设备id
        /// </summary>
        /// <returns></returns>
        public string GetNewDeviceId()
        {
            var newId = DeviceInfoOper.Instance.GetLastId() + 1;
            return StringHelper.Instance.GetIntStringWithZero(newId.ToString(), 10);
        }

        /// <summary>
        /// 获取新客户no
        /// </summary>
        /// <returns></returns>
        public string GetNewCustomerNo()
        {
            var newNo = CustomerInfoOper.Instance.GetLastNo() + 1;
            return StringHelper.Instance.GetIntStringWithZero(newNo.ToString(), 10);
        }

        /// <summary>
        /// 获取某个设备的信息
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public DeviceInfo GetDeviceByNo(int no, AllInOne_UserInfo user)
        {
            return DeviceInfoOper.Instance.GetByNo(no, user);
        }

        /// <summary>
        /// 验证设备报警范围min小于max
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string ValidateDeviceAlarmConfigReq(UpdateDeviceAlarmConfigReq req)
        {
            if (req.StdFlowLow != null && req.StdFlowUpper != null)
            {
                if (Convert.ToDecimal(req.StdFlowLow) > Convert.ToDecimal(req.StdFlowUpper))
                    return "标况流量范围错误";
            }
            if (req.WorkFlowLow != null && req.WorkFlowUpper != null)
            {
                if (Convert.ToDecimal(req.WorkFlowLow) > Convert.ToDecimal(req.WorkFlowUpper))
                    return "工况流量范围错误";
            }
            if (req.PressLow != null && req.PressUpper != null)
            {
                if (Convert.ToDecimal(req.PressLow) > Convert.ToDecimal(req.PressUpper))
                    return "压力范围错误";
            }
            if (req.TempLow != null && req.TempUpper != null)
            {
                if (Convert.ToDecimal(req.TempLow) > Convert.ToDecimal(req.TempUpper))
                    return "温度范围错误";
            }
            if (req.PowerLow != null && req.PowerUpper != null)
            {
                if (Convert.ToDecimal(req.PowerLow) > Convert.ToDecimal(req.PowerUpper))
                    return "供电电压范围错误";
            }
            return "";
        }

        /// <summary>
        /// 新增操作记录
        /// </summary>
        /// <param name="content"></param>
        /// <param name="userId"></param>
        public void AddOperateRecord(string content, int userId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            AllInOne_OperateRecord o = new AllInOne_OperateRecord
            {
                content = content,
                operatorId = userId.ToString(),
                operateTime = DateTime.Now
            };
            AllInOne_OperateRecordOper.Instance.Add(o);
        }

        /// <summary>
        /// 由客户编号获取客户的信息
        /// </summary>
        /// <param name="no"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public CustomerInfo GetCustomerByNo(string no, AllInOne_UserInfo user)
        {
            return CustomerInfoOper.Instance.GetByNo(no, user);
        }

        /// <summary>
        /// 根据用户编号获取用户视图
        /// </summary>
        /// <param name="no"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public CustomerView GetCustomerViewByNo(string no, AllInOne_UserInfo user)
        {
            return CustomerInfoOper.Instance.GetViewByNo(no, user);
        }

        /// <summary>
        /// 获取口径列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetCaliber()
        {
            return new List<string>
            {
                "DN25",
            "DN50",
            "DN80",
            "DN100",
            "DN150",
            "DN200",
            "DN250",
            "DN300",
            "DN400",
            "CG-1.6",
            "CG-2.5",
            "CG-4",
            "CG-6",
            "CG-16",
            "CG-25",
            "CG-40",
            "CG-60",
            "CG-100",
            "LLJ",

            };
        }

        /// <summary>
        /// 获取价格类型列表
        /// </summary>
        /// <returns></returns>
        public List<FluidInfo0301> GetFluidList()
        {
            return FluidInfo0301Oper.Instance.GetList();
        }

        /// <summary>
        /// 补全用户可能存在的权限
        /// </summary>
        public void Complete()
        {
            AllInOne_UserPermissionViewOper.Instance.Complete();
        }

        /// <summary>
        /// 获取webConfig里的参数
        /// </summary>
        /// <returns></returns>
        public Config2 GetConfig()
        {
            return new Config2();
        }

        /// <summary>
        /// 获取FMModel，用于导出excel
        /// </summary>
        /// <param name="req"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<FMModel> GetFMListForExcel(HisReq req, AllInOne_UserInfo user)
        {
            var size = 999999;
            var res = new List<FMModel>();
            var startTime = req.startTime;
            var endTime = req.endTime;
            if (startTime != null && endTime != null)
            {
                if (Convert.ToDateTime(endTime) < Convert.ToDateTime(startTime))
                {
                    return res;
                }
            }

            var tableNames = DeviceInfoOper.Instance.GetHisTableNamesByUserLastNameAndCustomerNo(req.lastName, req.customerNo);
            if (tableNames.Count == 0)
                return res;
            else
            {
                var selectUnion = HisOper.Instance.GetDBUnion(tableNames);
                res = HisOper.Instance.GetList2(req, size, tableNames);
            }
            return res;
            //var str = HisOper.Instance.GetDBUnion(sameIds);
        }

        /// <summary>
        /// 由登录账号获取相关的设备视图列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DeviceView> GetDeviceByUser(AllInOne_UserInfo user)
        {
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = AllInOne_UserInfoOper.Instance.GetFatherLastCId(user);
            else
                lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            return DeviceInfoOper.Instance.GetDeviceViews(lastCId);
        }

        /// <summary>
        /// 获取低一级的账号，不管是员工还是子账号。
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetSon(AllInOne_UserInfo user)
        {
            return AllInOne_UserInfoOper.Instance.GetSonViews(user);

        }

        /// <summary>
        /// 获取某个表的最新n条报警记录
        /// </summary>
        /// <param name="meterNo"></param>
        /// <returns></returns>
        public List<AllInOne_AlarmInfo> GetAlarmByMeterNo(int meterNo, AllInOne_UserInfo user)
        {
            return AllInOne_AlarmInfoOper.Instance.GetByMeterNo(meterNo, user);
        }

        /// <summary>
        /// 获取小区列表信息
        /// </summary>
        /// <returns></returns>
        public List<EstateInfo> GetEstateList()
        {
            return EstateInfoOper.Instance.GetList();
        }

        /// <summary>
        /// 验证用户的密码对不对
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool ConfirmUser(AllInOne_UserInfo user, string pwd)
        {
            if (user.pwd != MD5Helper.Instance.StrToMD5_UTF8(pwd))
                return false;
            return true;
        }

        /// <summary>
        /// 为Excel获取用量的总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<StdSumReport> GetStdSumExcelCount(GetStdSumReq req)
        {
            if (req.lastName != null)
                return HisOper.Instance.GetHisDataListForChartForCompany(req);
            else
                return HisOper.Instance.GetHisDataListForChart(req, "FM0000000001");
        }

        /// <summary>
        /// 为打印用量获取数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<List<string>> GetStdSumForPrint(GetStdSumReq req)
        {
            var list = GetStdSumExcelCount(req);
            var type = req.type;
            var format = "";
            switch (type)
            {
                case "year":
                    format = "yyyy-MM";
                    break;
                case "month":
                    format = "yyyy-MM-dd";
                    break;
                case "day":
                    format = "yyyy-MM-dd HH:mm:ss";
                    break;
                default:
                    break;
            }
            var r = new List<List<string>>();
            for (int i = 0; i < list.Count; i++)
            {
                var temp = new List<string> { list[i].dt.ToString(format), list[i].span };
                r.Add(temp);
            }
            return r;
        }

        /// <summary>
        /// 获取充值数据的列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<chargeReport> GetChargeExcelCount(GetChargeReportReq req)
        {
            if (req.lastName != null)
                return ICChargeRecordOper.Instance.GetLineListForCompany(req);
            else
                return ICChargeRecordOper.Instance.GetLineList(req);
        }

        /// <summary>
        /// 为打印充值记录获取数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<List<string>> GetChargeForPrint(GetChargeReportReq req)
        {
            var list = GetChargeExcelCount(req);
            var type = req.type;
            var format = "";
            switch (type)
            {
                case "year":
                    format = "yyyy-MM";
                    break;
                case "month":
                    format = "yyyy-MM-dd";
                    break;
                case "day":
                    format = "yyyy-MM-dd HH:mm:ss";
                    break;
                default:
                    break;
            }
            var r = new List<List<string>>();
            for (int i = 0; i < list.Count; i++)
            {
                //var temp = new List<string> { list[i].dt.ToString(format), list[i].sumVolume, list[i].sumMoney };
                var temp = new List<string> { list[i].dt.ToString(format), list[i].sumVolume };
                r.Add(temp);
            }
            return r;
        }

        /// <summary>
        /// 获取用户下所有的客户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<CustomerView> GetCustomerListByUser(AllInOne_UserInfo user)
        {
            return CustomerInfoOper.Instance.GetListByUser(user);
        }

        /// <summary>
        /// 由经度在页面的长度，计算缩放比例
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public int GetMapLevelForLng(decimal km)
        {
            var meter = km * 1000;
            if (meter / 20 < 40)
                return 19;
            if (meter / 50 < 40)
                return 18;
            if (meter / 100 < 40)
                return 17;
            if (meter / 200 < 40)
                return 16;
            if (meter / 500 < 40)
                return 15;
            if (meter / 1000 < 40)
                return 14;
            if (meter / 2000 < 40)
                return 13;
            if (meter / 5000 < 40)
                return 12;
            if (meter / 10000 < 40)
                return 11;
            if (meter / 20000 < 40)
                return 10;
            if (meter / 25000 < 40)
                return 9;
            if (meter / 50000 < 40)
                return 8;
            if (meter / 100000 < 40)
                return 7;
            if (meter / 200000 < 40)
                return 6;
            if (meter / 500000 < 40)
                return 5;
            if (meter / 1000000 < 40)
                return 4;
            if (meter / 2000000 < 40)
                return 3;
            if (meter / 5000000 < 40)
                return 2;
            else
                return 1;
        }

        /// <summary>
        /// 由纬度在页面的长度，计算缩放比例
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public int GetMapLevelForLat(decimal km)
        {
            var meter = km * 1000;
            if (meter / 20 < 16)
                return 19;
            if (meter / 50 < 16)
                return 18;
            if (meter / 100 < 16)
                return 17;
            if (meter / 200 < 16)
                return 16;
            if (meter / 500 < 16)
                return 15;
            if (meter / 1000 < 16)
                return 14;
            if (meter / 2000 < 16)
                return 13;
            if (meter / 5000 < 16)
                return 12;
            if (meter / 10000 < 16)
                return 11;
            if (meter / 20000 < 16)
                return 10;
            if (meter / 25000 < 16)
                return 9;
            if (meter / 50000 < 16)
                return 8;
            if (meter / 100000 < 16)
                return 7;
            if (meter / 200000 < 16)
                return 6;
            if (meter / 500000 < 16)
                return 5;
            if (meter / 1000000 < 16)
                return 4;
            if (meter / 2000000 < 16)
                return 3;
            if (meter / 5000000 < 16)
                return 2;
            else
                return 1;
        }

        /// <summary>
        /// 防止class中tds-xx之类的越来越多
        /// </summary>
        /// <param name="listSort"></param>
        /// <returns></returns>
        public List<string> ClearListSorts(List<string> listSort)
        {
            if (listSort != null)
            {
                for (int i = 0; i < listSort.Count; i++)
                {
                    if (listSort[i].Contains("th-sort sorting "))
                        listSort[i] = "th-sort sorting ";
                    else if (listSort[i].Contains("th-sort sorting_asc"))
                        listSort[i] = "th-sort sorting_asc";
                    else if (listSort[i].Contains("th-sort sorting_desc"))
                        listSort[i] = "th-sort sorting_desc";
                }
            }
            return listSort;
        }

        /// <summary>
        /// 获取已处理报警和未处理报警的条数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Counts> GetAlarmCount(AllInOne_UserInfo user)
        {
            var r = AllInOne_AlarmInfoOper.Instance.GetSafeAndAlarmCountMonth(user);
            //var r = AllInOne_AlarmInfoOper.Instance.GetSafeAndAlarmCount(user);
            var list = new List<Counts> {
                new Counts{ counts ="0"},
                 new Counts{ counts ="0"}
            };
            for (int i = 0; i < r.Count; i++)
            {
                list[i].counts = r[i].counts;
            }
            return list;
        }

        /// <summary>
        /// 获取子账号的列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserLv GetUserSelects(AllInOne_UserInfo user)
        {
            var r = new UserLv(user);
            if (user.level == 100)
            {
                var userLvList2 = new List<UserLv>();
                var list = AllInOne_UserInfoOper.Instance.GetSonCompany(user);
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var temp = new UserLv(item);
                        userLvList2.Add(temp);
                    }
                    var secondFirstUser = list.First();
                    var list2 = AllInOne_UserInfoOper.Instance.GetSonCompany(secondFirstUser);
                    if (list2.Count > 0)
                    {
                        var userLvList3 = new List<UserLv>();
                        foreach (var item in list2)
                        {
                            var temp = new UserLv(item);
                            userLvList3.Add(temp);
                        }
                        userLvList2.First().list = userLvList3;
                    }
                    r.list = userLvList2;
                }
            }
            else if (user.level == 99)
            {
                var userLvList2 = new List<UserLv>();
                var list = AllInOne_UserInfoOper.Instance.GetSonCompany(user);
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var temp = new UserLv(item);
                        userLvList2.Add(temp);
                    }
                    //var secondFirstUser = list.First();
                    //var list2 = AllInOne_UserInfoOper.Instance.GetSonCompany(secondFirstUser);
                    //if (list2.Count > 0)
                    //{
                    //    var userLvList3 = new List<UserLv>();
                    //    foreach (var item in list2)
                    //    {
                    //        var temp = new UserLv(item);
                    //        userLvList3.Add(temp);
                    //    }
                    //    userLvList2.First().list = userLvList3;
                    //}
                    r.list = userLvList2;
                }
            }
            //else if (user.level == 98) {//分公司应该没什么好筛选了吧，就这样了

            //}
            return r;
        }
        #endregion

        public int GetLastAlarmId(AllInOne_UserInfo user)
        {
            var list = AllInOne_AlarmInfoOper.Instance.GetNewAlarm(user, 0);
            var lastAlarmId = 0;
            if (list.Count == 0)
                return lastAlarmId;
            return list.OrderByDescending(p => p.Id).First().Id;
        }

    }
}