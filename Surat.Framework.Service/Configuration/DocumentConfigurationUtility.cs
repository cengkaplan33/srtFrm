using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Document.Base.Application;
using Surat.Document.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Xml;

namespace Surat.Framework.Service.Configuration
{
    public class ServiceConfigurationUtility
    {
        public static DocumentParametersView GetDocumentParameters(DocumentApplicationContext context)
        {
            DocumentParametersView parameters = new DocumentParametersView();

            try
            {
                HttpRuntimeSection httpRuntimeSection = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                parameters.MaxRequestLength = httpRuntimeSection.MaxRequestLength;
                parameters.ExecutionTimeoutAsSeconds = (int)httpRuntimeSection.ExecutionTimeout.TotalSeconds;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Web.config"));

                XmlNode n = xmlDoc.SelectSingleNode("//requestFiltering/requestLimits"); // /configuration/System.webServer/Security/requestFiltering

                if (n != null)
                {
                    parameters.MaxAllowedContentLength = Convert.ToInt32(n.Attributes["maxAllowedContentLength"].Value);
                }
            }
            catch (Exception exception)
            {
                throw new ConfigurationAccessException(context.FrameworkContext, "GetDocumentParameters", context.SystemId, exception); 
            }

            return parameters;
        }
    }
}
