using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Model.Entities
{
    public class Workgroup : AuditableEntityBase<int>
    {
        [Index][MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string ObjectTypeName { get; set; }
        public int? ParentId { get; set; }
        public bool isCompanySite { get; set; }
    }
}
