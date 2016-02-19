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
    public class ToolbarItem : EntityBase<int>
    {
        [Index]
        public int ToolbarId { get; set; }
        [MaxLength(30)] 
        public string ObjectTypeName { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public byte OrderInToolbar { get; set; }
        [MaxLength(100)]
        public string ImagePath { get; set; }
        public virtual Toolbar Toolbar { get; set; }
    }
}
