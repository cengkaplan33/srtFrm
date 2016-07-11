using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using Surat.Base.Mail;
using Surat.Base.Application;


namespace Surat.Base.Mail
{
    public class MailTemplateFactory
    {        

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods

        public static MailMessage GetNewExceptionEMailTemplate(FrameworkContext context, Int64 id, string user, DateTime logDate, string data)
        {

            StringBuilder messageBody = new StringBuilder();
            MailMessage message = new MailMessage();

            //message.Subject = context.Globalization.GetGlobalizationKeyValue(context.SystemId, "ExceptionEMailSubject");

            if (id > 0)
                messageBody.AppendLine(context.Globalization.GetGlobalizationKeyValue(context.SystemId, "Id") + " : " + id.ToString());

            if (user != "0" && user.Trim().Length > 0)
                messageBody.AppendLine(context.Globalization.GetGlobalizationKeyValue(context.SystemId, "User") + " : " + user);

            messageBody.AppendLine(context.Globalization.GetGlobalizationKeyValue(context.SystemId, "Date") + " : " + logDate.ToString("yyyyMMdd-hhmmss"));
            messageBody.AppendLine(context.Globalization.GetGlobalizationKeyValue(context.SystemId, "Message") + " :" + data);

            message.Body = messageBody.ToString();
            message.From = new MailAddress(context.Mail.SMTPMailFrom);
            message.To.Add(context.Product.CustomerSystemAdministratorEmail1);
            message.To.Add(context.Product.ProducerSupportEmail);

            return message;
        }

        public static MailMessage GetNewPasswordEMailTemplate(FrameworkContext context,string newPassword,string eMailTo)
        {
            MailMessage message = new MailMessage();

            message.Subject = "";//context.Globalization.GetGlobalizationKeyValue("GeneratedPasswordEMailSubject");

            message.Body = "";// context.Globalization.GetGlobalizationKeyValue("GeneratedPasswordEMailMessage") + context.Globalization.GetGlobalizationKeyValue("Password") + ":" + newPassword;
            message.From = new MailAddress(context.Mail.SMTPMailFrom);

            message.To.Add(eMailTo);

            return message;
        }

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
