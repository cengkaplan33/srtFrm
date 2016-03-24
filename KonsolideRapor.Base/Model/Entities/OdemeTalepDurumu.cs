using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Model.Entities
{
    public class OdemeTalepDurumu : AuditableEntityBase<int>
    {
        [Required(ErrorMessage = "Durum Alanı Gereklidir.")]
        [MaxLength(50)]
        public string  Durum { get; set; }
        public bool IsBanka { get; set; }
        public bool IsOdeme { get; set; }
        public bool IsTahsilat { get; set; }
    }
}
