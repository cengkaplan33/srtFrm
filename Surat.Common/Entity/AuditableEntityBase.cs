using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Entity
{
    public abstract class AuditableEntityBase<T> : EntityBase<T>,IAuditableEntity
    {
        public int InsertedByUser { get; set; }
        public DateTime InsertedDate { get; set; }
        public int? ChangedByUser { get; set; }
        public DateTime? ChangedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
