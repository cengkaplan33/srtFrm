using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.Common.Data;
using Surat.Common.Utilities;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Providers
{
    public class SuratAuthenticationProvider : IAuthenticationProvider
    {
        public UserDetailedView ValidateUser(ApplicationContextBase applicationContextParameter,string userName, string password)
        {
            SuratUser user;
            UserDetailedView currentUser = null;
            FrameworkContext context = (FrameworkContext)applicationContextParameter;
            UserRepository userRepository = new UserRepository(context.Security);
            CompanySiteRepository companySiteRepository = new CompanySiteRepository(context.Security);

            user = userRepository.GetObjectByParameters(p => p.UserName == userName);
        
            if (user != null)
            {
                if (user.Password == password)
                {
                    if (user.IsLocked)
                        throw new SuratBusinessException(context, "ValidateUser", context.SystemId, 
                            string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.Message.UserLocked),userName));

                    if (!user.IsActive)
                        throw new SuratBusinessException(context,"ValidateUser", context.SystemId, 
                            string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.Message.UserIsNotActive),userName));

                    if (user.DefaultRole == null)
                        throw new SuratBusinessException(context,"ValidateUser", context.SystemId, context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.Message.UserDefaultRoleMissing));

                    if (user.DefaultWorkgroup == null)
                        throw new SuratBusinessException(context,"ValidateUser", context.SystemId, context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.Message.UserDefaultWorkgroupMissing));

                    currentUser = new UserDetailedView();

                    currentUser.Name = user.Name;
                    currentUser.ShortName = user.UserName;
                    currentUser.UserId = user.Id;
                    currentUser.DefaultRole = user.DefaultRole;
                    currentUser.DefaultWorkgroup = user.DefaultWorkgroup;
                    currentUser.IsAdmin = userRepository.IsAdmin(currentUser.ShortName,currentUser.UserId);

                    if (!currentUser.IsAdmin)
                    { 
                        if (user.LastPasswordChangedDate.HasValue)
                        {
                            if (user.LastPasswordChangedDate.Value.AddDays(context.Security.MaxPasswordChangePeriodAsDays) < TimeUtility.GetCurrentDateTime())
                            {
                                throw new SuratBusinessException(context, "ValidateUser", context.SystemId, context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.Message.PasswordChangePeriodExceeded));
                            }
                        }
                        else throw new NullValueException(context,"LastPasswordChangedDate", context.SystemId);

                        currentUser.CompanySites = userRepository.GetUserCompanySites(user.Id);       
                    }
                    else
                    {
                        currentUser.CompanySites = companySiteRepository.GetAllCompanySites();
                    }                    

                    if (currentUser.CompanySites.Count == 0)
                        throw new SuratBusinessException(context,"ValidateUser", context.SystemId, context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.Message.UserCompanyMissing));
                    else
                    {
                        currentUser.SelectedCompanySite = currentUser.CompanySites.ElementAt(0);
                    }

                }
                else throw new WrongPasswordException(context,"ValidateUser", context.SystemId);                
            } else throw new RecordNotFoundException(context,"ValidateUser", context.SystemId, string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.ExceptionType.RecordNotFound),userName));
       
            return currentUser;
        }

        public void LockUser(ApplicationContextBase applicationContextParameter, string userName, string password,bool isLocked)
        {
            int initializedDBContextId;
            SuratUser user;
            FrameworkContext context = (FrameworkContext)applicationContextParameter;
            UserRepository userRepository = new UserRepository(context.Security);

            user = userRepository.GetObjectByParameters(p => p.UserName == userName);

            if (user != null)
            {
                initializedDBContextId = context.InitializeDBContext();

                user.IsLocked = isLocked;
                userRepository.Update(user);

                context.CommitDBChanges(initializedDBContextId);                                        
            }
            else
            {
                throw new RecordNotFoundException(context, "Lockuser", context.SystemId, 
                    String.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.ExceptionType.RecordNotFound),userName));
            }
        }


        public string GetUserPassword(ApplicationContextBase applicationContextParameter, int userId)
        {
            string password; 
            SuratUser user;
            FrameworkContext context = (FrameworkContext)applicationContextParameter;
            UserRepository userRepository = new UserRepository(context.Security);

            user = userRepository.GetById(userId);

            if (user != null)
                password = user.Password;
            else throw new RecordNotFoundException(context,"GetUserPassword.User", context.SystemId,
                string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.ExceptionType.RecordNotFound), userId));

            return password;           
        }
    }
}
