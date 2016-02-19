namespace Surat.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V1412 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GlobalizationKeys", new[] { "SystemId" });
            AlterColumn("dbo.ExternalSystemsUsers", "Password", c => c.String(maxLength: 35));
            AlterColumn("dbo.GlobalizationKeys", "SystemId", c => c.Int(nullable: false));
            AlterColumn("dbo.SuratUsers", "Password", c => c.String(nullable: false, maxLength: 35));
            CreateIndex("dbo.GlobalizationKeys", "SystemId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GlobalizationKeys", new[] { "SystemId" });
            AlterColumn("dbo.SuratUsers", "Password", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.GlobalizationKeys", "SystemId", c => c.Short(nullable: false));
            AlterColumn("dbo.ExternalSystemsUsers", "Password", c => c.String(maxLength: 30));
            CreateIndex("dbo.GlobalizationKeys", "SystemId");
        }
    }
}
