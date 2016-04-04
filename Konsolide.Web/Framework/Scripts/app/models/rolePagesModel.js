define(['kendo'],
    function (kendo) {
        var rolesModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                SystemId: { type: "number"},
                Name: { type: "string", validation: { required: true } },
                ObjectTypePrefix: { type: "string", validation: { required: true } },
                ObjectTypeName: { type: "string", validation: { required: true } },
                BigImagePath: { type: "string", validation: { required: true } },
                SmallImagePath: { type: "string", validation: { required: true } },
                IsAccess: { type: "boolean", title: "Yetki Durumu" }
            }
        });
        return rolesModel;
    });