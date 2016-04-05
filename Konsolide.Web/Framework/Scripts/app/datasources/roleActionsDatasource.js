define(['kendo', 'roleActionsModel', 'util'],
function (kendo, roleActionsModel, util) {


    var roleActionsDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/Roles/GetRoleActions",
                dataType: "json"
            }
        },
        batch: false,
        serverPaging: false,
        serverSorting: false,
        serverFiltering: false,
        pageSize: 25,
        cache: false,
        schema: {
            model: roleActionsModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return roleActionsDatasource;

});