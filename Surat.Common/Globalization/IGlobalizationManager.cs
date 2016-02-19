using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Globalization
{
    public interface IGlobalizationManager
    {
        string GetGlobalizationKeyValue(int systemId,string globalizationKey);
        List<GlobalizationKeyView> GetGlobalizationKeyValueList(int systemId,Culture culture);
        void InsertGlobalizationKeyValues(string globalizationKey,int systemId,List<GlobalizationKeyValueView> keyValues);
    }
}
