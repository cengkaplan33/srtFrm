using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Log
{
    public interface ITraceManager
    {
        void AppendLine(string systemName, string traceInformation, TraceLevel traceLevel);
        void WriteTraceToFile();
    }
}
