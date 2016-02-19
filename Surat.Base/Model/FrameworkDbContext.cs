using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Common.Data;
using Surat.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Model
{
    public class FrameworkDbContext :DbContext
    {
        #region Constructor

        public FrameworkDbContext(string connection): base(connection)
        {
            this.systemName = Constants.Application.FrameworkSystemName;
        }
        public FrameworkDbContext(): base("name=SuratFrameworkConnection")
        {
            Database.SetInitializer<FrameworkDbContext>(null);
            this.systemName = Constants.Application.FrameworkSystemName;
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

        public DbSet<AccessibleItem> AccessibleItems { get; set; }
        public DbSet<FailedLogin> FailedLogins { get; set; }
        public DbSet<DBRowStateChange> DBRowStateChanges { get; set; }
        public DbSet<SuratAction> Actions { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<GlobalizationKey> GlobalizationKeys { get; set; }
        public DbSet<GlobalizationKeyValue> GlobalizationKeyValues { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<ParameterValue> ParameterValues { get; set; } 
        public DbSet<ParameterChange> ParameterChanges { get; set; }
        public DbSet<RelationGroup> RelationGroups { get; set; }
        public DbSet<SuratRole> Roles { get; set; }
        public DbSet<SuratSystem> Systems { get; set; }
        public DbSet<SuratUser> Users { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Toolbar> Toolbars { get; set; }
        public DbSet<ToolbarItem> ToolbarItems { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<UserShortcut> UserShortcuts { get; set; }
        public DbSet<Workgroup> Workgroups { get; set; }
        public DbSet<CompanySite> CompanySites { get; set; }
        public DbSet<CompanySiteDBPeriod> CompanySiteDBPeriods { get; set; }
        public DbSet<SuratService> Services { get; set; }
        public DbSet<SuratServiceMethod> ServiceMethods { get; set; }
        public DbSet<ExternalSystemsUser> ExternalSystemsUsers { get; set; }

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
