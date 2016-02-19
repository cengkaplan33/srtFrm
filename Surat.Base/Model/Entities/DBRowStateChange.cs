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
    public class DBRowStateChange : EntityBase<int>
    {
        public byte DBObjectType { get; set; }
        public long DBObjectId { get; set; }
        public DateTime ChangedDate { get; set; }
        public int ChangedByUser { get; set; }
        [MaxLength(20)]
        public string NewState { get; set; }
    }
}
