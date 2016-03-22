using KonsolideRapor.Base.Configuration;
using KonsolideRapor.Base.Manage;
using KonsolideRapor.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Application
{
    public class KonsolideRaporContextFactory
    {
        #region Methods

        public static KonsolideRaporContext GetNewKonsolideRaporContext(IKonsolideRaporApplicationManager konsolideRaporApplicationManager)
        {
            KonsolideRaporContext konsolideRaporContext = new KonsolideRaporContext(konsolideRaporApplicationManager);

            return konsolideRaporContext;
        }

        public static KonsolideRaporConfigurationContext GetNewConfigurationContext(IKonsolideRaporApplicationManager konsolideRaporApplicationManager)
        {
            KonsolideRaporConfigurationContext configurationContext = new KonsolideRaporConfigurationContext(konsolideRaporApplicationManager);

            return configurationContext;
        }

        #endregion
    }
}
