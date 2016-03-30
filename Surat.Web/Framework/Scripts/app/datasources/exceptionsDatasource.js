define(['kendo', 'exceptionsModel','util'],
function (kendo, exceptionsModel,util) {


    var exceptionsDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/Exceptions/GetExceptions",
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
            model: exceptionsModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return exceptionsDatasource;

});