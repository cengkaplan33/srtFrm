define(['kendo', 'rolePagesModel','util'],
function (kendo, rolePagesModel,util) {


    var rolePagesDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/Roles/GetRolePages",
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
            model: rolePagesModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return rolePagesDatasource;

});