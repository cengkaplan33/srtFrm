using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Model.Entities
{
    public class OdemeTalep : AuditableEntityBase<int>
    {
        public DateTime Tarih { get; set; }
    }
}
