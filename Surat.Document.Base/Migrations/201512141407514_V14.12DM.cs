namespace Surat.Document.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V1412DM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileTypes", "IconFileName", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.SuratDocuments", new[] { "RelatedObjectType", "RelatedObjectId" }, name: "IX_Document_RelatedObjectType_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SuratDocuments", "IX_Document_RelatedObjectType_Id");
            DropColumn("dbo.FileTypes", "IconFileName");
        }
    }
}
