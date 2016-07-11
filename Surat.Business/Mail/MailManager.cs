using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Surat.Base.Application;
using Surat.Base.Mail;
using Surat.Common.Log;
using Surat.Common.Application;
using Surat.Common.Mail;

namespace Surat.Business.Mail
{
    public class MailManager : IMailManager
    {
        #region Constructor

        public MailManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager; 
        }

        #endregion

        #region Private Members

        private IFrameworkManager frameworkManager;
        private FrameworkContext applicationContext;
        private ITraceManager traceManager;

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

        public MailContext Context
        {
            get
            {
                return this.ApplicationContext.Mail;
            }
        }


        #endregion

        #region IMailManager

        public void Send(MailMessage message)
        {
            SmtpClient smtpClient = null;
            string sss;
            try
            {
                //smtpClient = SMTPFactory.GetNewSmtpClient(this.ApplicationContext.Mail);
                //smtpClient.EnableSsl = true;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                //smtpClient.Send(message);

                SerendipEMailServiceClient.SerendipEMailClient client = new SerendipEMailServiceClient.SerendipEMailClient();

                var mail = new SerendipEMailServiceClient.SerendipEMail() { };
                mail.MailFrom = message.From.Address;
                mail.MailTo = String.Join(";", message.To.Select(x => x.Address));
                mail.MailBody = message.Body;

                sss= client.SendEMail(mail);
                
                //SerendipEMailServiceDB.connection dosyasının içindeki veri.
                 //data source=172.27.10.20\kaynakdbt;user id=SerendipEMailService;password=S+E*Ser1-3/5;initial catalog=SerendipEMailServiceDB;MultipleActiveResultSets=true;
            }
            catch //(Exception exception)
            {
                //To Do
                //throw new SMTPMailException(applicationContext.SystemName, exception);
            }
        }

        #endregion

        #region Methods        

        #endregion
    }
}
