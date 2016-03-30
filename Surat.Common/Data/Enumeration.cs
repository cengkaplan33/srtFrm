using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Common.Data
{
    public enum FrameworkProviderType
    {
        Surat = 0,
        Serendip = 1
    }

    public enum AccessRight
    {
        Execute = 1,        
        Read = 2,
        Modify = 3,
        Share = 4, 
        Full = 9,       
    }

    public enum ExceptionLevel
    {
        Critical = 0,
        Error = 1,
        Information = 2
    }

    public enum DBRowState
    {
        Active = 0,
        InActive = 1
    }

    public enum Culture
    {
        Turkish = 0,
        English = 1
    }

    public enum TraceLevel
    {
        NoTrace = 0,
        Basic = 1,
        Detail = 2
    }

    public enum UserEventType
    {
        FormClick = 0,
        ToolbarClick = 1
    }

    public enum AccessibleItemDBObjectType
    {
        Page = 1,
        Action = 2,
        Right = 3,
        Service = 4,
        ServiceMethod = 5
    }

    public enum ParameterDBObjectType
    {
        System = 0,
        Page = 1
    }

    public enum ParameterOwnerDBObjectType
    {      
        Workgroup = 0,
        User = 1
    }

    public enum DelegateObjectType
    {
        User = 0,  
        Role = 1,
        CompanySite = 2,
        Service = 3
    }

    public enum DBEnvironmentType
    {
        Production = 0,
        Test = 1,
        Archive = 2
    }
    public enum ContextMode
    {
        StartWithNewContext = 0,
        ReuseActiveContext = 1
    }
    public enum PropertyType
    {
        Integer = 0,
        Decimal = 1,
        String = 2,
        DateTime = 3,
        ContentType = 4
    }
}
