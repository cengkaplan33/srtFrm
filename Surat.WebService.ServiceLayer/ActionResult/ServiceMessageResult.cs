using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Surat.WebService.ServiceLayer
{
    public class ServiceErrorMessageResult : IHttpActionResult
    {
        string message;
        HttpRequestMessage request;

        public ServiceErrorMessageResult(string value, HttpRequestMessage currentRequest)
        {
            message = value;
            request = currentRequest;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(message),
                RequestMessage = request
            };

            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            return Task.FromResult(response);
        }
    }
}
