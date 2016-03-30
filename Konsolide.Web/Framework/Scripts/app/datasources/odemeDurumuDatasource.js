define(['kendo', 'odemeDurumuModel','util'],
function (kendo, odemeDurumuModel,util) {


    var odemeDurumuDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                async: false,
                url: "/DurumTanimlari/GetDurumTanimlari",
                dataType: "json"
            },
            create: {
                type: "POST",
                url: "/DurumTanimlari/Add",
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
                url: "/DurumTanimlari/Update",
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
                url: "/DurumTanimlari/Delete",
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
            model: odemeDurumuModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return odemeDurumuDatasource;

});