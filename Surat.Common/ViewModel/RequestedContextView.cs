using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class InitializedContextView
    {        
        public int Id { get; set; }
        public int ParentId { get; set; }
        public ContextMode ContextMode { get; set; }
    }
}
