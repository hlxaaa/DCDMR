using System.Text;
using Common.Helper;
using Common.Extend;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using DbOpertion.Models;
using HHTDCDMR.Models.Extend.Req;

namespace DbOpertion.DBoperation
{
    public partial class AllInOne_OperateRecordOper : SingleTon<AllInOne_OperateRecordOper>
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<AllInOne_OperateRecordView> GetList(OperRecordReq req, int size, AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);
            var search = req.search ?? "";
            var order = req.orderField;
            var desc = Convert.ToBoolean(req.isDesc);
            var index = Convert.ToInt32(req.pageIndex);
            //var size = 5;
            var orderStr = $"order by {order} ";
            if (desc)
                orderStr += " desc ";
            else
                orderStr += " asc ";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                          { "@lastcid", lastCId },
            };

            var condition = " 1=1  and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid)   ";
            if (!search.IsNullOrEmpty())
                condition += " and (operatorName like @search or content like @search)";
            return SqlHelper.Instance.GetViewPaging<AllInOne_OperateRecordView>("AllInOne_OperateRecordView", "select * from AllInOne_OperateRecordView ", condition, index, size, orderStr, dict);
        }

        /// <summary>
        /// 获取区域列表总数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int GetCount(OperRecordReq req, AllInOne_UserInfo user)
        {
            var lastCId = AllInOne_UserInfoOper.Instance.GetLastCId(user);//-txy 迷
            var search = req.search ?? "";
            var dict = new Dictionary<string, string>
            {
                { "@search", $"%{search}%" },
                          { "@lastcid", lastCId },
            };

            var condition = " 1=1   and (cid1=@lastcid or cid2=@lastcid or cid3=@lastcid or cid4=@lastcid) ";
            if (!search.IsNullOrEmpty())
                condition += " and (operatorName like @search or content like @search)";
            var list = SqlHelper.Instance.GetDistinctCount<AllInOne_OperateRecordView>("AllInOne_OperateRecordView", condition, dict);
            return list.Count;
        }

   

    }
}
