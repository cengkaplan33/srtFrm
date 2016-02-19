using Surat.Document.Base.Configuration;
using Surat.Document.Base.Index;
using Surat.Document.Base.Manage;
using Surat.Document.Base.Search;
using Surat.Document.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Document.Base.Application
{
    public class DocumentContextFactory
    {
        #region Methods

        public static DocumentContext GetNewDocumentContext(IDocumentApplicationManager documentApplicationManager)
        {
            DocumentContext documentContext = new DocumentContext(documentApplicationManager);            

            return documentContext;
        }

        public static DocumentConfigurationContext GetNewConfigurationContext(IDocumentApplicationManager documentApplicationManager)
        {
            DocumentConfigurationContext configurationContext = new DocumentConfigurationContext(documentApplicationManager);

            return configurationContext;
        }

        public static DocumentIndexContext GetNewIndexContext(IDocumentApplicationManager documentApplicationManager)
        {
            DocumentIndexContext indexContext = new DocumentIndexContext(documentApplicationManager);

            return indexContext;
        }

        public static DocumentSearchContext GetNewSearchContext(IDocumentApplicationManager documentApplicationManager)
        {
            DocumentSearchContext searchContext = new DocumentSearchContext(documentApplicationManager);            

            return searchContext;
        }

        #endregion       
    }
}
