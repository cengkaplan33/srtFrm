define(['kendo'],
    function (kendo) {
        var userPagesModel = new kendo.data.Model.define({
            id: "PageId",
            fields: {
                PageId: { type: "number", editable: false, nullable: true },
                PageName: { type: "string", validation: { required: true } },
                IsRoleEffect: { type: "boolean", editable: false },
                IsPageAccess: { type: "string" },
                IzinVer: { type: "boolean", title: "İzin Ver" },
                Yasakla: {type: "boolean", title: "Yasakla"}
            }
        });
        return userPagesModel;
    });