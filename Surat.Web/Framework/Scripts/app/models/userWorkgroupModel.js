define(['kendo'],
    function (kendo) {
        var userWorkgroupModel = new kendo.data.Node.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: false },
                parentId: { field: "ParentId", nullable: true },
                Name: { validation: { required: true } },
                ObjectTypeName: { validation: { required: true } },
                isCompanySite: { title: "Firma Sitesi mi?" },
                IsActive: { type: "boolean" },
                CompanyId:{type:"number"}
            }

        });
        return userWorkgroupModel;
    });