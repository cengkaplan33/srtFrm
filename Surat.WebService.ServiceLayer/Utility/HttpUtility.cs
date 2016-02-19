using Surat.Common.ViewModel;
using Surat.WebService.Application;
using Surat.WebService.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Surat.WebService.ServiceLayer.Utility
{
    public class HttpUtility
    {
        public static HttpResponseMessage PrepareResponseMessage(UserDetailedView currentUser,HttpRequestMessage request,HttpStatusCode statusCode,string messageKey)
        {
            HttpResponseMessage responseMessage = null;
            ServiceApplicationManager applicationManager = new ServiceApplicationManager(currentUser);

            try
            {
                //applicationManager.Framework.GetGlobalizationKeyValue(applicationManager.Context.SystemId);
            }
            catch (Exception)
            {

                throw;
            }
            responseMessage = request.CreateResponse(statusCode, ServiceConstants.Message.AuthenticationKeyNotFound);
            return responseMessage;
        }
    }
}
