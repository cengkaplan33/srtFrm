using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.ViewModel
{
    public class UserDetailedView
    {       
        public int UserId { get; set; }
        public string ShortName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public long SessionId { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionFinish { get; set; }
        public string RequestIP { get; set; }
        public CompanySiteView SelectedCompanySite { get; set; }
        public List<CompanySiteView> CompanySites { get; set; }
        public int? DefaultRole { get; set; }
        public int? DefaultWorkgroup { get; set; }
    }
}

