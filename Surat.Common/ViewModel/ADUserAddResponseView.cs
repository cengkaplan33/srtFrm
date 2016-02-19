using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class ADUserAddResponseView
    {
        public bool Added{ get; set; }
        public bool PasswordSet { get; set; }
        public bool Enabled { get; set; }
        public string StatusMessage { get; set; }
    }
}
