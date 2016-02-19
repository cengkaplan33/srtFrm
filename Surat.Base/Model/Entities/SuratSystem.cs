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
    public class SuratSystem : AuditableEntityBase<int>
    {
        [Index]
        public int? ParentId { get; set; }
        [MaxLength(30)]
        public string ObjectTypeName { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string SmallImagePath { get; set; }
        [MaxLength(100)]
        public string LargeImagePath { get; set; }
    }
}
