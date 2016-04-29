//using Surat.Common.Data;
//using Surat.Common.ViewModel;
//using Surat.WebServer.Application;
//using Surat.WebServer.Base;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Surat.WebServer.Serendip
//{
//    public class SerendipService
//    {
//        private WebApplicationManager webApplicationManager;
//        private SuratControllerBase suratControllerBase;
        
//        public WebApplicationManager WebApplicationManager
//        {
//            get
//            {
//                if (webApplicationManager == null)
//                    webApplicationManager = new WebApplicationManager();

//                return webApplicationManager;
//            }
//            set { webApplicationManager = value; }
//        }

//        public SuratControllerBase SuratControllerBase
//        {
//            get
//            {
//                if (suratControllerBase == null)
//                    suratControllerBase = new SuratControllerBase();

//                return suratControllerBase;
//            }
//            set { suratControllerBase = value; }
//        }

//        public List<ExternalSystemsUsersView> KullaniciMasterDbVeritabanlari(string returnUrl)
//        {
//            return this.suratControllerBase.Serendip.KullaniciMasterDbVeritabanlari;
//        }
//    }
//}