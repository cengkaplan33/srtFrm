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
    public class ExceptionLog : EntityBase<int>
    {
        [Index("IX_ExceptionLog_Date_ExceptionLevel", 1)]
        public DateTime LogDate { get; set; }
        [Index("IX_ExceptionLog_Date_ExceptionLevel", 2)]
        [Required]
        public int SystemId { get; set; }
        [Required]
        public byte ExceptionLevel { get; set; }
        [Required][MaxLength(50)]
        public string ExceptionType { get; set; }
        [Required][MaxLength(5000)]
        public string Data { get; set; }
        [Required]
        public int InsertedByUser { get; set; }
    }
}
