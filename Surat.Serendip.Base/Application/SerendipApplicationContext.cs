using Surat.Base;
using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.SerendipApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.SerendipApplication.Base
{
    public class SerendipApplicationContext : IDisposable
    {

        #region Constructor

        public SerendipApplicationContext(FrameworkContext frameworkContext)            
        {
            this.frameworkContext = frameworkContext;
            systemName = SerendipConstants.Application.SerendipSystemName;
            systemId = this.frameworkContext.Configuration.GetSystemIdByName(SerendipConstants.Application.SerendipSystemName);
        }
        #endregion

        #region Private Members 
  
        private FrameworkContext frameworkContext;
        private string systemName;
        private int systemId;
        private string dbKeyName;
        private string firmaDonem;
        private string dbUserName;
        private string dbUserPassword;
        private ExternalSystemsUsersView externalUser = null;
         
        #endregion

        #region Public Members   

        public FrameworkContext FrameworkContext
        {
            get
            {
                return frameworkContext;
            }
        }

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

        public ExternalSystemsUsersView ExternalUser
        {
            get
            {
                if (externalUser == null)
                    throw new NullValueException(this.FrameworkContext, "externalUser", this.SystemId);

                return externalUser;
            }
            set
            {
                externalUser = value;
            }
        }

        public string DBKeyName
        {
            get
            {
                if (string.IsNullOrEmpty(dbKeyName))
                    throw new NullValueException(this.FrameworkContext, "DBKeyName", this.SystemId);

                return dbKeyName;
            }
            set
            {
                dbKeyName = value;
            }
        }

        public string FirmaDonem
        {
            get
            {
                if (string.IsNullOrEmpty(firmaDonem))
                    throw new NullValueException(this.FrameworkContext, "FirmaDonem", this.SystemId);

                return firmaDonem;
            }
            set
            {
                firmaDonem= value;
            }
        }

        public string DBUserName
        {
            get
            {
                if (string.IsNullOrEmpty(dbUserName))
                    throw new NullValueException(this.FrameworkContext, "DBUserName", this.SystemId);

                return dbUserName;
            }
            set
            {
                dbUserName = value;
            }
        }

        public string DBUserPassword
        {
            get
            {
                if (string.IsNullOrEmpty(dbUserPassword))
                    throw new NullValueException(this.FrameworkContext, "DBUserPassword", this.SystemId);

                return dbUserPassword;
            }
            set
            {
                dbUserPassword = value;
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

