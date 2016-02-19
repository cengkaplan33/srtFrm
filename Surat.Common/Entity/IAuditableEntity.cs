using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Entity
{
    public interface  IAuditableEntity
    {
        int InsertedByUser { get; set; }
        DateTime InsertedDate { get; set; }
        int? ChangedByUser { get; set; }
        DateTime? ChangedDate { get; set; }
        bool IsActive { get; set; }
    }
}
