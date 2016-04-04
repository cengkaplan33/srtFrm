using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Model.Entities
{
    public class HazirDegerTablosu : AuditableEntityBase<int>
    {
        [Required(ErrorMessage = "Hazır Değer Kodu alanı gereklidir")]
        [Range(1, 999999999999, ErrorMessage = "Değer 0 veya 0 dan küçük olamaz")]
        public string Kod { get; set; }

        [Required(ErrorMessage = "Değer Alanı Gereklidir.")]
        [MaxLength(100)]
        public string HazirDeger { get; set; }
        public int OdemeTalepDurumuId { get; set; }
        public int BankId { get; set; }

        [Required(ErrorMessage = "Tür Alanı Gereklidir.")]
        [MaxLength(40)]
        public string Tur { get; set; }

        public DateTime Tarih { get; set; }

        public decimal TL { get; set; }

        public decimal USD { get; set; }

        public decimal EURO { get; set; }

        public int WorkGroupId { get; set; }
    }
}
