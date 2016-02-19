using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Surat.Common.Data;
using Surat.Base.Application;

namespace Surat.Base.Exceptions
{
    public class SMTPMailException : ExceptionBase
    {
        #region Constructors

        public SMTPMailException(FrameworkContext context, int systemId)
            : this(context,systemId, "", null)
        {

        }
        public SMTPMailException(FrameworkContext context, int systemId, Exception innerException)
            : this(context, systemId, "", innerException)
        {
            
        }
        public SMTPMailException(FrameworkContext context, int systemId, string customMessage)
            : this(context, systemId, customMessage, null)
        {

        }
        public SMTPMailException(FrameworkContext context, int systemId, string customMessage, Exception innerException)
            : base(context, systemId, customMessage, ExceptionLevel.Error, innerException)
        {
            if (string.IsNullOrEmpty(customMessage))
                if (context != null)
                    this.StandartMessage = context.Globalization.GetGlobalizationKeyValue(systemId, Constants.ExceptionType.SMTPMail);
                else this.StandartMessage = Constants.ExceptionType.SMTPMail;
        }

        #endregion
    }
}
