define(['kendo'],
    function (kendo) {
        var hazirDegerlerTablosuModel = new  kendo.data.Node.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: false },
                parentId: { field: "ParentId", nullable: true },
                Kod: { type: "number", validation: { required: true } },
                HazirDeger: { type: "string", editable: false, validation: { required: "Hazır Değer alanı gereklidir." } },
                OdemeTahsilat: { type: "number", validation: { required: true } },
                Tur: { type: "string", validation: { required: true } },
                Tarih: { type: "date" },
                TL: { type: "number" },
                USD: { type: "number" },
                EURO: { type: "number" },
                WorkGroupId: { type: "number" },
                UserId: { type: "number" },
                IsActive: { type: "boolean", title: "Aktif mi?" },
                OdemeTalepDurumuId: { type: "number", title: "Talep Durumu" },
                Durum: { type: "string", title: "Talep Durumu" },
                BankId: { type: "number", title: "BankaId" },
                Name: { type: "string", title: "Banka" }
            }
        });
        return hazirDegerlerTablosuModel;
    });