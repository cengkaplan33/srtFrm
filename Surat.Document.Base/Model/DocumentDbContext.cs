using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Common.Data;
using Surat.Common.Utilities;
using Surat.Document.Base.Model.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Base.Model
{
    public class DocumentDbContext :DbContext
    {
        #region Constructor

        public DocumentDbContext()
            : base("name=SuratFrameworkConnection")
        {
            Database.SetInitializer<DocumentDbContext>(null);
            this.systemName = Constants.Application.DocumentManagementSystemName;
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

        public DbSet<SuratDocument> Documents { get; set; }
        public DbSet<DocumentChange> DocumentChanges { get; set; }
        public DbSet<DocumentCheckout> DocumentCheckouts { get; set; }
        public DbSet<DocumentGroup> DocumentGroups { get; set; }
        public DbSet<DocumentShare> DocumentShares { get; set; }
        public DbSet<DocumentStore> DocumentStores { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentTypeProperty> ContentTypeProperties { get; set; }
        public DbSet<ContentTypePropertyValue> ContentTypePropertyValues { get; set; }

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
