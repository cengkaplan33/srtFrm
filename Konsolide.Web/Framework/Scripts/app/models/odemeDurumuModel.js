define(['kendo'],
    function (kendo) {
        var odemeDurumuModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false },
                Durum: { type: "string", validation: { required: true } },
                IsOdeme: { type: "boolean", title: "Ödeme Ekranı?" },
                IsTahsilat: { type: "boolean", title: "Tahsilat Ekranı?" },
                IsBanka: { type: "boolean", title: "Banka Ekranı?" },
                IsKasa: { type: "boolean", title: "Kasa Ekranı" },
                IsCek: { type: "boolean", title: "Çek Ekranı" },
                IsActive: { type: "boolean", title: "Aktif?" }
            }
        });
        return odemeDurumuModel;
    });