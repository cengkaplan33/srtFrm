define(['kendo'],
    function (kendo) {
        var userMasterDbVeritabanlariModel = new kendo.data.Model.define({
            id: "DatabaseName",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                DelegateDBObjectId: { type: "number", editable: false, },
                DelegateDBObjectType: { type: "string", editable: false, },
                SystemId: { type: "number", editable: false, },
                UserName: { type: "string", title: "", editable: true, validation: { required: { message: "Zorunlu Alan" } } },
                Password: { type: "string", editable: true, validation: { required: { message: "Zorunlu Alan" } } },
                FirmaDonemTipi: { type: "number", editable: false, },
                FirmaDonem: { type: "string", editable: false, },
                FirmaDonemId: { type: "number", editable: false, },
                DatabaseName: { type: "string", editable: false, },
                VarsayilanMi: { type: "boolean", editable: true }
            }
        });
        return userMasterDbVeritabanlariModel;
    });