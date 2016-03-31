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
    public class PagesListView
    {
        public int  Id { get; set; }
        public int SystemId { get; set; }
        public int? SystemParentId { get; set; }
        public string PageName { get; set; }
        public string SystemName { get; set; }
        public string ObjectTypePrefix { get; set; }
        public Boolean IsAccessControlRequired { get; set; }
        public Boolean IsVisibleInMenu { get; set; }
        public int InsertedByUser { get; set; }
        public string InsertedByUserName { get; set; }
        public DateTime InsertedDate { get; set; }
        public int? ChangedByUser { get; set; }
        public string ChangedByUserName { get; set; }
        public DateTime? ChangedDate { get; set; }

    }
}
