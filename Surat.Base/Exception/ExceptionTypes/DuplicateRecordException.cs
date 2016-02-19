using Surat.Base.Application;
using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Exceptions
{
    public class DuplicateRecordException : ExceptionBase
    {
        #region Constructors

        public DuplicateRecordException(FrameworkContext context, string parameterName, int systemId)
            : this(context,parameterName, systemId, "", null)
        {

        }
        public DuplicateRecordException(FrameworkContext context, string parameterName, int systemId, Exception innerException)
            : this(context, parameterName, systemId, "", innerException)
        {
            
        }
        public DuplicateRecordException(FrameworkContext context, string parameterName, int systemId, string customMessage)
            : this(context, parameterName, systemId, customMessage, null)
        {

        }
        public DuplicateRecordException(FrameworkContext context, string parameterName, int systemId, string customMessage, Exception innerException)
            : base(context, systemId, customMessage, ExceptionLevel.Error, innerException)
        {
            this.ParameterName = parameterName;
            if (string.IsNullOrEmpty(customMessage))
                if (context != null)
                    this.StandartMessage = context.Globalization.GetGlobalizationKeyValue(systemId, Constants.ExceptionType.DBDuplicateKey);
                else this.StandartMessage = Constants.ExceptionType.DBDuplicateKey;
        }

        #endregion
    }
}
