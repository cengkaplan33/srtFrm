namespace KonsolideRapor.Base.Migrations
{
    using Surat.Base.Model.Entities;
    using Surat.Common.Utilities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KonsolideRapor.Base.Model.KonsolideRaporDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(KonsolideRapor.Base.Model.KonsolideRaporDbContext context)
        {
            
           
            this.AddBanks(context);
        }
        private void AddBanks(KonsolideRapor.Base.Model.KonsolideRaporDbContext context)
        {
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id=1,
                Code = "1",
                Name = "Akbank",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 2,
                Code = "2",
                Name = "Albaraka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 3,
                Code = "3",
                Name = "Bankasya",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 4,
                Code = "4",
                Name = "Finansbank",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 5,
                Code = "5",
                Name = "Garanti",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 6,
                Code = "6",
                Name = "Ýþ Bankasý",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 7,
                Code = "7",
                Name = "Kuveyttürk",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 8,
                Code = "8",
                Name = "Türkiye Finans",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 9,
                Code = "9",
                Name = "Vakýfbank",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 10,
                Code = "10",
                Name = "Yapýkredi",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 11,
                Code = "11",
                Name = "Ziraat Bankasý",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
        }
    }
}
