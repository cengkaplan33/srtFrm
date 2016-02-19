define(['kendo', 'userModel'],
function (kendo, userModel) {


    var userDatasource = new kendo.data.DataSource({
        autoSync:false,
        transport: {
            read: {
               
                url: "/Users/GetUsers",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/Users/Add",
                dataType: "json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                    }
                }
            },
            destroy: {
               
                type: "POST",
                url: "/Users/Delete",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.warning(result.Result);
                    }
                }
            },
            update: {
               
                type: "POST",
                url: "/Users/Update",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
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
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: userModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return userDatasource;

});