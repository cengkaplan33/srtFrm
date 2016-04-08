define(['kendo'],
   function (kendo) {
       var userActionsModel = new kendo.data.Model.define({
           id: "PageId",
           fields: {
               ActionId: { type: "number", editable: false },
               ActionName: { type: "string", editable: false },
               IsRoleEffect: { type: "boolean", editable: false },
               IsAccess: { type: "boolean", editable: false },
               IzinVer: { type: "number", title: "İzin Ver" },
               Yasakla: { type: "number", title: "Yasakla" }
           }
       });
       return userActionsModel;
   });