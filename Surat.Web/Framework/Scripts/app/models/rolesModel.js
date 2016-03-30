define(['kendo'],
    function (kendo) {
        var rolesModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                Name: { type: "string", validation: { required: true } },
                ObjectTypeName: { type: "string", validation: { required: true } },
                IsActive: { type: "boolean", title: "Aktif mi?" },
                Pages:{type:"string"}
            }
        });
        return rolesModel;
    });