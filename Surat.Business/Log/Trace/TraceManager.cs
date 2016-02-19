using Surat.Base.Application;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.Log;
using Surat.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Business.Log
{
    public class TraceManager : ITraceManager
    {
        #region Constructor

        public TraceManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;
            traceData = new StringBuilder();
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;        
        private IFrameworkManager frameworkManager;
        private static object lockTraceDataObject = new object();
        private static StringBuilder traceData;        

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

        #endregion

        #region Methods        

        private string GetTraceFileName()
        {
            string traceFileName;

            traceFileName = TimeUtility.GetCurrentDateTime().ToString("yyyyMMdd") + "-" + "Trace.txt";

            return traceFileName;
        }

        private string GetTraceFilePath()
        {
            string traceFilePath;

            traceFilePath = this.ApplicationContext.System.ApplicationRootFolderPath + this.ApplicationContext.Log.TraceFolderName + "\\";

            return traceFilePath;
        }     

        #endregion

        #region ITraceManager

        public void AppendLine(string systemName, string traceInformation, TraceLevel traceLevel)
        {
            if (this.ApplicationContext.Log.TraceLevel >= traceLevel)
            {
                if (traceData.Length > this.ApplicationContext.Log.TraceBufferSizeAsKbyte * 1024)
                {
                    lock (lockTraceDataObject)
                    {
                        WriteTraceToFile();
                    }
                }

                traceData.Append(TimeUtility.GetCurrentDateTime().ToString("yyyyMMdd-hhmmss.fff").PadRight(20));
                traceData.Append(systemName.PadRight(20));
                traceData.AppendLine(traceInformation);
            }
        }

        public void WriteTraceToFile()
        {
            traceData.AppendLine(TimeUtility.GetCurrentDateTime().ToString("yyyyMMdd-hhmmss.fff").PadRight(20) + "Trace data has been written.");
            FileUtility.WriteFile(GetTraceFilePath(), GetTraceFileName(), traceData.ToString(), true);
            traceData.Clear();
        }

        #endregion
    }
}
