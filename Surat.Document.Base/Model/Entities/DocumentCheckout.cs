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
    public class DocumentCheckout : AuditableEntityBase<int>
    {
        [Index]
        public int DocumentId { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime CheckoutExpirationDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public int CheckoutBy { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }
    }
}
