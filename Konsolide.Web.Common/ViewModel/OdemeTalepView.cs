using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Web.Common.ViewModel
{
    public class OdemeTalepView
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public int PaymentCollectingId { get; set; }
        public string Name { get; set; }
        public decimal TL { get; set; }
        public decimal USD { get; set; }
        public decimal EURO { get; set; }
        public int FirmaId { get; set; }
        public int OdemeTalepDurumuId { get; set; }
        public string Durum { get; set; }
        public bool IsActive { get; set; }
    }
}
