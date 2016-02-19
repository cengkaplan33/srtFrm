using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Common.ViewModel
{
    public class DocumentUploadView
    {
        public long Id { get; set; }
        public string RelatedObjectType { get; set; }
        public long RelatedObjectId { get; set; }
        public string FileName { get; set; }
        public int FileTypeId { get; set; }
        public string Notes { get; set; }
        public byte[] DocumentData { get; set; }
    }
}
