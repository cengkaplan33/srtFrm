define(['kendo', 'pagesModel'],
function (kendo, pagesModel) {


    var pagesDatasource = new kendo.data.DataSource({
        autoSync: false,
        transport: {
            read: {

                url: "/Pages/GetPages",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/Pages/Add",
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
                url: "/Pages/Update",
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
                url: "/Pages/Delete",
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
        serverFiltering: true,
        pageSize: 10,
        cache: false,
        schema: {
            data: "data", // records are returned in the "data" field of the response
            total: "total", // total number of records is in the "total" field of the response
            errors: function (response) {
                return response.error; 
            },
            model: pagesModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); 
        }
    });

    return pagesDatasource;

});