using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Business.Mail;
using Surat.Base.Application;
using Surat.Common.Data;
using Surat.Base.Log;
using Surat.Common.ViewModel;
using Surat.Base.Mail;
using Surat.Base.Repositories;
using Surat.Common.Utilities;
using Surat.Base;
using System.Data.Entity.Validation;
using Surat.Common.Log;
using Surat.Common.Application;
using Surat.Common.Mail;
using Surat.Common.Globalization;

namespace Surat.Business.Log
{
    public class ExceptionManager
    {
        #region Constructor

        public ExceptionManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;  
            this.logProvider = ExceptionLogProviderFactory.GetNewExceptionLogProvider(this.ApplicationContext, this, Constants.Log.ExceptionToDB);
        }

        #endregion

        #region Private Members

        private IFrameworkManager frameworkManager;
        private FrameworkContext applicationContext;
        private ITraceManager traceManager;
        private IMailManager mailManager;
        private IGlobalizationManager globalizationManager;
        private IExceptionLogProvider logProvider;
        private ExceptionLogRepository exceptionLogRepository;        

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

        public ITraceManager Trace
        {
            get
            {
                if (traceManager == null)
                    traceManager = this.ApplicationContext.FrameworkManager.GetTraceManager();

                return traceManager;
            }
        }

        public IGlobalizationManager Globalization
        {
            get
            {
                if (globalizationManager == null)
                    globalizationManager = this.ApplicationContext.FrameworkManager.GetGlobalizationManager();

                return globalizationManager;
            }
        }

        public IMailManager Mail
        {
            get
            {
                if (mailManager == null)
                    mailManager = this.ApplicationContext.FrameworkManager.GetMailManager();

                return mailManager;
            }
        }

        public IExceptionLogProvider LogProvider
        {
            get
            {
                return logProvider;
            }
        }

        public LogContext Context
        {
            get
            {
                return this.ApplicationContext.Log;
            }
        }        

        #endregion

        #region Repository

        public ExceptionLogRepository ExceptionLogRepository
        {
            get
            {
                if (exceptionLogRepository == null)
                    exceptionLogRepository = new ExceptionLogRepository(this.Context);

                return exceptionLogRepository;
            }
        }

        #endregion

        #region Methods

        public List<ExceptionView> GetExceptionsList()
        {
            try
            {
                List <ExceptionView> exceptions = null;

                exceptions = (from exceptionsLog in this.Context.ApplicationContext.DBContext.ExceptionLogs
                              join systems in this.Context.ApplicationContext.DBContext.Systems on exceptionsLog.SystemId equals systems.Id
                              select new ExceptionView()
                              {
                                  Id = exceptionsLog.Id,
                                  LogDate = exceptionsLog.LogDate,
                                  SystemName = systems.Name,
                                  ExceptionLevel = exceptionsLog.ExceptionLevel,
                                  ExceptionType = exceptionsLog.ExceptionType,
                                  Data = exceptionsLog.Data,
                                  ApplicationName = exceptionsLog.ApplicationName,
                                  ApplicationBaseType = exceptionsLog.ApplicationBaseType,
                                  HostName = exceptionsLog.HostName,
                                  Source = exceptionsLog.Source,
                                  Message = exceptionsLog.Message,
                                  StatusCode = exceptionsLog.StatusCode,
                                  InsertUserName = exceptionsLog.InsertUserName,
                                  AllXml = exceptionsLog.AllXml
                              }).ToList();

                return exceptions;
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "GetExceptionsList", this.ApplicationContext.SystemId, exception);
            }
        }


        public void SaveException(ExceptionLog exceptionlogItem)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                this.ExceptionLogRepository.Add(exceptionlogItem);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch(Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "SaveException", this.ApplicationContext.SystemId,exception);
            }
        }
        public string Publish(ApplicationContextBase context,Exception exception,UserDetailedView currentUser)
        {
            ExceptionLog exceptionlogItem;
            string shortExceptionMessage;

            try
            {                
                exceptionlogItem = GetExceptionLogItem(context, exception, currentUser);
                shortExceptionMessage = exceptionlogItem.Data;

                exceptionlogItem.Data = exceptionlogItem.Data + " " + exception.ToString();
                
                
                HandleWriteExceptionLog(exceptionlogItem);
            }
            catch
            {
                shortExceptionMessage = this.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.Message.ExceptionNotPublished);
            }

            return shortExceptionMessage;
        }

        private ExceptionLog GetExceptionLogItem(ApplicationContextBase context,Exception exceptionToLog, UserDetailedView currentUser)
        { 
            //To do : Inner exception üzerinden hiyerarşik olarak, tüm exceptionların bulunması ve loglanması
            StringBuilder exceptionData = new StringBuilder();
            ExceptionBase platformException;
            string entityValidationErrors = string.Empty;
            ExceptionLog exceptionlogItem = new ExceptionLog();

            if (exceptionToLog.GetType().BaseType == typeof(ExceptionBase))
            {
                platformException = (ExceptionBase)exceptionToLog;
                exceptionData.AppendLine(platformException.ParameterName);    
 
                if (string.IsNullOrEmpty(platformException.CustomMessage))
                    exceptionData.AppendLine(platformException.CustomMessage);

                if (string.IsNullOrEmpty(platformException.StandartMessage))
                    exceptionData.AppendLine(platformException.StandartMessage);

                exceptionlogItem.SystemId = platformException.SystemId;
            }
            else 
            {
                exceptionlogItem.SystemId = context.SystemId;
                exceptionlogItem.Data = exceptionToLog.Message;
            
            };

            exceptionlogItem.InsertUserName = currentUser == null ? "": currentUser.Name;
            exceptionlogItem.HostName = context.MachineName;
            exceptionlogItem.AllXml = "";
            exceptionlogItem.ApplicationBaseType = context.ApplicationBaseType;
            exceptionlogItem.Message = exceptionToLog.Message;
            exceptionlogItem.Source = exceptionToLog.Source;
            exceptionlogItem.StatusCode = 0;
            exceptionlogItem.ApplicationName = context.ApplicationName;
            exceptionData.AppendLine(GetInnerExceptionMessages(exceptionToLog));
            exceptionlogItem.Data = exceptionData.ToString();            
            exceptionlogItem.ExceptionType = exceptionToLog.GetType().Name;
            exceptionlogItem.LogDate = TimeUtility.GetCurrentDateTime();
            exceptionlogItem.ApplicationVariables = context.CurrentVariables;

            if (currentUser != null)
            {
                exceptionlogItem.InsertedByUser = currentUser.UserId;
                exceptionlogItem.InsertUserName = currentUser.Name;
            }
            else
            {
                exceptionlogItem.InsertedByUser = 0;
                exceptionlogItem.InsertUserName =  "";
            }

            exceptionlogItem.AllXml = SerializeUtility.SerializeToXML(context.CurrentVariables);

            return exceptionlogItem;
        }

        private string GetInnerExceptionMessages(Exception exception)
        {
            Exception currentException = exception;
            StringBuilder messages = new StringBuilder();

            do
            {
                if (IsEntityValidationException(currentException))
                {
                    string entityValidationErrors = ExceptionUtility.GetEntityValidationErrors((DbEntityValidationException)currentException);

                    if (!string.IsNullOrEmpty(entityValidationErrors))
                        messages.AppendLine(currentException.GetType().Name + " " + entityValidationErrors);
                }

                messages.AppendLine(currentException.Message);
                currentException = currentException.InnerException;
            } while (currentException != null);
            

            return messages.ToString();
        }

        private bool IsEntityValidationException(Exception exception)
        {
            bool result = false;

            if (exception.GetType() == typeof(DbEntityValidationException))
                result = true;     

            return result;
        }     

        private void HandleWriteExceptionLog(ExceptionLog exceptionlogItem)
        {
            IExceptionLogProvider internalLogProvider; 

            try
            {
                //var a = 3;
                //var b = 0;
                //var c = a / b;

                logProvider.WriteExceptionLog(exceptionlogItem);  //DB Write            
            }
            catch
            {     
                //Try to write to file
                try
                {
                    //var a = 3;
                    //var b = 0;
                    //var c = a / b;

                    internalLogProvider = ExceptionLogProviderFactory.GetNewExceptionLogProvider(this.ApplicationContext, this, Constants.Log.ExceptionToFile);
                    internalLogProvider.WriteExceptionLog(exceptionlogItem);
                }
                catch
                {
                    MailMessage exceptionMail = MailTemplateFactory.GetNewExceptionEMailTemplate(this.ApplicationContext,exceptionlogItem.Id,exceptionlogItem.InsertedByUser.ToString(),exceptionlogItem.LogDate,exceptionlogItem.Data);
                    this.Mail.Send(exceptionMail);  
                }
            }
        }

    }



#endregion

}
