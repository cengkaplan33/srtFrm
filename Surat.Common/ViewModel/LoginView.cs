using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surat.Common.ViewModel
{
    public class LoginView
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool isActiveDirectoryUser { get; set; }
        public bool RememberMe { get; set; }
    }
}