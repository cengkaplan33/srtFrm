using Surat.Base.Providers;
using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Business.Base
{
    public class FrameworkProviderFactory
    {
        public static IAuthenticationProvider GetNewFrameworkProvider(string frameworkProviderType)
        {
            IAuthenticationProvider newFrameworkProvider = null;

            switch (frameworkProviderType)
            {
                case Constants.FrameworkType.Serendip:
                    newFrameworkProvider = new SerendipAuthenticationProvider();
                    break;
                case Constants.FrameworkType.Surat:
                    newFrameworkProvider = new SuratAuthenticationProvider();
                    break;
                default:
                    throw new ApplicationException("Geçersiz Framework Tip");
            }

            return newFrameworkProvider;
        }
    }
}
