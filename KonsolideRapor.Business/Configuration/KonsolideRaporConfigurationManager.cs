using KonsolideRapor.Base.Application;
using KonsolideRapor.Base.Configuration;
using KonsolideRapor.Common.Application;
using Surat.Base.Application;
using Surat.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Business.Configuration
{
    public class KonsolideRaporConfigurationManager
    {
        #region Constructor

        public KonsolideRaporConfigurationManager(IKonsolideRaporApplicationManager konsolideRaporApplicationManager)
        {

        }

        #endregion

        #region Private Members

        private IKonsolideRaporApplicationManager konsolideRaporApplicationManager;
        private KonsolideRaporApplicationContext applicationContext;
        private IFrameworkManager frameworkApplicationManager;
        private FrameworkContext frameworkContext;

        #endregion

        #region Public Members

        public IKonsolideRaporApplicationManager KonsolideRaporApplicationManager
        {
            get
            {
                return konsolideRaporApplicationManager;
            }
            set { konsolideRaporApplicationManager = value; }
        }

        public KonsolideRaporApplicationContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (KonsolideRaporApplicationContext)this.KonsolideRaporApplicationManager.GetKonsolideRaporApplicationContext();

                return applicationContext;
            }
        }

        public KonsolideRaporConfigurationContext Context
        {
            get
            {
                return this.ApplicationContext.Configuration;
            }
        }

        public IFrameworkManager Framework
        {
            get
            {
                if (frameworkApplicationManager == null)
                    frameworkApplicationManager = this.KonsolideRaporApplicationManager.GetFrameworkManager();

                return frameworkApplicationManager;
            }
        }

        public FrameworkContext FrameworkContext
        {
            get
            {
                if (frameworkContext == null)
                    frameworkContext = (FrameworkContext)this.Framework.GetApplicationContext();

                return frameworkContext;
            }
        }

        #endregion

        #region Repository

   

        #endregion

        #region Methods

        #endregion

    }
}
