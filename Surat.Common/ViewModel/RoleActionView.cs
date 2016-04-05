using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class RoleActionView
    {
        public int ActionId { get; set; }
        public int IsAccessible { get; set; }
        public int? RelationGroupId { get; set; }
        public int? AccessibleItemId { get; set; }
        public string ActionName { get; set; }

    }
}
