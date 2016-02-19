using Surat.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Application
{
    public class SystemContext
    {
        #region Constructor

        public SystemContext(FrameworkDbContext dbContextParameter)
        {
            dbContext = dbContextParameter;
        }

        #endregion

        #region Private Members

        private FrameworkDbContext dbContext;
        private string applicationRootFolderPath;
        private string frameworkProductName;
        private string frameworkProductVersion;
        private DateTime frameworkProductVersionDate;

        #endregion

        #region Public Members

        public FrameworkDbContext DBContext
        {
            get { return dbContext; }
            set { dbContext = value; }
        }

        public string ApplicationRootFolderPath
        {
            get { return applicationRootFolderPath; }
            set { applicationRootFolderPath = value; }
        }

        public string FrameworkProductName
        {
            get { return frameworkProductName; }
            set { frameworkProductName = value; }
        }

        public string FrameworkProductVersion
        {
            get { return frameworkProductVersion; }
            set { frameworkProductVersion = value; }
        }

        public DateTime FrameworkProductVersionDate
        {
            get { return frameworkProductVersionDate; }
            set { frameworkProductVersionDate = value; }
        }

        #endregion

    }
}
