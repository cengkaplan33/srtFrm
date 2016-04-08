﻿define(['kendo', 'workgroupModel', 'util'],
function (kendo, workgroupModel, util) {
    var workgroupDatasource = new kendo.data.TreeListDataSource({
        transport: {

            read: {
                url: "/Workgroups/GetWorkgroups",
                type: "POST"
            }
        },
        batch: false,
        cache: false,
        expanded: true,
        schema: {
            model: workgroupModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return workgroupDatasource;

});