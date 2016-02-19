using Newtonsoft.Json;
using Surat.Common.Utilities;
using Surat.WebService.Common.Data;
using Surat.WebServiceBase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Surat.WebService.ServiceLayer
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {           
            string authenticationToken;

            if (!request.RequestUri.AbsolutePath.Contains(ServiceConstants.Application.AuthenticationRequest)) //Login İsteği mi?
            {
                if (request.Headers.Contains(ServiceConstants.Application.AuthenticationKeyName))
                    authenticationToken = request.Headers.GetValues(ServiceConstants.Application.AuthenticationKeyName).FirstOrDefault();
                else return request.CreateResponse(HttpStatusCode.Forbidden, ServiceConstants.Message.AuthenticationKeyNotFound); //ToDo: Globalization eklenmeli
                
                byte[] byteArray = Convert.FromBase64String(authenticationToken);
                AuthenticationToken authenticationTokenObject = JsonConvert.DeserializeObject<AuthenticationToken>(Encoding.UTF8.GetString(byteArray));

                //Controller üzerinden erişimesi gerekiyor.
                request.Properties.Add(ServiceConstants.Application.AuthenticationToken, authenticationTokenObject);

                if (authenticationTokenObject.StartTime.AddDays(1) < TimeUtility.GetCurrentDateTime()) //ToDo : Expiration 1 gün olarak ayarlancı. Parametre yapılmalıdır.
                    return request.CreateResponse(HttpStatusCode.Forbidden, ServiceConstants.Message.ServisTicketExpired); //ToDo: Globalization eklenmeli
            }

            //Allow the request to process further down the pipeline
            var response = await base.SendAsync(request, cancellationToken);
     
            //Return the response back up the chain
            return response;
        }
    }
}
