define(['kendo'],
    function (kendo) {
        var systemModel = new kendo.data.Node.define({

            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: false },
                parentId: { field: "ParentId", nullable: true },
                ObjectTypeName: { type: "string", validation: { required: true } },
                Name: { type: "string", validation: { required: true } },
                SmallImagePath: {type:"string", validation: { required: true } },
                LargeImagePath:{type:"string", validation:{required:true}},
                IsActive: { type: "boolean" }
            }

        });
        return systemModel;
    });