using Surat.Base;
using Surat.Base.Application;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Document.Base.Configuration;
using Surat.Document.Base.Index;
using Surat.Document.Base.Manage;
using Surat.Document.Base.Model;
using Surat.Document.Base.Search;
using Surat.Document.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Document.Base.Application
{
    public class DocumentApplicationContext : ApplicationContextBase,IApplicationContext
    {

        #region Constructor

        public DocumentApplicationContext(IDocumentApplicationManager documentApplicationManager)
            : base(documentApplicationManager.GetFrameworkManager(),Constants.Application.DocumentManagementSystemName)
        {
            this.documentApplicationManager = documentApplicationManager;
        }

        #endregion

        #region Private Members       

        private IDocumentApplicationManager documentApplicationManager;
        private IFrameworkManager frameworkApplicationManager;
        private FrameworkContext frameworkContext;
        private DocumentContext document;
        private DocumentConfigurationContext configuration;
        private DocumentSearchContext search;
        private DocumentIndexContext index;
 
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

        public new DocumentDbContext DBContext
        {
            get
            {
                return (DocumentDbContext)dbContext;
            }
            set { dbContext = value; }
        }

        public DocumentContext Document
        {
            get
            {
                if (document == null)
                    document = DocumentContextFactory.GetNewDocumentContext(this.DocumentApplicationManager);

                return document;
            }
            set { document = value; }
        }

        public DocumentConfigurationContext Configuration
        {
            get
            {
                if (configuration == null)
                    configuration = DocumentContextFactory.GetNewConfigurationContext(this.DocumentApplicationManager);

                return configuration;
            }
            set { configuration = value; }
        }

        public DocumentSearchContext Search
        {
            get
            {
                if (search == null)
                    search = DocumentContextFactory.GetNewSearchContext(this.DocumentApplicationManager);

                return search;
            }
            set { search = value; }
        }

        public DocumentIndexContext Index
        {
            get
            {
                if (index == null)
                    index = DocumentContextFactory.GetNewIndexContext(this.DocumentApplicationManager);

                return Index;
            }
            set { index = value; }
        }
       
        #endregion

        #region IApplicationContext

        public UserDetailedView GetCurrentUser()
        {
            return this.FrameworkContext.CurrentUser;
        }

        #endregion

        #region Methods        
   
        #endregion      
  
        #region IDisposable

        public override void Dispose()
        {

        }

        #endregion
    }
}

