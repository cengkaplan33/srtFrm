using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Surat.Base.Application;
using Surat.Base.Model.Entities;

namespace Surat.Business.Log
{
    public class DBExceptionLogProvider : IExceptionLogProvider
    {      

        #region Constructor

        public DBExceptionLogProvider(FrameworkContext applicationContext, ExceptionManager exceptionManager)
        {
            this.applicationContext = applicationContext;
            this.exceptionManager = exceptionManager;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private ExceptionManager exceptionManager;

        #endregion

        #region IExceptionLogProvider Members

        public void WriteExceptionLog(ExceptionLog exceptionlogItem)
        {
            exceptionManager.SaveException(exceptionlogItem);
        }

        #endregion
    }
}
