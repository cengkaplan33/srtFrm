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
    public class DocumentStore : AuditableEntityBase<int>
    {
        public int WorkgroupId { get; set; }
        [Required][MaxLength(50)]
        public string Name { get; set; }
        [Required][MaxLength(30)]
        public string TypeName { get; set; }
        [Required][MaxLength(100)]
        public string RootFilePath { get; set; }
        public int RootDocumentGroupId { get; set; }
        public int SizeInGB { get; set; }
        public int MaximumDocumentCount { get; set; }
        public int MaximumDocumentSizeInMB { get; set; }
    }
}
