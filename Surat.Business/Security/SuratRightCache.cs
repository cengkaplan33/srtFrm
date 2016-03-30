using System;
using System.Collections.Generic;
using System.Linq;

namespace Surat.Business.Security
{

    using MyEnumReg = System.Collections.Generic.Dictionary<string, DataEnumKeyValue>;

    public interface IDataEnumService
    {
        //SuratRight ById(int id);
        //SuratRight ByName(string name);
        List<Surat.Base.Model.Entities.SuratRight> List();
        long? Create(SuratRight row);
        //DataEnumMaxPoint GetMaxPoint(string EnumType);
    }


    public class DataEnumKeyValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RelatedName { get; set; }
        public int SystemId { get; set; }
    }

    public class DataEnumEntity
    {
        public string EnumType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RelatedName { get; set; }
        public int DisplayOrder { get; set; }
        public int SystemId { get; set; }
    }

    public class SuratRightCache
    {
        private static MyEnumReg _registered;
        private static CacheItem _items;
        private static CacheItem _empty;

        static SuratRightCache()
        {
            _registered = new Dictionary<string, DataEnumKeyValue>(StringComparer.OrdinalIgnoreCase);
            _empty = new CacheItem();
        }

        public static CacheItem Items
        {
            get { EnsureItems(); return _items; }
        }

        //public static CacheItem EnumType(string enumType)
        //{
        //    EnsureItems();
        //    CacheItem item;
        //    if (!_items.TryGetValue(enumType, out item))
        //        return _empty;
        //    return item;
        //}

        public static void Reset()
        {
            _items = null;
        }

        public static bool IsRegistered(string name)
        {
            DataEnumKeyValue item;
            if (!_registered.TryGetValue(name, out item))
                return false;

            return true;
        }

        public static void RegisterKey(string name, string description, int systemId)
        {
            DataEnumKeyValue item;
            if (!_registered.TryGetValue(name, out item))
            {
                _registered[name] = new DataEnumKeyValue()
                {
                    Name = name,
                    Description = description,
                    SystemId = systemId
                };
            }
        }

        private static void EnsureItems()
        {
            if (_items != null)
                return;

            var temp = new CacheItem();
            List<DataEnumKeyValue> list = null;
            var row = new Surat.Base.Model.Entities.SuratRight();
            DataEnumItem item = null;

            //var rowList = IoC.Resolve<IDataEnumService>().List();
            var rowList = new Surat.Base.Model.FrameworkDbContext().SuratRights.ToList();

            foreach (var rowDe in rowList)
            {
                if (!temp._byName.TryGetValue(rowDe.Name, out item))
                {
                    var de = new DataEnumItem(rowDe.Id, rowDe.Name, rowDe.Description, rowDe.SystemId);
                    temp._byId[rowDe.Id] = de;
                    temp._byName[rowDe.Name] = de;
                    temp._inDisplayOrder.Add(de);
                }
            }



            foreach (var reg in _registered)
            {
                if (!temp._byName.ContainsKey(reg.Key))
                {
                    list = list ?? new List<DataEnumKeyValue>();
                    list.Add(reg.Value);
                }
            }

            if (list != null && list.Count > 0)
            {
                foreach (var k in list)
                {
                    var entity = new Surat.Base.Model.Entities.SuratRight()
                    {
                        Name = k.Name,
                        Description = k.Description,
                        SystemId = k.SystemId,
                        InsertedDate= Surat.Common.Utilities.TimeUtility.GetCurrentDateTime(),
                        InsertedByUser = 1,
                        IsActive = true
                    };

                    using (var dbcontext = new Surat.Base.Model.FrameworkDbContext())
                    {
                        dbcontext.SuratRights.Add(entity);
                        dbcontext.SaveChanges();
                    }

                    var de = new DataEnumItem(entity.Id, entity.Name, entity.Description, entity.SystemId);
                    temp._byId[entity.Id] = de;
                    temp._byName[k.Name] = de;
                    temp._inDisplayOrder.Add(de);
                }
            }

            _items = temp;
        }

        public class CacheItem
        {
            internal CacheItem()
            {
                _byName = new Dictionary<string, DataEnumItem>(StringComparer.OrdinalIgnoreCase);
                _byId = new Dictionary<int, DataEnumItem>();
                _inDisplayOrder = new List<DataEnumItem>();
            }

            internal Dictionary<string, DataEnumItem> _byName;
            internal Dictionary<int, DataEnumItem> _byId;
            internal List<DataEnumItem> _inDisplayOrder;

            public ICollection<DataEnumItem> InDisplayOrder
            {
                get { return _inDisplayOrder; }
            }

            public Int32? this[string name]
            {
                get
                {
                    DataEnumItem item;
                    if (_byName.TryGetValue(name, out item))
                        return item.Id;
                    return null;
                }
            }

            public string this[int id]
            {
                get
                {
                    DataEnumItem item;
                    if (_byId.TryGetValue(id, out item))
                        return item.Name;
                    return null;
                }
            }
        }

        public class DataEnumItem
        {
            public DataEnumItem()
            {

            }

            public DataEnumItem(int id, string name, string description, int systemId)
            {
                Id = id;
                Name = name;
                Description = description;
                SystemId = systemId;
            }

            public DataEnumItem(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; private set; }
            public string Name { get; private set; }
            public string Description { get; private set; }
            public int SystemId { get; private set; }
        }
    }
}