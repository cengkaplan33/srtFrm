using Surat.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.ViewModel
{
    public class ExternalSystemsUsersView
    {
        public int? Id { get; set; }
        public DelegateObjectType DelegateDBObjectType { get; set; }
        public long DelegateDBObjectId { get; set; }
        public int SystemId { get; set; }
        public int SuratUserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? FirmaDonemId { get; set; }
        public string FirmaDonem { get; set; }
        public MasterDBFirmaDonemiTipi FirmaDonemTipi { get; set; }
        public string DatabaseName { get; set; }
        public bool VarsayilanMi { get; set; }
        public string FirmaDonemTipiName { get; set; }
    }
}

