define(['kendo', 'workgroupModel', 'util'],
function (kendo, workgroupModel, util) {
    var workgroupDatasource = new kendo.data.TreeListDataSource({
        transport: {

            read: {
                url: "/Workgroups/GetUserWorkgroups",
                type: "POST"
            }
        },
        batch: false,
        schema: {
            model: workgroupModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return workgroupDatasource;

});