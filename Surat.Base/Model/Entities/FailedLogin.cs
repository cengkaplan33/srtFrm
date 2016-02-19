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
    public class FailedLogin : EntityBase<int>
    {
        public int UserId { get; set; }       
        public DateTime LoginDate { get; set; }
        [MaxLength(20)]
        public string PasswordUsed { get; set; }
    }
}
