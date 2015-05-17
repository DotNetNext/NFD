using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace COM.Utility
{
    /// <summary>     
    /// 1. 功能：缓存操作公共类
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>     
    /// <typeparam name="K">Key</typeparam>     
    /// <typeparam name="V">Value</typeparam>     
    public class CacheManager<K, V>
    {

        #region 全局变量
        private static CacheManager<K, V> _instance = null;
        private static readonly object _instanceLock = new object();
        #endregion

        #region 构造函数
             
        private CacheManager() { }
        #endregion

        #region  属性
        /// <summary>         
        /// Gets the <see cref="V"/> with the specified key.         
        /// </summary>         
        /// <value></value>      
        public V this[K key]
        {
            get { return (V)HttpRuntime.Cache[CreateKey(key)]; }
        }
        #endregion

        #region 公共方法
        	
        /// <summary>         
        /// Determines whether the specified key contains key.         
        /// </summary>         
        /// <param name="key">The key.</param>         
        /// <returns> /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.        /// /// </returns>         
        public bool ContainsKey(K key)
        {
            return HttpRuntime.Cache[CreateKey(key)] != null;
        }

        /// <summary>         
        /// 获取缓存值         
        /// </summary>         
        /// <param name="key">key</param>         
        /// <returns></returns>         
        public V Get(K key)
        {
            return (V)HttpRuntime.Cache.Get(CreateKey(key));
        }

        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>         
        public static CacheManager<K, V> GetInstance()
        {
            if (_instance == null)
                lock (_instanceLock)
                    if (_instance == null)
                        _instance = new CacheManager<K, V>();
            return _instance;
        }

        /// <summary>         
        /// 插入缓存        
        /// </summary>         
        /// <param name="key"> key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        public void Insert(K key, V value, int cacheDurationInSeconds)
        {
            Insert(key, value, cacheDurationInSeconds, CacheItemPriority.Default);
        }

        /// <summary>         
        /// 插入缓存.         
        /// </summary>         
        /// <param name="key">key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        /// <param name="priority">缓存项属性</param>         
        public void Insert(K key, V value, int cacheDurationInSeconds, CacheItemPriority priority)
        {
            string keyString = CreateKey(key);
            HttpRuntime.Cache.Insert(keyString, value, null, DateTime.Now.AddSeconds(cacheDurationInSeconds), Cache.NoSlidingExpiration, priority, null);
        }

        /// <summary>         
        /// 删除缓存         
        /// </summary>         
        /// <param name="key">key</param>         
        public void Remove(K key)
        {
            HttpRuntime.Cache.Remove(CreateKey(key));
        }

         		
        /// <summary>         
        ///创建KEY   
        /// </summary>         
        /// <param name="key">Key</param>         
        /// <returns></returns>         
        private static string CreateKey(K key)
        {
            return key + typeof(V).ToString() + key.GetHashCode();
        }


        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {

            System.Web.Caching.Cache _cache = HttpRuntime.Cache;

            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();

            ArrayList al = new ArrayList();

            while (CacheEnum.MoveNext())
            {

                al.Add(CacheEnum.Key);

            }

            foreach (string key in al)
            {

                _cache.Remove(key);

            }

            show();

        }


        /// <summary>
        /// 清除所有包含关键字的缓存
        /// </summary>
        /// <param name="removeKey">关键字</param>
        public static void RemoveAllCacheByKey(string removeKey)
        {

            System.Web.Caching.Cache _cache = HttpRuntime.Cache;

            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();

            ArrayList al = new ArrayList();

            while (CacheEnum.MoveNext())
            {

                al.Add(CacheEnum.Key);

            }

            foreach (string key in al)
            {

                if (key.Contains(removeKey))
                {
                    _cache.Remove(key);
                }

            }

            show();

        }

        //显示所有缓存
        public static string show()
        {

            string str = "";

            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (CacheEnum.MoveNext())
            {

                str += "缓存名<b>[" + CacheEnum.Key + "]</b><br />";

            }

            return "当前网站总缓存数:" + HttpRuntime.Cache.Count + "<br />" + str;

        }
        #endregion

    }
}
