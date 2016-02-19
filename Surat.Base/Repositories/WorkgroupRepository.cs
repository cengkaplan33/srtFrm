using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model;
using Surat.Base.Model.Entities;
using Surat.Base.Security;
using Surat.Common.Data;
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
    public class WorkgroupRepository : GenericRepository<Workgroup>
    {
        //İlk aşamada, bazı nesneler daha ilk değerlerini almadan, sistem ile ilgili bilgiye ihtiyaç duyulduğu için, özel geliştirmeler yapıldı. (DBContext in geçirildiği contructor ve direkt DBContext in cast edilerek kullanılması. Context parametresi null kontrolü.)
        #region Constructor

        public WorkgroupRepository(SecurityContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;
        }

        public WorkgroupRepository(FrameworkDbContext dbContext)
            : base(dbContext)
        {

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

        public FrameworkDbContext DBContext
        {
            get
            {
                return (FrameworkDbContext)(this.GenericDBContext);
            }
        }    

        #endregion

        #region Methods

        public List<Workgroup> GetActiveWorkGroups()
        {
            return this.GetObjectsByParameters(p => p.IsActive == true).ToList();
        }

        public List<WorkgroupView> GetAllActiveWorkGroups()
        {
            List<WorkgroupView> allWorkgroups;

            //Diğer contructor ile gelme durumu var. Context olmayabilir. bu sebeble, dbcontext üzerinden kullanıldı.
            allWorkgroups = (from workgroups in this.DBContext.Workgroups
                             where (workgroups.IsActive == true)
                             select new WorkgroupView()
                             {
                                 WorkgroupId = workgroups.Id,
                                 ParentWorkgroupId = workgroups.ParentId,
                                 WorkgroupName = workgroups.Name,
                                 IsCompanySite = workgroups.isCompanySite
                             }).ToList();

            return allWorkgroups;
        }

        public int? GetParentWorkgroupId(int workgroupId)
        {
            Workgroup selectedWorkgroup;

            selectedWorkgroup = this.GetById(workgroupId);

            if (selectedWorkgroup == null)
                throw new RecordNotFoundException(this.Context.ApplicationContext,"Workgroup", this.Context.ApplicationContext.SystemId,
                    string.Format(this.Context.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.Context.ApplicationContext.SystemId, Constants.ExceptionType.RecordNotFound), workgroupId));

            return selectedWorkgroup.ParentId;
        }       

        #endregion        
    }
}
