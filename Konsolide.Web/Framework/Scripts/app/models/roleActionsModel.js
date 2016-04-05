define(['kendo'],
    function (kendo) {
        var roleActionsModel = new kendo.data.Model.define({
                id: "ActionId",
                fields: {
                ActionId: { type: "number", editable: false, nullable: true },
                IsAccessible: { type: "number" },
                RelationGroupId: { type: "number" },
                AccessibleItemId: { type: "number" },
                ActionName: { type: "string"}
            }
        });
        return roleActionsModel;
    });