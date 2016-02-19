using Surat.Base.Application;
using Surat.Base.Model;
using Surat.Common.Application;
using Surat.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Mail
{
    public class MailContext
    {
        #region Constructor

        public MailContext(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private FrameworkDbContext dbContext;
        private String smtpServerIP;
        private int smtpServerPort;
        private String smtpServerUser;
        private String smtpServerUserPassword;
        private String smtpMailFrom;
        private bool smtpEnableSSL;

        #endregion

        #region Public Members

        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.FrameworkManager.GetApplicationContext();

                return applicationContext;
            }
        }

        public IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
        }

        public ITraceManager Trace
        {
            get
            {
                return this.FrameworkManager.GetTraceManager();
            }
        }

        public FrameworkDbContext DBContext
        {
            get { return dbContext; }
            set { dbContext = value; }
        }

        public String SMTPServerIP
        {
            get { return smtpServerIP; }
            set { smtpServerIP = value; }
        }

        public int SMTPServerPort
        {
            get { return smtpServerPort; }
            set { smtpServerPort = value; }
        }

        public String SMTPServerUser
        {
            get { return smtpServerUser; }
            set { smtpServerUser = value; }
        }

        public String SMTPServerUserPassword
        {
            get { return smtpServerUserPassword; }
            set { smtpServerUserPassword = value; }
        }
        public String SMTPMailFrom
        {
            get { return smtpMailFrom; }
            set { smtpMailFrom = value; }
        }

        public bool SMTPEnableSSL
        {
            get { return smtpEnableSSL; }
            set { smtpEnableSSL = value; }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

    }
}
