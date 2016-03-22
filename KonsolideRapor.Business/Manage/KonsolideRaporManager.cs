using KonsolideRapor.Base.Application;
using KonsolideRapor.Base.Manage;
using KonsolideRapor.Base.Repositories;
using KonsolideRapor.Common.Application;
using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Common.Application;
using Surat.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Business.Manage
{
    public class KonsolideRaporManager
    {
        #region Constructor

        public KonsolideRaporManager(IKonsolideRaporApplicationManager konsolideRaporApplicationManager)
        {
            this.konsolideRaporApplicationManager = konsolideRaporApplicationManager;
        }

        #endregion

        #region Private Members

        private IKonsolideRaporApplicationManager konsolideRaporApplicationManager;
        private KonsolideRaporApplicationContext applicationContext;
        private IFrameworkManager frameworkApplicationManager;
        private FrameworkContext frameworkContext;
        private ISecurityManager securityManager;
        private BankRepository bank;
        private PaymentCollectingRepository paymentCollecting;
        #endregion

        #region Public Members

        public IKonsolideRaporApplicationManager KonsolideRaporApplicationManager
        {
            get
            {
                return konsolideRaporApplicationManager;
            }
        }

        public KonsolideRaporApplicationContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (KonsolideRaporApplicationContext)this.KonsolideRaporApplicationManager.GetKonsolideRaporApplicationContext();

                return applicationContext;
            }
        }

        public KonsolideRaporContext Context
        {
            get
            {
                return this.ApplicationContext.KonsolideRapor;
            }
        }

        public IFrameworkManager Framework
        {
            get
            {
                if (frameworkApplicationManager == null)
                    frameworkApplicationManager = this.KonsolideRaporApplicationManager.GetFrameworkManager();

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


        public BankRepository Bank
        {
            get
            {
                if (bank == null)
                    bank = new BankRepository(this.ApplicationContext.KonsolideRapor);

                return bank;
            }
        }


        public PaymentCollectingRepository PaymentCollecting
        {
            get
            {
                if (paymentCollecting == null)
                    paymentCollecting = new PaymentCollectingRepository(this.ApplicationContext.KonsolideRapor);

                return paymentCollecting;
            }
        }

        #endregion

        #region Methods

        #region Bank Method

        public List<Bank> GetBankList()
        {
            try
            {
                return this.Bank.GetAll().ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetBankList", this.ApplicationContext.SystemId, exception);
            }
        }

        public List<Bank> GetActiveBankList()
        {
            try
            {
                return this.Bank.GetObjectsByParameters(m => m.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetBankList", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SaveBank(Bank bank)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                if (bank.Id == 0)
                {

                    this.Bank.Add(bank);
                }
                else
                {
                    Bank selectedBank = this.Bank.GetObjectByParameters(p => p.Id == bank.Id);

                    selectedBank.IsActive = bank.IsActive;
                    selectedBank.Code = bank.Code;
                    selectedBank.Name = bank.Name;
                    this.Bank.Update(selectedBank);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "SaveBank", this.ApplicationContext.SystemId, exception);
            }
        }

        public void DestroyBank(Bank bank)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();


                Bank selectedBank = this.Bank.GetObjectByParameters(p => p.Id == bank.Id);

                selectedBank.IsActive = false;
                this.Bank.Update(selectedBank);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "SaveBank", this.ApplicationContext.SystemId, exception);
            }
        }

        #endregion

        #region PaymentCollecting Method

        public List<PaymentCollecting> GetPaymentCollectingList()
        {
            try
            {
                return this.PaymentCollecting.GetAll().ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetPaymentCollectingList", this.ApplicationContext.SystemId, exception);
            }
        }


        public List<PaymentCollecting> GetActivePaymentCollectingList()
        {
            try
            {
                return this.PaymentCollecting.GetObjectsByParameters(m => m.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetActivePaymentCollectingList", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SavePaymentCollecting(PaymentCollecting paymentCollecting)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                if (paymentCollecting.Id == 0)
                {

                    this.PaymentCollecting.Add(paymentCollecting);
                }
                else
                {
                    PaymentCollecting selectedPayCol = this.PaymentCollecting.GetObjectByParameters(p => p.Id == paymentCollecting.Id);

                    selectedPayCol.IsActive = true;
                    selectedPayCol.Code = paymentCollecting.Code;
                    selectedPayCol.Name = paymentCollecting.Name;
                    selectedPayCol.IsCollection = paymentCollecting.IsCollection;
                    selectedPayCol.IsPayment = paymentCollecting.IsPayment;
                    this.PaymentCollecting.Update(selectedPayCol);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "savePaymentCollectings", this.ApplicationContext.SystemId, exception);
            }
        }

        public void DestroyPaymentCollecting(PaymentCollecting paymentCollecting)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();


                PaymentCollecting selectedPayCol = this.PaymentCollecting.GetObjectByParameters(p => p.Id == paymentCollecting.Id);

                selectedPayCol.IsActive = false;
                this.PaymentCollecting.Update(selectedPayCol);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "savePaymentCollectings", this.ApplicationContext.SystemId, exception);
            }
        }

        #endregion

        #endregion

    }
}
