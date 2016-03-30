using Surat.Base.Model.Entities;
using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Model.Entities
{
    public class OdemeTalep : AuditableEntityBase<int>
    {
        [Required(ErrorMessage = "Code Alanı Gereklidir.")]       
        public DateTime Tarih { get; set; }
        public int WorkgroupId { get; set; }
        public int PaymentCollectingId { get; set; }
        [ForeignKey("PaymentCollectingId")]
        public PaymentCollecting PaymentCollecting { get; set; }
        public int OdemeTalepDurumuId { get; set; }
        [ForeignKey("OdemeTalepDurumuId")]
        public OdemeTalepDurumu OdemeTalepDurumu { get; set; }
        public Nullable<decimal> TL { get; set; }
        public Nullable<decimal> USD { get; set; }
        public Nullable<decimal> EURO { get; set; }
        [Required(ErrorMessage="Talep türü gereklidir")]
        [MaxLength(10)]
        public string TalepTuru { get; set; }
        public string Aciklama { get; set; }

    }
}
