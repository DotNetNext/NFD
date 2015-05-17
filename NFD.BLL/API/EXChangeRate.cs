using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Utility;
using COM.Extension;
using NFD.Entities.Common;
namespace NFD.BLL.API
{
    public class EXChangeRate
    {
        /// <summary>
        /// 获取美元汇率
        /// </summary>
        public static decimal USD()
        {
            try
            {
                string key=PubConst.CACHE_EXCHANGERATE_USD;
                var cm = CacheManager<string, decimal>.GetInstance();
                if (cm.ContainsKey(key)) return cm[key];
                string url = "http://api.k780.com:88/?app=finance.rate&scur=USD&tcur=CNY&appkey=10003&sign=b59bc3ef6191eb9f747dd4e83c99f2a4&format=json";
                var rate = Convert.ToDecimal( RequestHelper.GetPageSource(url).ToModel<EXChangeRateModel>().result.rate.ToString("n2"));
                cm.Insert(key,rate,60*60*24);
                return rate;
            }
            catch (Exception)
            {
                return Convert.ToDecimal(6.2);
            }

        }
    }

    public class EXChangeRateModel
    {
        public APIresult result { get; set; }
    }

    public class APIresult
    {
        public decimal rate { get; set; }

    }
}
