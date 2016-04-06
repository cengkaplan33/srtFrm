using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class UserAccessiblePageView
    {
        public int PageId { get; set; }     
        public string PageName { get; set; }
        public bool IsRoleEffect { get; set; }
        public bool IzinVer { get; set; }
        public bool Yasakla { get; set; }
        public string IsPageAccess { get; set; }
        
    }
}
