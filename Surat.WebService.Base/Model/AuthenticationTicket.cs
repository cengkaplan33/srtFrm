using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.WebServiceBase.Model
{
    public class AuthenticationToken
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime StartTime { get; set; }
    }
}
