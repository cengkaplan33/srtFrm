define(['kendo', 'bankModel','util'],
function (kendo, bankModel,util) {


    var bankDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                async: false,
                url: "/Bankalar/GetBanks",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/Bankalar/Add",
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
                url: "/Bankalar/Update",
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
                url: "/Bankalar/Delete",
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
            total: "total",
            model: bankModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return bankDatasource;

});