define(['kendo'],
    function (kendo) {
        var userMasterDbVeritabanlariModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                DelegateDBObjectId: { type: "number", title: ""},
                DelegateDBObjectType: { type: "string", title: ""},
                SystemId: { type: "number", title: ""},
                UserName: { type: "string", title: "" },
                Password: { type: "string" },
                FirmaDonemTipi: { type: "number" },
                FirmaDonem: { type: "string" },
                FirmaDonemId: { type: "number"},
                DatabaseName: { type: "string"}
            }
        });
        return userMasterDbVeritabanlariModel;
    });