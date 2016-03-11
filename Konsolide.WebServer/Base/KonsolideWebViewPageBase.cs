using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Konsolide.WebServer.Base
{
    public class KonsolideWebViewPageBase<T> : WebViewPage<T>
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
