using Surat.Base.Application;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Business.Log;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Document.Base.Application;
using Surat.Document.Base.Model;
using Surat.Document.Business.Configuration;
using Surat.Document.Business.Index;
using Surat.Document.Business.Manage;
using Surat.Document.Business.Search;
using Surat.Document.Common.Application;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Surat.Document.Business.Application
{
    public class DocumentApplicationManager : ApplicationManager, IDocumentApplicationManager
    {

        #region Constructor

        public DocumentApplicationManager():this(null)
        {

        }

        public DocumentApplicationManager(FrameworkApplicationManager applicationManager)
        {
            if (applicationManager != null)
                this.framework = applicationManager;
            else this.framework = InitializeFramework();  
        }

        #endregion

        #region Private Members

        private DocumentApplicationContext context;
        private FrameworkApplicationManager framework;
        private DocumentManager documentManager;
        private DocumentSearchManager searchManager;
        private DocumentIndexManager indexManager;
        private DocumentConfigurationManager configurationManager;        
 
        #endregion

        #region Public Members

        public FrameworkApplicationManager Framework
        {
            get
            {
                if (framework == null)
                    framework = InitializeFramework();

                return framework;
            }
        }

        public DocumentApplicationContext Context
        {
            get
            {
                if (context == null)
                    InitializeDocumentContext();

                return context;
            }
        }

        public DocumentManager DocumentManager
        {
            get
            {
                if (documentManager == null)
                    InitializeDocumentManager();

                return documentManager;
            }
        }

        public DocumentSearchManager Search
        {
            get
            {
                if (searchManager == null)
                    InitializeSearchManager();

                return searchManager;
            }
        }

        public DocumentConfigurationManager Configuration
        {
            get
            {
                if (configurationManager == null)
                    InitializeConfigurationManager();

                return configurationManager;
            }
        }

        public DocumentIndexManager Index
        {
            get
            {
                if (indexManager == null)
                    InitializeIndexManager();

                return indexManager;
            }
        }       

        #endregion

        #region IDocumentApplicationManager

        public IApplicationContext GetDocumentApplicationContext()
        {
            return this.Context;
        }

        public IFrameworkManager GetFrameworkManager()
        {
            return this.Framework;
        }

        #endregion
        
        #region Methods

        private FrameworkApplicationManager InitializeFramework()
        {
            FrameworkApplicationManager framework;

            framework = new FrameworkApplicationManager();

            //if (currentUser != null)
            //    framework = new FrameworkApplicationManager(currentUser);
            //else framework = new FrameworkApplicationManager();

            return framework;
        }

        private void InitializeDocumentContext()
        {
            context = new DocumentApplicationContext(this);
            
            context.DBContext = new DocumentDbContext();

            this.Framework.Trace.AppendLine(this.Context.SystemName, "DocumentContext Initialized.", TraceLevel.Basic);
        }

        private void InitializeDocumentManager()
        {            
            documentManager = new DocumentManager(this);
            this.Framework.Trace.AppendLine(this.Context.SystemName, "DocumentManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeConfigurationManager()
        {
            configurationManager = new DocumentConfigurationManager(this);
            this.Framework.Trace.AppendLine(this.Context.SystemName, "ConfigurationManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeSearchManager()
        {
            searchManager = new DocumentSearchManager(this);
            this.Framework.Trace.AppendLine(this.Context.SystemName, "SearchManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeIndexManager()
        {
            indexManager = new DocumentIndexManager(this);
            this.Framework.Trace.AppendLine(this.Context.SystemName, "IndexManager Initialized.", TraceLevel.Basic);
        }        

        #endregion              

        #region Dispose
        
        public override void Dispose()
        {
            this.Framework.Trace.WriteTraceToFile();
            this.Context.Dispose();            
        }

        #endregion        
    }
}

