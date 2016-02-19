using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Providers
{
    public class SerendipAuthenticationProvider : IAuthenticationProvider
    {
        public UserDetailedView ValidateUser(ApplicationContextBase applicationContextParameter,string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void LockUser(ApplicationContextBase applicationContextParameter, string userName, string password,bool isLocked)
        {
            throw new NotImplementedException();
        }


        public string GetUserPassword(ApplicationContextBase applicationContextParameter, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
