define(['kendo'],
    function (kendo) {
        var userModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: true },
                Name: { type: "string", title: "Ad Soyad",validation: { required: true } },
                UserName: { type: "string", title: "Kullanıcı Adı", validation: { required: true } },
                Password: { type: "string", title: "Şifre",validation: { required: true } },
                Notes: { type: "string", title: "Notlar" },
                LastPasswordChangedDate: { type: "date" },
                IsLocked: { type: "boolean", title: "Kilitli mi?" },
                IsActiveDirectoryUser: { type: "boolean", title: "ActiveDirectory kullanıcısı mı?" },
                IsExternalUser: { type: "boolean", title: "Dış kullanıcı mı?" },
                DefaultRole: { type: "number", title: "Varsayılan Rolü" },
                DefaultWorkgroup: { type: "number", title: "Varsayılan Çalışma Grubu" },
                InsertedByUser: { type: "number" },
                InsertedDate: { type: "date" },
                ChangedByUser: { type: "number" },
                ChangedDate: { type: "date" },
                IsActive: { type: "boolean", title: "Aktif mi?" },
                Roles: { type: "string" },
                Pages: { type: "string" },
                Actions: { type: "string" }
            }
        });
        return userModel;
    });