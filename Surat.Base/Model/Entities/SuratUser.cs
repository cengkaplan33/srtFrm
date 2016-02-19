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
    public class SuratUser : AuditableEntityBase<int>
    {
        [Index][Required][MaxLength(30)]
        public string UserName { get; set; }
        [Index][Required][MaxLength(30)]
        public string Name { get; set; }
        [Required][MaxLength(35)]
        public string Password { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }        
        public bool IsLocked { get; set; }
        public bool IsActiveDirectoryUser { get; set; }
        public bool IsExternalUser { get; set; }
        public int? DefaultRole { get; set; }
        public int? DefaultWorkgroup { get; set; }
    }
}
