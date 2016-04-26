using Surat.Base.ActiveDirectory;
using Surat.Base.Cache;
using Surat.Base.Configuration;
using Surat.Base.Exceptions;
using Surat.Base.Globalization;
using Surat.Base.Log;
using Surat.Base.Mail;
using Surat.Base.Model;
using Surat.Base.Security;
using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Application
{
    public class ApplicationContextFactory
    {
        #region Methods               

        public static SecurityContext GetNewSecurityContext(FrameworkContext context)
        {
            SecurityContext securityContext = new SecurityContext(context.FrameworkManager);

            securityContext.MaxWrongPasswordAttempts = ConfigurationUtility.GetParameter<byte>(context, "MaxWrongPasswordAttempts");
            securityContext.MinPasswordLength = ConfigurationUtility.GetParameter<byte>(context, "MinPasswordLength");
            securityContext.MaxPasswordChangePeriodAsDays = ConfigurationUtility.GetParameter<byte>(context, "MaxPasswordChangePeriodAsDays");
            securityContext.CookieExpirationTimeAsDays = ConfigurationUtility.GetParameter<byte>(context, "CookieExpirationTimeAsDays");
            securityContext.WebSessionTimeoutInMinutes = ConfigurationUtility.GetParameter<byte>(context, "WebSessionTimeoutInMinutes");
            securityContext.IsCaptchaActive = ConfigurationUtility.GetParameter<bool>(context, "IsCaptchaActive");
            securityContext.IsRememberMeActive = ConfigurationUtility.GetParameter<bool>(context, "IsRememberMeActive");

            return securityContext;
        }

        public static ActiveDirectoryContext GetNewActiveDirectoryContext(FrameworkContext context)
        {
            ActiveDirectoryContext adContext = new ActiveDirectoryContext(context.DBContext);

            //adContext.Service = "LDAP://172.24.0.100:389";
            //adContext.Domain = "Kaynakholding";
            //adContext.UserName = "kanalist2";
            //adContext.Password = "ka.112233";

            //adContext.Service = "LDAP://172.27.40.54:389";
            //adContext.Domain = "demo.local";
            //adContext.UserName = "serendiptest";
            //adContext.Password = "asdf.123456";

            adContext.Container = ConfigurationUtility.GetParameter<string>(context, "ActiveDirectoryContainer");
            adContext.Domain = ConfigurationUtility.GetParameter<string>(context, "ActiveDirectoryDomain");
            adContext.UserName = ConfigurationUtility.GetParameter<string>(context, "ActiveDirectoryUserName");
            adContext.Password = ConfigurationUtility.GetParameter<string>(context, "ActiveDirectoryUserPassword");
            return adContext;
        }

        public static LogContext GetNewLogContext(FrameworkContext context)
        {
            LogContext logContext = new LogContext(context.FrameworkManager);

            logContext.TraceFolderName = ConfigurationUtility.GetParameter<string>(context, "TraceFolderName");
            logContext.ExceptionFolderName = ConfigurationUtility.GetParameter<string>(context, "ExceptionFolderName");
            logContext.TraceLevel = (TraceLevel)ConfigurationUtility.GetParameter<byte>(context, "TraceLevel");
            logContext.TraceBufferSizeAsKbyte = ConfigurationUtility.GetParameter<Int16>(context, "TraceBufferSizeAsKbyte");

            return logContext;
        }

        public static MailContext GetNewMailContext(FrameworkContext context)
        {
            MailContext mailContext = new MailContext(context.FrameworkManager);

            mailContext.SMTPServerIP = ConfigurationUtility.GetParameter<string>(context, "SMTPServerIP");
            mailContext.SMTPServerPort = ConfigurationUtility.GetParameter<int>(context, "SMTPServerPort");
            mailContext.SMTPServerUser = ConfigurationUtility.GetParameter<string>(context, "SMTPServerUser");
            mailContext.SMTPServerUserPassword = ConfigurationUtility.GetParameter<string>(context, "SMTPServerUserPassword");
            mailContext.SMTPMailFrom = ConfigurationUtility.GetParameter<string>(context, "SMTPMailFrom");
            mailContext.SMTPEnableSSL = ConfigurationUtility.GetParameter<bool>(context, "SMTPEnableSSL");

            return mailContext;
        }

        public static ProductContext GetNewProductContext(FrameworkContext context)
        {
            ProductContext productContext = new ProductContext(context.DBContext);

            productContext.ProductName = ConfigurationUtility.GetParameter<string>(context, "ProductName");
            productContext.ProductVersion = ConfigurationUtility.GetParameter<string>(context, "ProductVersion");
            productContext.ProductVersionDate = ConfigurationUtility.GetParameter<string>(context, "ProductVersionDate");
            productContext.ProductLogoPath = ConfigurationUtility.GetParameter<string>(context, "ProductLogoPath");
            productContext.ProducerName = ConfigurationUtility.GetParameter<string>(context, "ProducerName");
            productContext.ProducerWebsite = ConfigurationUtility.GetParameter<string>(context, "ProducerWebsite");
            productContext.ProducerSupportEmail = ConfigurationUtility.GetParameter<string>(context, "ProducerSupportEmail");
            productContext.ProducerCentralPhone = ConfigurationUtility.GetParameter<string>(context, "ProducerCentralPhone");

            productContext.CustomerName = ConfigurationUtility.GetParameter<string>(context, "CustomerName");
            productContext.CustomerProductName = ConfigurationUtility.GetParameter<string>(context, "CustomerProductName");
            productContext.CustomerAdress = ConfigurationUtility.GetParameter<string>(context, "CustomerAdress");
            productContext.CustomerWebsite = ConfigurationUtility.GetParameter<string>(context, "CustomerWebsite");
            productContext.CustomerLogoPath = ConfigurationUtility.GetParameter<string>(context, "CustomerLogoPath");
            productContext.CustomerCentralPhone = ConfigurationUtility.GetParameter<string>(context, "CustomerCentralPhone");

            productContext.CustomerManagerName = ConfigurationUtility.GetParameter<string>(context, "CustomerManagerName");
            productContext.CustomerManagerEmail1 = ConfigurationUtility.GetParameter<string>(context, "CustomerManagerEmail1");
            productContext.CustomerManagerMobilePhone = ConfigurationUtility.GetParameter<string>(context, "CustomerManagerMobilePhone");
            productContext.CustomerManagerPhoneInternal = ConfigurationUtility.GetParameter<string>(context, "CustomerManagerPhoneInternal");

            productContext.CustomerSystemAdministratorName = ConfigurationUtility.GetParameter<string>(context, "CustomerSystemAdministratorName");
            productContext.CustomerSystemAdministratorEmail1 = ConfigurationUtility.GetParameter<string>(context, "CustomerSystemAdministratorEmail1");
            productContext.CustomerSystemAdministratorMobilePhone = ConfigurationUtility.GetParameter<string>(context, "CustomerSystemAdministratorMobilePhone");
            productContext.CustomerSystemAdministratorPhoneInternal = ConfigurationUtility.GetParameter<string>(context, "CustomerWebsite");

            return productContext;
        }

        public static SystemContext GetNewSystemContext(FrameworkContext context)
        {
            SystemContext systemContext = new SystemContext(context.DBContext);

            systemContext.ApplicationRootFolderPath = ConfigurationUtility.GetParameter<string>(context, "ApplicationRootFolderPath");
            systemContext.FrameworkProductName = ConfigurationUtility.GetParameter<string>(context, "FrameworkProductName");
            systemContext.FrameworkProductVersion = ConfigurationUtility.GetParameter<string>(context, "FrameworkProductVersion");
            systemContext.FrameworkProductVersionDate = ConfigurationUtility.GetParameter<DateTime>(context, "FrameworkProductVersionDate");

            return systemContext;
        }

        public static CacheContext GetNewCacheContext(FrameworkContext context)
        {
            CacheContext cacheContext = new CacheContext(context.FrameworkManager);

            //Parametreler ihtiyaç durumunda

            return cacheContext;
        }

        public static GlobalizationContext GetNewGlobalizationContext(FrameworkContext context)
        {
            GlobalizationContext globalizationContext = new GlobalizationContext(context.FrameworkManager);

            globalizationContext.ApplicationCulture = (Culture)ConfigurationUtility.GetParameter<byte>(context, "ApplicationCulture");
            globalizationContext.CurrentCulture = globalizationContext.ApplicationCulture;

            return globalizationContext;
        }        

        public static ConfigurationContext GetNewConfigurationContext(FrameworkContext context)
        {
            ConfigurationContext configurationContext = new ConfigurationContext(context.FrameworkManager);
            configurationContext.FrameworkProviderType = (FrameworkProviderType)ConfigurationUtility.GetParameter<byte>(context, "FrameworkProviderType");

            return configurationContext;
        }

        #endregion       
    }
}
