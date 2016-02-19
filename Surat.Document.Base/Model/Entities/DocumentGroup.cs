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
    public class DocumentGroup : AuditableEntityBase<int>
    {
        [Required][MaxLength(50)]
        public string Name { get; set; }
        [Required][MaxLength(30)]
        public string ShortName { get; set; }
        [Index]
        public int ParentId { get; set; }
    }
}
