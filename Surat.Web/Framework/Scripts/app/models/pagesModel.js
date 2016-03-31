define(['kendo'],
    function (kendo) {
        var pagesModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false },
                SystemName: { type: "string", editable: false },
                ObjectTypePrefix: { type: "string", validation: { required: true } },
                PageName: { type: "string", validation: { required: true } },
                IsAccessControlRequired: { type: "boolean", title: "Erişim İzni Gerekiyor mu?" },
                IsVisibleInMenu: { type: "boolean", title: "Menüde Görünsün mü?" },
                InsertedByUserName: { type: "string", editable: false },
                InsertedDate: { type: "date", editable: false },
                ChangedByUserName: { type: "string", editable: false },
                ChangedDate: { type: "date", editable: false },
                IsActive: { type: "boolean",title:"Aktif mi?" }
            }
        });
        return pagesModel;
    });