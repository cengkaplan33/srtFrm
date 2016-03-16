using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.WebServer.ViewModel
{
    public class LoginView
    {
        public string RedirectPage { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsValid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RememberMe { get; set; }
    }
}
