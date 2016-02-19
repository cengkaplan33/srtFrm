using Surat.Common.Cache;
using Surat.Common.Globalization;
using Surat.Common.Log;
using Surat.Common.Mail;
using Surat.Common.Security;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Application
{
    public interface IFrameworkManager
    {
        IApplicationContext GetApplicationContext();
        bool IsApplicationContextInitialized();
        List<SystemDetailedView> GetSystems();
        List<ParameterValueView> GetSystemParameters();
        List<WorkgroupView> GetActiveWorkGroups();
        ISecurityManager GetSecurityManager();
        ITraceManager GetTraceManager();
        IGlobalizationManager GetGlobalizationManager();
        ICacheManager GetCacheManager();
        IMailManager GetMailManager();
    }
}
