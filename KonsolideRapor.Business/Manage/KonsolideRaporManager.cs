﻿using KonsolideRapor.Base.Application;
using KonsolideRapor.Base.Manage;
using KonsolideRapor.Base.Model.Entities;
using KonsolideRapor.Base.Repositories;
using KonsolideRapor.Common.Application;
using KonsolideRapor.Web.Common.ViewModel;
using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Common.Application;
using Surat.Common.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private OdemeTalepRepository odemeTalep;
        private OdemeTalepDurumuRepository odemeTalepDurumu;
        private HazirDegerlerTablosuRepository hazirDegerTablosu;
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

        public OdemeTalepRepository OdemeTalep
        {
            get
            {
                if (odemeTalep == null)
                    odemeTalep = new OdemeTalepRepository(this.ApplicationContext.KonsolideRapor);

                return odemeTalep;
            }
        }

        public OdemeTalepDurumuRepository OdemeTalepDurumu
        {
            get
            {
                if (odemeTalepDurumu == null)
                    odemeTalepDurumu = new OdemeTalepDurumuRepository(this.ApplicationContext.KonsolideRapor);

                return odemeTalepDurumu;
            }
        }
        public HazirDegerlerTablosuRepository HazirDegerlerTablosu
        {
            get
            {
                if (hazirDegerTablosu == null)
                    hazirDegerTablosu = new HazirDegerlerTablosuRepository(this.ApplicationContext.KonsolideRapor);

                return hazirDegerTablosu;
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

        public List<PaymentCollecting> GetOdemeTurleri()
        {
            try
            {
                return this.PaymentCollecting.GetObjectsByParameters(m => m.IsActive == true & m.IsPayment == true).ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetActivePaymentCollectingList", this.ApplicationContext.SystemId, exception);
            }
        }

        public List<PaymentCollecting> GetTahsilatTurleri()
        {
            try
            {
                return this.PaymentCollecting.GetObjectsByParameters(m => m.IsActive == true & m.IsCollection == true).ToList();
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

        #region ÖdemeTalep Methods
        public List<OdemeTalepView> GetAktifOdemeTalepleri()
        {
            List<OdemeTalepView> odemeTalepleri;
            string query = @"select talep.Id,talep.Tarih,talep.PaymentCollectingId,payment.Name,talep.TL,talep.USD,talep.EURO,talep.WorkgroupId,talep.OdemeTalepDurumuId,durum.Durum,talep.Aciklama,talep.IsActive,talep.TalepTuru from
OdemeTaleps talep
join PaymentCollectings payment
on talep.PaymentCollectingId=payment.Id 
join OdemeTalepDurumus durum
on talep.OdemeTalepDurumuId=durum.Id
where talep.IsActive=1 and talep.TalepTuru='odeme' and talep.WorkgroupId=@WorkgroupId
";
            try
            {
                odemeTalepleri = this.Context.ApplicationContext.DBContext.Database.SqlQuery<OdemeTalepView>(query,new SqlParameter("@WorkgroupId",this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup)).ToList();
                return odemeTalepleri;
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetAktifOdemeTalepleri", this.ApplicationContext.SystemId, exception);
            }
        }
        public List<OdemeTalepView> GetAktifTahsilatTalepleri()
        {
            List<OdemeTalepView> odemeTalepleri;
            string query = @"select talep.Id,talep.Tarih,talep.PaymentCollectingId,payment.Name,talep.TL,talep.USD,talep.EURO,talep.WorkgroupId,talep.OdemeTalepDurumuId,durum.Durum,talep.Aciklama,talep.IsActive,talep.TalepTuru from
OdemeTaleps talep
join PaymentCollectings payment
on talep.PaymentCollectingId=payment.Id 
join OdemeTalepDurumus durum
on talep.OdemeTalepDurumuId=durum.Id
where talep.IsActive=1 and talep.TalepTuru='tahsilat'  and talep.WorkgroupId=@WorkgroupId
";
            try
            {
                odemeTalepleri = this.Context.ApplicationContext.DBContext.Database.SqlQuery<OdemeTalepView>(query, new SqlParameter("@WorkgroupId", this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup)).ToList();
                return odemeTalepleri;
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetAktifOdemeTalepleri", this.ApplicationContext.SystemId, exception);
            }
        }
        public void SaveOdemeTalep(OdemeTalep odemeTalep)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                if (odemeTalep.Id == 0)
                {
                    odemeTalep.WorkgroupId =(int)this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup;
                    odemeTalep.TalepTuru = "odeme";
                    this.OdemeTalep.Add(odemeTalep);
                }
                else
                {
                    OdemeTalep selectedOdemeTalep = this.OdemeTalep.GetObjectByParameters(p => p.Id == odemeTalep.Id);

                    selectedOdemeTalep.WorkgroupId = (int)this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup;
                    selectedOdemeTalep.Tarih = odemeTalep.Tarih;
                    selectedOdemeTalep.TL = odemeTalep.TL;
                    selectedOdemeTalep.USD = odemeTalep.USD;
                    selectedOdemeTalep.EURO = odemeTalep.EURO;
                    selectedOdemeTalep.PaymentCollectingId = odemeTalep.PaymentCollectingId;
                    selectedOdemeTalep.TalepTuru = "odeme";
                    selectedOdemeTalep.Aciklama = odemeTalep.Aciklama;
                    selectedOdemeTalep.OdemeTalepDurumuId = odemeTalep.OdemeTalepDurumuId;
                    this.OdemeTalep.Update(selectedOdemeTalep);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "SaveOdemeTalep", this.ApplicationContext.SystemId, exception);
            }
        }
        public void SaveTahsilatTalep(OdemeTalep odemeTalep)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                if (odemeTalep.Id == 0)
                {
                    odemeTalep.WorkgroupId = (int)this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup;
                    odemeTalep.TalepTuru = "tahsilat";
                    this.OdemeTalep.Add(odemeTalep);
                }
                else
                {
                    OdemeTalep selectedOdemeTalep = this.OdemeTalep.GetObjectByParameters(p => p.Id == odemeTalep.Id);

                    selectedOdemeTalep.WorkgroupId = (int)this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup;
                    selectedOdemeTalep.Tarih = odemeTalep.Tarih;
                    selectedOdemeTalep.TL = odemeTalep.TL;
                    selectedOdemeTalep.USD = odemeTalep.USD;
                    selectedOdemeTalep.EURO = odemeTalep.EURO;
                    selectedOdemeTalep.PaymentCollectingId = odemeTalep.PaymentCollectingId;
                    selectedOdemeTalep.OdemeTalepDurumuId = odemeTalep.OdemeTalepDurumuId;
                    selectedOdemeTalep.TalepTuru = "tahsilat";
                    selectedOdemeTalep.Aciklama = odemeTalep.Aciklama;
                    this.OdemeTalep.Update(selectedOdemeTalep);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "SaveTahsilatTalep", this.ApplicationContext.SystemId, exception);
            }
        }
        public void DestroyOdemeTalep(OdemeTalep odemeTalep)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();


                OdemeTalep selectedOdemeTalep = this.OdemeTalep.GetObjectByParameters(p => p.Id == odemeTalep.Id);

                selectedOdemeTalep.IsActive = false;
                this.OdemeTalep.Update(selectedOdemeTalep);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "DestroyOdemeTalep", this.ApplicationContext.SystemId, exception);
            }
        }
        #endregion

        #region Ödeme Talep Durumu Method

        public List<OdemeTalepDurumu> GetActiveDurumTanimlari()
        {
            try
            {
                return this.OdemeTalepDurumu.GetObjectsByParameters(m => m.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetActivePaymentCollectingList", this.ApplicationContext.SystemId, exception);
            }
        }

        public List<OdemeTalepDurumu> GetActiveOdemeDurumTanimlari()
        {
            try
            {
                return this.OdemeTalepDurumu.GetObjectsByParameters(m => m.IsActive == true & m.IsOdeme == true).ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetActiveOdemeDurumTanimlari", this.ApplicationContext.SystemId, exception);
            }
        }

        public List<OdemeTalepDurumu> GetActiveTahsilatDurumTanimlari()
        {
            try
            {
                return this.OdemeTalepDurumu.GetObjectsByParameters(m => m.IsActive == true & m.IsTahsilat == true).ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetActiveOdemeDurumTanimlari", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SaveDurumTanimi(OdemeTalepDurumu odemeTalepDurumu)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                if (odemeTalepDurumu.Id == 0)
                {

                    this.OdemeTalepDurumu.Add(odemeTalepDurumu);
                }
                else
                {
                    OdemeTalepDurumu selectedKonsolideState = this.OdemeTalepDurumu.GetObjectByParameters(p => p.Id == odemeTalepDurumu.Id);

                    selectedKonsolideState.IsActive = true;
                    selectedKonsolideState.Durum = odemeTalepDurumu.Durum;
                    selectedKonsolideState.IsBanka = odemeTalepDurumu.IsBanka;
                    selectedKonsolideState.IsOdeme = odemeTalepDurumu.IsOdeme;
                    selectedKonsolideState.IsTahsilat = odemeTalepDurumu.IsTahsilat;
                    this.OdemeTalepDurumu.Update(selectedKonsolideState);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "saveDurumTanimi", this.ApplicationContext.SystemId, exception);
            }
        }

        public void DestroyDurumTanimi(OdemeTalepDurumu odemeTalepDurumu)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();


                OdemeTalepDurumu selectedKonsolideState = this.OdemeTalepDurumu.GetObjectByParameters(p => p.Id == odemeTalepDurumu.Id);

                selectedKonsolideState.IsActive = false;
                this.OdemeTalepDurumu.Update(selectedKonsolideState);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "saveKonsolideState", this.ApplicationContext.SystemId, exception);
            }
        }

        #endregion

        #region Hazır Değerler Tablosu Methods
        public List<HazirDegerTablosu> GetHazirDegerlerList()
        {
            try
            {
                return this.HazirDegerlerTablosu.GetAll().Where(m=>m.IsActive==true&m.WorkGroupId==this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup).OrderBy(m=>m.Kod).ToList();
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "GetHazirDegerlerList", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SaveHazirDegerTablosu(HazirDegerTablosu hazirDegerTablosu)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                if (hazirDegerTablosu.Id == 0)
                {
                    hazirDegerTablosu.Tarih = DateTime.Now.Date;
                    hazirDegerTablosu.WorkGroupId = (int)this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup;
                    this.HazirDegerlerTablosu.Add(hazirDegerTablosu);
                }
                else
                {
                    HazirDegerTablosu selectedHazirDegerTablosu = this.HazirDegerlerTablosu.GetObjectByParameters(p => p.Id == hazirDegerTablosu.Id);

                   
                    selectedHazirDegerTablosu.HazirDeger =hazirDegerTablosu.HazirDeger;
                    selectedHazirDegerTablosu.EURO = hazirDegerTablosu.EURO;
                    selectedHazirDegerTablosu.OdemeTahsilat = hazirDegerTablosu.OdemeTahsilat;
                    selectedHazirDegerTablosu.TL = hazirDegerTablosu.TL;
                    selectedHazirDegerTablosu.USD = hazirDegerTablosu.USD;
                    selectedHazirDegerTablosu.Tarih = DateTime.Now.Date;
                    selectedHazirDegerTablosu.Tur= hazirDegerTablosu.Tur;
                    selectedHazirDegerTablosu.Kod = hazirDegerTablosu.Kod;
                    selectedHazirDegerTablosu.WorkGroupId =(int)this.Context.ApplicationContext.CurrentUser.DefaultWorkgroup;
                    this.HazirDegerlerTablosu.Update(selectedHazirDegerTablosu);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "SaveHazirDegerTablosu", this.ApplicationContext.SystemId, exception);
            }
        }

        public void DestroyHazirDegerTablosu(HazirDegerTablosu hazirDegerTablosu)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();


                HazirDegerTablosu selectedHazirDegerTablosu = this.HazirDegerlerTablosu.GetObjectByParameters(p => p.Id == hazirDegerTablosu.Id);

                selectedHazirDegerTablosu.IsActive = false;
                this.HazirDegerlerTablosu.Update(selectedHazirDegerTablosu);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.FrameworkContext, "DestroyBank", this.ApplicationContext.SystemId, exception);
            }
        }
        #endregion

        #endregion

    }
}
