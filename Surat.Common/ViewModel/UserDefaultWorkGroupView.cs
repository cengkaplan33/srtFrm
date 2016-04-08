using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class UserDefaultWorkGroupView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ObjectTypeName { get; set; }
        public int? ParentId { get; set; }
        public bool isCompanySite { get; set; }
        public int CompanyId { get; set; }
    }
}
