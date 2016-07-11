using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Globalization;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.Common.Application;
using Surat.Common.Cache;
using Surat.Common.Data;
using Surat.Common.Globalization;
using Surat.Common.Log;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Business.Globalization
{
    public class GlobalizationManager : IGlobalizationManager
    {
        #region Constructor

        public GlobalizationManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members

        private IFrameworkManager frameworkManager;
        private FrameworkContext applicationContext;
        private ITraceManager traceManager;
        private ICacheManager cacheManager;
        private List<ApplicationLanguage> applicationLanguages;
        private GlobalizationKeyRepository globalizationKey;
        private GlobalizationKeyValueRepository globalizationKeyValue;

        #endregion

        #region Public Members

        public IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
        }

        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.FrameworkManager.GetApplicationContext();

                return applicationContext;
            }
        }

        public GlobalizationContext Context
        {
            get
            {
                return this.ApplicationContext.Globalization;
            }
        }

        public List<ApplicationLanguage> ApplicationLanguages
        {
            get
            {
                if (applicationLanguages == null)
                    applicationLanguages = this.GetApplicationLanguages();

                return applicationLanguages;
            }
        }

        public ITraceManager Trace
        {
            get
            {
                if(traceManager == null)
                    traceManager = this.ApplicationContext.FrameworkManager.GetTraceManager();

                return traceManager;
            }
        }

        public ICacheManager Cache
        {
            get
            {
                if (cacheManager == null)
                    cacheManager = this.ApplicationContext.FrameworkManager.GetCacheManager();

                return cacheManager;
            }
        }        

        #endregion

        #region Repositories

        public GlobalizationKeyRepository GlobalizationKey
        {
            get
            {
                if (globalizationKey == null)
                    globalizationKey = new GlobalizationKeyRepository(this.ApplicationContext.Globalization);

                return globalizationKey;
            }
        }

        public GlobalizationKeyValueRepository GlobalizationKeyValueRepository
        {
            get
            {
                if (globalizationKeyValue == null)
                    globalizationKeyValue = new GlobalizationKeyValueRepository(this.ApplicationContext.Globalization);

                return globalizationKeyValue;
            }
        }

        #endregion

        #region IGlobalizationManager

        public string GetGlobalizationKeyValue(int systemId,string globalizationKey)
        {
            string globalizationKeyValue;

            try
            {
                globalizationKeyValue = this.Context.GetGlobalizationKeyValue(systemId,globalizationKey);
            }
            catch
            {
                globalizationKeyValue = globalizationKey; // Return key if not found or problem.
            }

            return globalizationKeyValue;
        }

        public List<GlobalizationKeyView> GetGlobalizationKeyValueList(int systemId,Culture culture)
        {
            List<GlobalizationKeyView> keyValueList;
            string cacheKeyName = Constants.CacheList.GlobalizationKeyValueList + systemId.ToString() + "-" + ((int)culture).ToString();

            keyValueList = (List<GlobalizationKeyView>)this.Cache.GetCachedObject(cacheKeyName);

            if (keyValueList == null)
            {
                keyValueList = this.GlobalizationKeyValueRepository.GetKeyValueListByCulture(systemId,(byte)culture);

                this.Cache.SetObjectInCache(cacheKeyName, keyValueList);
            }

            return keyValueList;
        }

        public void InsertGlobalizationKeyValues(string globalizationKey, int systemId,List<GlobalizationKeyValueView> keyValues)
        {
            SaveGlobalizationKeyValues(globalizationKey, systemId, keyValues);
            //Reset Context
            this.Context.GlobalizationKeyValueList = null;
            //Delete Cache
            foreach (GlobalizationKeyValueView keyValue in keyValues)
            {
                this.Cache.RemoveCachedObject(Constants.CacheList.GlobalizationKeyValueList + keyValue.CultureId.ToString());
            }  
            //Cache ve Context eklenen değerler ile yeniden yüklenecektir.
        }

        #endregion

        #region Methods

        private List<ApplicationLanguage> GetApplicationLanguages()
        {
            List<ApplicationLanguage> languages = new List<ApplicationLanguage>();
            ApplicationLanguage language;


            language = new ApplicationLanguage();
            language.CultureCode = 0;
            language.CultureName = "tr-TR";
            languages.Add(language);

            language = new ApplicationLanguage();
            language.CultureCode = 1;
            language.CultureName = "en-US";
            languages.Add(language);

            return languages;

        }

        private void SaveGlobalizationKeyValues(string globalizationKey, int systemId, List<GlobalizationKeyValueView> keyValues)
        {
            int initializedDBContextId;
            GlobalizationKeyValue newKeyValue;
            GlobalizationKey newKey;

            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                //Key
                newKey = new GlobalizationKey();
                newKey.Name = globalizationKey;
                newKey.SystemId = systemId;
                this.GlobalizationKey.Add(newKey);
                this.ApplicationContext.CommitDBChanges(initializedDBContextId);//ToDo : Parent Key almak için koyuldu. TransactionScope ile bir çözüm geliştirilmelidir.

                initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                foreach (GlobalizationKeyValueView keyValue in keyValues)
                {
                    newKeyValue = new GlobalizationKeyValue();
                    newKeyValue.GlobalizationKeyId = newKey.Id;
                    newKeyValue.CultureId = keyValue.CultureId;
                    newKeyValue.Content = keyValue.Value;

                    this.GlobalizationKeyValueRepository.Add(newKeyValue);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"SaveGlobalizationKeyValues", this.ApplicationContext.SystemId,exception);
            }
        }

        public string GetCurrentCultureName()
        {
            ApplicationLanguage language;
            string cultureName;

            language = this.ApplicationLanguages.Find(p => p.CultureCode == (byte)this.Context.CurrentCulture);

            if (language != null)
                cultureName = language.CultureName;
            else throw new InvalidTypeException(this.ApplicationContext,"Culture", this.ApplicationContext.SystemId,
                string.Format(this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.ExceptionType.ItemNotFound), this.Context.CurrentCulture.ToString()));

            return cultureName;
        }        

        public bool IsDefaultSystemCulture()
        {
            bool result = false;

            //if (this.Context.CurrentCulture == "tr-TR")
            //    result = true;

            return result;
        }        

        #endregion
    }
}
