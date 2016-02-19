using Surat.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Common.Application
{
    public interface IDocumentApplicationManager
    {
        IApplicationContext GetDocumentApplicationContext();
        IFrameworkManager GetFrameworkManager();
    }
}
