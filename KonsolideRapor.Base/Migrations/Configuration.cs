namespace KonsolideRapor.Base.Migrations
{
    using KonsolideRapor.Base.Model.Entities;
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
            this.AddOdemeTalepDurumlari(context);
        }
        private void AddBanks(KonsolideRapor.Base.Model.KonsolideRaporDbContext context)
        {
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id=1,
                Code = "1",
                Name = "Akbank",
                ObjectType="Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 2,
                Code = "2",
                Name = "Albaraka",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 3,
                Code = "3",
                Name = "Bankasya",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 4,
                Code = "4",
                Name = "Finansbank",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 5,
                Code = "5",
                Name = "Garanti",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 6,
                Code = "6",
                Name = "Ýþ Bankasý",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 7,
                Code = "7",
                Name = "Kuveyttürk",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 8,
                Code = "8",
                Name = "Türkiye Finans",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 9,
                Code = "9",
                Name = "Vakýfbank",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 10,
                Code = "10",
                Name = "Yapýkredi",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 11,
                Code = "11",
                Name = "Ziraat Bankasý",
                ObjectType = "Banka",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 12,
                Code = "12",
                Name = "Kasa",
                ObjectType = "Kasa",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
            context.Bankalar.AddOrUpdate(p => p.Id, new Bank
            {
                Id = 13,
                Code = "13",
                Name = "Çek",
                ObjectType = "Çek",
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime(),

            });
        }
        private void AddOdemeTalepDurumlari(KonsolideRapor.Base.Model.KonsolideRaporDbContext context)
        {
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id=1,
                Durum="Bekliyor",                
                IsOdeme=true,
                IsTahsilat=true,
                IsBanka = false,
                IsKasa=false,
                IsCek=false,
                IsActive=true,
                InsertedByUser=1,
                InsertedDate=TimeUtility.GetCurrentDateTime(),
                
            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 2,
                Durum = "Ödenmedi",
                IsOdeme = true,
                IsTahsilat = true,
                IsBanka = false,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 3,
                Durum = "Ödendi",
                IsOdeme = true,
                IsTahsilat = false,
                IsBanka = false,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 4,
                Durum = "Ýptal Edildi",
                IsOdeme = true,
                IsTahsilat = true,
                IsBanka = false,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 5,
                Durum = "Tahsil Edildi",
                IsOdeme = false,
                IsTahsilat = true,
                IsBanka = false,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 6,
                Durum = "Kasa Mevcudu",
                IsOdeme = false,
                IsTahsilat = false,
                IsBanka = false,
                IsKasa = true,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 7,
                Durum = "Portföyde",
                IsOdeme = false,
                IsTahsilat = false,
                IsBanka = false,
                IsKasa = false,
                IsCek = true,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 8,
                Durum = "Karþýlýksýz",
                IsOdeme = false,
                IsTahsilat = false,
                IsBanka = false,
                IsKasa = false,
                IsCek = true,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 9,
                Durum = "Bloke",
                IsOdeme = false,
                IsTahsilat = false,
                IsBanka = true,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 10,
                Durum = "Nakit",
                IsOdeme = false,
                IsTahsilat = false,
                IsBanka = true,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 11,
                Durum = "Tahsilde",
                IsOdeme = false,
                IsTahsilat = false,
                IsBanka = true,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
            context.OdemeTalepDurumu.AddOrUpdate(p => p.Id, new OdemeTalepDurumu
            {
                Id = 12,
                Durum = "Teminatta",
                IsOdeme = false,
                IsTahsilat = false,
                IsBanka = true,
                IsKasa = false,
                IsCek = false,
                IsActive = true,
                InsertedByUser = 1,
                InsertedDate = TimeUtility.GetCurrentDateTime()

            });
        }
    }
}
