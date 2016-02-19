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
    public class CompanySite : AuditableEntityBase<int>
    {
        [Index]
        public int RelatedWorkgroupId { get; set; }
        public int? CompanyCode { get; set; }
    }
}
