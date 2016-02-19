using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.WebService.Common.Data
{
    public class ServiceConstants
    {

        #region Application

        public class Application
        {
            public const String AuthenticationKeyName = "SuratAuthenticationKey";
            public const String AuthenticationRequest = "Authentication/Login";
            public const String AuthenticationToken = "AuthenticationToken";
        }

        #endregion   

         #region Message

        public class Message
        {
            public const String ServiceAuthenticationFailed = "ServiceAuthenticationFailed|Servis üzerinden kimlik belirleme yapılamadı.|Service authentication operation has not been completed.";
            public const String AuthenticationKeyNotFound = "AuthenticationKeyNotFound|Kimlik belirleme anahtarı bulunamadı.|Authentication key was not found.";
            public const String ServisTicketExpired = "ServisTicketExpired|Servis erişim bilgisinin(Ticket) süresi doldu.Tekrar doğrulama yapılmalıdır.|Servis ticket was expired. You must re-authenticate.";
        }

         #endregion

    }
}
