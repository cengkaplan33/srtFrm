using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surat.Common.ViewModel
{
    public class UserSessionView
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime? SessionEnd { get; set; }
        public string IP { get; set; }
    }
}