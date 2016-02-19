using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surat.Base.Configuration;
using Surat.Common.Utilities;
using Surat.Base.Application;
using Surat.Base.Model;
using Surat.Base.Exceptions;
using Surat.Common.ViewModel;
using Surat.Base.Repositories;
using Surat.Base.Cache;
using Surat.Common.Data;

namespace Surat.Base.Configuration
{
    public class ConfigurationUtility
    {
        #region Private members

        private static List<WorkgroupView> allWorkGroups;        

        #endregion

        #region Methods

        public static T GetParameter<T>(FrameworkContext context, string parameterName)
        {            
            T result = default(T);
            ParameterValueView parameter = null;

            try
            {
                parameter = GetParameterInternal(context, parameterName);
            }
            catch (Exception exception)
            {                
                throw new ItemNotFoundException(context,"ConfigurationUtility.GetParameter", context.SystemId,
                    string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.ExceptionType.ItemNotFound), parameterName),
                    exception);
            }            

            if (parameter == null)
                throw new ItemNotFoundException(context,parameterName, context.SystemId,string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.ExceptionType.ItemNotFound), parameterName));
            else
            {
                try
                {
                    result = TypeConversionUtility.To<T>(parameter.ParameterValue);
                }
                catch (Exception exception)
                {
                    throw new InvalidInputException(context,parameterName, context.SystemId, exception);
                }
            }

            return result;
        }

        private static ParameterValueView GetParameterInternal(FrameworkContext context, string parameterName)
        {
            ParameterValueView parameter = null;
            int startUpWorkgroupId = 0;
            WorkgroupView startUpWorkgroup = null;
            List<ParameterValueView> systemParameterValues = context.FrameworkManager.GetSystemParameters();//ConfigurationContext üzerinden alınamaz. Çünkü o daha oluşturulmamış olabilir.

            if (context.IsCurrentUserAssigned) // Parametre okuma esnasında, kullanıcı belli mi?
            {
                // Kullanıcı seviyesinde parametre alınır.
                parameter = systemParameterValues.Where(p => p.ParameterTypeName == parameterName && p.ParameterOwnerDBObjectType == Common.Data.ParameterOwnerDBObjectType.User && p.ParameterOwnerDBObjectId == context.CurrentUser.UserId).FirstOrDefault();

                if (parameter != null)
                    return parameter;

                startUpWorkgroupId = context.CurrentUser.DefaultWorkgroup.Value;
            }            
            
            //Default workgrouptan başlayarak, yukarı doğru tarama gerçekleştirilecek. En sonda, Root (system) parametrelerine ulaşılacaktır.
            //Parametre işlemleri ilk aşama olduğu için, direkt erişim yapıldı. Framework alt yapısı (Context ve diğer Context ler) yeni hazırlanıyor. 

            List<WorkgroupView> allWorkgroups = (List<WorkgroupView>)CacheUtility.GetCachedObject(Constants.CacheList.WorkgroupList);
            if (allWorkgroups == null)
            {
                using (WorkgroupRepository workgroupRepository = new WorkgroupRepository(context.DBContext))
                {
                    allWorkGroups = workgroupRepository.GetAllActiveWorkGroups();
                    CacheUtility.SetObjectInCache(Constants.CacheList.WorkgroupList, allWorkGroups);
                }
            }

            if (startUpWorkgroupId == 0) //Başlangıç workgroup bulunamadığı için, RootWorkgroup bulunmalı ve set edilmelidir.
                startUpWorkgroup = allWorkGroups.Where(p => p.ParentWorkgroupId == null).FirstOrDefault(); //RootWorkgroup
            else startUpWorkgroup = allWorkGroups.Where(p => p.WorkgroupId == startUpWorkgroupId).FirstOrDefault();

            if (startUpWorkgroup == null)
                throw new RecordNotFoundException(context,"GetParameter.Workgroup", context.SystemId,
                    string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId,Constants.ExceptionType.RecordNotFound), startUpWorkgroupId));

            parameter = ProcessWorkgroup(context, parameterName, startUpWorkgroup, allWorkGroups, systemParameterValues);

            return parameter;
        }

        private static ParameterValueView ProcessWorkgroup(FrameworkContext context, string parameterName, WorkgroupView workgroup, List<WorkgroupView> allWorkgroups, List<ParameterValueView> systemParameterValues)
        {
            ParameterValueView parameter = null;

            parameter = systemParameterValues.Where(p => p.ParameterTypeName == parameterName && p.ParameterOwnerDBObjectType == Common.Data.ParameterOwnerDBObjectType.Workgroup && p.ParameterOwnerDBObjectId == workgroup.WorkgroupId).FirstOrDefault();

            if (parameter == null)
            {
                WorkgroupView parentWorkgroup = allWorkgroups.Where(p => p.WorkgroupId == workgroup.ParentWorkgroupId).FirstOrDefault();

                if (parentWorkgroup != null)
                    parameter = ProcessWorkgroup(context, parameterName, parentWorkgroup, allWorkgroups,systemParameterValues);
                else throw new RecordNotFoundException(context,"ProcessWorkgroup.ParentworkGroup", context.SystemId,
                    string.Format(context.Globalization.GetGlobalizationKeyValue(context.SystemId, Constants.ExceptionType.RecordNotFound), workgroup.ParentWorkgroupId));
            }

            return parameter;
        }

        #endregion
    }
}
