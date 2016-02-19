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
    public class Page : AuditableEntityBase<int>
    {
        [Index]
        public int SystemId { get; set; }
        [MaxLength(100)]
        public string ObjectTypePrefix { get; set; }
        [MaxLength(30)]
        public string ObjectTypeName { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsAccessControlRequired { get; set; }
        public bool IsVisibleInMenu { get; set; }
        [MaxLength(100)]
        public string BigImagePath { get; set; }
        [MaxLength(100)]
        public string SmallImagePath { get; set; }

        public virtual SuratSystem System { get; set; }
    }
}
