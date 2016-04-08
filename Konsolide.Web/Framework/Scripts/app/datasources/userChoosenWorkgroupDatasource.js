define(['kendo', 'choosedWorkgroupModel', 'util'],
function (kendo, choosedWorkgroupModel, util) {
    var workgroupDatasource = new kendo.data.DataSource({
        transport: {

            read: {
                url: "/Users/GetChoosedWorkgroupId",
                type: "POST"
            }
        },
        batch: false,
        cache: false,
        expanded: true,
        schema: {
            model: choosedWorkgroupModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return workgroupDatasource;

});