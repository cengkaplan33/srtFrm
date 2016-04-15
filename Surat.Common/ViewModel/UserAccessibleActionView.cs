using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class UserAccessibleActionView
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public bool IsRoleEffect { get; set; }
        public int? IzinVer { get; set; }
        public int? Yasakla { get; set; }
        public Boolean IsAccess { get; set; }

    }
}
