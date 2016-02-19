using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Common.ViewModel
{
    public class DocumentParametersView
    {
        public int ExecutionTimeoutAsSeconds { get; set; }
        public int MaxRequestLength { get; set; }
        public int MaxAllowedContentLength { get; set; }
    }
}
