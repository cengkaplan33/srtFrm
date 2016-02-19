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
    public class UserSession : EntityBase<long>
    {
        [Index("IX_UserSession_User_SessionStart", 1)]
        public int UserId { get; set; }
        [Index("IX_UserEvent_User_SessionStart", 2)]
        public DateTime SessionStart { get; set; }  
        public DateTime? SessionEnd { get; set; }
        [MaxLength(15)]
        public string IP { get; set; }
    }
}
