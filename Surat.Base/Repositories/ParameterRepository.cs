using Surat.Base.Configuration;
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
    public class ParameterRepository : GenericRepository<Parameter>
    {
        //İlk aşamada, bazı nesneler daha ilk değerlerini almadan, sistem ile ilgili bilgiye ihtiyaç duyulduğu için, özel geliştirmeler yapıldı. (DBContext in geçirildiği contructor ve direkt DBContext in cast edilerek kullanılması. Context parametresi null kontrolü.)
        #region Constructor

        public ParameterRepository(ConfigurationContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;  
        }

        public ParameterRepository(FrameworkDbContext dbContext)
            : base(dbContext)
        {
            this.GenericDBContext = dbContext;
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

        public FrameworkDbContext FrameworkDBContext
        {
            get { return (FrameworkDbContext)this.GenericDBContext; }
        } 

        #endregion

        #region Methods

        public List<Parameter> GetParametersBySystemId(int systemId)
        {
            return this.GetObjectsByParameters(p => p.IsActive == true & p.DBObjectId == systemId).ToList();
        }
        public List<ParameterValueView> GetParametersBySystem(int systemId)
        {
            List<ParameterValueView> systemParameters;

            //Context oluşturulmadan parametre değerleri okunması gerekiyor. Onun için, Direkt FrameworkDBContext üzerinden okundu.
            systemParameters = (from parameterValues in this.FrameworkDBContext.ParameterValues
                                join parameters in this.FrameworkDBContext.Parameters on parameterValues.ParameterId equals parameters.Id
                                where (parameters.DBObjectId == systemId && parameters.DBObjectType == (int)ParameterDBObjectType.System)
                                select new ParameterValueView
                                {
                                    ParameterOwnerDBObjectType = (ParameterOwnerDBObjectType)parameterValues.ParameterOwnerDBObjectType,
                                    ParameterOwnerDBObjectId = parameterValues.ParameterOwnerDBObjectId,
                                    ParameterTypeName = parameters.TypeName,
                                    ParameterValue = parameterValues.Value
                                }).ToList();
            
            return systemParameters;
        }       

        #endregion        
    }
}
