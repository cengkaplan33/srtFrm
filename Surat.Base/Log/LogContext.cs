using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surat.Base.Application;
using Surat.Common.Data;
using Surat.Base.Model;
using Surat.Common.Application;
using Surat.Common.Log;

namespace Surat.Base.Log
{
    public class LogContext : IDisposable
    {
        #region Constructor

        public LogContext(IFrameworkManager frameworkManager)
        { 
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private String exceptionFolderName;
        private String traceFolderName;
        private TraceLevel traceLevel;
        private Int16 traceBufferSizeAsKbyte;

        #endregion

        #region Public Members

        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.FrameworkManager.GetApplicationContext();
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

        public String TraceFolderName
        {
            get { return traceFolderName; }
            set { traceFolderName = value; }
        }

        public String ExceptionFolderName
        {
            get { return exceptionFolderName; }
            set { exceptionFolderName = value; }
        }

        public TraceLevel TraceLevel
        {
            get { return traceLevel; }
            set { traceLevel = value; }
        }

        public Int16 TraceBufferSizeAsKbyte
        {
            get { return traceBufferSizeAsKbyte; }
            set { traceBufferSizeAsKbyte = value; }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

    }
}
