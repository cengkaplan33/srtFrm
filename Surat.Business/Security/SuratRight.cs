using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Base.Providers;
using Surat.Base.Repositories;
using Surat.Base.Security;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.Log;
using Surat.Common.Security;
using Surat.Common.Utilities;
using Surat.Common.ViewModel;
using Surat.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Surat.Business.Security
{
    [Serializable]
    public abstract class AbsSuratRight : IEquatable<AbsSuratRight>
    {
        internal Int32 _valueId;
        internal string _valueKey;
        internal string _description;
        internal Int32 _systemId;

        protected AbsSuratRight(string valueKey, string description, int? systemId)
        {
            if (valueKey.IsEmptyOrNull())
                throw new ArgumentNullException("valueKey");

            if (description.IsEmptyOrNull())
                throw new ArgumentNullException("description");

            if (systemId == null)
                throw new ArgumentNullException("systemId");

            _valueKey = valueKey;
            _valueId = -1;
            _description = description;
            _systemId = systemId.Value;
        }

        protected AbsSuratRight(string valueKey)
        {
            if (valueKey == null)
                throw new ArgumentNullException("valueKey");

            _valueKey = valueKey;
            _valueId = -1;
        }

        protected AbsSuratRight(Int32 valueId)
        {
            _valueId = valueId;
            _valueKey = null;
        }

        public void EnsureValid()
        {
            if (!IsValid)
            {
                if (_valueKey != null)
                    throw InvalidKeyException();
                else
                    throw InvalidIdException();
            }
        }

        public bool IsValid
        {
            get
            {
                return
                    (_valueKey != null || _valueId != -1);
            }
        }

        public static AbsSuratRight CreateSuratRight(Type enumType, Int32 value)
        {
            return (AbsSuratRight)Activator.CreateInstance(enumType, new object[] { value });
        }

        public static AbsSuratRight ConvertFromInt32(Type objectType, Int32 value)
        {
            AbsSuratRight result = CreateSuratRight(objectType, value);
            if (!result.IsValid)
                throw result.InvalidIdException();

            return result;
        }

        private Exception InvalidKeyException()
        {
            return new InvalidOperationException(String.Format(
                "SuratRight tipi için {1} kodlu değer bulunamadı!", _valueKey));
        }

        private Exception InvalidIdException()
        {
            throw new InvalidOperationException(String.Format(
                "SuratRight tipi için {0} numaralı değer bulunamadı!", _valueId));
        }

        public static AbsSuratRight ConvertFromString(Type objectType, String value)
        {
            value = value.TrimToNull();
            if (value == null)
                return null;

            Int32 v;
            if (Int32.TryParse(value, out v))
                return ConvertFromInt32(objectType, v);

            SuratRight result = (SuratRight)Activator.CreateInstance(objectType, new object[] { value });

            if (!result.IsValid)
                throw result.InvalidKeyException();

            return result;
        }

        public string Key
        {
            get
            {
                if (_valueKey != null)
                    return _valueKey;

                var key = SuratRightCache.Items[_valueId];
                if (key == null)
                    throw InvalidIdException();

                return key;
            }
        }

        public Int32 Id
        {
            get
            {
                if (_valueKey == null)
                    return _valueId;

                var valueId = SuratRightCache.Items[_valueKey];
                if (valueId == null)
                    throw InvalidKeyException();
                return valueId.Value;
            }
        }

        public static bool operator !=(AbsSuratRight l, AbsSuratRight r)
        {
            if (Object.ReferenceEquals(l, null))
                return !Object.ReferenceEquals(r, null);
            else if (Object.ReferenceEquals(r, null))
                return true;

            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            if (_valueKey == null)
                return _valueId.GetHashCode();
            else
                return _valueKey.GetHashCode();
        }

        public override string ToString()
        {
            if (IsValid)
                return Key;
            else
                return _valueKey ?? _valueId.ToInvariant();
        }

        public static bool operator ==(AbsSuratRight l, AbsSuratRight r)
        {
            if (Object.ReferenceEquals(l, null))
                return Object.ReferenceEquals(r, null);
            else if (Object.ReferenceEquals(r, null))
                return false;

            return l.Equals(r);
        }

        public override bool Equals(object obj)
        {
            return (obj is AbsSuratRight) && Equals((AbsSuratRight)obj);
        }

        public bool Equals(AbsSuratRight other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;

            if (this._valueKey != null &&
                other._valueKey != null)
                return this._valueKey == other._valueKey;

            if (this._valueKey == null &&
                other._valueKey == null)
                return this._valueId == other._valueId;

            if (this.IsValid &&
                other.IsValid)
                return this.Id == other.Id;

            return false;
        }
    }

    public static class SuratRightExtensions
    {
        public static T Validate<T>(this T value) where T : AbsSuratRight
        {
            value.EnsureValid();
            return value;
        }

        public static Int32? ToInt32(this AbsSuratRight value)
        {
            if (value == null)
                return null;
            else
                return value.Id;
        }

        public static T Register<T>(this T value) where T : AbsSuratRight
        {
            if (value._valueKey == null)
                throw new ArgumentOutOfRangeException("SuratRight.Register sadece key ile belirli enum lar için çağrılmalıdır!");

            SuratRightCache.RegisterKey(value._valueKey, value._description, value._systemId);
            return value;
        }
    }


     
    public class SuratRight : AbsSuratRight
    {

        public SuratRight(string valueKey) : base(valueKey) { }
        public SuratRight(int valueId) : base(valueId) { }
        public SuratRight(string valueKey, string description, int systemId) : base(valueKey, description, systemId) { }

        public static SuratRight FromInt32(Int32? value)
        {
            if (value == null)
                return null;
            else
                return new SuratRight(value.Value);
        }
    }

    //public static partial class SuratRights
    //{
    //    public static readonly SuratRight Auditor = new SuratRight("Auditor", "Auditor hakkının açıklamasını buraya girelim inş.  :)  ", 2).Register();
    //    public static readonly SuratRight AuditManagement = new SuratRight("AuditManagement", "AuditManagement hakkının açıklamasını buraya girelim inş.  :)  ", 2).Register();
    //}


   //public static bool IsUserHasRight(int userId, SuratRight accessRight)
   //     {
   //         if (accessRight == null)
   //             throw   Exception("accessRight");

   //         int accessRightId = accessRight.Id;

   //         var userItem = EnsureUserItem(userId);

   //         if (userItem.RevokedRights != null &&
   //             userItem.RevokedRights.Contains(accessRightId))
   //             return false;

   //         if (userItem.GrantedRights != null &&
   //             userItem.GrantedRights.Contains(accessRightId))
   //             return true;

   //         if (userItem.Roles != null)
   //         {
   //             var roleItems = EnsureRoleItems(userItem.Roles);
   //             foreach (var roleItem in roleItems)
   //                 if (roleItem.GrantedRights != null &&
   //                     roleItem.GrantedRights.Contains(accessRightId))
   //                     return true;
   //         }

   //         return false;
   //     }
   // // public static bool HasRight(AccessRight right)
   // //{
   // //        return IoC.Resolve<IUserRightChecker>().HasRight(right);
   // //}
}
