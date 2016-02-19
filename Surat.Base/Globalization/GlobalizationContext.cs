using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surat.Base.Application;
using Surat.Base.Model;
using Surat.Common.ViewModel;
using Surat.Base.Repositories;
using Surat.Common.Data;
using Surat.Common.Application;
using Surat.Common.Log;
using Surat.Common.Globalization;
using Surat.Base.Model.Entities;

namespace Surat.Base.Globalization
{
    public class GlobalizationContext : IDisposable
    {
        #region Constructor

        public GlobalizationContext(IFrameworkManager frameworkManager)
        { 
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members        

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private Culture currentCulture;
        private Culture applicationCulture;
        private Dictionary<int,Dictionary<byte, List<GlobalizationKeyView>>> globalizationKeyValueList;

        #endregion

        #region Public Members

        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.FrameworkManager.GetApplicationContext();

                return applicationContext;
            }
        }

        public ITraceManager Trace
        {
            get
            {
                return this.FrameworkManager.GetTraceManager();
            }
        }

        public IGlobalizationManager Globalization
        {
            get
            {
                return this.FrameworkManager.GetGlobalizationManager();
            }
        }

        public IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
        }        

        public Culture ApplicationCulture
        {
            get
            {
                return applicationCulture;
            }
            set { applicationCulture = value; }
        }

        public Culture CurrentCulture
        {
            get
            {
                return currentCulture;
            }
            set
            {
                currentCulture = value;
            }
        }

        public Dictionary<int,Dictionary<byte, List<GlobalizationKeyView>>> GlobalizationKeyValueList //System bazında ve Dillere göre değerler
        {
            get
            {
                if (globalizationKeyValueList == null)
                    globalizationKeyValueList = new Dictionary<int,Dictionary<byte, List<GlobalizationKeyView>>>();                

                return globalizationKeyValueList;
            }
            set
            {
                globalizationKeyValueList = value;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

        #region Methods

        public string GetGlobalizationKeyValue(int systemId,string globalizationKey)
        {
            string[] globalizationKeyArray = null;
            string globalizationKeyValue = string.Empty;
            GlobalizationKeyView selectedGlobalizationKey;
            Dictionary<byte,List<GlobalizationKeyView>> systemKeyValueList;
            List<GlobalizationKeyView> cultureKeyValueList;

            if (!this.GlobalizationKeyValueList.ContainsKey(systemId))
            {
                systemKeyValueList = new Dictionary<byte, List<GlobalizationKeyView>>();
                cultureKeyValueList = this.Globalization.GetGlobalizationKeyValueList(systemId, this.CurrentCulture);
                systemKeyValueList.Add((byte)this.CurrentCulture, cultureKeyValueList);
                this.globalizationKeyValueList.Add(systemId, systemKeyValueList);
            }
            else //System var. Culture a göre kontrol yapılacak.
            {
                systemKeyValueList = this.globalizationKeyValueList[systemId];
                if (!systemKeyValueList.ContainsKey((byte)this.CurrentCulture))
                {
                    cultureKeyValueList = this.Globalization.GetGlobalizationKeyValueList(systemId, this.CurrentCulture);
                    systemKeyValueList.Add((byte)this.CurrentCulture, cultureKeyValueList);
                }
                else
                {
                    cultureKeyValueList = systemKeyValueList[(byte)(this.CurrentCulture)];
                }
            }

            try
            {
                globalizationKeyArray = ParseGlobalizationKey(globalizationKey);// 0 : Key 1 : Turkish 2 :English 
                selectedGlobalizationKey = cultureKeyValueList.Where(p => p.SystemId == systemId && p.Key == globalizationKeyArray[0]).FirstOrDefault();
                if (selectedGlobalizationKey != null)
                    globalizationKeyValue = selectedGlobalizationKey.Value;
                else
                {
                    this.Globalization.InsertGlobalizationKeyValues(globalizationKeyArray[0], systemId, PrepareGlobalizationKeyValues(globalizationKeyArray));
                    globalizationKeyValue = SelectKeyValue(globalizationKeyArray);
                }
            }
            catch
            {
                globalizationKeyValue = SelectKeyValue(globalizationKeyArray);
            }           

            return globalizationKeyValue;
        }  
      
        private string[] ParseGlobalizationKey(string globalizationKey)
        {
            string[] key;
            
            key = globalizationKey.Split('|');

            return key;
        }

        private string SelectKeyValue(string[] globalizationKeyArray)
        {
            string keyValue = globalizationKeyArray[0];

            if (this.CurrentCulture == Culture.Turkish)
            {
                if (globalizationKeyArray.Count() > 0)
                    keyValue = globalizationKeyArray[1]; // 1: Turkish
            }
            else if (this.CurrentCulture == Culture.English)
            {
                if (globalizationKeyArray.Count() > 1)
                    keyValue = globalizationKeyArray[2]; // 2: English
            }

            return keyValue;
        }

        private List<GlobalizationKeyValueView> PrepareGlobalizationKeyValues(string[] globalizationKeyArray)
        {
            List<GlobalizationKeyValueView> keyValueList = new List<GlobalizationKeyValueView>();

            GlobalizationKeyValueView turkishKeyValue = new GlobalizationKeyValueView();
            turkishKeyValue.CultureId = (byte)Culture.Turkish;
            turkishKeyValue.Value = globalizationKeyArray[1];
            keyValueList.Add(turkishKeyValue);

            GlobalizationKeyValueView englishKeyValue = new GlobalizationKeyValueView();
            englishKeyValue.CultureId = (byte)Culture.English;
            englishKeyValue.Value = globalizationKeyArray[2];
            keyValueList.Add(englishKeyValue);

            return keyValueList;
        }

        #endregion
    }
}
