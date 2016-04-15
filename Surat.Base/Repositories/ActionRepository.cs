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
using Surat.Common.Data;

namespace Surat.Base.Repositories
{
    public class ActionRepository : GenericRepository<SuratAction>
    {
        #region Constructor

        public ActionRepository(SecurityContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;
        }

        #endregion

        #region Private Members

        private SecurityContext context;

        #endregion

        #region Public Members

        public SecurityContext Context
        {
            get { return context; }
        }

        #endregion

        #region Methods       

        public List<AccessibleActionView> GetAllActions()
        {
            List<AccessibleActionView> accessibleActions;

            accessibleActions = (from Actions in this.Context.ApplicationContext.DBContext.Actions
                                 select new AccessibleActionView
                                 {
                                     ActionId = Actions.Id,
                                     TypeName = Actions.TypeName,
                                 }).ToList();

            return accessibleActions;
        }

        public List<UserAccessibleActionView> GetAllActionForUser()
        {
            List<UserAccessibleActionView> actions;
            actions = (from action in this.Context.ApplicationContext.DBContext.Actions
                       where (action.IsActive == true )
                       orderby (action.Name)
                     select new UserAccessibleActionView
                     {
                         ActionId = action.Id,
                         ActionName = action.Name,
                         Description = action.Description,
                         Type = action.Type,
                     }).ToList();

            return actions;
        }

        #endregion
    }
}
