using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.SerendipApplication.Common
{
    public class SerendipConstants
    {
        #region Application
        public class Application
        {
            public const String SerendipSystemName = "Serendip";
        }
        #endregion   
   
        #region Application
        public class Message
        {
            public const String SerendipIntegrationFailed = "SerendipIntegrationFailed|Serendip entegrasyonu sağlanamadı.|Serendip integration has not been succeeded.";
            public const String SerendipUserNotFound = "SerendipUserNotFound|Serendip kullanıcısı bulunamadı.Kullanıcı adı veya parola hatalı olabilir.|Serendip user was not found.";
        }
        #endregion
    }
}
