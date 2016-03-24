using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surat.Common.ViewModel
{
    public class ExceptionView
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string SystemName { get; set; }
        public byte ExceptionLevel { get; set; }
        public string ExceptionType { get; set; }
        public string Data { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationBaseType { get; set; }
        public string HostName { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string InsertUserName { get; set; }
        public string AllXml { get; set; }
    }
}