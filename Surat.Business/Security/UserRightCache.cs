using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surat.Business.Security
{
    public class UserRightCache
    {
        public const int MaxItems = 100000;
        public const int MakeSpaceBy = 100;

        //private static SqlSelect _userInRolesQuery;
        //private static Parameter _userInRolesParam;
        //private static SqlSelect _userRightsQuery;
        //private static Parameter _userRightsParam;
        private static object _sync;

        private static HashSet<int> _emptyIntSet;

        private static LeastRecentlyUsedCache<UserItem> _userItemCache;
        private static Dictionary<Int32, UserItem> _userItemById;

        private static LeastRecentlyUsedCache<RoleItem> _roleItemCache;
        private static Dictionary<Int32, RoleItem> _roleItemById;

        static UserRightCache()
        {
            _sync = new Object();

            // USER IN ROLES
            //var userInRoleFld = UserInRoleRow.Fields;
            //_userInRolesQuery = new SqlSelect(userInRoleFld.RoleId).From(userInRoleFld.TableName);
            //_userInRolesParam = _userInRolesQuery.AutoParam();
            //_userInRolesQuery.Where(new Filter(userInRoleFld.UserId) == _userInRolesParam);

            // USER RIGHTS
            //var userRightsFld = UserRightRow.Fields;
            //_userRightsQuery = new SqlSelect(userRightsFld.AccessRightId, userRightsFld.IsRevoke).From(userRightsFld.TableName);
            //_userRightsParam = _userRightsQuery.AutoParam();
            //_userRightsQuery.Where(new Filter(userRightsFld.UserId) == _userRightsParam);

            _userItemCache = new LeastRecentlyUsedCache<UserItem>(MaxItems, MakeSpaceBy, RemoveUserItemCallback);
            _userItemById = new Dictionary<int, UserItem>();

            _roleItemCache = new LeastRecentlyUsedCache<RoleItem>(MaxItems, MakeSpaceBy, RemoveRoleItemCallback);
            _roleItemById = new Dictionary<int, RoleItem>();

            _emptyIntSet = new HashSet<int>();
        }

        private static UserItem EnsureUserItem(int userId)
        {
            UserItem userItem;
            lock (_sync)
            {
                if (_userItemById.TryGetValue(userId, out userItem))
                {
                    _userItemCache.Use(userItem);
                    return userItem;
                }
            }

            userItem = new UserItem();
            userItem.UserId = userId;

            var roles = new HashSet<int>();

            using (var dbcontext = new Surat.Base.Model.FrameworkDbContext())
            {
                //distinct bile gerekmeyebilir.
                roles = new HashSet<int>(dbcontext.RelationGroups.Where(x => x.IsActive == true & x.WorkgroupId == 0 & (x.RoleId.Value != 0) & x.UserId == userId).Select(x => x.Id).Distinct().ToList());
                //relationGroupForUser = dbcontext.RelationGroups.Where(x => x.IsActive == true & x.WorkgroupId == 0 & x.RoleId.Value == 0 & x.UserId == userId).Select(x => x.Id).FirstOrDefault();
            }
            userItem.Roles = roles;

            HashSet<int> grants = null;
            HashSet<int> revokes = null;

            using (var dbcontext = new Surat.Base.Model.FrameworkDbContext())
            {
                //var accessibleItems = dbcontext.AccessibleItems.Where(x => x.IsActive == true & x.DBObjectType == (int)Surat.Common.Data.AccessibleItemDBObjectType.Right &  x.RelationGroupId == relationGroupForUser).ToList();
                var accessibleItems = dbcontext.AccessibleItems.Join(dbcontext.RelationGroups,
                    a => a.RelationGroupId, r => r.Id, (a, r) => new { a, r })
                    .Where(x => x.r.IsActive == true & x.r.WorkgroupId == 0 & x.r.RoleId.Value == 0 & x.r.UserId == userId &
                        x.a.IsActive == true & x.a.DBObjectType == (int)Surat.Common.Data.AccessibleItemDBObjectType.Right)
                    .Select(x => x.a).ToList();

                grants = new HashSet<int>(accessibleItems.Where(x => x.AccessRightTypeId == 1).Select(x => (int)x.DBObjectId).ToList());
                revokes = new HashSet<int>(accessibleItems.Where(x => x.AccessRightTypeId == 0).Select(x => (int)x.DBObjectId).ToList());
            }
            userItem.GrantedRights = grants;
            userItem.RevokedRights = revokes;
 

            lock (_sync)
            {
                UserItem old;
                if (_userItemById.TryGetValue(userId, out old))
                    _userItemCache.Remove(old);

                _userItemCache.Use(userItem);
                _userItemById[userId] = userItem;
            }

            return userItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"> RelationGroupId ye karşılık geliyor. role için ilgili RelationGroup tablosunda UserId == 0 ve WorkGroupId == 0 şartına uyanlar role kaydı anlamına geliyor.</param>
        /// <returns></returns>
        private static List<RoleItem> EnsureRoleItems(HashSet<int> roles)
        {
            List<int> missing = null;
            List<RoleItem> items = new List<RoleItem>(roles.Count);

            lock (_sync)
            {
                RoleItem roleItem;

                foreach (var roleId in roles)
                    if (_roleItemById.TryGetValue(roleId, out roleItem))
                    {
                        _roleItemCache.Use(roleItem);
                        items.Add(roleItem);
                    }
                    else
                    {
                        if (missing == null)
                            missing = new List<int>();
                        missing.Add(roleId);
                    }
            }

            if (missing != null)
            {
                Dictionary<int, HashSet<int>> roleRights = new Dictionary<int, HashSet<int>>();

                using (var dbcontext = new Surat.Base.Model.FrameworkDbContext())
                {
                    HashSet<int> rights = null;
                    var accessibleItems = dbcontext.AccessibleItems.Where(x => x.IsActive == true & missing.Contains(x.RelationGroupId)).ToList();
                    accessibleItems.Where(x => x.AccessRightTypeId == 1).ToList().
                        ForEach(x =>
                        {
                            if (!roleRights.TryGetValue(x.RelationGroupId, out rights))
                            {
                                rights = new HashSet<int>();
                                roleRights[x.RelationGroupId] = rights;
                            }

                            rights.Add(x.Id);
                        });
                }

                lock (_sync)
                {
                    HashSet<int> rights = null;
                    foreach (var roleId in missing)
                    {
                        RoleItem roleItem = new RoleItem();
                        roleItem.RoleId = roleId;
                        rights = null;
                        if (roleRights.TryGetValue(roleId, out rights))
                            roleItem.GrantedRights = rights;

                        RoleItem old;
                        if (_roleItemById.TryGetValue(roleId, out old))
                            _roleItemCache.Remove(old);

                        _roleItemCache.Use(roleItem);
                        _roleItemById[roleId] = roleItem;
                        items.Add(roleItem);
                    }
                }
            }

            return items;
        }

        public static bool IsUserHasRight(int userId, Surat.Business.Security.SuratRight accessRight)
        {
            if (accessRight == null)
                throw new ArgumentNullException("accessRight");

            int accessRightId = accessRight.Id;

            var userItem = EnsureUserItem(userId);

            if (userItem.RevokedRights != null &&
                userItem.RevokedRights.Contains(accessRightId))
                return false;

            if (userItem.GrantedRights != null &&
                userItem.GrantedRights.Contains(accessRightId))
                return true;

            if (userItem.Roles != null)
            {
                var roleItems = EnsureRoleItems(userItem.Roles);
                foreach (var roleItem in roleItems)
                    if (roleItem.GrantedRights != null &&
                        roleItem.GrantedRights.Contains(accessRightId))
                        return true;
            }

            return false;
        }

        public static HashSet<int> GetUserRights(int userId)
        {
            var userItem = EnsureUserItem(userId);
            var result = new HashSet<int>();
            var revoked = userItem.RevokedRights;

            if (userItem.Roles != null)
            {
                var roleItems = EnsureRoleItems(userItem.Roles);
                foreach (var roleItem in roleItems)
                    if (roleItem.GrantedRights != null)
                        foreach (var r in roleItem.GrantedRights)
                            result.Add(r);
            }

            var granted = userItem.GrantedRights;
            if (granted != null)
                foreach (var r in granted)
                    result.Add(r);


            if (revoked != null)
                foreach (var r in revoked)
                    result.Remove(r);

            return result;
        }

        public static HashSet<int> GetUserRoles(int userId)
        {
            var userItem = EnsureUserItem(userId);
            return userItem.Roles ?? _emptyIntSet;
        }

        public static void RemoveByUserId(int userId)
        {
            UserItem item;
            lock (_sync)
            {
                if (_userItemById.TryGetValue(userId, out item))
                    _userItemCache.Remove(item);
            }
        }

        public static void RemoveByRoleId(int roleId)
        {
            RoleItem item;
            lock (_sync)
            {
                if (_roleItemById.TryGetValue(roleId, out item))
                    _roleItemCache.Remove(item);
            }
        }

        public static void RemoveAllUsers()
        {
            lock (_sync)
            {
                _userItemCache.Clear(false);
                _userItemById.Clear();
            }
        }

        public static void RemoveAllRoles()
        {
            lock (_sync)
            {
                _roleItemCache.Clear(false);
                _roleItemById.Clear();
            }
        }

        private static void RemoveUserItemCallback(UserItem item)
        {
            _userItemById.Remove(item.UserId);
        }

        private static void RemoveRoleItemCallback(RoleItem item)
        {
            _roleItemById.Remove(item.RoleId);
        }

        private class UserItem : LeastRecentlyUsedCacheNode
        {
            public int UserId;
            public HashSet<int> Roles;
            public HashSet<int> GrantedRights;
            public HashSet<int> RevokedRights;
        }

        private class RoleItem : LeastRecentlyUsedCacheNode
        {
            public int RoleId;
            public HashSet<int> GrantedRights;
        }
    }
     
}