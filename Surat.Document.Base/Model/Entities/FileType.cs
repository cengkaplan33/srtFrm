using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Base.Model.Entities
{
    public class FileType : EntityBase<int>
    {
        [Required][MaxLength(4)]
        public string TypeName { get; set; }
        [Required][MaxLength(30)]
        public string Name { get; set; } 
        [Required][MaxLength(30)]
        public string IconFileName { get; set; }
    }
}
