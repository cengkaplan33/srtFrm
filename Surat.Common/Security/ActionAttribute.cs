using Surat.Common.Data;
using System;

namespace Surat.Common.Security
{
    public class ActionAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string SystemName { get; private set; }
        public ActionType Type { get; private set; }

        public ActionAttribute(String Name, String Description, String SystemName,ActionType Type)
        {
            this.Name = Name;
            this.Description = Description;
            this.SystemName = SystemName;
            this.Type = Type;
        }
    }
}