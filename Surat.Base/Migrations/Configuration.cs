namespace Surat.Base.Migrations
{
    using Surat.Base.Model;
    using Surat.Base.Model.Entities;
    using Surat.Common.Data;
    using Surat.Common.Migrations;
    using Surat.Common.Utilities;
    using Surat.SerendipApplication.Common;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Surat.Base.Model.FrameworkDbContext>
    {
        private Dictionary<string, string> systemParameterValues;

        public Configuration()
        {
            
            AutomaticMigrationsEnabled = true;
            systemParameterValues = new Dictionary<string, string>();
            //Sees metodunu Debub etmek için gereklidir.
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
        }

        protected override void Seed(FrameworkDbContext context)
        { 
            AddAdminUser(context);
            AddRoles(context);
            AddWorkgroups(context);
            AddSystems(context);
            AddSystemPages(context);
            AddGeneralParameters(context);
            AddLogParameters(context);
            AddMailParameters(context);
            AddGlobalizationParameters(context);
            AddSecurityParameters(context);
            AddProductParameters(context); 
           
            //Parameter Id değerine ihtiyaça var. Parametre değerleri girilecektir.
            context.SaveChanges();
            
            AddParameterValues(context);
            context.SaveChanges();
        }

        private void AddAdminUser(FrameworkDbContext context)
        {
            context.Users.AddOrUpdate(
                        user => user.Id,
                        new SuratUser
                        {
                            Id = 1,
                            UserName = Constants.Application.Admin,
                            Password = "1",
                            Name = "Administrator",
                            DefaultRole = 1, //Admin Role
                            DefaultWorkgroup = 1, //Root Workgroup
                            IsActive = true,
                            IsLocked = false,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });
            
            //User yetkilendirmesi için Relation tablosunda kaydı olmalı.
            context.RelationGroups.AddOrUpdate(
                        user => user.Id,
                        new RelationGroup
                        {
                            Id = 1,
                            RoleId = 0,
                            UserId = 1, //Admin
                            WorkgroupId = 0,
                            IsActive = true,
                            InsertedByUser = 1, 
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });

            //Set Workgroup - Company

            context.RelationGroups.AddOrUpdate(
                        user => user.Id,
                        new RelationGroup
                        {
                            Id = 2,
                            RoleId = 0,
                            UserId = 1, //Admin
                            WorkgroupId = 1, //Root
                            IsActive = true,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });
        }

        private void AddRoles(FrameworkDbContext context)
        {
            context.Roles.AddOrUpdate(
                        role => role.Id,
                        new SuratRole
                        { 
                            Id = 1,
                            ObjectTypeName = Constants.Application.Admin,
                            Name = "Administrator",
                            IsActive = true,       
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });
        }

        private void AddWorkgroups(FrameworkDbContext context)
        {
            context.Workgroups.AddOrUpdate(
                        workgroup => workgroup.Id,
                        new Workgroup
                        {    
                            Id = 1,
                            ParentId = null,
                            ObjectTypeName = Constants.Application.RootWorkgroup,
                            Name = Constants.Application.RootWorkgroup,
                            IsActive = true,
                            CompanyId = 1,
                            isCompanySite = true,   
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });

            //İlişkili CompanySite kaydı
            context.CompanySites.AddOrUpdate(
                        workgroup => workgroup.Id,
                        new CompanySite
                        {
                            Id = 1,
                            RelatedWorkgroupId = 1, //RootWorkgroup
                            CompanyCode = 0,
                            IsActive = true,   
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });
        }

        private void AddSystems(FrameworkDbContext context)
        {
            context.Systems.AddOrUpdate(
                        system => system.Id,
                        new SuratSystem
                        {
                            Id = 1,// Root System
                            ParentId = null, //Kendo Tree null değeri beklediği için bu şekilde yapıldı.
                            ObjectTypeName = Constants.Application.PlatformSystemName,
                            Name = Constants.Application.PlatformSystemName,
                            IsActive = true,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });

            context.Systems.AddOrUpdate(
                        system => system.Id,
                        new SuratSystem
                        {
                            Id = 2, //Framework System
                            ParentId = 1, 
                            ObjectTypeName = Constants.Application.FrameworkSystemName,
                            Name = Constants.Application.FrameworkSystemName,
                            IsActive = true,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });

            context.Systems.AddOrUpdate(
                        system => system.Id,
                        new SuratSystem
                        {
                            Id = 3, //WebFramework
                            ParentId = 1,
                            ObjectTypeName = Constants.Application.WebFrameworkSystemName,
                            Name = Constants.Application.WebFrameworkSystemName,
                            IsActive = true,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });

            context.Systems.AddOrUpdate(
                        system => system.Id,
                        new SuratSystem
                        {
                            Id = 4, //DocumentManagement
                            ParentId = 1,
                            ObjectTypeName = Constants.Application.DocumentManagementSystemName,
                            Name = Constants.Application.DocumentManagementSystemName,
                            IsActive = true,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });

            context.Systems.AddOrUpdate(
                        system => system.Id,
                        new SuratSystem
                        {
                            Id = 5, //ServiceFramework
                            ParentId = 1,
                            ObjectTypeName = Constants.Application.ServiceFrameworkSystemName,
                            Name = Constants.Application.ServiceFrameworkSystemName,
                            IsActive = true,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });

            context.Systems.AddOrUpdate(
                        system => system.Id,
                        new SuratSystem
                        {
                            Id = 6, //Serendip
                            ParentId = 1,
                            ObjectTypeName = SerendipConstants.Application.SerendipSystemName,
                            Name = SerendipConstants.Application.SerendipSystemName,
                            IsActive = true,
                            InsertedByUser = 1,
                            InsertedDate = TimeUtility.GetCurrentDateTime()
                        });
        }        

        private void AddGeneralParameters(FrameworkDbContext context)
        {
            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "FrameworkProviderType"))
            {
                context.Parameters.AddOrUpdate(
                           parameter => parameter.Id,
                           new Parameter
                           {
                               DBObjectType = 0,
                               DBObjectId = 2, //Framework system
                               TypeName = "FrameworkProviderType",
                               IsActive = true,
                               InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                               InsertedDate = TimeUtility.GetCurrentDateTime()
                           });
                systemParameterValues.Add("FrameworkProviderType", "0");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "FrameworkProductName"))
            {
                context.Parameters.AddOrUpdate(
                           parameter => parameter.Id,
                           new Parameter
                           {
                               DBObjectType = 0,
                               DBObjectId = 2, //Framework system
                               TypeName = "FrameworkProductName",
                               IsActive = true,
                               InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                               InsertedDate = TimeUtility.GetCurrentDateTime()
                           });
                systemParameterValues.Add("FrameworkProductName", "Surat Framework");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "FrameworkProductVersion"))
            {
                context.Parameters.AddOrUpdate(
                           parameter => parameter.Id,
                           new Parameter
                           {
                               DBObjectType = 0,
                               DBObjectId = 2, //Framework system
                               TypeName = "FrameworkProductVersion",
                               IsActive = true,
                               InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                               InsertedDate = TimeUtility.GetCurrentDateTime()
                           });
                systemParameterValues.Add("FrameworkProductVersion", "V0.9");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "FrameworkProductVersionDate"))
            {
                context.Parameters.AddOrUpdate(
                           parameter => parameter.Id,
                           new Parameter
                           {
                               DBObjectType = 0,
                               DBObjectId = 2, //Framework system
                               TypeName = "FrameworkProductVersionDate",
                               IsActive = true,
                               InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                               InsertedDate = TimeUtility.GetCurrentDateTime()
                           });
                systemParameterValues.Add("FrameworkProductVersionDate", "01/01/2016");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ApplicationRootFolderPath"))
            {
                context.Parameters.AddOrUpdate(
                           parameter => parameter.Id,
                           new Parameter
                           {
                               DBObjectType = 0,
                               DBObjectId = 2, //Framework system
                               TypeName = "ApplicationRootFolderPath",
                               IsActive = true,
                               InsertedByUser = 1,
                               InsertedDate = TimeUtility.GetCurrentDateTime()
                           });
                systemParameterValues.Add("ApplicationRootFolderPath", "C:\\SURATPLATFORM\\");
            }
        }

        private void AddLogParameters(FrameworkDbContext context)
        {
            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "TraceFolderName"))
            {
                context.Parameters.AddOrUpdate(
                            parameter => parameter.Id,
                            new Parameter
                            {
                                DBObjectType = 0,
                                DBObjectId = 2, //Framework system
                                TypeName = "TraceFolderName",
                                IsActive = true,
                                InsertedByUser = 1,
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
                systemParameterValues.Add("TraceFolderName", "Traces");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ExceptionFolderName"))
            {
                context.Parameters.AddOrUpdate(
                 parameter => parameter.Id,
                 new Parameter
                 {
                     DBObjectType = 0,
                     DBObjectId = 2, //Framework system
                     TypeName = "ExceptionFolderName",
                     IsActive = true,
                     InsertedByUser = 1,
                     InsertedDate = TimeUtility.GetCurrentDateTime()
                 });
                systemParameterValues.Add("ExceptionFolderName", "Exceptions");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "TraceLevel"))
            {
                context.Parameters.AddOrUpdate(
                 parameter => parameter.Id,
                 new Parameter
                 {
                     DBObjectType = 0,
                     DBObjectId = 2, //Framework system
                     TypeName = "TraceLevel",
                     IsActive = true,
                     InsertedByUser = 1,
                     InsertedDate = TimeUtility.GetCurrentDateTime()
                 });
                systemParameterValues.Add("TraceLevel", "2");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "TraceBufferSizeAsKbyte"))
            {
                context.Parameters.AddOrUpdate(
                 parameter => parameter.Id,
                 new Parameter
                 {
                     DBObjectType = 0,
                     DBObjectId = 2, //Framework system
                     TypeName = "TraceBufferSizeAsKbyte",
                     IsActive = true,
                     InsertedByUser = 1,
                     InsertedDate = TimeUtility.GetCurrentDateTime()
                 });
                systemParameterValues.Add("TraceBufferSizeAsKbyte", "100");
            }
        }        

        private void AddMailParameters(FrameworkDbContext context)
        {
            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "SMTPServerIP"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "SMTPServerIP",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("SMTPServerIP", "172.27.10.43");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "SMTPServerPort"))
            {
                context.Parameters.AddOrUpdate(
                            parameter => parameter.Id,
                            new Parameter
                            {
                                DBObjectType = 0,
                                DBObjectId = 2, //Framework system
                                TypeName = "SMTPServerPort",
                                IsActive = true,
                                InsertedByUser = 1,
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
                systemParameterValues.Add("SMTPServerPort", "25");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "SMTPServerUser"))
            {
                context.Parameters.AddOrUpdate(
                            parameter => parameter.Id,
                            new Parameter
                            {
                                DBObjectType = 0,
                                DBObjectId = 2, //Framework system
                                TypeName = "SMTPServerUser",
                                IsActive = true,
                                InsertedByUser = 1,
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
                systemParameterValues.Add("SMTPServerUser", "bilgi.inka");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "SMTPServerUserPassword"))
            {
                context.Parameters.AddOrUpdate(
                            parameter => parameter.Id,
                            new Parameter
                            {
                                DBObjectType = 0,
                                DBObjectId = 2, //Framework system
                                TypeName = "SMTPServerUserPassword", // To Do : şifreli koyulmalı
                                IsActive = true,
                                InsertedByUser = 1,
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
                systemParameterValues.Add("SMTPServerUserPassword", "bin.1234");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "SMTPMailFrom"))
            {
                context.Parameters.AddOrUpdate(
                            parameter => parameter.Id,
                            new Parameter
                            {
                                DBObjectType = 0,
                                DBObjectId = 2, //Framework system
                                TypeName = "SMTPMailFrom", // To Do : şifreli koyulmalı
                                IsActive = true,
                                InsertedByUser = 1,
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
                systemParameterValues.Add("SMTPMailFrom", "bilgi.inka@kaynak.com.tr");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "SMTPEnableSSL"))
            {
                context.Parameters.AddOrUpdate(
                            parameter => parameter.Id,
                            new Parameter
                            {
                                DBObjectType = 0,
                                DBObjectId = 2, //Framework system
                                TypeName = "SMTPEnableSSL", // To Do : şifreli koyulmalı
                                IsActive = true,
                                InsertedByUser = 1,
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
                systemParameterValues.Add("SMTPEnableSSL", "True");
            }
        }

        private void AddGlobalizationParameters(FrameworkDbContext context)
        {
            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ApplicationCulture"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ApplicationCulture",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ApplicationCulture", "0");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CurrentCulture"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CurrentCulture",
                                 IsCustomizable = true,
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CurrentCulture", "0");
            }
        }

        private void AddSecurityParameters(FrameworkDbContext context)
        {
            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "MaxWrongPasswordAttempts"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "MaxWrongPasswordAttempts",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("MaxWrongPasswordAttempts", "3");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "MinPasswordLength"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "MinPasswordLength",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("MinPasswordLength", "8");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CookieExpirationTimeAsDays"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CookieExpirationTimeAsDays",
                                 IsCustomizable = true,
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CookieExpirationTimeAsDays", "1");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "MaxPasswordChangePeriodAsDays"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "MaxPasswordChangePeriodAsDays",
                                 IsCustomizable = true,
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("MaxPasswordChangePeriodAsDays", "15");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "IsCaptchaActive"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "IsCaptchaActive",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("IsCaptchaActive", "False");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "IsRememberMeActive"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "IsRememberMeActive",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("IsRememberMeActive", "False");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "WebSessionTimeoutInMinutes"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "WebSessionTimeoutInMinutes",
                                 IsCustomizable = true,
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("WebSessionTimeoutInMinutes", "10");
            }
        }

        private void AddProductParameters(FrameworkDbContext context)
        {
            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProductName"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProductName",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProductName", "SURATFRAMEWORK");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProductVersion"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProductVersion",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProductVersion", "Version 1.0");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProductVersionDate"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProductVersionDate",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProductVersionDate", "2015");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProductLogoPath"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProductLogoPath",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProductLogoPath", "");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProducerName"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProducerName",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProducerName", "SÜRAT TEKNOLOJİ");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProducerWebsite"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProducerWebsite",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProducerWebsite", "http://www.suratteknoloji.com/");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProducerSupportEmail"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProducerSupportEmail",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProducerSupportEmail", "ertan.deniz@surat.com.tr");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "ProducerCentralPhone"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "ProducerCentralPhone",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("ProducerCentralPhone", "+90 216 522 11 50");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerName"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerName",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerName", "MÜŞTERİ");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerProductName"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerProductName",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerProductName", " MÜŞTERİ SİSTEMİ");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerAdress"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerAdress",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerAdress", "MÜŞTERİ ADRESİ");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerWebsite"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerWebsite",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerWebsite", "http://www.musteri.com/");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerLogoPath"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerLogoPath",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerLogoPath", "");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerCentralPhone"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerCentralPhone",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerCentralPhone", "+90 216 999 99 99");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerManagerName"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerManagerName",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerManagerName", "Şirket Yöneticisi");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerManagerEmail1"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerManagerEmail1",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerManagerEmail1", "yonetici@musteri.com");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerManagerMobilePhone"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerManagerMobilePhone",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerManagerMobilePhone", "555 555 55 55");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerManagerPhoneInternal"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerManagerPhoneInternal",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerManagerPhoneInternal", "9999");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerSystemAdministratorName"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerSystemAdministratorName",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerSystemAdministratorName", "Sistem Yöneticisi");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerSystemAdministratorEmail1"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerSystemAdministratorEmail1",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerSystemAdministratorEmail1", "admin@musteri.com");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerSystemAdministratorMobilePhone"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerSystemAdministratorMobilePhone",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerSystemAdministratorMobilePhone", "555 555 55 55");
            }

            if (!MigrationUtility.CheckEntityByParameter<Parameter>(context, p => p.TypeName == "CustomerSystemAdministratorPhoneInternal"))
            {
                context.Parameters.AddOrUpdate(
                             parameter => parameter.Id,
                             new Parameter
                             {
                                 DBObjectType = 0,
                                 DBObjectId = 2, //Framework system
                                 TypeName = "CustomerSystemAdministratorPhoneInternal",
                                 IsActive = true,
                                 InsertedByUser = 1,
                                 InsertedDate = TimeUtility.GetCurrentDateTime()
                             });
                systemParameterValues.Add("CustomerSystemAdministratorPhoneInternal", "8888");
            }
        }

        private void AddSystemPages(FrameworkDbContext context)
        {
            if (!MigrationUtility.CheckEntityByParameter<Page>(context, p => p.ObjectTypeName == "Users"))            
            { 
                context.Pages.AddOrUpdate(
                            system => system.Id,
                            new Page
                            {
                                SystemId = 2,
                                Name = "Kullanıcılar",
                                ObjectTypePrefix = "Users/Index",
                                ObjectTypeName = "Users",
                                IsVisibleInMenu = true,
                                IsAccessControlRequired = true,
                                BigImagePath = "",
                                SmallImagePath = "",
                                IsActive = true,
                                InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
            }

            if (!MigrationUtility.CheckEntityByParameter<Page>(context, p => p.ObjectTypeName == "Roles"))
            {
                context.Pages.AddOrUpdate(
                            system => system.Id,
                            new Page
                            {
                                SystemId = 2,
                                Name = "Roller",
                                ObjectTypePrefix = "Roles/Index",
                                ObjectTypeName = "Roles",
                                IsVisibleInMenu = true,
                                IsAccessControlRequired = true,
                                BigImagePath = "",
                                SmallImagePath = "",
                                IsActive = true,
                                InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
            }

            if (!MigrationUtility.CheckEntityByParameter<Page>(context, p => p.ObjectTypeName == "Workgroups"))
            {
                context.Pages.AddOrUpdate(
                            system => system.Id,
                            new Page
                            {
                                SystemId = 2,
                                Name = "Çalışma grupları",
                                ObjectTypePrefix = "Workgroups/Index",
                                ObjectTypeName = "Workgroups",
                                IsVisibleInMenu = true,
                                IsAccessControlRequired = true,
                                BigImagePath = "",
                                SmallImagePath = "",
                                IsActive = true,
                                InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
            }

            if (!MigrationUtility.CheckEntityByParameter<Page>(context, p => p.ObjectTypeName == "Systems"))
            {
                context.Pages.AddOrUpdate(
                            system => system.Id,
                            new Page
                            {
                                SystemId = 2,
                                Name = "Sistemler",
                                ObjectTypePrefix = "Systems/Index",
                                ObjectTypeName = "Systems",
                                IsVisibleInMenu = true,
                                IsAccessControlRequired = true,
                                BigImagePath = "",
                                SmallImagePath = "",
                                IsActive = true,
                                InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
            }

            if (!MigrationUtility.CheckEntityByParameter<Page>(context, p => p.ObjectTypeName == "Parameters"))
            {
                context.Pages.AddOrUpdate(
                            system => system.Id,
                            new Page
                            {
                                SystemId = 2,
                                Name = "Parametreler",
                                ObjectTypePrefix = "Parameters/Index",
                                ObjectTypeName = "Parameters",
                                IsVisibleInMenu = true,
                                IsAccessControlRequired = true,
                                BigImagePath = "",
                                SmallImagePath = "",
                                IsActive = true,
                                InsertedByUser = 1, //To do : User ve zaman tekrar ele alinacak
                                InsertedDate = TimeUtility.GetCurrentDateTime()
                            });
            }
        }

        private void AddParameterValues(FrameworkDbContext context)
        {
            int parameterId;

            foreach (KeyValuePair<string,string> item in systemParameterValues)
            {
                try
                {
                    parameterId = context.Parameters.Where(p => p.TypeName == item.Key).FirstOrDefault().Id;
                }
                catch
                {
                    //ToDo : Exception publish
                    parameterId = 0;
                }
                
                context.ParameterValues.AddOrUpdate(
                          parameter => parameter.Id,
                          new ParameterValue
                          {
                              ParameterId = parameterId,
                              ParameterOwnerDBObjectId = 1, //Root workgroup
                              ParameterOwnerDBObjectType = 0, //Workgroup type
                              Value = item.Value,
                              IsActive = true,
                              InsertedByUser = 1,
                              InsertedDate = TimeUtility.GetCurrentDateTime()
                          }); 
            }            
        }
    }
}
