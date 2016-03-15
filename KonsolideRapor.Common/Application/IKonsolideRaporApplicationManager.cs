using Surat.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Common.Application
{
    public interface IKonsolideRaporApplicationManager
    {
        IApplicationContext GetKonsolideRaporApplicationContext();
        IFrameworkManager GetFrameworkManager();
    }
}
