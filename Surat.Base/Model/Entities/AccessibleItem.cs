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
    public class AccessibleItem : AuditableEntityBase<int>
    {        
        [Index]
        public int RelationGroupId { get; set; }
        public byte DBObjectType { get; set; }
        public long DBObjectId { get; set; }
        public byte AccessRightTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
