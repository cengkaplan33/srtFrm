using Surat.Base.Configuration;
using Surat.Base.Model.Entities;
using Surat.Base.Security;
using Surat.Common.ViewModel;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Repositories
{
    public class PageRepository : GenericRepository<Page>
    {
        #region Constructor

        public PageRepository(ConfigurationContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;
        }

        #endregion

        #region Private Members

        private ConfigurationContext context;

        #endregion

        #region Public Members

        public ConfigurationContext Context
        {
            get { return context; }
        }

        #endregion

        #region Methods

        public List<Page> GetSystemPages(int systemId)
        {
            return this.GetObjectsByParameters(p => p.IsActive == true & p.SystemId == systemId).ToList();
        }

        public List<AccessiblePageView> GetAllPages()
        {
            List<AccessiblePageView> accessiblePages;

            accessiblePages = (from pages in this.Context.ApplicationContext.DBContext.Pages
                               join systems in this.Context.ApplicationContext.DBContext.Systems on pages.SystemId equals systems.Id 
                               where (pages.IsActive == true)
                               select new AccessiblePageView
                               {
                                   PageId = pages.Id,
                                   SystemParentId = systems.ParentId,
                                   SystemName = systems.ObjectTypeName, //To do : Name tanımlanınca o koyulacak.
                                   SystemId = pages.SystemId,
                                   PageName = pages.Name,
                                   ObjectTypePrefix = pages.ObjectTypePrefix,
                                   ObjectTypeName = pages.ObjectTypePrefix,
                                   BigImagePath = pages.BigImagePath,
                                   SmallImagePath = pages.SmallImagePath
                               }).ToList();

            return accessiblePages;
        }

        #endregion
    }
}
