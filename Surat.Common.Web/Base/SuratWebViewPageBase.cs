using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Surat.WebServer.Base
{
    public class SuratWebViewPageBase<T> : WebViewPage<T>
    {   
        //public SomeType MyProperty { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            //MyProperty = new SomeType();
        }

        public override void Execute()
        {
        }
    }
}
