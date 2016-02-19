using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.Utilities
{
    public class TimeUtility
    {
        #region Constructor

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public static DateTime GetMininumDateTimeValue()
        {
            return new DateTime(1753, 1, 1, 12, 0, 0);
        }

        public static DateTime GetMaximumDateTimeValue()
        {
            return new DateTime(9999, 12, 31, 23, 59, 59);
        }

        #endregion

    }
}
