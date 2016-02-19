using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Surat.Common.Data;
using Surat.Base.Application;

namespace Surat.Base.Exceptions
{
    public class WrongPasswordException : ExceptionBase
    {
        #region Constructors

        public WrongPasswordException(FrameworkContext context, string parameterName, int systemId)
            : this(context, parameterName, systemId, "", null)
        {

        }
        public WrongPasswordException(FrameworkContext context, string parameterName, int systemId, Exception innerException)
            : this(context, parameterName, systemId, "", innerException)
        {
            
        }
        public WrongPasswordException(FrameworkContext context, string parameterName, int systemId, string customMessage)
            : this(context, parameterName, systemId, customMessage, null)
        {

        }
        public WrongPasswordException(FrameworkContext context, string parameterName, int systemId, string customMessage, Exception innerException)
            : base(context, systemId, customMessage, ExceptionLevel.Error, innerException)
        {
            this.ParameterName = parameterName;
            if (string.IsNullOrEmpty(customMessage))
                if (context != null)
                    this.StandartMessage = context.Globalization.GetGlobalizationKeyValue(systemId, Constants.ExceptionType.WrongPassword);
                else this.StandartMessage = Constants.ExceptionType.WrongPassword;
        }

        #endregion
    }
}
