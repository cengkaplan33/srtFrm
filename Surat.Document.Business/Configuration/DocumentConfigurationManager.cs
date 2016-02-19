using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Surat.Base.Application;
using Surat.Base.Mail;
using Surat.Document.Base.Application;
using Surat.Document.Base.Manage;
using Surat.Document.Base.Index;
using Surat.Document.Base.Search;
using Surat.Document.Base.Configuration;
using Surat.Documents.Base.Repositories;
using Surat.Document.Common.Application;
using Surat.Common.Application;

namespace Surat.Document.Business.Configuration
{
    public class DocumentConfigurationManager
    {
        #region Constructor

        public DocumentConfigurationManager(IDocumentApplicationManager documentApplicationManager)
        {
            
        }

        #endregion

        #region Private Members

        private IDocumentApplicationManager documentApplicationManager;
        private DocumentApplicationContext applicationContext;
        private IFrameworkManager frameworkApplicationManager;
        private FrameworkContext frameworkContext;
        private FileTypeRepository fileType;

        #endregion

        #region Public Members        

        public IDocumentApplicationManager DocumentApplicationManager
        {
            get
            {
                return documentApplicationManager;
            }
            set { documentApplicationManager = value; }
        }

        public DocumentApplicationContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (DocumentApplicationContext)this.DocumentApplicationManager.GetDocumentApplicationContext();

                return applicationContext;
            }
        }

        public DocumentConfigurationContext Context
        {
            get
            {
                return this.ApplicationContext.Configuration;
            }
        }

        public IFrameworkManager Framework
        {
            get
            {
                if (frameworkApplicationManager == null)
                    frameworkApplicationManager = this.DocumentApplicationManager.GetFrameworkManager();

                return frameworkApplicationManager;
            }
        }

        public FrameworkContext FrameworkContext
        {
            get
            {
                if (frameworkContext == null)
                    frameworkContext = (FrameworkContext)this.Framework.GetApplicationContext();

                return frameworkContext;
            }
        }

        #endregion

        #region Repository

        public FileTypeRepository FileType
        {
            get
            {
                if (fileType == null)
                    fileType = new FileTypeRepository(this.ApplicationContext.Configuration);

                return fileType;
            }
        }

        #endregion

        #region Methods

        #endregion

    }
}
