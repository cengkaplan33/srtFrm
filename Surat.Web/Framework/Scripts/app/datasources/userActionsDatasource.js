define(['kendo', 'userActionsModel', 'util'],
function (kendo, userActionsModel, util) {


    var userActionsDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/Users/GetUserActions",
                dataType: "json"
            }
        },
        batch: false,
        serverPaging: false,
        serverSorting: false,
        serverFiltering: false,
        pageSize: 20,
        cache: false,
        schema: {
            model: userActionsModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return userActionsDatasource;

});