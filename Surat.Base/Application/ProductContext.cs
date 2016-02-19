using Surat.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Application
{
    public class ProductContext
    {
        #region Constructor

        public ProductContext(FrameworkDbContext dbContextParameter)
        {
            dbContext = dbContextParameter;
        }

        #endregion

        #region Private Members

        private FrameworkDbContext dbContext;
        private String productName;
        private String productVersion;
        private String productVersionDate;
        private String productLogoPath;
        private String producerName;
        private String producerWebsite;
        private String producerCentralPhone;
        private String producerSupportEmail;
        private String customerName;
        private String customerProductName;
        private String customerAdress;
        private String customerWebsite;
        private String customerLogoPath; 
        private String customerCentralPhone;
        private String customerManagerName;
        private String customerManagerEmail1;
        private String customerManagerMobilePhone;
        private String customerManagerPhoneInternal;
        private String customerSystemAdministratorName;
        private String customerSystemAdministratorEmail1;
        private String customerSystemAdministratorMobilePhone;
        private String customerSystemAdministratorPhoneInternal;

        #endregion

        #region Public Members

        public String ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public String ProductVersion
        {
            get { return productVersion; }
            set { productVersion = value; }
        }

        public String ProductVersionDate
        {
            get { return productVersionDate; }
            set { productVersionDate = value; }
        }

        public String ProductLogoPath
        {
            get { return productLogoPath; }
            set { productLogoPath = value; }
        }

        public String ProducerName
        {
            get { return producerName; }
            set { producerName = value; }
        }

        public String ProducerWebsite
        {
            get { return producerWebsite; }
            set { producerWebsite = value; }
        }

        public String ProducerSupportEmail
        {
            get { return producerSupportEmail; }
            set { producerSupportEmail = value; }
        }

        public String ProducerCentralPhone
        {
            get { return producerCentralPhone; }
            set { producerCentralPhone = value; }
        }

        public String CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public String CustomerProductName
        {
            get { return customerProductName; }
            set { customerProductName = value; }
        }

        public String CustomerAdress
        {
            get { return customerAdress; }
            set { customerAdress = value; }
        }

        public String CustomerWebsite
        {
            get { return customerWebsite; }
            set { customerWebsite = value; }
        }

        public String CustomerLogoPath
        {
            get { return customerLogoPath; }
            set { customerLogoPath = value; }
        }

        public String CustomerCentralPhone
        {
            get { return customerCentralPhone; }
            set { customerCentralPhone = value; }
        }

        public String CustomerManagerName
        {
            get { return customerManagerName; }
            set { customerManagerName = value; }
        }

        public String CustomerManagerEmail1
        {
            get { return customerManagerEmail1; }
            set { customerManagerEmail1 = value; }
        }

        public String CustomerManagerMobilePhone
        {
            get { return customerManagerMobilePhone; }
            set { customerManagerMobilePhone = value; }
        }

        public String CustomerManagerPhoneInternal
        {
            get { return customerManagerPhoneInternal; }
            set { customerManagerPhoneInternal = value; }
        }

        public String CustomerSystemAdministratorName
        {
            get { return customerSystemAdministratorName; }
            set { customerSystemAdministratorName = value; }
        }

        public String CustomerSystemAdministratorEmail1
        {
            get { return customerSystemAdministratorEmail1; }
            set { customerSystemAdministratorEmail1 = value; }
        } 

        public String CustomerSystemAdministratorMobilePhone
        {
            get { return customerSystemAdministratorMobilePhone; }
            set { customerSystemAdministratorMobilePhone = value; }
        }       

        public String CustomerSystemAdministratorPhoneInternal
        {
            get { return customerSystemAdministratorPhoneInternal; }
            set { customerSystemAdministratorPhoneInternal = value; }
        }

        #endregion

        #region Methods

        #endregion

    }
}
