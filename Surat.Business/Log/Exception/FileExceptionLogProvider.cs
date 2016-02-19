using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Surat.Base.Model.Entities;
using Surat.Common.Utilities;
using Surat.Base.Application;

namespace Surat.Business.Log
{
    public class FileExceptionLogProvider : IExceptionLogProvider
    {
        #region Constructor

        public FileExceptionLogProvider(FrameworkContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;

        #endregion

        #region IExceptionLogProvider Members

        public void WriteExceptionLog(ExceptionLog exceptionlogItem)
        {
            string data;
            try
            {
                data = SerializeUtility.SerializeToXML(exceptionlogItem);
                FileUtility.WriteFile(GetExceptionFilePath(), GetExceptionFileName(), data, false);
            }
            catch //To do : Ele alınmalı
            {

            }
        }

        #endregion

        #region Methods

        private string GetExceptionFilePath()
        {
            string exceptionFilePath;

            exceptionFilePath = applicationContext.System.ApplicationRootFolderPath +
                                applicationContext.Log.ExceptionFolderName + "\\" +
                                applicationContext.SystemName + "\\";

            return exceptionFilePath;
        }

        private string GetExceptionFileName()
        {
            string exceptionFileName;

            exceptionFileName = TimeUtility.GetCurrentDateTime().ToString("yyyyMMdd-hhmmss.fff") + "-" + "Exception.txt";

            return exceptionFileName;
        }

        #endregion
    }
}
