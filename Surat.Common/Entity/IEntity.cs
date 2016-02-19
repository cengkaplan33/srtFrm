using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.Entity
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
