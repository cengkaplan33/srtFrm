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
    public class RelationGroupAccessiblePageView
    {
        public int PageId { get; set; }
        public int SystemId { get; set; }
        public int? SystemParentId { get; set; }
        public string PageName { get; set; }
        public string SystemName { get; set; }
        public string ObjectTypeName { get; set; }
        public int HasAccess { get; set; }
    }
}
