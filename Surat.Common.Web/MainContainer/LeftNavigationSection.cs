using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.WebServer
{
    public class LeftNavigationSection
    {
        public string Title { get; private set; }
        public string Key { get; private set; }
        public string Url { get; private set; }
        public string CssClass { get; private set; }
        public string SectionType { get; private set; }
        public List<LeftNavigationSection> Sections { get; set; }
        public List<LeftNavigationLink> Links { get; private set; }

        public void Add(LeftNavigationLink link)
        {
            Links.Add(link);
        }

        public LeftNavigationSection(string key, string title)
        {
            Links = new List<LeftNavigationLink>();
            Key = key;
            Title = title;
            Sections = new List<LeftNavigationSection>();
        }

        public LeftNavigationSection(string key, string title, string cssClass)
        {
            Links = new List<LeftNavigationLink>();
            Key = key;
            Title = title;
            CssClass = cssClass;
            Sections = new List<LeftNavigationSection>();
        }

        public LeftNavigationSection(string key, string title, string cssClass, string url, string sectionType)
        {
            Links = new List<LeftNavigationLink>();
            Key = key;
            Title = title;
            CssClass = cssClass;
            Url = url;
            SectionType = sectionType;
            Sections = new List<LeftNavigationSection>();
        }
    }
}
