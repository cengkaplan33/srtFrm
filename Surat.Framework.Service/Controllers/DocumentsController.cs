using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Document.Base.Model.Entities;
using Surat.Document.Business.Application;
using Surat.Document.Common.ViewModel;
using Surat.Framework.Service.Configuration;
using Surat.WebService.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml;

namespace Surat.WebService.Controllers
{
    public class DocumentsController : SuratServiceBase
    {
        public IHttpActionResult GetDocumentParameters()
        {
            DocumentParametersView parameters = new DocumentParametersView();
            DocumentApplicationManager applicationManager = new DocumentApplicationManager(this.ServiceApplicationManager.Framework);
            try
            {
                parameters = ServiceConfigurationUtility.GetDocumentParameters(applicationManager.Context);
                return Ok(parameters);
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception), this.Request);
            }
        }

        public IHttpActionResult GetDocument(long id)
        {
            DocumentApplicationManager applicationManager;
            DocumentDownloadView document;

            try
            {
                applicationManager = new DocumentApplicationManager(this.ServiceApplicationManager.Framework);
                document = applicationManager.DocumentManager.GetDocument(this.CurrentServiceUser, id);
                return Ok(document);
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception), this.Request);
            }
        }

        [ResponseType(typeof(List<FileType>))]
        public IHttpActionResult GetAllFileTypes()
        {            
            DocumentApplicationManager applicationManager;
            List<FileType> fileTypes = null;

            try
            {
                applicationManager = new DocumentApplicationManager(this.ServiceApplicationManager.Framework);
                fileTypes = applicationManager.Configuration.FileType.GetAll().ToList();
                return Ok(fileTypes);
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception), this.Request);
            }
        }

        [ResponseType(typeof(List<ObjectDocumentView>))]
        public IHttpActionResult GetDocumentsByObjectId(string objectTypeName, long objectId)
        {            
            DocumentApplicationManager applicationManager;
            List<ObjectDocumentView> documents = null;

            try
            {
                applicationManager = new DocumentApplicationManager(this.ServiceApplicationManager.Framework);
                documents = applicationManager.DocumentManager.Document.GetDocumentsByObjectId(objectTypeName, objectId);
                return Ok(documents);
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception), this.Request);
            }
        }

        [HttpPost]
        [ResponseType(typeof(DocumentUploadResultView))]
        public IHttpActionResult UploadDocument(DocumentUploadView document)
        {
            DocumentApplicationManager applicationManager;
            DocumentUploadResultView result;
            IHttpActionResult message = null;
            try
            { 
                applicationManager = new DocumentApplicationManager(this.ServiceApplicationManager.Framework);
                result = new DocumentUploadResultView();
                result.Id = applicationManager.DocumentManager.SaveDocument(this.CurrentServiceUser, document);
                message = Ok<DocumentUploadResultView>(result);
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception), this.Request);                                 
            }
            
            return message;
        }

        [HttpPost]
        public IHttpActionResult UploadDocuments(IEnumerable<DocumentUploadView> documents)
        {
            IHttpActionResult message;
            DocumentApplicationManager applicationManager;

            try
            {
                applicationManager = new DocumentApplicationManager(this.ServiceApplicationManager.Framework);
                applicationManager.DocumentManager.SaveDocuments(this.CurrentServiceUser, documents);
                message = Ok(this.ServiceApplicationManager.GetGlobalizationKeyValue(this.ServiceApplicationManager.Framework.Context.SystemId,Constants.Message.OperationCompleted));
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception), this.Request);
            }

            return message;
        }

        [HttpDelete]
        public IHttpActionResult DeleteDocumentById(long id)
        {
            IHttpActionResult message;
            DocumentApplicationManager applicationManager;

            try
            {
                applicationManager = new DocumentApplicationManager(this.ServiceApplicationManager.Framework);
                applicationManager.DocumentManager.DeleteDocument(id);
                message = Ok(true);
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception), this.Request);
            }

            return message;
        }
    }
}
