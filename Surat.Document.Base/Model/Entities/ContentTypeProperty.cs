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
    public class ContentTypeProperty : AuditableEntityBase<int>
    {
        [Index][Required]
        public int ContentTypeId { get; set; }
        [Required][MaxLength(50)]
        public string Name { get; set; }
        [Required][MaxLength(50)]
        public string TypeName { get; set; }
        [Required]
        public byte PropertyType { get; set; }
        /// <summary>
        /// Özellikler sıralanır. Değer istendiğinde; 0 > ler sıraya göre birleştirilerek verilir.
        /// </summary>
        public int DefaultOrder { get; set; }
    }
}
