define(['kendo', 'userRolesModel', 'util'],
function (kendo, userRolesModel, util) {


    var userRolesDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/Users/GetUserRoles",
                dataType: "json"
            }
        },
        batch: false,
        serverPaging: false,
        serverSorting: false,
        serverFiltering: false,
        pageSize: 10,
        cache: false,
        schema: {
            model: userRolesModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return userRolesDatasource;

});