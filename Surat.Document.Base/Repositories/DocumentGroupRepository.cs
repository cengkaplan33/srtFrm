using Surat.Base.Configuration;
using Surat.Base.Exceptions;
using Surat.Base.Security;
using Surat.Common.ViewModel;
using Surat.Document.Base.Configuration;
using Surat.Document.Base.Manage;
using Surat.Document.Base.Model.Entities;
using Surat.Document.Common.Data;
using Surat.Document.Common.ViewModel;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Documents.Base.Repositories
{
    public class DocumentGroupRepository : GenericRepository<DocumentGroup>
    {
        #region Constructor

        public DocumentGroupRepository(DocumentContext contextParameter)
            : base(contextParameter.ApplicationContext.DBContext)
        {
            this.context = contextParameter;
        }

        #endregion

        #region Private Members

        private DocumentContext context;

        #endregion

        #region Public Members

        public DocumentContext Context
        {
            get { return context; }
        }

        #endregion

        #region Methods       
   
        public bool ChildGroupExist(int parentGroupId,string groupName,out int groupId)
        {
            bool result = false;
            DocumentGroup group;

            group = this.Context.ApplicationContext.DBContext.DocumentGroups.Where(p => p.ParentId == parentGroupId && p.Name == groupName).FirstOrDefault();
            if (group != null)
            {
                groupId = group.Id;
                result = true;
            }
            else groupId = 0;
            
            return result;
        }
        #endregion
    }
}
