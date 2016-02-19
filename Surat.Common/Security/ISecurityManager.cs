using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Security
{
    public interface ISecurityManager
    {
        int GetUserDefaultWorkgroup(int userId);
        int? GetParentWorkgroupId(int workgroupId);
    }
}
