using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Base.Model.Entities
{
    public class DocumentVersion : AuditableEntityBase<int>
    {
        [Index]
        public int DocumentId { get; set; }
        public DateTime VersionDate { get; set; }
        [MaxLength(10)]
        public string VersionNumber { get; set; }
        [Index("IX_DocumentVersionStore_Group", 1)]
        public int DocumentStoreId { get; set; }
        [Index("IX_DocumentVersionStore_Group", 2)]
        public int DocumentGroupId { get; set; }
    }
}
