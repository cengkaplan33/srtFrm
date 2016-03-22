using KonsolideRapor.Base.Application;
using KonsolideRapor.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Configuration
{
    public class KonsolideRaporConfigurationContext
    {
        #region Constructor

        public KonsolideRaporConfigurationContext(IKonsolideRaporApplicationManager konsolideRaporApplicationManager)
        {
            this.applicationManager = konsolideRaporApplicationManager;
        }

        #endregion

        #region Private Members

        private KonsolideRaporApplicationContext applicationContext;
        private IKonsolideRaporApplicationManager applicationManager;

        #endregion

        #region Public Members

        public IKonsolideRaporApplicationManager ApplicationManager
        {
            get
            {
                return applicationManager;
            }
        }

        public KonsolideRaporApplicationContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (KonsolideRaporApplicationContext)this.ApplicationManager.GetKonsolideRaporApplicationContext();

                return applicationContext;
            }
        }

        #endregion

    }
}
