using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.ViewModel
{
    public class ExternalSystemsUsersView
    {
        public DelegateObjectType DelegateDBObjectType { get; set; }
        public long DelegateDBObjectId { get; set; }
        public int SystemId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } 
    }
}

