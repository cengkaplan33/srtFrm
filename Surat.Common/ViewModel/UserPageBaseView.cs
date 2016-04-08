using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class UserPageBaseView
    {
        public int PageId { get; set; }     
        public int? RoleId { get; set; }
        public int? RelationGrupId { get; set;}
        public int? AccessibleItemId { get; set; }
        public byte AccessRightTypeId { get; set; }
    }
}
