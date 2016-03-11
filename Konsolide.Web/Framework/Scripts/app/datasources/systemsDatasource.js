define(['kendo', 'systemsModel'],
function (kendo, systemsModel) {
    var systemsDatasource = new kendo.data.TreeListDataSource({
        async:false,
        transport: {

            read: {
                url: "/Systems/GetSystems",
                type: "POST"
            },
            create: {
                type: "POST",
                url: "/Systems/Add",
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
                url: "/Systems/Delete",
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
                url: "/Systems/Update",
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
        schema: {
            errors: function (response) {
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: systemsModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return systemsDatasource;

});