using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.WebServer
{
    public class LeftNavigationLink
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Controller { get; set; }
        public string CssClass { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public List<LeftNavigationLink> Links { get; private set; }

        public void Add(LeftNavigationLink link)
        {
            Links.Add(link);
        }

        public LeftNavigationLink()
        {
            Links = new List<LeftNavigationLink>();
            Url = null;
        }
    }
}
