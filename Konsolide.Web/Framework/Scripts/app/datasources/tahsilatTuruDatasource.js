define(['kendo', 'odemeTuruModel','util'],
function (kendo, odemeTuruModel,util) {


    var odemeTuruDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/PaymentCollecting/GetTahsilatTurleri",
                dataType: "json"
            }

        },
        schema: {
            // data: "data", // records are returned in the "data" field of the response
            // total: "total", // total number of records is in the "total" field of the response
            model: odemeTuruModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return odemeTuruDatasource;

});