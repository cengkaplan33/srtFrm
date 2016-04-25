using Surat.Base.Application;
using Surat.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.ActiveDirectory
{
    public class ActiveDirectoryContext
    {
        #region Constructor

        public ActiveDirectoryContext(FrameworkDbContext dbContextParameter)
        {
            dbContext = dbContextParameter;
        }

        #endregion

        #region Private Members

        private FrameworkDbContext dbContext;
        private string service;
        private string domain;
        private string userName;
        private string password;
        private string container;
        #endregion

        #region Public Members       

        public FrameworkDbContext DBContext
        {
            get { return dbContext; }
            set { dbContext = value; }
        }

        public string Service
        {
            get { return service; }
            set { service = value; }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Container
        {
            get { return container; }
            set { container = value; }
        }
        #endregion

        #region Methods

        #endregion

    }
}
