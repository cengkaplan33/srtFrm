define(['kendo', 'odemeTalepModel'],
function (kendo, odemeTalepModel) {


    var tahsilatTalepDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                async: false,
                url: "/TahsilatTalep/GetTahsilatTalepleri",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/TahsilatTalep/Add",
                dataType: "json",
                data: function (data) {
                    data.Tarih = kendo.toString(data.Tarih, "dd-MM-yyyy");
                    // repeat for all your date fields
                    return data;
                },
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                        tahsilatTalepDatasource.read();
                    }
                }
            },
            update: {

                type: "POST",
                url: "/TahsilatTalep/Update",
                dataType: "Json",
                data: function (data) {
                    data.Tarih = kendo.toString(data.Tarih, "dd-MM-yyyy");
                    // repeat for all your date fields
                    return data;
                },
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                        tahsilatTalepDatasource.read();
                    }
                }
            },
            destroy: {

                type: "POST",
                url: "/TahsilatTalep/Delete",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.warning(result.Result);
                        tahsilatTalepDatasource.read();
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
            total: "total", // total number of records is in the "total" field of the response
            errors: function (response) {
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: odemeTalepModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return tahsilatTalepDatasource;

});