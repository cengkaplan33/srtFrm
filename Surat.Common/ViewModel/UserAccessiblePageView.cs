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
        public int? IzinVer { get; set; }
        public int? Yasakla { get; set; }
        public Boolean IsPageAccess { get; set; }
        
    }
}
