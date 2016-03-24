define(['kendo'],
    function (kendo) {
        var odemeTalepModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                Tarih: { type: "date"},
                PaymentCollectingId: { type: "number" },
                Name:{type:"string"},
                OdemeTalepDurumuId: { type: "number" },
                Durum:{type:"string"},
                FirmaId: { type: "number" },
                TL: { type: "number" },
                USD: { type: "number" },
                EURO: { type: "number" },
                Aciklama: { type: "string" },
                Talepturu:{type:"string"},
                IsActive: { type: "boolean", title: "Aktif mi?" }
            }
        });
        return odemeTalepModel;
    });