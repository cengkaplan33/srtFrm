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
    public class RelationGroup : AuditableEntityBase<int>
    {
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public int? WorkgroupId { get; set; }
    }
}
