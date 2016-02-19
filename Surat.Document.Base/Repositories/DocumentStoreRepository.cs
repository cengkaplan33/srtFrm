using Surat.Base.Configuration;
using Surat.Base.Exceptions;
using Surat.Base.Security;
using Surat.Common.ViewModel;
using Surat.Document.Base.Configuration;
using Surat.Document.Base.Manage;
using Surat.Document.Base.Model.Entities;
using Surat.Document.Common.Data;
using Surat.Document.Common.ViewModel;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Documents.Base.Repositories
{
    public class DocumentStoreRepository : GenericRepository<DocumentStore>
    {
        #region Constructor

        public DocumentStoreRepository(DocumentContext contextParameter)
            : base(contextParameter.ApplicationContext.DBContext)
        {
            this.context = contextParameter;
        }

        #endregion

        #region Private Members

        private DocumentContext context;

        #endregion

        #region Public Members

        public DocumentContext Context
        {
            get { return context; }
        }

        #endregion

        #region Methods
        
        public DocumentStoreView GetDocumentStoreByWorkgroupId(int workgroupId)
        {
            DocumentStoreView documentStore;

            documentStore = (from documentStores in this.Context.ApplicationContext.DBContext.DocumentStores
                             where (documentStores.WorkgroupId == workgroupId)
                             select new DocumentStoreView
                             {
                                 Id =documentStores.Id,
                                 MaximumDocumentCount = documentStores.MaximumDocumentCount,
                                 MaximumDocumentSizeInMB = documentStores.MaximumDocumentSizeInMB,
                                 Name = documentStores.Name,
                                 RootDocumentGroupId = documentStores.RootDocumentGroupId,
                                 RootFilePath = documentStores.RootFilePath,
                                 SizeInGB = documentStores.SizeInGB,
                                 TypeName = documentStores.TypeName
                             }).FirstOrDefault();

            return documentStore;
        }

        #endregion
    }
}
