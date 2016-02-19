using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Surat.Base.Application;
using Surat.Base.Mail;
using Surat.Common.Log;
using Surat.Common.ViewModel;
using Surat.Business.Application;
using Newtonsoft.Json;
using Surat.WebServiceBase.Model;
using Surat.Common.Utilities;
using Surat.Base.Exceptions;
using Surat.Common.Data;
using Surat.WebService.Common.Data;

namespace Surat.WebService.Business
{
    public class AuthenticationManager
    {
        #region Constructor

        public AuthenticationManager(FrameworkApplicationManager framework)
        {
            this.framework = framework; 
        }

        #endregion

        #region Private Members

        private FrameworkApplicationManager framework;

        #endregion

        #region Public Members           


        #endregion

        #region Methods      
  
        public string Authenticate(string userName, string password)
        {
            AuthenticationToken token;
            string json;
            string tokenString;            

            try
            {
                UserDetailedView currentUser = this.framework.Security.ValidateUser(userName, password);
                this.framework.StartUserSession(currentUser);

                token = new AuthenticationToken() { UserName = userName, Password = password, StartTime = TimeUtility.GetCurrentDateTime() };
                json = JsonConvert.SerializeObject(token);
                tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.framework.Context, "Authenticate", 0, this.framework.GetGlobalizationKeyValue(0,ServiceConstants.Message.ServiceAuthenticationFailed), exception); //ToDo : SystemId verilmeli
            }

            return tokenString;
        }

        #endregion

    }
}
