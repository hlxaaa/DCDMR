using System.Text;
using Common.Helper;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using System.Data.SqlClient;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_UserInfoOper : SingleTon<AllInOne_UserInfoOper>
    {
        /// <summary>
        /// 用户的cids，补全
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public AllInOne_UserInfo CompleteCIds(AllInOne_UserInfo user, AllInOne_UserInfo pUser)
        {
            if (pUser.cId1 != null)
                user.cId1 = pUser.cId1;
            else
            {
                user.cId1 = $"{pUser.id}-{DateTime.Now.ToString("yyyyMMddHHMmmss")}";
                return user;
            }
            if (pUser.cId2 != null)
                user.cId2 = pUser.cId2;
            else
            {
                user.cId2 = $"{pUser.id}-{DateTime.Now.ToString("yyyyMMddHHMmmss")}";
                return user;
            }
            if (pUser.cId3 != null)
                user.cId3 = pUser.cId3;
            else
            {
                user.cId3 = $"{pUser.id}-{DateTime.Now.ToString("yyyyMMddHHMmmss")}";
                return user;
            }
            if (pUser.cId4 != null)
                user.cId4 = pUser.cId4;
            else
            {
                user.cId4 = $"{pUser.id}-{DateTime.Now.ToString("yyyyMMddHHMmmss")}";
                return user;
            }
            return user;
        }

        /// <summary>
        /// 获取用户的最后一个cid
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public String GetLastCId(AllInOne_UserInfo user)
        {
            if (user.cId4 != null)
                return user.cId4;
            if (user.cId3 != null)
                return user.cId3;
            if (user.cId2 != null)
                return user.cId2;
            return user.cId1;
        }

        /// <summary>
        /// 员工账号获取公司的cid
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetFatherLastCId(AllInOne_UserInfo user)
        {
            if (user.cId4 != null)
                return user.cId3;
            if (user.cId3 != null)
                return user.cId2;
            if (user.cId2 != null)
                return user.cId1;
            return "impossible";
        }

        /// <summary>
        /// 如果是员工，则找他公司的范围
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string DeviceGetLastCID(AllInOne_UserInfo user)
        {
            var lastCId = "";
            if ((bool)user.isStaff)
                lastCId = GetFatherLastCId(user);
            else
                lastCId = GetLastCId(user);
            return lastCId;
        }

        /// <summary>
        /// 获取子账号
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<AllInOne_UserInfo> GetSon(AllInOne_UserInfo user)
        {
            var parentId = user.id;
            return SqlHelper.Instance.GetByCondition<AllInOne_UserInfo>($" parentId ={parentId} and isdeleted=0 ");
        }

        /// <summary>
        /// 获取子公司的账号
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<AllInOne_UserInfo> GetSonCompany(AllInOne_UserInfo user)
        {
            var parentId = user.id;
            return SqlHelper.Instance.GetByCondition<AllInOne_UserInfo>($" parentId ={parentId} and isdeleted=0 and isStaff=0 ");
        }

        /// <summary>
        /// 获取自己范围内的操作者，包括自己
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<AllInOne_UserInfo> GetMyOpertor(AllInOne_UserInfo user)
        {
            var list = GetSon(user);
            list.Add(user);
            return list;
        }

        /// <summary>
        /// 获取子账号的视图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<AllInOne_UserPermissionView> GetSonViews(AllInOne_UserInfo user)
        {
            var parentId = user.id;
            return SqlHelper.Instance.GetByCondition<AllInOne_UserPermissionView>($" isStaff=0 and parentId ={parentId} and isdeleted=0 ");
        }

        /// <summary>
        /// 由用户id获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AllInOne_UserInfo GetById(int userId)
        {
            return SqlHelper.Instance.GetById<AllInOne_UserInfo>("AllInOne_UserInfo", userId);
        }

        public List<AllInOne_UserInfo> GetSonsByFatherName(string name, AllInOne_UserInfo user)
        {
            var lastCId = GetLastCId(user);

            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastCId },
                  { "@name", name },
            };
            string str = $@"select * from AllInOne_UserInfo where parentId in (
select id from AllInOne_UserInfo where name=@name and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid))";
            return SqlHelper.Instance.ExecuteGetDt<AllInOne_UserInfo>(str, dict);
        }

        /// <summary>
        /// 根据名字获取最后一个cid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetLastCIdByName(string name)
        {
            var dict = new Dictionary<string, string>
            {
                { "@name", name }
            };
            string str = $"select * from AllInOne_UserInfo where name=@name and isdeleted=0 ";
            var list = SqlHelper.Instance.ExecuteGetDt<AllInOne_UserInfo>(str, dict);
            if (list.Count > 0)
            {
                return GetLastCId(list.First());
            }
            return null;
        }

        /// <summary>
        /// 获取自己及子孙的账号、员工
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<AllInOne_UserInfo> GetSonAndGrandSon(AllInOne_UserInfo user)
        {
            var lastcid = GetLastCId(user);
            var dict = new Dictionary<string, string>
            {
                  { "@lastcid", lastcid },
            };
            var str = $" select * from AllInOne_UserInfo where isdeleted=0 and  (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            return SqlHelper.Instance.ExecuteGetDt<AllInOne_UserInfo>(str, dict);
        }
    }
}
