define(['kendo', 'odemeTuruModel'],
function (kendo, odemeTuruModel) {


    var odemeTuruDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/PaymentCollecting/GetOdemeTurleri",
                dataType: "json"
            }
       
        },
        schema: {
           // data: "data", // records are returned in the "data" field of the response
           // total: "total", // total number of records is in the "total" field of the response
            errors: function (response) {
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: odemeTuruModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return odemeTuruDatasource;

});