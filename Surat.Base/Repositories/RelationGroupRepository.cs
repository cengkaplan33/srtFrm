using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Base.Security;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Repositories
{
    public class RelationGroupRepository : GenericRepository<RelationGroup>
    {
        #region Constructor

        public RelationGroupRepository(SecurityContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;
        }

        #endregion

        #region Private Members

        public SecurityContext context;

        #endregion

        #region Public Members

        public SecurityContext Context
        {
            get { return context; }
        }

        #endregion

        #region Methods

        public int GetIdByParameters(int userId, int roleId, int workgroupId)
        {
            RelationGroup relationGroup;

            relationGroup = this.Context.ApplicationContext.DBContext.RelationGroups.Where(p => p.UserId == userId && p.RoleId == roleId && p.WorkgroupId == workgroupId).FirstOrDefault();

            if (relationGroup == null)
                throw new RecordNotFoundException(this.Context.ApplicationContext, "RelationGroup.GetIdByParameters-" + "UserId=" + userId.ToString() + " RoleId=" + roleId.ToString() + " WorkgroupId=" + workgroupId.ToString(), 
                    context.ApplicationContext.SystemId);

            return relationGroup.Id;
        }

        #endregion
    }
}
