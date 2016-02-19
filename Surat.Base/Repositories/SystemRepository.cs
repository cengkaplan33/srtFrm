using Surat.Base.Configuration;
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
    public class SystemRepository : GenericRepository<SuratSystem>
    {
        //İlk aşamada, bazı nesneler daha ilk değerlerini almadan, sistem ile ilgili bilgiye ihtiyaç duyulduğu için, özel geliştirmeler yapıldı. (DBContext in geçirildiği contructor ve direkt DBContext in cast edilerek kullanılması. Context parametresi null kontrolü.)
        #region Constructor

        public SystemRepository(ConfigurationContext contextParameter)
            : base(contextParameter.ApplicationContext.DBContext)
        {
            this.context = contextParameter;
        }
        
        public SystemRepository(FrameworkDbContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region Private Members

        private ConfigurationContext context;

        #endregion

        #region Public Members 

        public ConfigurationContext Context
        {
            get 
            { 
                if (context == null)
                    throw new NullValueException(this.Context.ApplicationContext,"SystemRepository.Context",this.Context.ApplicationContext.SystemId);
                return context; 
            }
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

        public List<SuratSystem> GetActiveSystems()
        {
            return this.GetObjectsByParameters(p => p.IsActive == true).ToList();
        }

        public List<SystemDetailedView> GetSystems()
        {
            List<SystemDetailedView> systems;

            systems = (from suratSystems in this.Context.ApplicationContext.DBContext.Systems                       
                       select new SystemDetailedView()
                       {
                           Id = suratSystems.Id,
                            ParentId = suratSystems.ParentId,
                           Name = suratSystems.Name,
                           ObjectTypeName = suratSystems.ObjectTypeName
          
                       }).ToList();

            return systems;
        }

        public int GetSystemIdByTypeName(string systemTypeName)
        {
            int systemId;

            
                if (context != null)
                {
                    systemId = this.Context.Systems.Where(p => p.ObjectTypeName == systemTypeName).FirstOrDefault().Id; //DB ye gidilmeden, Session üzerinden alındı.
                }
                else
                {                                         
                    try
                    {
                        systemId = this.DBContext.Systems.Where(p => p.ObjectTypeName == systemTypeName).First().Id;  //Cnfig okuma ilk aşamada gerekli olduğu için (Henüz sistem hazır değil), özel bir yöntem ile okunuyor. (Özel constructor) 
                    }
                    catch (Exception exception)
                    {
                        //Context olmadığı için, framework ve Globalization a erişimde yok.
                        throw new RecordNotFoundException(null, systemTypeName, 0,exception); //ToDo : Frameowrk System Id verilmel. İsimden erişilerek.
                    }
                }           

            return systemId;
        }

        public string GetTypeNameById(int systemId)
        {
            string systemTypeName;

            try
            {
                if (context != null)
                    systemTypeName = this.Context.Systems.Where(p => p.Id == systemId).FirstOrDefault().ObjectTypeName; //DB ye gidilmeden, Session üzerinden alındı.
                else systemTypeName = this.DBContext.Systems.Where(p => p.Id == systemId).First().ObjectTypeName;  //Cnfig okuma ilk aşamada gerekli olduğu için (Henüz sistem hazır değil), özel bir yöntem ile okunuyor. (Özel constructor)
            }
            catch (Exception exception)
            {
                throw new RecordNotFoundException(this.Context.ApplicationContext, "System.GetTypeNameById", this.Context.ApplicationContext.SystemId,
                    string.Format(this.Context.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.Context.ApplicationContext.SystemId, Constants.ExceptionType.RecordNotFound), systemId),
                    exception);
            }

            return systemTypeName;
        }        

        #endregion

        
    }
}
