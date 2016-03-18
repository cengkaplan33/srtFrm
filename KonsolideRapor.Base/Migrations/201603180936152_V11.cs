namespace KonsolideRapor.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V11 : DbMigration
    {
        public override void Up()
        {
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
        }
    }
}
