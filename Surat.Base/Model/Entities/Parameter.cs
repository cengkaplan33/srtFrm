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
    public class Parameter : AuditableEntityBase<int>
    {
        [Index("IX_Parameter_DbObjectType_DBObjectId", 1)]
        public byte DBObjectType { get; set; }
        [Index("IX_Parameter_DbObjectType_DBObjectId", 2)]
        public long DBObjectId { get; set; }
        [MaxLength(50)][Required]
        public string TypeName { get; set; }
        public bool IsCustomizable { get; set; }
        [MaxLength(100)]
        public string Explanation { get; set; }
    }
}
