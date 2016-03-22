define(['kendo'],
    function (kendo) {
        var bankModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                Code: { type: "string", validation: { required: true } },
                Name: { type: "string", validation: { required: true } },
                IsActive: { type: "boolean", title: "Aktif mi?" }
            }
        });
        return bankModel;
    });