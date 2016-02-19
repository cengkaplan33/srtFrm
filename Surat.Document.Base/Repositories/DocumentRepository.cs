using Surat.Base.Configuration;
using Surat.Base.Exceptions;
using Surat.Base.Security;
using Surat.Common.ViewModel;
using Surat.Document.Base.Configuration;
using Surat.Document.Base.Manage;
using Surat.Document.Base.Model.Entities;
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
    public class DocumentRepository : GenericRepository<SuratDocument>
    {
        #region Constructor

        public DocumentRepository(DocumentContext contextParameter)
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

        public List<ObjectDocumentView> GetDocumentsByObjectId(string objectTypeName, long objectId)
        {           
            List<ObjectDocumentView> documents = null;

            documents = (from suratDocuments in this.Context.ApplicationContext.DBContext.Documents
                         join fileTypes in this.Context.ApplicationContext.DBContext.FileTypes on suratDocuments.FileTypeId equals fileTypes.Id
                         where (suratDocuments.RelatedObjectType == objectTypeName && suratDocuments.RelatedObjectId == objectId && suratDocuments.IsActive == true)
                         select new ObjectDocumentView 
                         { 
                              FileName = suratDocuments.FileName,
                              FileTypeId = suratDocuments.FileTypeId,
                              Id = suratDocuments.Id,
                              Notes = suratDocuments.Notes
                         }
                        ).ToList(); 

            return documents;
        }

        public ExistingDocumentView GetDocumentById(long id)
        {
            ExistingDocumentView document;

            document = (from suratDocuments in this.Context.ApplicationContext.DBContext.Documents
                        where (suratDocuments.Id == id)
                        select new ExistingDocumentView
                        {
                            FileName = suratDocuments.FileName,
                            InsertedDate = suratDocuments.InsertedDate
                        }).FirstOrDefault();

            if (document == null)
                throw new RecordNotFoundException(null,"GetDocumentById", 0); //ToDo : systemId verilmelidir. Framework Context verilmelidir.

            return document;
        }

        #endregion
    }
}
