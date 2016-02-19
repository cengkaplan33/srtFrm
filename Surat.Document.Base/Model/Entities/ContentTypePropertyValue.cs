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
    public class ContentTypePropertyValue : EntityBase<int>
    {
        [Index("IX_PropertyValue_StoreId_DocumentId", 1)][Required]
        public int DocumentStoreId { get; set; }
        [Index("IX_PropertyValue_StoreId_DocumentId", 2)]
        [Required]
        public int DocumentId { get; set; } 
        [Index][Required]
        public int PropertyId { get; set; }       
        [Required][MaxLength(200)]
        public string PropertyValue { get; set; }
    }
}
