using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surat.Common.ViewModel
{
    public class UserView
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Notes { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public bool IsLocked { get; set; }
        public bool IsActiveDirectoryUser { get; set; }
        public bool IsExternalUser { get; set; }

        public int? DefaultRole { get; set; }
        public int? DefaultWorkgroup { get; set; }

        public string WorkgroupName;
        public string CompanyName;
    }
}