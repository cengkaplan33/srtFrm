using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surat.Base.Configuration;
using Surat.Common.Utilities;
using Surat.Base.Application;
using Surat.Base.Model;
using Surat.Base.Exceptions;
using Surat.Common.ViewModel;
using Surat.Base.Repositories;
using System.Runtime.Caching;

namespace Surat.Base.Cache

{
    public class CacheUtility
    {
        #region Private members  
        
        private static ObjectCache cache = MemoryCache.Default; 

        #endregion

        #region Methods 
      
        public static object GetCachedObject(string cacheKeyName)
        {
            object cachedObject;

            cachedObject = cache[cacheKeyName];

            return cachedObject;
        }

        public static void SetObjectInCache(string cacheKeyName, object objectToCache)
        {
            CacheItemPolicy policy = new CacheItemPolicy();

            policy.AbsoluteExpiration = TimeUtility.GetCurrentDateTime().AddMinutes(1); //ToDo : Parametre
            cache.Set(cacheKeyName, objectToCache, policy);
        }

        public static void RemoveCachedObject(string cacheKeyName)
        {
            if (cache.Contains(cacheKeyName))
                cache.Remove(cacheKeyName);
        }

        public static void ResetCache()
        {
            List<string> cacheKeys = cache.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                cache.Remove(cacheKey);
            }
        }

        #endregion
    }
}
