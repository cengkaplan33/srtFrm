using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.Utilities
{
    public class TypeConversionUtility
    {

        #region Methods

        public static T To<T> (string parameterValue)
        {
            return (T)Convert.ChangeType(parameterValue, typeof(T));
        }

        public static bool? ConvertStringValueToNullableBoolean(string stringValue)
        {
            bool? result = null;

            switch (stringValue)
            {
                case "0": result = false;
                    break;
                case "1": result = true;
                    break;
                default:
                    break;
            }

            return result;
        }

        #endregion

    }
}
