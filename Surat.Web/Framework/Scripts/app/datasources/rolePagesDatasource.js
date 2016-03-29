define(['kendo', 'rolePagesModel'],
function (kendo, rolePagesModel) {


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
            errors: function (response) {
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: rolePagesModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return rolePagesDatasource;

});