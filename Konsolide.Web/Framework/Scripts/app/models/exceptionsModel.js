define(['kendo'],
    function (kendo) {
        var exceptionsModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false },
                LogDate: {type: "date", editable: false },
                SystemName: { type: "string", editable: false },
                ExceptionType: { type: "string", editable: false },
                Data: { type: "string", editable: false },
                ApplicationName: { type: "string", editable: false },
                ApplicationBaseType: { type: "string", editable: false },
                HostName: { type: "string", editable: false },
                Source: { type: "string", editable: false },
                Message: { type: "string", editable: false },
                StatusCode: { type: "number", editable: false },
                InsertUserName: { type: "string", editable: false }
            }
        });
        return exceptionsModel;
    });