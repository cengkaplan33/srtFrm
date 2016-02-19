using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Application
{
    public interface IApplicationContext
    {
        UserDetailedView GetCurrentUser();
    }
}
