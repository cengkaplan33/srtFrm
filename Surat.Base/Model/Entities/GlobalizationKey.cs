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
    public class GlobalizationKey : EntityBase<int>
    {
        [Index]
        public int SystemId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
