using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Surat.Base.Application;
using Surat.Base.Mail;
using Surat.Document.Base.Application;
using Surat.Document.Base.Manage;
using Surat.Document.Common.ViewModel;
using Surat.Documents.Base.Repositories;
using Surat.Document.Base.Model.Entities;
using Surat.Base.Exceptions;
using Surat.Common.Data;
using System.IO;
using Surat.Common.ViewModel;
using Surat.Business.Application;
using Surat.Document.Common.Data;
using Surat.Common.Utilities;
using Surat.Document.Common.Application;
using Surat.Common.Application;
using Surat.Common.Security;

namespace Surat.Document.Business.Manage
{
    public class DocumentManager
    {
        #region Constructor

        public DocumentManager(IDocumentApplicationManager documentApplicationManager)
        {
            this.documentApplicationManager = documentApplicationManager;
        }

        #endregion

        #region Private Members

        private IDocumentApplicationManager documentApplicationManager;
        private DocumentApplicationContext applicationContext;
        private IFrameworkManager frameworkApplicationManager;
        private FrameworkContext frameworkContext;
        private ISecurityManager securityManager;
        private DocumentRepository document;
        private DocumentStoreRepository documentStore;
        private DocumentGroupRepository documentGroup;        

        #endregion

        #region Public Members        

        public IDocumentApplicationManager DocumentApplicationManager
        {
            get
            {
                return documentApplicationManager;
            }
        }

        public DocumentApplicationContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (DocumentApplicationContext)this.DocumentApplicationManager.GetDocumentApplicationContext();

                return applicationContext;
            }
        }

        public DocumentContext Context
        {
            get
            {
                return this.ApplicationContext.Document;
            }
        }

        public IFrameworkManager Framework
        {
            get
            {
                if (frameworkApplicationManager == null)
                    frameworkApplicationManager = this.DocumentApplicationManager.GetFrameworkManager();

                return frameworkApplicationManager;
            }
        }

        public FrameworkContext FrameworkContext
        {
            get
            {
                if (frameworkContext == null)
                    frameworkContext = (FrameworkContext)this.Framework.GetApplicationContext();

                return frameworkContext;
            }
        }

        public ISecurityManager SecurityManager
        {
            get
            {
                if (securityManager == null)
                    securityManager = this.Framework.GetSecurityManager();

                return securityManager;
            }
        }

        #endregion         

        #region Repositories


        public DocumentRepository Document
        {
            get
            {
                if (document == null)
                    document = new DocumentRepository(this.ApplicationContext.Document);

                return document;
            }
        }

        public DocumentStoreRepository DocumentStore
        {
            get
            {
                if (documentStore == null)
                    documentStore = new DocumentStoreRepository(this.ApplicationContext.Document);

                return documentStore;
            }
        }

        public DocumentGroupRepository DocumentGroup
        {
            get
            {
                if (documentGroup == null)
                    documentGroup = new DocumentGroupRepository(this.ApplicationContext.Document);

                return documentGroup;
            }
        }

        #endregion

        #region Methods

        private void WriteFile(string storePath, string fileName,byte[] documentData)
        {
            try
            {                
                if (!Directory.Exists(storePath))
                {
                    Directory.CreateDirectory(storePath);
                }
                File.WriteAllBytes(storePath + fileName, documentData);
            }
            catch (Exception exception)
            {
                throw new FileException(this.FrameworkContext,"WriteFile", this.ApplicationContext.SystemId, exception);
            }            
        }

        public DocumentDownloadView GetDocument(UserDetailedView user,long id)
        {
            ExistingDocumentView selectedDocument;
            string documentGroupPath;
            DocumentStoreView documentStore;
            DocumentDownloadView document = new DocumentDownloadView();

            selectedDocument = this.Document.GetDocumentById(id);
            documentStore = this.GetUserDocumentStore(user);
            documentGroupPath = GetAttachmentDocumentGroupPath(selectedDocument.InsertedDate);
            document.DocumentData = File.ReadAllBytes(documentStore.RootFilePath + documentGroupPath + GetFileNameAsDocumentId(selectedDocument.FileName,id));

            return document;
        }

        public long SaveDocument(UserDetailedView currentUser,DocumentUploadView document)
        {
            SuratDocument documentObject;
            DocumentStoreView documentStore;
            string documentGroupPath;
            DateTime documentInsertedDate;
            int documentGroupId;

            try
            {
                int initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                //Store bulunması
                documentStore = this.GetUserDocumentStore(currentUser);

                if (document.Id == 0)
                {
                    documentObject = new SuratDocument();

                    documentObject.ContentTypeId = 0;
                    documentObject.DocumentOwnerUserId = currentUser.UserId;
                    documentObject.DocumentStoreId = documentStore.Id;

                    this.Document.Add(documentObject);
                }
                else
                {
                    documentObject = this.Document.GetById(document.Id);
                    if (documentObject == null)
                        throw new RecordNotFoundException(this.FrameworkContext, "SaveDocument", this.ApplicationContext.SystemId,
                        string.Format(this.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.ExceptionType.RecordNotFound), document.Id));

                    this.Document.Update(documentObject);
                }                

                documentObject.FileName = document.FileName;
                documentObject.FileTypeId = document.FileTypeId;
                documentObject.RelatedObjectId = document.RelatedObjectId;
                documentObject.RelatedObjectType = document.RelatedObjectType;
                documentObject.Notes = document.Notes;                
                                
                
                //Eksik grupların eklenmesi
                if (documentObject.Id != 0)
                    documentInsertedDate = documentObject.InsertedDate;
                else documentInsertedDate = TimeUtility.GetCurrentDateTime();
                documentGroupId = this.AddMissingDocumentGroups(documentStore.RootDocumentGroupId,documentInsertedDate);

                //Yeni kayıt için, grubun güncellenmesi
                if (documentObject.Id == 0)
                    documentObject.DocumentGroupId = documentGroupId;

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

                //ToDo : Dosya yazılmazsa, işlem geri alınmalı.
                if (document.DocumentData != null)
                {
                    documentGroupPath = GetAttachmentDocumentGroupPath(documentObject.InsertedDate);
                    this.WriteFile(documentStore.RootFilePath + documentGroupPath, GetFileNameAsDocumentId(documentObject.FileName,documentObject.Id), document.DocumentData);
                }
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "SaveDocument", this.ApplicationContext.SystemId,exception);
            }

            return documentObject.Id;
        }

        public void SaveDocuments(UserDetailedView user,IEnumerable<DocumentUploadView> documents)
        {
            int initializedDBContextId;
            //ToDo : Doküman ve DB arasındaki işin bütünlüğü ele alınmalıdır. (Transaction Dokümanları kapsamıyor)
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                foreach (DocumentUploadView document in documents)
                {
                    this.SaveDocument(user,document);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext,"SaveDocuments", this.ApplicationContext.SystemId,exception);
            }
        }

        public void DeleteDocument(long id)
        {
            int initializedDBContextId;
            SuratDocument document;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                document = this.Document.GetById(id);
                if (document == null)
                    throw new RecordNotFoundException(this.FrameworkContext, "DeleteDocument", this.ApplicationContext.SystemId,
                        string.Format(this.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.ExceptionType.RecordNotFound), id)
                        );

                document.IsActive = false;

                this.Document.Update(document);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "SaveDocuments", this.ApplicationContext.SystemId,exception);
            }
        }

        public DocumentStoreView GetUserDocumentStore(UserDetailedView user)
        {
            DocumentStoreView documentStore;
            int workgroupId;            

            workgroupId = this.SecurityManager.GetUserDefaultWorkgroup(user.UserId);
            documentStore = FindWorkgroupStore(workgroupId);

            if (documentStore == null)
                throw new SuratBusinessException(this.FrameworkContext, "GetUserDocumentStore", this.ApplicationContext.SystemId, this.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, DocumentConstants.Message.UserDocumentStoreNotFound));

            return documentStore;
        }

        private DocumentStoreView FindWorkgroupStore(int workgroupId)
        {
            DocumentStoreView documentStore;
            int? parentWorkgroupId;

            documentStore = this.DocumentStore.GetDocumentStoreByWorkgroupId(workgroupId);

            if (documentStore == null)
            {
                parentWorkgroupId = this.SecurityManager.GetParentWorkgroupId(workgroupId);

                if (parentWorkgroupId.HasValue)
                    documentStore = FindWorkgroupStore(parentWorkgroupId.Value);
            }
            
            return documentStore;
        }

        private string GetAttachmentDocumentGroupPath(DateTime documentInsertedDate)
        {
            string path = DocumentConstants.Application.AttachmentsGroupShortName + "\\" +
                          documentInsertedDate.Year.ToString() + "\\" +
                          documentInsertedDate.Month.ToString() + "\\" +
                          documentInsertedDate.Day.ToString() + "\\";

            return path;
        }

        private int AddMissingDocumentGroups(int rootDocumentGroupId,DateTime documentInsertedDate)
        {
            int attachmentsDocumentGroupId, yearsDocumentGroupId, monthsDocumentGroupId, daysDocumentGroupId;
            int initializedDBContextId = this.ApplicationContext.InitializeDBContext();

            if (!this.DocumentGroup.ChildGroupExist(rootDocumentGroupId, DocumentConstants.Application.AttachmentsGroupName, out attachmentsDocumentGroupId))
            {
                attachmentsDocumentGroupId = AddAttachmentsDocumentGroup(rootDocumentGroupId);
                yearsDocumentGroupId = AddYearsDocumentGroup(attachmentsDocumentGroupId, documentInsertedDate.Year);
                monthsDocumentGroupId = AddMonthsDocumentGroup(yearsDocumentGroupId, documentInsertedDate.Month);
                daysDocumentGroupId = AddMonthsDocumentGroup(monthsDocumentGroupId, documentInsertedDate.Day);
            }
            else
            {
                if (!this.DocumentGroup.ChildGroupExist(attachmentsDocumentGroupId, documentInsertedDate.Year.ToString(), out yearsDocumentGroupId)) //YearsGroup Check
                    yearsDocumentGroupId = AddYearsDocumentGroup(attachmentsDocumentGroupId, documentInsertedDate.Year);

                if (!this.DocumentGroup.ChildGroupExist(yearsDocumentGroupId, documentInsertedDate.Month.ToString(), out monthsDocumentGroupId)) //YearsGroup Check
                    monthsDocumentGroupId = AddMonthsDocumentGroup(yearsDocumentGroupId, documentInsertedDate.Month);

                if (!this.DocumentGroup.ChildGroupExist(monthsDocumentGroupId, documentInsertedDate.Day.ToString(), out daysDocumentGroupId)) //YearsGroup Check
                    daysDocumentGroupId = AddMonthsDocumentGroup(monthsDocumentGroupId, documentInsertedDate.Day);
            }

            this.ApplicationContext.CommitDBChanges(initializedDBContextId);

            return daysDocumentGroupId;
        }

        private int AddAttachmentsDocumentGroup(int parentDocumentGroupId)
        {
            DocumentGroup attachmentsDocumentGroup = new DocumentGroup();

            int initializedDBContextId = this.ApplicationContext.InitializeDBContext(ContextMode.StartWithNewContext);
            attachmentsDocumentGroup.ParentId = parentDocumentGroupId;
            attachmentsDocumentGroup.Name = DocumentConstants.Application.AttachmentsGroupName;
            attachmentsDocumentGroup.ShortName = DocumentConstants.Application.AttachmentsGroupShortName;
            attachmentsDocumentGroup.IsActive = true;
            this.DocumentGroup.Add(attachmentsDocumentGroup);

            this.ApplicationContext.CommitDBChanges(initializedDBContextId);//ToDo:Id almak için bu şekilde yapıldı. Farklı çözüm geliştirilecek.

            return attachmentsDocumentGroup.Id;
        }

        private int AddYearsDocumentGroup(int parentDocumentGroupId, int year)
        {
            DocumentGroup yearDocumentGroup = new DocumentGroup();

            int initializedDBContextId = this.ApplicationContext.InitializeDBContext(ContextMode.StartWithNewContext);
            yearDocumentGroup.ParentId = parentDocumentGroupId;
            yearDocumentGroup.Name = year.ToString();
            yearDocumentGroup.ShortName = yearDocumentGroup.Name;
            yearDocumentGroup.IsActive = true;
            this.DocumentGroup.Add(yearDocumentGroup);
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);//ToDo:Id almak için bu şekilde yapıldı. Farklı çözüm geliştirilecek.

            return yearDocumentGroup.Id;
        }

        private int AddMonthsDocumentGroup(int parentDocumentGroupId, int month)
        {
            DocumentGroup monthDocumentGroup = new DocumentGroup();

            int initializedDBContextId = this.ApplicationContext.InitializeDBContext(ContextMode.StartWithNewContext);
            monthDocumentGroup.ParentId = parentDocumentGroupId;
            monthDocumentGroup.Name = month.ToString();
            monthDocumentGroup.ShortName = monthDocumentGroup.Name;
            monthDocumentGroup.IsActive = true;
            this.DocumentGroup.Add(monthDocumentGroup);
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);//ToDo:Id almak için bu şekilde yapıldı. Farklı çözüm geliştirilecek.

            return monthDocumentGroup.Id;
        }

        private int AddDaysDocumentGroup(int parentDocumentGroupId, int day)
        {
            DocumentGroup dayDocumentGroup = new DocumentGroup();

            int initializedDBContextId = this.ApplicationContext.InitializeDBContext(ContextMode.StartWithNewContext);
            dayDocumentGroup.ParentId = parentDocumentGroupId;
            dayDocumentGroup.Name = day.ToString();
            dayDocumentGroup.ShortName = dayDocumentGroup.Name;
            dayDocumentGroup.IsActive = true;
            this.DocumentGroup.Add(dayDocumentGroup);
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);//ToDo:Id almak için bu şekilde yapıldı. Farklı çözüm geliştirilecek.

            return dayDocumentGroup.Id;
        }

        private string GetFileNameAsDocumentId(string fileName,long documentId)
        {
            return documentId.ToString() + System.IO.Path.GetExtension(fileName);
        }

        #endregion

    }
}
