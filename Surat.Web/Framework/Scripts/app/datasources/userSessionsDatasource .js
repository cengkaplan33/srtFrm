define(['kendo', 'userSessionsModel'],
function (kendo, userSessionsModel) {


    var userSessionsDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/UserSessions/GetUserSessions",
                dataType: "json"
            }

        },
        batch: false,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        pageSize: 10,
        cache: false,
        schema: {
            data: "data", // records are returned in the "data" field of the response
            total: "total", // total number of records is in the "total" field of the response
            errors: function (response) {
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: userSessionsModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return userSessionsDatasource;

});