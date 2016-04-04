define(['kendo'],
    function (kendo) {
        var userRolesModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                Name: { type: "string", validation: { required: true } },
                ObjectTypeName: { type: "string", validation: { required: true } },
                IsAccess: { type: "boolean", title: "Yetki Durumu" }
            }
        });
        return userRolesModel;
    });