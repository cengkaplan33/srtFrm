define(['kendo', 'odemeTalepModel', 'util'],
function (kendo, odemeTalepModel, util) {


    var odemeTalepDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                async: false,
                url: "/OdemeTalep/GetOdemeTalepleri",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/OdemeTalep/Add",
                dataType: "json",
                data: function (data) {
                    data.Tarih = kendo.toString(data.Tarih, "dd-MM-yyyy");
                    if (data.TL != null) {
                        data.TL = data.TL.toString().replace(".", ",");
                    }
                    if (data.EURO != null) {
                        data.EURO = data.EURO.toString().replace(".", ",");
                    }
                    if (data.USD != null) {
                        data.USD = data.USD.toString().replace(".", ",");
                    }
                    // repeat for all your date fields
                    return data;
                },
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                        odemeTalepDatasource.read();
                    }
                }
            },
            update: {
                type: "POST",
                url: "/OdemeTalep/Update",
                dataType: "Json",
                data: function (data) {
                    data.Tarih = kendo.toString(data.Tarih, "dd-MM-yyyy");
                    if (data.TL != null) {
                        data.TL = data.TL.toString().replace(".", ",");
                    }
                    if (data.EURO != null) {
                        data.EURO = data.EURO.toString().replace(".", ",");
                    }
                    if (data.USD != null) {
                        data.USD = data.USD.toString().replace(".", ",");
                    }
                    // repeat for all your date fields
                    return data;
                },
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                        odemeTalepDatasource.read();
                    }
                }
            },
            destroy: {

                type: "POST",
                url: "/OdemeTalep/Delete",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.warning(result.Result);
                        odemeTalepDatasource.read();
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
        aggregate: [
       { field: "TL", aggregate: "sum" },
       { field: "USD", aggregate: "sum" },
       { field: "EURO", aggregate: "sum" }
        ],
        columns: [
        { field: "TL", footerTemplate: "Total Count: #=sum#" },
        { field: "USD", footerTemplate: "Total Count: #=sum#" },
        { field: "EURO", footerTemplate: "Total Count: #=sum#" }
        ],

        schema: {
            data: "data", // records are returned in the "data" field of the response
            total: "total",
            model: odemeTalepModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return odemeTalepDatasource;

});