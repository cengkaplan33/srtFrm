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
    public class SuratRight : AuditableEntityBase<int>
    {
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public int SystemId { get; set; }

        public virtual SuratSystem System { get; set; }
    }
}
