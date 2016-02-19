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
    public class DocumentShare : AuditableEntityBase<int>
    {
        [Index]
        public int DocumentId { get; set; }
        public DateTime SharedDate { get; set; }
        public DateTime ShareExpirationDate { get; set; }
        [Index]
        public int SharedByUserId { get; set; }
        [Index]
        public int SharedToRelationId { get; set; }
    }
}
