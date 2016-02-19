using Surat.Common.Data;
using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.ViewModel
{
    public class ParameterValueView
    {
        public ParameterOwnerDBObjectType ParameterOwnerDBObjectType { get; set; }
        public int ParameterOwnerDBObjectId { get; set; }
        public string ParameterTypeName { get; set; }
        public string ParameterValue { get; set; }
    }
}
