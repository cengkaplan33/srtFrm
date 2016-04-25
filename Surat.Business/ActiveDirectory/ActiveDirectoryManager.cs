using Surat.Base.ActiveDirectory;
using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Base.Providers;
using Surat.Base.Repositories;
using Surat.Base.Security;
using Surat.Business.Configuration;
using Surat.Business.Globalization;
using Surat.Business.Log;
using Surat.Common.Data;
using Surat.Common.Utilities;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

namespace Surat.Business.ActiveDirectory
{
    public class ActiveDirectoryManager
    {
        #region Constructor

        public ActiveDirectoryManager(FrameworkContext applicationContextParameter, SuratConfigurationManager configurationManager)
        {
            this.applicationContext = applicationContextParameter;  
            this.configurationManager = configurationManager;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private SuratConfigurationManager configurationManager;
        private DirectoryEntry directoryEntry;
      
        #endregion

        #region Public Members   

        public FrameworkContext ApplicationContext
        {
            get
            {
                return applicationContext;
            }
        }        
        
        public ActiveDirectoryContext Context
        {
            get
            {
                return applicationContext.ActiveDirectory;
            }
        }

        public SuratConfigurationManager Configuration
        {
            get
            {
                return configurationManager;
            }
        }        
        
        #endregion

        #region Methods 

        private String GetProperty(DirectoryEntry adUser, String propertyName)
        {
            if (adUser.Properties.Contains(propertyName))
            {
                return adUser.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private DirectoryEntry GetADUserEntry(string userName)
        {
            DirectoryEntry userEntry = null;

            try
            {
                directoryEntry = new DirectoryEntry(this.Context.Service, this.Context.UserName, this.Context.Password, AuthenticationTypes.Secure | AuthenticationTypes.Sealing | AuthenticationTypes.ServerBind);
                DirectorySearcher directorySearch = new DirectorySearcher(directoryEntry);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
                SearchResult results = directorySearch.FindOne();

                if (results != null)
                {
                    userEntry = results.GetDirectoryEntry();
                }

                directoryEntry.Close();
            }
            catch (Exception exception)
            {
                this.ApplicationContext.Trace.AppendLine(this.ApplicationContext.SystemName, "Exception : " + exception.ToString(), TraceLevel.Basic);
                throw exception;
            }

            return userEntry;
        }

        private bool CheckADUser(string userName)
        {
            if (GetADUserEntry(userName) != null)
                return true;
            else return false;
        }
        public bool ActiveDirectoryUserCheck()
        {
            PrincipalContext cn = new PrincipalContext(ContextType.Domain, this.Context.Domain, this.Context.Container, this.Context.UserName, this.Context.Password);
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(cn, Environment.UserName);
            if (userPrincipal != null)
                return true;
           
            return false;
        }
        public ADUserDetailView GetADUser(string userName)
        {
           
        
            
            
           // bool status = cn.ValidateCredentials("zafer.akkose", "gebzE.198s");
          
            ADUserDetailView adUser = null;
            try
            {
                DirectoryEntry user = GetADUserEntry(userName);

                adUser = new ADUserDetailView();

                adUser.FirstName = GetProperty(user, ADProperties.FIRSTNAME);
                adUser.MiddleName = GetProperty(user, ADProperties.MIDDLENAME);
                adUser.LastName = GetProperty(user, ADProperties.LASTNAME);
                adUser.DisplayName = GetProperty(user, ADProperties.DISPLAYNAME);
                adUser.Title = GetProperty(user, ADProperties.TITLE);
                adUser.EmailAddress = GetProperty(user, ADProperties.EMAILADDRESS);
                adUser.Mobile = GetProperty(user, ADProperties.MOBILE);
                adUser.Company = GetProperty(user, ADProperties.COMPANY);
                adUser.Department = GetProperty(user, ADProperties.DEPARTMENT);

                adUser.ManagerName = GetProperty(user, ADProperties.MANAGER);

                if (!String.IsNullOrEmpty(adUser.ManagerName))
                {
                    String[] managerArray = adUser.ManagerName.Split(',');
                    adUser.ManagerName = managerArray[0].Replace("CN=", "");
                }

                adUser.Extension = GetProperty(user, ADProperties.EXTENSION);
                adUser.Fax = GetProperty(user, ADProperties.FAX);
                adUser.HomePhone = GetProperty(user, ADProperties.HOMEPHONE);
                adUser.LoginName = GetProperty(user, ADProperties.LOGINNAME);
                adUser.Country = GetProperty(user, ADProperties.COUNTRY);
                adUser.State = GetProperty(user, ADProperties.STATE);
                adUser.City = GetProperty(user, ADProperties.CITY);
                adUser.StreetAddress = GetProperty(user, ADProperties.STREETADDRESS);
                adUser.PostalCode = GetProperty(user, ADProperties.POSTALCODE);                
            }
            catch (Exception exception)
            {
                this.ApplicationContext.Trace.AppendLine(this.ApplicationContext.SystemName, "Exception : " + exception.ToString(), TraceLevel.Basic);
            }

            return adUser;
        }

        public ADUserAddResponseView AddADUser(ADUserView user)
        {
            ADUserAddResponseView response = new ADUserAddResponseView();

            try
            {
                if (!CheckADUser(user.UserName))
                {
                    response.Added = AddADUserInternal(user);

                    if (SetPassword(user.UserName, user.Password))
                        response.PasswordSet = true;
                    else response.StatusMessage = "Kullanıcı açılmış, ama şifresi ayarlanamamıştır.";

                    if (EnableADUser(user.UserName,true))
                        response.Enabled = true;
                    else response.StatusMessage = "Kullanıcı açılmış, ama aktif hale getirilememiştir.";
                }
                else
                {
                    response.StatusMessage = user.UserName + " - Kullanıcı kodu ile bir kullanıcı olduğu için, işlem gerçekleştirilmemiştir.";
                }
            }
            catch (Exception exception)
            {
                this.ApplicationContext.Trace.AppendLine(this.ApplicationContext.SystemName, "Exception : " + exception.ToString(), TraceLevel.Basic);
            }

            return response;
        }

        private bool AddADUserInternal(ADUserView user)
        {
            bool result = false;
            DirectoryEntry newUser = null;
            try
            {
                directoryEntry = new DirectoryEntry(this.Context.Service, this.Context.UserName, this.Context.Password, AuthenticationTypes.Secure);
                newUser = directoryEntry.Children.Add("CN=" + user.UserName, "user");

                newUser.Properties["samAccountName"].Value = user.UserName;
                newUser.Properties["displayName"].Add(user.DisplayName);
                newUser.CommitChanges();
                newUser.Close();
                result = true;
            }
            catch (Exception exception)
            {
                this.ApplicationContext.Trace.AppendLine(this.ApplicationContext.SystemName, "Exception : " + exception.ToString(), TraceLevel.Basic);
                throw exception;
            }

            return result;
        }

        public bool SetPassword(string userName,string password)
        {
            bool result = false;
            DirectoryEntry user = null;
            const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
            const long ADS_OPTION_PASSWORD_METHOD = 7;
            const int ADS_PASSWORD_ENCODE_CLEAR = 1;

            try
            {
                user = GetADUserEntry(userName);

                if (user != null)
                {
                    user.Invoke("SetOption", new object[] { ADS_OPTION_PASSWORD_PORTNUMBER, 389 });
                    user.Invoke("SetOption", new object[] { ADS_OPTION_PASSWORD_METHOD, ADS_PASSWORD_ENCODE_CLEAR });
                    user.Invoke("SetPassword", new object[] { password });
                    user.Properties["pwdLastSet"].Value = 0; //User Must Change password at next login - Check koymak anlamındadır.
                    user.CommitChanges();
                    result = true;
                    user.Close();
                }
            }
            catch (Exception exception)
            {
                this.ApplicationContext.Trace.AppendLine(this.ApplicationContext.SystemName, "Exception : " + exception.ToString(), TraceLevel.Basic);
            }

            return result;
        }

        public bool EnableADUser(string userName, bool isEnabled)
        {
            bool result = false;

            try
            {
                DirectoryEntry userEntry = GetADUserEntry(userName);

                if (userEntry != null)
                {
                    int propertyValue = (int)userEntry.Properties["userAccountControl"].Value;

                    //ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE
                    if (isEnabled)
                        propertyValue = propertyValue & ~0x2;
                    else propertyValue = propertyValue & 0x2;

                    userEntry.Properties["userAccountControl"].Value = propertyValue;
                    userEntry.CommitChanges();
                    result = true;
                }
            }
            catch (Exception exception)
            {
                this.ApplicationContext.Trace.AppendLine(this.ApplicationContext.SystemName, "Exception : " + exception.ToString(), TraceLevel.Basic);
            }               

            return result;
        }
 
        #endregion
    }
}
