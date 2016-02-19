using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Surat.Base.Application;
using Surat.Base.Mail;
using Surat.Base.Cache;
using Surat.Common.Utilities;
using Surat.Common.Log;
using Surat.Common.Cache;
using Surat.Common.Application;

namespace Surat.Business.Cache
{
    public class CacheManager : ICacheManager
    {
        #region Constructor

        public CacheManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;  
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private ITraceManager traceManager;

        #endregion

        #region Public Members 
   
        public IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
        }
    
        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.FrameworkManager.GetApplicationContext();

                return applicationContext;
            }
        }

        public CacheContext Context
        {
            get
            {
                return this.applicationContext.Cache;
            }
        }

        public ITraceManager Trace
        {
            get
            {
                if (traceManager == null)
                    traceManager = this.ApplicationContext.FrameworkManager.GetTraceManager();

                return traceManager;
            }
        }

        #endregion        

        #region ICacheManager

        public object GetCachedObject(string cacheKeyName)
        {
            return CacheUtility.GetCachedObject(cacheKeyName);
        }

        public void SetObjectInCache(string cacheKeyName, object objectToCache)
        {
            CacheUtility.SetObjectInCache(cacheKeyName, objectToCache);
        }

        public void RemoveCachedObject(string cacheKeyName)
        {
            CacheUtility.RemoveCachedObject(cacheKeyName);
        }

        #endregion

        #region Methods  

        public void ResetCache()
        {
            CacheUtility.ResetCache();
        }

        #endregion           
    }
}
