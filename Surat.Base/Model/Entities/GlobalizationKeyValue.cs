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
    public class GlobalizationKeyValue : EntityBase<int>
    {
        [Index("IX_GlobalizationKeyValue_Id_CultureName", 1)]
        public int GlobalizationKeyId { get; set; }
        [Index("IX_GlobalizationKeyValue_Id_CultureName", 2)]
        public byte CultureId { get; set; }
        [MaxLength(200)]
        public string Content { get; set; }
    }
}
