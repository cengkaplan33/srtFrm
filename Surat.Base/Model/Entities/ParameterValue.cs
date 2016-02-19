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
    public class ParameterValue : AuditableEntityBase<int>
    {
        [Index]
        public byte ParameterOwnerDBObjectType { get; set; }
        public int ParameterOwnerDBObjectId { get; set; }
        public int ParameterId { get; set; }  
        [MaxLength(100)]
        public string Value { get; set; }
    }
}
