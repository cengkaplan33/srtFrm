define(['kendo'],
    function (kendo) {
        var userPagesModel = new kendo.data.Model.define({
            id: "PageId",
            fields: {
                PageId: { type: "number", editable: false, nullable: true },
                PageName: { type: "string", validation: { required: true } },
                IsRoleEffect: { type: "boolean", editable: false },
                IsPageAccess: { type: "string" },
                IzinVer: { type: "number", title: "İzin Ver" },
                Yasakla: {type: "number", title: "Yasakla"}
            }
        });
        return userPagesModel;
    });