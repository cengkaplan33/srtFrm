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
    public class UserEvent : EntityBase<long>
    {
        [Index("IX_UserEvent_User_Date_EventType", 1)]
        public int UserId { get; set; }
        [Index("IX_UserEvent_User_Date_EventType", 2)]
        public DateTime EventDate { get; set; }
        [Index("IX_UserEvent_User_Date_EventType", 3)]
        public byte EventTypeId { get; set; }
        public long DBObjectId { get; set; }
    }
}
