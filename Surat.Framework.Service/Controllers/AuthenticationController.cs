using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Document.Base.Model.Entities;
using Surat.Document.Business.Application;
using Surat.Document.Common.ViewModel;
using Surat.WebService.ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Surat.WebService.Controllers
{
    public class AuthenticationController : SuratServiceBase
    {
        [HttpGet]
        public IHttpActionResult Login(string userName, string password)
        {
            string token = string.Empty;                      
            
            try
            {
                token = this.ServiceApplicationManager.Authentication.Authenticate(userName, password);
                return Ok(token);
            }
            catch (Exception exception)
            {
                return new ServiceErrorMessageResult(this.ServiceApplicationManager.PublishException(exception),this.Request);
            }
        }

    }
}
