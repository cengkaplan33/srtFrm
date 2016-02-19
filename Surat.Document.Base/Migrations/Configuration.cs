namespace Surat.Document.Base.Migrations
{
    using Surat.Common.Utilities;
    using Surat.Document.Base.Model;
    using Surat.Document.Base.Model.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DocumentDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DocumentDbContext context)
        {
            AddFileTypes(context);
            AddRootDocumentGroup(context);
            AddRootDocumentStore(context);            

            context.SaveChanges();
        }

        private void AddRootDocumentStore(DocumentDbContext context)
        {
            context.DocumentStores.AddOrUpdate(
                        documentStore => documentStore.Id,
                        new DocumentStore
                        {
                            Id = 1,
                            Name = "Root",
                            TypeName = "Root",
                            RootDocumentGroupId = 1,
                            MaximumDocumentCount = 10000,
                            MaximumDocumentSizeInMB = 100,
                            RootFilePath = "C:\\DM\\",
                            WorkgroupId = 1,
                            SizeInGB = 100,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime(),
                            IsActive = true                           
                        });
        }

        private void AddRootDocumentGroup(DocumentDbContext context)
        {
            context.DocumentGroups.AddOrUpdate(
                        documentGroup => documentGroup.Id,
                        new DocumentGroup
                        {
                            Id = 1,
                            Name = "Döküman Sistemi",
                            ShortName = "DM",
                            ParentId = 0,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime(),
                            IsActive = true
                        });
        }

        private void AddFileTypes(DocumentDbContext context)
        {
            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 1,
                            Name = "Word 97",
                            TypeName = "doc",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 2,
                            Name = "Word",
                            TypeName = "docx",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 3,
                            Name = "Excel 97",
                            TypeName = "xls",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 4,
                            Name = "Excel",
                            TypeName = "xlsx",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 5,
                            Name = "Powerpoint 97",
                            TypeName = "ppt",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 6,
                            Name = "Powerpoint",
                            TypeName = "pptx",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 7,
                            Name = "Text",
                            TypeName = "txt",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 8,
                            Name = "PDF",
                            TypeName = "pdf",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 9,
                            Name = "BitMap",
                            TypeName = "bmp",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 10,
                            Name = "Multimedia Audio/Video",
                            TypeName = "avi",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 11,
                            Name = "Windows sound",
                            TypeName = "wav",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 12,
                            Name = "Multimedia Audio/Video",
                            TypeName = "avi",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 13,
                            Name = "Graphics Interchange Format",
                            TypeName = "gif",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 14,
                            Name = "Web sayfası",
                            TypeName = "htm",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                        fileType => fileType.Id,
                        new FileType
                        {
                            Id = 15,
                            Name = "Web sayfası",
                            TypeName = "html",
                            IconFileName = "X"
                        });

            context.FileTypes.AddOrUpdate(
                       fileType => fileType.Id,
                       new FileType
                       {
                           Id = 16,
                           Name = "Web sayfası",
                           TypeName = "html",
                           IconFileName = "X"
                       });

            context.FileTypes.AddOrUpdate(
                       fileType => fileType.Id,
                       new FileType
                       {
                           Id = 17,
                           Name = "JPEG",
                           TypeName = "jpeg",
                           IconFileName = "X"
                       });

            context.FileTypes.AddOrUpdate(
                       fileType => fileType.Id,
                       new FileType
                       {
                           Id = 18,
                           Name = "TIFF",
                           TypeName = "tif",
                           IconFileName = "X"
                       });

            context.FileTypes.AddOrUpdate(
                       fileType => fileType.Id,
                       new FileType
                       {
                           Id = 19,
                           Name = "ZIP",
                           TypeName = "zip",
                           IconFileName = "X"
                       });
        }
    }
}
