using Surat.Common.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surat.Base.Model.Entities
{
    public class SuratAction : AuditableEntityBase<int>
    {
        [Index]
        [MaxLength(100)]
        public string TypeName { get; set; }
        public int SystemId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
    }
}
