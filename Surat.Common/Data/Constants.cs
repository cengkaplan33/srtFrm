using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Data
{
    public class Constants
    {
        #region Parameters
        public class Parameter
        {
            public const String Digits = "0123456789";
            public const String UpperCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public const String SpecialCharacters = "*#?$%&/}{][><"; //ToDo : Parametre haline getirilebilir
        }
        #endregion

        #region Application
        public class Application
        {
            public const String PlatformSystemName = "SuratApplicationPlatform";
            public const String FrameworkSystemName = "SuratFramework";
            public const String DocumentManagementSystemName = "DocumentManagement";
            public const String WebFrameworkSystemName = "SuratWebFramework";
            public const String ServiceFrameworkSystemName = "SuratServiceFramework";
            public const string Admin = "Admin";
            public const string RootWorkgroup = "RootWorkgroup";
        }
        #endregion

        #region Framework
        public class FrameworkType
        {
            public const string Serendip = "Serendip";
            public const string Surat = "Surat";
        }
        #endregion

        #region Web
        public class Web
        {
            public const String SPAHomePrefix = "Home/Spa#/";
            public const String RedirectLoginAction = "~/Account/Login";      
            public const String RedirectLogoutAction = "~/Account/LogOut";
            public static readonly string[] UnAuthorizedActions = { "Account/Login", "Account/UserLogin", "Account/LogOut" };
            public static readonly string[] SharedActions = { "Home/Spa", "home/index", "Account/LogOut"};
        }
        #endregion

        #region Message

        public class Message
        {
            public const String FrameworkNotInitialized = "FrameworkNotInitialized|Alt yapı bileşenleri(Framework) başlatılamadı.|Framework has not been initialized.";
            public const String OperationNotCompleted = "OperationNotCompleted|İşlem tamamlanamadı.|Operation has not been completed.";
            public const String UserNotAuthorized = "UserNotAuthorized|Kullanıcı {0} yetkili değildir.|User {0} has not been authorized.";
            public const String OperationCompleted = "OperationCompleted|İşlem tamamlandı.|Operation has been completed.";
            public const String RequiredField = "RequiredField|{0} alanı zorunludur.|Field {0} is required.";
            public const String ValueMustBeNumeric = "ValueMustBeNumeric|Değer nümerik olmalıdır.|Value must be numeric";
            public const String ContactToSystemAdministrator = "ContactToSystemAdministrator|Sistem yöneticiniz ile irtibata geçiniz.|Contact to System Administrator.";
            public const String ConfirmDelete = "ConfirmDelete|Silmek istiyor musunuz?|Do you want to delete?";
            public const String ValidateStartAndEndDate = "ValidateStartAndEndDate|Başlangıç tarihi, Bitiş tarihinden büyük olamaz.|EndDate must be greater than StartDate.";
            public const String ChildElementsMustBeDeleted = "ChildElementsMustBeDeleted|Öncelikli olarak, bağlı elemanlar silinmelidir.|Firstly, Child elements must deleted.";
            public const String ParentMustBeSelected = "ParentMustBeSelected|Üst eleman seçilmelidir.|Parent must be selected.";
            public const String WrongFormat = "WrongFormat|Yanlış format.|Format is wrong.";
            public const String UserLocked = "UserLocked|Kullanıcı {0} kilitlidir.|User {0} is locked.";
            public const String UserIsNotActive = "UserIsNotActive|Kullanıcı {0} aktif değildir.|User {0} is not active.";
            public const String UserDefaultRoleMissing = "UserDefaultRoleMissing|Kullanıcının standart(default) rolü eksiktir.|User does not has a default role.";
            public const String UserDefaultWorkgroupMissing = "UserDefaultWorkgroupMissing|Kullanıcının standart çalışma grubu eksiktir.|User does not has a default workgroup.";
            public const String UserCompanyMissing = "UserCompanyMissing|Kullanının ilişkili firma bilgisi eksiktir.|User does not has a company related.";
            public const String PasswordChangePeriodExceeded = "PasswordChangePeriodExceeded|Şifre değiştirme süresi aşıldı.|The period of changing password has been exceeded.";
            public const String PasswordLenghtRule = "PasswordLenghtRule|Şifre minimum {0} karakter uzunlukta olmalıdır.|Password must be at least {0} characters long.";
            public const String PasswordAtLeastOneDigitRule = "PasswordAtLeastOneDigitRule|Şifrede en az bir rakam olmalıdır.|Password must contain at least one digit.";
            public const String PasswordAtLeastOneUpperCharacterRule = "PasswordAtLeastOneUpperCharacterRule|Şifrede en az bir büyük harf olmalıdır.|Password must contain at least one capital character.";
            public const String PasswordAtLeastOneSpecialCharacter = "PasswordAtLeastOneSpecialCharacter|Şifrede en az bir özel karakter olmalıdır.|Password must contain at least one special character.";
            public const String FrameworkSessionNotStarted = "FrameworkSessionNotStarted|Sistem oturumu başlatılamadı.|Framework session has not been started.";
            public const String FrameworkSessionNotClosed = "FrameworkSessionNotClosed|Sistem oturumu kapatılamadı.|Framework session has not been closed.";
            public const String ExceptionNotPublished = "ExceptionNotPublished|İstisna(Hata) yayınlanamadı.|Exception has not been published.";
            public const String WorkgroupCompanySiteNotFound = "WorkgroupCompanySiteNotFound|{0} Çalışma grubuna ait firma tanım kaydı bulunamadı.|Workgroup {0} does not has a related company record.";            
        }

        #endregion

        #region Page
        public class Page
        {
            public class ToolbarCommand
            {
                public const String Save = "Save";
                public const String New = "New";
                public const String Edit = "Edit";
                public const String Delete = "Delete";
                public const String Close = "Close";
                public const String Export = "Export";
                public const String Return = "Return";
                public const String Accept = "Accept";
                public const String SendEMail = "SendEMail";
            }

            public class ExportCommand
            {
                public const String ExportToPdf = "ExportToPdf";
                public const String ExportToWord = "ExportToWord";
                public const String ExportToExcel = "ExportToExcel";
                public const String ExportToCSV = "ExportToCSV";
            }
        }
        #endregion

        #region Exception

        public class ExceptionType
        {
            public const String DBContextNotInitialized = "DBContextNotInitializedException|DBContext hazırlanması aşamasında hata oluştu.|DBContext has not been initialized.";
            public const String ContextInitialization = "ContextInitializationException|Context hazırlanması aşamasında hata oluştu.|Context has not been initialized.";
            public const String RecordNotFound = "RecordNotFoundException|{0} parametresi ile kayıt bulunamamıştır.| Record with parameter {0} was not found.";
            public const String ItemNotFound = "ItemNotFoundException|{0} elemanı bulunamamıştır.|The item {0} was not found.";
            public const String NullValue = "NullValueException|Alanda değer yoktur.|The field has null value.";
            public const String InvalidType = "InvalidTypeException|{0} geçersiz tiptir.|The type {0} is invalid.";
            public const String InvalidInput = "InvalidInputException|Geçersiz bilgi girişi ile karşılaşılmıştır.|The input is invalid.";
            public const String InterfaceNotImplemented = "InterfaceNotImplementedException|Arayüz gerçekleştirilmemiştir.|The interface has not been implemented.";
            public const String EntityProcess = "DBException|Veritabanı işleminde hata oluştu.|Database operation has not been executed.";
            public const String File = "FileException|Dosya işleminde hata oluştu.|File operation has not been executed.";
            public const String DBDuplicateKey = "DBDuplicateKeyException|Aynı anahtar değerli kayıt bulunmaktadır.|Record with same key exists in the database.";
            public const String AuthenticationFailed = "AuthenticationFailedException|Kimlik belirleme işlemi başarısız oldu.|Authentication has not been succeeded.";
            public const String SMTPMail = "SMTPMailException|SMTP mail hatası oluştu.|Mail has not been sent with SMTP protocol.";
            public const String WrongPassword = "WrongPasswordException|Şifre hatalıdır.|Password is wrong.";
            public const String ForceChangePassword = "ForceChangePasswordException|Şifre değiştirilmelidir.|Password must be changed.";
            public const String SuratBusinessException = "SuratBusinessException|İş süreci ile ilgili hata oluştu.|Application has a business exception.";
            public const String MissingFeature = "MissingFeatureException|{0} Özelliği henüz geliştirilmemiştir.|The feature {0} is missing.";
            public const String Security = "SecurityException|Güvenlik problemi bulunmaktadır.|Application has a security problem.";
            public const String Configuration = "ConfigurationException|Konfigurasyon erişimi problemi bulunmaktadır.|Application has a configuration access problem.";
        }

        #endregion

        #region Log

        public class Log
        {
            public const String ExceptionToDB = "Log.ExceptionToDB";
            public const String ExceptionToFile = "Log.ExceptionToFile";
        }

        #endregion

        #region Globalization

        public class Globalization
        {

        }

        #endregion

        #region Cache

        public class CacheList
        {
            public const String WorkgroupList = "WorkgroupList";
            public const String ParameterValueList = "ParameterValueList";
            public const String SystemList = "SystemList";
            public const String UserAccessiblePages = "UserAccessiblePages";
            public const String UserAccessibleActions = "UserAccessibleActions";
            public const String ExternalSystemsUsers = "ExternalSystemsUsers";
            public const String CompanySites = "CompanySites";
            public const String GlobalizationKeyValueList = "GlobalizationKeyValueList";
        }

        #endregion
    }
}
