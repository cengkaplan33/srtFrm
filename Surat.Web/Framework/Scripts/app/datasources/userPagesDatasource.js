define(['kendo', 'userPagesModel', 'util'],
function (kendo, userPagesModel, util) {


    var userPagesDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/Users/GetUserPages",
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
            model: userPagesModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return userPagesDatasource;

});