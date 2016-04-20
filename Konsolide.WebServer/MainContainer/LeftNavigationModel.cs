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

            Sections = new List<LeftNavigationSection>();
            Sections.Add(new LeftNavigationSection("Home", "Anasayfa", "fa fa-home", "", "0"));

            var systemsSection = new LeftNavigationSection("SistemIslemleri", "Sistem İşlemleri", "fa fa-suitcase", "", "0");
            InitKullaniciIslemleri(systemsSection);
            InitTanimlar(systemsSection);
            InitIzlemeEkranlari(systemsSection);

            var konsolideSection = new LeftNavigationSection("KonsolideIslemleri", "Konsolide İşlemleri", "fa fa-report", "", "0");
            InitKonsolideTanimlar(konsolideSection);
            InitKonsolideIslemler(konsolideSection);
            konsolideSection.Add(new LeftNavigationLink { Title = "Raporlar", Controller = "#", CssClass = "fa fa-report" });


            Sections.Add(systemsSection);
            Sections.Add(konsolideSection);

            SetActivePath(navigationPath);

            //LeftNavigationSection systemsSection;
            //LeftNavigationLink sectionSubLink;
            //string systemTypeName;
            //List<AccessiblePageView> accessiblePagesList = this.WebApplicationManager.KonsolideRapor.Framework.Context.Configuration.UserAccessiblePages;

            //Sections = new List<LeftNavigationSection>();

            //#region MainPage

            //var dashboard = new LeftNavigationSection("Home", "Anasayfa", "fa fa-home", "Spa", "0");
            //Sections.Add(dashboard);

            //#endregion

            ////ToDo : Recursive yapı ile, sistemler hiyerarşik olarak işlenmeli. Şuan bir seviye alt sistemler işlendi.
            ////int rootSystemId = this.WebApplicationManager.KonsolideRapor.Framework.Configuration.System.GetSystemIdByTypeName(KonsolideRaporConstants.Application.PlatformSystemName);
            //int rootSystemId = this.WebApplicationManager.KonsolideRapor.Framework.Configuration.System.GetSystemIdByTypeName(Constants.Application.PlatformSystemName);

            //List<SystemView> systems = GetSubSystems(rootSystemId, accessiblePagesList);

            //systemsSection = new LeftNavigationSection("Sistemler", "Sistemler", "fa fa-suitcase", "", "0");

            //foreach (SystemView system in systems)
            //{
            //    systemTypeName = this.WebApplicationManager.KonsolideRapor.Framework.Configuration.System.GetTypeNameById(system.Id);
            //    List<AccessiblePageView> systemPages = GetSystemPages(system.Id, accessiblePagesList);
            //    sectionSubLink = new LeftNavigationLink { Title = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.KonsolideRapor.Framework.Context.SystemId,systemTypeName), Controller = "#" };

            //    foreach (AccessiblePageView page in systemPages)
            //    {
            //        sectionSubLink.Add(new LeftNavigationLink { Title = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.KonsolideRapor.Framework.Context.SystemId,page.PageName), Controller = Constants.Web.SPAHomePrefix + page.ObjectTypePrefix });
            //    }

            //    systemsSection.Links.Add(sectionSubLink);
            //}

            //Sections.Add(systemsSection);
            //SetActivePath(navigationPath);
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
        {
            var sss = (from systemPages in allAccessiblePages
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

        public List<AccessiblePageView> GetSystemPages(int systemId, List<AccessiblePageView> allAccessiblePages)
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

        public static System.Text.StringBuilder GetChildCategory(LeftNavigationSection Section, LeftNavigationLink ActiveLink, int maxDeep, int currentDeep)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (Section == null)
                return sb;


            if (currentDeep < maxDeep)
            {

                sb = new System.Text.StringBuilder();


                sb.Append("<li>");
                sb.Append("<a href=\"#" + Section.Url + "\" class=\"dropdown-toggle\">");
                sb.Append("<i class=\" " + Section.CssClass + "\" ></i>");
                sb.Append("<span class=\"menu-text\"> " + Section.Title + "</span>");



                if (Section.Links.Count > 0 || Section.Sections.Count > 0)
                {
                    sb.Append("<b class=\"arrow fa fa-angle-down\"></b> </a>");
                    sb.Append("<ul class=\"submenu\">");
                }
                else
                    sb.Append("</a>");

                foreach (var link in Section.Links)
                {
                    if (link == ActiveLink)
                    {
                        link.CssClass = link.CssClass == null ? " active " : link.CssClass += " active ";
                        link.CssClass = link.Links.Count > 0 ? link.CssClass += " open " : link.CssClass;
                    }

                    var url = link.Url == null ? ("~/" + link.Controller) : link.Url;
                    if (url.StartsWith("~/"))
                        url = System.Web.VirtualPathUtility.ToAbsolute(url);
                    var target = link.Target;
                    if (target != null)
                        target += " target=" + target;

                    var toogleClass = "";
                    if (link.Links.Count > 0)
                    {
                        toogleClass = "class=dropdown-toggle";
                    }

                    sb.Append("<li class=\"classthreeLevelText\">");
                    sb.Append("<a href=\"#" + url + "\" " + target + " >");
                    //sb.Append("<i class=\"fa fa-double-angle-right\">");
                    sb.Append("<i class=\" menu-icon fa " + link.CssClass + "\" ></i>" + link.Title);
                    sb.Append("</a>");
                    sb.Append("</li>");
                    //sb.Append("<li class='m-Link " + link.CssClass + "' > <a class='m-LinkText' href='" + url + "' " + target + ">" + link.Title + " </a> </li>");
                }

                if (Section.Sections != null)
                    foreach (var cSec in Section.Sections)
                        sb.Append(GetChildCategory(cSec, ActiveLink, maxDeep, currentDeep++));

                if (Section.Links.Count > 0 || Section.Sections.Count > 0)
                    sb.Append("</ul>");

                sb.Append("</li>");
                return sb;
            }
            else return sb;
        }

        private void InitKullaniciIslemleri(LeftNavigationSection ParentSections)
        {
            var kullaniciIslemleri = new LeftNavigationSection("KullaniciIslemleri", "Kullanıcı İşlemleri", " fa fa-anchor ");

            if (this.WebApplicationManager.Framework.Security.HasRight("Users/Index"))
                kullaniciIslemleri.Links.Add(new LeftNavigationLink { Title = "Kullanıcılar", Controller = "Users/Index", CssClass = "UsersIndex fa fa-user" });

            if (this.WebApplicationManager.Framework.Security.HasRight("Roles/Index"))
                kullaniciIslemleri.Links.Add(new LeftNavigationLink() { Title = "Roller", Controller = "Roles/Index", CssClass = "RolesIndex  fa fa-users" });

            if (kullaniciIslemleri.Links.Count > 0)
                ParentSections.Sections.Add(kullaniciIslemleri);
        }

        private void InitTanimlar(LeftNavigationSection ParentSections)
        {

            var tanimlar = new LeftNavigationSection("Tanimlar", "Tanımlar", " fa fa-cogs ");

            if (this.WebApplicationManager.Framework.Security.HasRight("Workgroups/Index"))
                tanimlar.Links.Add(new LeftNavigationLink { Title = "Çalışma Grupları", Controller = "Workgroups/Index", CssClass = "WorkgroupsIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("Parameters/Index"))
                tanimlar.Links.Add(new LeftNavigationLink { Title = "Parametreler", Controller = "Parameters/Index", CssClass = "ParametersIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("Workgroups/Index"))
                tanimlar.Links.Add(new LeftNavigationLink { Title = "Dil Tanımları", Controller = "Workgroups/Index", CssClass = "WorkgroupsIndex" });

            if (tanimlar.Links.Count > 0)
                ParentSections.Sections.Add(tanimlar);

        }

        private void InitIzlemeEkranlari(LeftNavigationSection ParentSections)
        {
            var izlemeEkranlari = new LeftNavigationSection("IzlemeEkranlari", "İzleme Ekranları", " fa fa-tachometer ");

            if (this.WebApplicationManager.Framework.Security.HasRight("UserSessions/Index"))
                izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "Kullanıcı Oturumları", Controller = "UserSessions/Index", CssClass = "UserSessionsIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("Exceptions/Index"))
                izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "Hatalar", Controller = "Exceptions/Index", CssClass = "ExceptionsIndex" });

            //if (this.WebApplicationManager.Framework.Security.HasRight("Workgroups/Index"))
            //izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "Tanımlı Dil Değerleri", Controller = "Workgroups/Index", CssClass = "WorkgroupsIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("Pages/Index"))
                izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "Sayfalar", Controller = "Pages/Index", CssClass = "PagesIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("Actions/Index"))
                izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "Aksiyonlar", Controller = "Actions/Index", CssClass = "ActionsIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("Systems/Index"))
                izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "Sistemler", Controller = "Systems/Index", CssClass = "SystemsIndex" });

            //if (this.WebApplicationManager.Framework.Security.HasRight("Workgroups/Index"))
            //izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "Dış Sistem Kullanıcıları", Controller = "Systems/Index", CssClass = "SystemsIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("RelationGroups/Index"))
                izlemeEkranlari.Links.Add(new LeftNavigationLink { Title = "İlişki Grupları", Controller = "RelationGroups/Index", CssClass = "RelationGroupsIndex" });

            if (izlemeEkranlari.Links.Count > 0)
                ParentSections.Sections.Add(izlemeEkranlari);
        }


        private void InitKonsolideTanimlar(LeftNavigationSection ParentSections)
        {
            var tanimlar = new LeftNavigationSection("Tanimlar", "Tanımlar", " fa fa-cogs ");

            if (this.WebApplicationManager.Framework.Security.HasRight("Bankalar/Index"))
                tanimlar.Links.Add(new LeftNavigationLink { Title = "Banka Tanımları", Controller = "Bankalar/Index", CssClass = "BankalarIndex fa fa-bank" });

            if (this.WebApplicationManager.Framework.Security.HasRight("PaymentCollecting/Index"))
                tanimlar.Links.Add(new LeftNavigationLink() { Title = "Ödeme Tanımları", Controller = "PaymentCollecting/Index", CssClass = "PaymentCollectingIndex  " });

            if (this.WebApplicationManager.Framework.Security.HasRight("DurumTanimlari/Index"))
                tanimlar.Links.Add(new LeftNavigationLink() { Title = "Durum Tanımları", Controller = "DurumTanimlari/Index", CssClass = "DurumTanimlariIndex  " });


            if (tanimlar.Links.Count > 0)
                ParentSections.Sections.Add(tanimlar);
        }

        private void InitKonsolideIslemler(LeftNavigationSection ParentSections)
        {

            var islemler = new LeftNavigationSection("Islemler", "İşlemler", "  ");

            if (this.WebApplicationManager.Framework.Security.HasRight("HazirDegerlerTablosu/Index"))
                islemler.Links.Add(new LeftNavigationLink { Title = "Banka Tablosu", Controller = "HazirDegerlerTablosu/Index", CssClass = "HazirDegerlerTablosuIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("OdemeTalep/Index"))
                islemler.Links.Add(new LeftNavigationLink { Title = "Ödeme Ekranı", Controller = "OdemeTalep/Index", CssClass = "OdemeTalepIndex" });

            if (this.WebApplicationManager.Framework.Security.HasRight("TahsilatTalep/Index"))
                islemler.Links.Add(new LeftNavigationLink { Title = "Tahsilat Ekranı", Controller = "TahsilatTalep/Index", CssClass = "TahsilatTalepIndex" });

            if (islemler.Links.Count > 0)
                ParentSections.Sections.Add(islemler);
        }

        #endregion
    }
}

