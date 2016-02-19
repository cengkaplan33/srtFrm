namespace Surat.Document.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2812 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SuratDocuments", "FileName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuratDocuments", "FileName", c => c.String(maxLength: 30));
        }
    }
}
