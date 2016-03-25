define(['kendo'],
    function (kendo) {
        var userSessionsModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false },
                UserName: { type: "string", editable: false },
                SessionStart: { type: "date", editable: false },
                SessionEnd: { type: "date", editable: false },
                IP: { type: "string", editable: false },
            }
        });
        return userSessionsModel;
    });