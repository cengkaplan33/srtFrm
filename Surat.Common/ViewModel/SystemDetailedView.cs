using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class SystemDetailedView
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public String Name { get; set; }
        public string ObjectTypeName { get; set; }
    }
}
