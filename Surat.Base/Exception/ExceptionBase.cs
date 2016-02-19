using Surat.Base.Application;
using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Surat.Base.Exceptions
{
    public class ExceptionBase : Exception
    {

        #region Constructors
   
        public ExceptionBase(FrameworkContext context,int systemId, string customMessage, ExceptionLevel exceptionLevel, Exception innerException)
            : base("", innerException)
        {
            this.context = context;
            this.SystemId = systemId;
            this.customMessage = customMessage;
            this.Level = exceptionLevel;
        }

        #endregion

        #region Private Members

        private FrameworkContext context;
        private int systemId;
        private ExceptionLevel level;
        private String standartMessage;
        private String customMessage;
        private String parameterName;

        #endregion

        #region Public Members

        public FrameworkContext Context
        {
            get { return context; }
            set { context = value; }
        }

        public int SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }

        public ExceptionLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        public string CustomMessage
        {
            get 
            {                
                return customMessage;
            }
        }

        public String StandartMessage
        {
            get
            {
                return StandartMessage;
            }
            set { standartMessage = value; }
        }      

        public override String Message
        {
            get { return GetExceptionMessage(); }
           
        }

        public String ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }        

        #endregion

        #region Methods

        private string GetExceptionMessage()
        {
            string message = string.Empty;

            if (!String.IsNullOrEmpty(customMessage))           
                message = this.CustomMessage + " " + this.StandartMessage + " " + base.Message;
            else message = this.StandartMessage + " " + base.Message;  

            return message;
        }

        #endregion

    }
}
