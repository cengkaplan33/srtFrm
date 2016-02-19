using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Cache
{
    public interface ICacheManager
    {
        object GetCachedObject(string cacheKeyName);
        void SetObjectInCache(string cacheKeyName, object objectToCache);
        void RemoveCachedObject(string cacheKeyName);
    }
}
