define(['kendo', 'rolesModel', 'util'],
function (kendo, rolesModel, util) {


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
            total: "total",
            model: rolesModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return rolesDatasource;

});