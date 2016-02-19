using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Log
{
    public class UserEventFactory
    {
        #region Constructor

        #endregion

        #region Private Members
       
        #endregion

        #region Public Members

        #endregion

        #region Methods

        //public static BILUserEvent GetNewUserEvent(FrameworkApplicationContext applicationContext, FormManager formManager,UserEventType userEventType, string eventResource)
        //{
        //    BILUserEvent userEvent = new BILUserEvent();

        //    userEvent.UserId = applicationContext.CurrentUser.UserId;
        //    userEvent.EventName = userEventType.ToString();
        //    userEvent.EventDatetime = applicationContext.Time.GetCurrentDateTime();

        //    switch (userEventType)
        //    {
        //        case UserEventType.FormClick:
        //            {
        //                userEvent.DBInstanceId = formManager.GetFormByObjectTypeName(eventResource).Id;
        //            }
        //            break;
        //        case UserEventType.ToolbarClick:
        //            {
        //                userEvent.DBInstanceId = Convert.ToInt32(eventResource);
        //            }
        //            break;
        //        default:
        //            throw new InvalidTypeException("GetNewUserEvent", applicationContext.SystemName);
        //    }                  

        //    return userEvent;
        //}
       
        #endregion

    }
}
