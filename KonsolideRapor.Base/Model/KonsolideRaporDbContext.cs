using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KonsolideRapor.Common.Data;
using Surat.Base.Model.Entities;
namespace KonsolideRapor.Base.Model
{
    public class KonsolideRaporDbContext:DbContext
    {
        #region Constructor

        public KonsolideRaporDbContext()
            : base("name=SuratFrameworkConnection")
        {
            Database.SetInitializer<KonsolideRaporDbContext>(null);
            this.systemName = KonsolideConstants.Application.PlatformSystemName;
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

        public DbSet<Bank> Bankalar { get; set; }

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
