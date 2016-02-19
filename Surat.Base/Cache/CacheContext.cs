using Surat.Base.Application;
using Surat.Base.Model;
using Surat.Common.Application;
using Surat.Common.Log;
using Surat.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Surat.Base.Cache
{
    public class CacheContext : IDisposable
    {
        #region Constructor

        public CacheContext(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private FrameworkDbContext dbContext;       

        #endregion

        #region Public Members  
      
        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext =(FrameworkContext)this.FrameworkManager.GetApplicationContext();

                return applicationContext;
            }
        }

        public IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
        }

        public ITraceManager Trace
        {
            get
            {
                return this.FrameworkManager.GetTraceManager();
            }
        }

        public FrameworkDbContext DBContext
        {
            get { return dbContext; }
            set { dbContext = value; }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

        #region Methods

        #endregion

    }
}
