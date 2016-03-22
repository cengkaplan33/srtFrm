namespace Surat.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SuratRights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 200),
                        SystemId = c.Int(nullable: false),
                        InsertedByUser = c.Int(nullable: false),
                        InsertedDate = c.DateTime(nullable: false),
                        ChangedByUser = c.Int(),
                        ChangedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SuratSystems", t => t.SystemId, cascadeDelete: true)
                .Index(t => t.SystemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SuratRights", "SystemId", "dbo.SuratSystems");
            DropIndex("dbo.SuratRights", new[] { "SystemId" });
            DropTable("dbo.SuratRights");
        }
    }
}
