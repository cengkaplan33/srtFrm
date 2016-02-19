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
    public class UserShortcut : EntityBase<int>
    {
        [Index]
        public int UserId { get; set; }       
        public int PageId { get; set; }
        public bool isActive { get; set; }
    }
}
