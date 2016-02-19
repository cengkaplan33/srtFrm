using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Business.Log
{
    public class ExceptionLogProviderFactory
    {

        #region Methods

        public static IExceptionLogProvider GetNewExceptionLogProvider(FrameworkContext applicationContext,ExceptionManager exceptionManager,String logProviderName)
        {
            IExceptionLogProvider logProvider = null;

            switch (logProviderName)
            {
                case "Log.ExceptionToDB":
                    logProvider = new DBExceptionLogProvider(applicationContext, exceptionManager);
                    break;
                case "Log.ExceptionToFile":
                    logProvider = new FileExceptionLogProvider(applicationContext);
                    break;
                default:
                    throw new InvalidTypeException(applicationContext,"ExceptionLogProvider",applicationContext.SystemId,
                        string.Format(applicationContext.Globalization.GetGlobalizationKeyValue(applicationContext.SystemId, Constants.ExceptionType.InvalidType), logProviderName));          
            }

            return logProvider;
        }

        #endregion       

    }
}
