define(['kendo', 'rolesModel'],
function (kendo, rolesModel) {


    var rolesDatasource = new kendo.data.DataSource({

        transport: {
            read: {
                async: false,
                url: "/Roles/GetRoles",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/Roles/Add",
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
                url: "/Roles/Delete",
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
                url: "/Roles/Update",
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
            model: rolesModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return rolesDatasource;

});