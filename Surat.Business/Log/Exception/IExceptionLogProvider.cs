using Surat.Base.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Business.Log
{
    public interface IExceptionLogProvider
    {
        void WriteExceptionLog(ExceptionLog exceptionLogItem);
    }
}
