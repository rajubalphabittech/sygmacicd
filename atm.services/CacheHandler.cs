using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace atm.services
{
    public static class CacheHandler
    {
        static readonly int CacheTime = WebConfigurationManager.AppSettings["CacheDuration"] == null ? 1 : int.Parse(WebConfigurationManager.AppSettings["CacheDuration"]);

        public static void Add<T>(T objInfo, string key)
        {
            HttpRuntime.Cache.Insert(key, objInfo, null, DateTime.Now.AddSeconds(CacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
        public static void Clear()
        {
            var enumerator = HttpRuntime.Cache.GetEnumerator();

            while (enumerator.MoveNext())
                HttpRuntime.Cache.Insert(enumerator.Key.ToString(), enumerator.Value,
                    null, DateTime.Now.AddSeconds(-1), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        public static bool Exists(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }
        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value =
                        default(T);
                    return false;
                }
                value = (T)HttpRuntime.Cache[key];
            }
            catch
            {
                value =
                    default(T);
                return false;
            }
            return true;
        }
    }
}