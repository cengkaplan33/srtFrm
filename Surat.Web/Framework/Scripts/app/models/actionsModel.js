define(['kendo'],
    function (kendo) {
        var actionsModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false },
                SystemName: { type: "string", editable: false },
                Name: { type: "string", validation: { required: true } },
                Type: { type: "number", editable: false },
                Description: { type: "string", editable: false },
                InsertedByUserName: { type: "string", editable: false },
                InsertedDate: { type: "date", editable: false },
                ChangedByUserName: { type: "string", editable: false },
                ChangedDate: { type: "date", editable: false }
            }
        });
        return actionsModel;
    });