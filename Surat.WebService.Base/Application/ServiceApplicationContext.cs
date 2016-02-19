using Surat.Base;
using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Surat.WebService.Application
{
    public class ServiceApplicationContext : IDisposable
    {
        #region Constructor

        public ServiceApplicationContext(FrameworkContext frameworkContext)            
        {
            this.frameworkContext = frameworkContext;
            systemName = Constants.Application.ServiceFrameworkSystemName;
            systemId = this.frameworkContext.Configuration.GetSystemIdByName(systemName);
        }
        #endregion

        #region Private Members

        private FrameworkContext frameworkContext;
        private string systemName;
        private int systemId;
        private Culture currentCulture;

        #endregion

        #region Public Members

        public int SystemId
        {
            get
            {
                return systemId;
            }
        }

        public string SystemName
        {
            get
            {
                return systemName;
            }
        }

        public FrameworkContext FrameworkContext
        {
            get
            {
                return frameworkContext;
            }
        }

        public Culture CurrentCulture
        {
            get
            {
                //ToDo : Session kullanıldığında oradan alınmalıdır. WebApplicationContext içindeki gibi olmalıdır.
                return this.FrameworkContext.Globalization.CurrentCulture;;
            }
            set
            {
                currentCulture = value;
            }
        }

        #endregion

        #region Methods

        #endregion

        #region IDisposable

        public void Dispose()
        {
            
        }

        #endregion
    }
}

