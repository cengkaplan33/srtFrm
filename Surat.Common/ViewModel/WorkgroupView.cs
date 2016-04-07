using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.ViewModel
{
    public class WorkgroupView
    {        
        public int WorkgroupId { get; set; }
        public int? ParentWorkgroupId { get; set; }
        public string WorkgroupName { get; set; }
        public int CompanyId { get; set; }
        public bool IsCompanySite { get; set; }
    }
}

