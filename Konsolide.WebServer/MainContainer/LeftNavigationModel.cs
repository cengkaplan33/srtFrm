using Surat.Common.Data;
using Surat.Common.ViewModel;
using KonsolideRapor.WebServer.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KonsolideRapor.Common.Data;
namespace KonsolideRapor.WebServer
{
    public partial class LeftNavigationModel
    {

        #region Constructor

        public LeftNavigationModel(string navigationPath)
        {
            
            LeftNavigationSection systemsSection;
            LeftNavigationLink sectionSubLink;
            string systemTypeName;
            List<AccessiblePageView> accessiblePagesList = this.WebApplicationManager.Framework.Context.Configuration.UserAccessiblePages;

            Sections = new List<LeftNavigationSection>();

            #region MainPage

            var dashboard = new LeftNavigationSection("Home", "Anasayfa", "fa fa-home", "Home", "0");
            Sections.Add(dashboard);

            #endregion

            //ToDo : Recursive yapı ile, sistemler hiyerarşik olarak işlenmeli. Şuan bir seviye alt sistemler işlendi.
            int rootSystemId = this.WebApplicationManager.Framework.Configuration.System.GetSystemIdByTypeName(KonsolideConstants.Application.PlatformSystemName);

            List<SystemView> systems = GetSubSystems(rootSystemId, accessiblePagesList);

            systemsSection = new LeftNavigationSection("Sistemler", "Sistemler", "fa fa-suitcase", "", "0");

            foreach (SystemView system in systems)
            {
                systemTypeName = this.WebApplicationManager.Framework.Configuration.System.GetTypeNameById(system.Id);
                List<AccessiblePageView> systemPages = GetSystemPages(system.Id, accessiblePagesList);
                sectionSubLink = new LeftNavigationLink { Title = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,systemTypeName), Controller = "#" };

                foreach (AccessiblePageView page in systemPages)
                {
                    sectionSubLink.Add(new LeftNavigationLink { Title = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,page.PageName), Controller = Constants.Web.SPAHomePrefix + page.ObjectTypePrefix });
                }

                systemsSection.Links.Add(sectionSubLink);
            }

            Sections.Add(systemsSection);
            SetActivePath(navigationPath);
        }

        #endregion

        #region Private Members

        private WebApplicationManager webApplicationManager;
 
        #endregion

        #region Public Members        

        public WebApplicationManager WebApplicationManager
        {
            get 
            {
                if (webApplicationManager == null)
                    webApplicationManager = new WebApplicationManager();

                return webApplicationManager; 
            }
            set { webApplicationManager = value; }
        }
        
        public LeftNavigationSection ActiveSection { get; set; }
        public LeftNavigationLink ActiveLink { get; set; }
        public LeftNavigationLink ActiveThreeLink { get; set; }
        public List<LeftNavigationSection> Sections { get; set; }

        #endregion

        #region Methods

        public List<SystemView> GetSubSystems(int parentSystemId, List<AccessiblePageView> allAccessiblePages)
        {var sss=  (from systemPages in allAccessiblePages
                                           where systemPages.SystemParentId == parentSystemId
                                           select new SystemView
                                           {
                                               Id = systemPages.SystemId,
                                               Name = systemPages.SystemName                   
                                           });
        var ddd = sss.ToString();
            List<SystemView> systemList = sss.GroupBy(g => new { g.Id }).Select(s => s.FirstOrDefault()).ToList();

            return systemList;
        }

        public List<AccessiblePageView> GetSystemPages(int systemId,List<AccessiblePageView> allAccessiblePages)
        {
            return allAccessiblePages.Where(p => p.SystemId == systemId).ToList();
        }

        public void SetActivePath(string navigationPath)
        {
            var path = (navigationPath ?? String.Empty).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (path.Length > 0)
            {
                var section = path[0];
                foreach (var sec in Sections)
                    if (sec.Key == section)
                    {
                        ActiveSection = sec;

                        if (path.Length > 1)
                        {
                            var link = "";
                            if (path.Length == 3)
                            {
                                link = String.Join("/", path.Skip(1).Take(1).ToArray());
                            }
                            else
                            {
                                link = String.Join("/", path.Skip(1).ToArray());
                            }

                            foreach (var lnk in ActiveSection.Links)
                                if (lnk.Controller == link)
                                {
                                    ActiveLink = lnk;

                                    if (path.Length > 2)
                                    {
                                        string threelink = string.Join("/", path.Skip(2).ToArray());

                                        foreach (var thlink in ActiveLink.Links)
                                        {
                                            var threedemo = thlink.Controller;
                                            if (threedemo == threelink)
                                            {
                                                ActiveThreeLink = thlink;
                                            }
                                        }
                                    }

                                    break;
                                }
                        }

                        break;
                    }
            }
        }

        #endregion        
    } 
}

