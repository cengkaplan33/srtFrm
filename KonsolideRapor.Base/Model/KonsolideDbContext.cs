using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KonsolideRapor.Common.Data;
namespace KonsolideRapor.Base.Model
{
    public class KonsolideDbContext:DbContext
    {
        #region Constructor

        public KonsolideDbContext()
            : base("name=SuratFrameworkConnection")
        {
            Database.SetInitializer<KonsolideDbContext>(null);
            this.systemName = KonsolideConstants.Application.KonsolideSystemName;
        }

        #endregion

        #region Private Members  

        private string systemName;
 
        #endregion

        #region Public Members

        public string SystemName
        {
            get { return systemName; }
        }

      

        #endregion

        #region Overrides

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
            
        //}

        #endregion

        #region Methods            

        #endregion     
    }
}
