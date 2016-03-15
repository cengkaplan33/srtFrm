using Surat.Base;
using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace KonsolideRapor.WebServer.Application
{
    public class WebApplicationContext : IDisposable
    {
        #region Constructor

        public WebApplicationContext(FrameworkContext frameworkContextParameter)           
        {            
            frameworkContext = frameworkContextParameter;
            systemName = Constants.Application.WebFrameworkSystemName;
            systemId = frameworkContext.Configuration.GetSystemIdByName(Constants.Application.WebFrameworkSystemName);
        }
        #endregion

        #region Private Members

        private FrameworkContext frameworkContext;
        private string systemName;
        private int systemId;
        private byte wrongPasswordProcessCount;
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
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session["CurrentCulture"] != null)
                        currentCulture = (Culture)HttpContext.Current.Session["CurrentCulture"];
                    else currentCulture = this.FrameworkContext.Globalization.CurrentCulture;

                return currentCulture;
            }
            set
            {
                currentCulture = value;
                if (HttpContext.Current != null)
                    HttpContext.Current.Session["CurrentCulture"] = currentCulture;
            }
        }

        public byte WrongPasswordProcessCount
        {
            get
            {
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session["WrongPasswordProcessCount"] != null)
                        wrongPasswordProcessCount = Convert.ToByte(HttpContext.Current.Session["WrongPasswordProcessCount"]);

                return wrongPasswordProcessCount;
            }
            set
            {                
                wrongPasswordProcessCount = value;
                if (HttpContext.Current != null)
                    HttpContext.Current.Session["WrongPasswordProcessCount"] = wrongPasswordProcessCount;   
            }
        }

        public UserDetailedView CurrentUser
        { 
            get
            { 
                return this.FrameworkContext.CurrentUser;
            }
            set
            {      
                if (HttpContext.Current != null)
                    HttpContext.Current.Session["CurrentUser"] = value;                
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

