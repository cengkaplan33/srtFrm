using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Entity
{
    public abstract class EntityBase<T> : IEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
