namespace Surat.Document.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2212 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentStores", "WorkgroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentStores", "WorkgroupId");
        }
    }
}
