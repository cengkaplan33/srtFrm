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
    public class ExternalSystemsUser : AuditableEntityBase<int>
    {
        [Index("IX_ExternalSystemsUser_DelegateDbObjectType_DBObjectId", 1)]
        public byte DelegateDBObjectType { get; set; }
        [Index("IX_ExternalSystemsUser_DelegateDbObjectType_DBObjectId", 2)]
        public long DelegateDBObjectId { get; set; }
        [Required]
        public int SystemId { get; set; }
        [MaxLength(30)]
        public string UserName { get; set; }
        [MaxLength(35)]
        public string Password { get; set; }
        public int FirmaDonemId { get; set; }
        [MaxLength(30)]
        public string FirmaDonem { get; set; }
        public Int16 FirmaDonemTipi { get; set; }
        [MaxLength(100)]
        public string DbName { get; set; }
        public bool VarsayilanMi { get; set; }
    }
}
