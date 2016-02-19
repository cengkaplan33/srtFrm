﻿using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Model.Entities
{
    public class SuratService : AuditableEntityBase<int>
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string TypeName { get; set; }
        public int SystemId { get; set; }
        [MaxLength(30)]
        public string UserName { get; set; }
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
