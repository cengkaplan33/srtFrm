using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.ViewModel
{
    public class CompanySiteView
    {
        public int Id { get; set; }
        public int WorkgroupId { get; set; }
        public int CompanyId { get; set; }
        public string WorkgroupName { get; set; }
        public int? CompanyCode { get; set; }
        public DBConnectionView SelectedDBConnection { get; set; }
        public List<DBConnectionView> DBConnections { get; set; }
    }
}

