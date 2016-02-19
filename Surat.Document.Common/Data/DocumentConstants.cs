using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Common.Data
{
    public class DocumentConstants
    {

        #region Application

        public class Application
        {
            public const String AttachmentsGroupName = "Attachments";
            public const String AttachmentsGroupShortName = "Attachments";
        }

        #endregion   
        #region Message

        public class Message
        {
            public const String UserDocumentStoreNotFound = "UserDocumentStoreNotFound|Kullanıcının dosya veritabanı bulunamadı.|User does not has a document store.";
        }

        #endregion      
    }
}
