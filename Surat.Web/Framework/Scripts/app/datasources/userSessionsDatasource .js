define(['kendo', 'userSessionsModel','util'],
function (kendo, userSessionsModel,util) {


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
            total: "total",
            model: userSessionsModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return userSessionsDatasource;

});