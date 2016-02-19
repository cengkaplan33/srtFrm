using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Surat.Common.Utilities
{
    public class ExceptionUtility
    {
        #region Constructor

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods

        public static string GetEntityValidationErrors(DbEntityValidationException entityValidationException)
        {
            StringBuilder validationErrors = new StringBuilder();

            foreach (var validationError in entityValidationException.EntityValidationErrors)
            {
                validationErrors.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    validationError.Entry.Entity.GetType().FullName, validationError.Entry.State));
                foreach (var internalValidationError in validationError.ValidationErrors)
                {
                    validationErrors.AppendLine(string.Format("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                        internalValidationError.PropertyName,
                        validationError.Entry.CurrentValues.GetValue<object>(internalValidationError.PropertyName),
                        internalValidationError.ErrorMessage));
                }
            }

            return validationErrors.ToString();
        }

        #endregion

    }
}
