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
    public class ParameterChange : EntityBase<int>
    {
        public int ParameterId { get; set; }
        [MaxLength(100)]
        public string OldValue { get; set; }
        public int InsertedByUser { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
