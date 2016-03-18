define(['kendo', 'paymentCollectingModel'],
function (kendo, paymentCollectingModel) {


    var paymentCollectingDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                async: false,
                url: "/PaymentCollecting/GetPaymentCollectings",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/PaymentCollecting/Add",
                dataType: "json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                    }
                }
            },
            update: {

                type: "POST",
                url: "/PaymentCollecting/Update",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                    }
                }
            },
            destroy: {

                type: "POST",
                url: "/PaymentCollecting/Delete",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.warning(result.Result);
                    }
                }
            }
        },
        batch: false,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: false,
        pageSize: 10,
        cache: false,
        schema: {
            data: "data", // records are returned in the "data" field of the response
            total: "total", // total number of records is in the "total" field of the response
            errors: function (response) {
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: paymentCollectingModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return paymentCollectingDatasource;

});