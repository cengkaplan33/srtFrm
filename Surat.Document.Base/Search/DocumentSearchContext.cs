using Surat.Base.Application;
using Surat.Base.Model;
using Surat.Document.Base.Application;
using Surat.Document.Base.Model;
using Surat.Document.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Document.Base.Search
{
    public class DocumentSearchContext
    {
        #region Constructor

        public DocumentSearchContext(IDocumentApplicationManager documentApplicationManager)
        {
            this.applicationManager = documentApplicationManager;
        }

        #endregion

        #region Private Members

        private DocumentApplicationContext applicationContext;
        private IDocumentApplicationManager applicationManager;

        #endregion

        #region Public Members

        public IDocumentApplicationManager ApplicationManager
        {
            get
            {
                return applicationManager;
            }
        }

        public DocumentApplicationContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (DocumentApplicationContext)this.ApplicationManager.GetDocumentApplicationContext();

                return applicationContext;
            }
        }

        #endregion

    }
}
