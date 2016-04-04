using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Common.ViewModel
{
     public class HazirOdemeView
    {
        public int OId { get; set; }
        public int? HBankId { get; set; }
        public int BId { get; set; }
        public string BName { get; set; }
        public string ODurum { get; set; }
        public decimal? HTL { get; set; }
        public decimal? HEURO { get; set; }
        public decimal? HUSD { get; set; }
        public int? HId { get; set; }
        public string Kod { get; set; }
        public string BObjectType { get; set; }
    }
}
