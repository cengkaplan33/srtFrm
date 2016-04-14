using Surat.Common.Data;

namespace Surat.Common.ViewModel
{
    public class SuratActionView
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public int SystemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SystemName { get; set; }
        public ActionType Type { get; set; }
    }
}
