using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Common.ViewModel
{
    public class HazirOdemeTablosuView
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Kod { get; set; }
        public string HazirDeger { get; set; }       
        public int OdemeTalepDurumuId { get; set; }
        public int BankId { get; set; }
        public string Tur { get; set; }
        public DateTime Tarih { get; set; }
        public decimal? TL { get; set; }
        public decimal? USD { get; set; }
        public decimal? EURO { get; set; }
        public int WorkGroupId { get; set; }
        public int? HId { get; set; }
    }
}
