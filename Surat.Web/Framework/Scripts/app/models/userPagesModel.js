define(['kendo'],
    function (kendo) {
        var userPagesModel = new kendo.data.Model.define({
            id: "PageId",
            fields: {
                PageId: { type: "number", editable: false },
                PageName: { type: "string", editable: false  },
                IsRoleEffect: { type: "boolean", editable: false },
                IsPageAccess: { type: "boolean", editable: false },
                IzinVer: { type: "number", title: "İzin Ver" },
                Yasakla: {type: "number", title: "Yasakla"}
            }
        });
        return userPagesModel;
    });