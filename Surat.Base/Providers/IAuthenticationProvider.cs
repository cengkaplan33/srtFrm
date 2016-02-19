using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Providers
{
    public interface IAuthenticationProvider
    {
        void LockUser(ApplicationContextBase applicationContextParameter, string userName, string password,bool isLocked);
        UserDetailedView ValidateUser(ApplicationContextBase applicationContextParameter,string userName, string password);
        string GetUserPassword(ApplicationContextBase applicationContextParameter, int userId);
    }
}
