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
    public class CompanySiteDBPeriod : AuditableEntityBase<int>
    {
        [Index][Required]
        public int CompanySiteId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDefault { get; set; }
        public byte DBEnvironmentType { get; set; }
        [MaxLength(150)]
        public string DBKeyName { get; set; }
        [MaxLength(150)][Required]
        public string ConnectionString { get; set; }
    }
}
