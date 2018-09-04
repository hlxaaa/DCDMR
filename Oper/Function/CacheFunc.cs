using BeIT.MemCached;
using Common;
using Common.Helper;
using HHTDCDMR.Oper.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHTDCDMR4._5.Oper.Function
{
    public class CacheFunc : SingleTon<CacheFunc>
    {
        public MemcachedClient cache = MemCacheHelper.GetMyConfigInstance();

        public string SetWxToken()
        {
            var token = AllFunc.Instance.GetWXToken();
            cache.Set("WxToken", token, DateTime.Now.AddMinutes(110));
            return token;
        }

        public string GetWxToken()
        {
            var token = "";
            var temp = cache.Get("WxToken");
            if (temp == null)
                token = SetWxToken();
            else
                token = temp.ToString();
            return token;
        }
    }
}