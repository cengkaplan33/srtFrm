using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using Surat.Base.Mail;

namespace Surat.Business.Mail
{
    public class SMTPFactory
    {        

        #region Private Members

        private String name;

        #endregion

        #region Public Members

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region Methods

        public static SmtpClient GetNewSmtpClient(MailContext context)
        {
            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = context.SMTPServerIP;
            smtpClient.Port = context.SMTPServerPort;
            smtpClient.EnableSsl = context.SMTPEnableSSL;
            smtpClient.Credentials = new NetworkCredential(context.SMTPServerUser, context.SMTPServerUserPassword);

            return smtpClient;
        }

        #endregion

    }
}
