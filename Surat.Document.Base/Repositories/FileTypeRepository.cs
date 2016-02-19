using Surat.Base.Configuration;
using Surat.Base.Security;
using Surat.Common.ViewModel;
using Surat.Document.Base.Configuration;
using Surat.Document.Base.Model.Entities;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Documents.Base.Repositories
{
    public class FileTypeRepository : GenericRepository<FileType>
    {
        #region Constructor

        public FileTypeRepository(DocumentConfigurationContext contextParameter)
            : base(contextParameter.ApplicationContext.DBContext)
        {
            this.context = contextParameter;
        }

        #endregion

        #region Private Members

        private DocumentConfigurationContext context;

        #endregion

        #region Public Members

        public DocumentConfigurationContext Context
        {
            get { return context; }
        }

        #endregion

        #region Methods      

        #endregion
    }
}
