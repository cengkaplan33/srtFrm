define(['kendo', 'systemsModel','util'],
function (kendo, systemsModel,util) {
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
            model: systemsModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return systemsDatasource;

});