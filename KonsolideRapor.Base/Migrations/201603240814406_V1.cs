namespace KonsolideRapor.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 15),
                        Name = c.String(nullable: false, maxLength: 40),
                        InsertedByUser = c.Int(nullable: false),
                        InsertedDate = c.DateTime(nullable: false),
                        ChangedByUser = c.Int(),
                        ChangedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OdemeTalepDurumus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Durum = c.String(nullable: false, maxLength: 50),
                        IsBanka = c.Boolean(nullable: false),
                        IsOdeme = c.Boolean(nullable: false),
                        IsTahsilat = c.Boolean(nullable: false),
                        InsertedByUser = c.Int(nullable: false),
                        InsertedDate = c.DateTime(nullable: false),
                        ChangedByUser = c.Int(),
                        ChangedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentCollectings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 200),
                        IsPayment = c.Boolean(nullable: false),
                        IsCollection = c.Boolean(nullable: false),
                        InsertedByUser = c.Int(nullable: false),
                        InsertedDate = c.DateTime(nullable: false),
                        ChangedByUser = c.Int(),
                        ChangedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PaymentCollectings");
            DropTable("dbo.OdemeTalepDurumus");
            DropTable("dbo.Banks");
        }
    }
}
