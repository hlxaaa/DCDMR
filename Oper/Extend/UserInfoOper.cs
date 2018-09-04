using Common.Helper;
using System.Collections.Generic;
using Common;
using DbOpertion.Models;
using System.Linq;

namespace DbOpertion.DBoperation
{
    public partial class UserInfoOper : SingleTon<UserInfoOper>
    {
        /// <summary>
        /// 根据账号获取某条记录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AllInOne_UserInfo GetByAccount(string account)
        {
            var dict = new Dictionary<string, string>
            {
                { "@account", account }
            };

            var list = SqlHelper.Instance.GetByCondition<AllInOne_UserInfo>("AllInOne_UserInfo", "account=@account and isdeleted=0 ", dict);
            return list.FirstOrDefault();
        }


        public void UpdateFactoryName(string name)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@name", name);
            var str = $" update factoryType set factoryName=@name where factoryNo='08' ";
            SqlHelper.Instance.ExcuteNon(str, dict);
        }
    }
}
