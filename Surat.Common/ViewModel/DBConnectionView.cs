using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.ViewModel
{
    public class DBConnectionView
    {
        public DBEnvironmentType DBEnvironmentType { get; set; }
        public string DBKeyName { get; set; }
        public string DBConnectionString { get; set; }
        public bool IsDefault { get; set; }
    }
}

