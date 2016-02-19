using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Common.ViewModel
{
    public class DocumentStoreView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string RootFilePath { get; set; }
        public int RootDocumentGroupId { get; set; }
        public int SizeInGB { get; set; }
        public int MaximumDocumentCount { get; set; }
        public int MaximumDocumentSizeInMB { get; set; }
    }
}
