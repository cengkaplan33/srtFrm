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
using Surat.Common.Application;
using Surat.Document.Common.Application;

namespace Surat.Document.Business.Search
{
    public class DocumentSearchManager
    {
        #region Constructor

        public DocumentSearchManager(IDocumentApplicationManager documentApplicationManager)
        {
            this.documentApplicationManager = documentApplicationManager; 
        }

        #endregion

        #region Private Members

        private IDocumentApplicationManager documentApplicationManager;
        private DocumentApplicationContext applicationContext;
        private IFrameworkManager frameworkApplicationManager;
        private FrameworkContext frameworkContext;

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

        public DocumentSearchContext Context
        {
            get
            {
                return this.ApplicationContext.Search;
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

        #region Methods        

        #endregion

    }
}
