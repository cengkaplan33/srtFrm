using KonsolideRapor.Base.Configuration;
using KonsolideRapor.Base.Manage;
using KonsolideRapor.Base.Model;
using KonsolideRapor.Base.Application;
using KonsolideRapor.Common.Application;
using KonsolideRapor.Common.Data;
using Surat.Base;
using Surat.Base.Application;
using Surat.Common.Application;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Application
{
    public class KonsolideRaporApplicationContext : ApplicationContextBase, IApplicationContext
    {

        #region Constructor

        public KonsolideRaporApplicationContext(IKonsolideRaporApplicationManager konsolideRaporApplicationManager)
            : base(konsolideRaporApplicationManager.GetFrameworkManager(),KonsolideRaporConstants.Application.KonsolideRaporSystemName)
        {
            this.konsolideRaporApplicationManager = konsolideRaporApplicationManager;
        }

        #endregion

        #region Private Members

        private IKonsolideRaporApplicationManager konsolideRaporApplicationManager;
        private IFrameworkManager frameworkApplicationManager;
        private FrameworkContext frameworkContext;
        private KonsolideRaporContext konsolideRapor;
        private KonsolideRaporConfigurationContext configuration;

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

        public new KonsolideRaporDbContext DBContext
        {
            get
            {
                return (KonsolideRaporDbContext)dbContext;
            }
            set { dbContext = value; }
        }

        public KonsolideRaporContext KonsolideRapor
        {
            get
            {
                if (konsolideRapor == null)
                    konsolideRapor =KonsolideRaporContextFactory.GetNewKonsolideRaporContext(this.KonsolideRaporApplicationManager);

                return konsolideRapor;
            }
            set { konsolideRapor = value; }
        }

        public KonsolideRaporConfigurationContext Configuration
        {
            get
            {
                if (configuration == null)
                    configuration = KonsolideRaporContextFactory.GetNewConfigurationContext(this.KonsolideRaporApplicationManager);

                return configuration;
            }
            set { configuration = value; }
        }

     
        #endregion

        #region IApplicationContext

        public UserDetailedView GetCurrentUser()
        {
            return this.FrameworkContext.CurrentUser;
        }

        #endregion

        #region Methods

        #endregion

        #region IDisposable

        public override void Dispose()
        {

        }

        #endregion
    }
}

