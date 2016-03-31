define(['kendo', 'hazirDegerlerTablosuModel', 'util'],
function (kendo, hazirDegerlerTablosuModel, util) {


    var hazirDegerlerTablosuDatasource = new kendo.data.TreeListDataSource({
        transport: {

            read: {
                url: "/HazirDegerlerTablosu/GetHazirDegerler",
                type: "POST"
            },
            create: {
                type: "POST",
                url: "/HazirDegerlerTablosu/Add",
                dataType: "json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        try {
                            var tree = $("#hazirDegerlerTanimGrid").data("kendoTreeList");
                            tree.dataSource.read();
                        } catch (e) {
                            var tree = $("#hazirDegerlerGrid").data("kendoTreeList");
                            tree.dataSource.read();
                        }
                        _notification.info(result.Result);
                    }
                }
            },
            destroy: {

                type: "POST",
                url: "/HazirDegerlerTablosu/Delete",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        try {
                            var tree = $("#hazirDegerlerTanimGrid").data("kendoTreeList");
                            tree.dataSource.read();
                        } catch (e) {
                            var tree = $("#hazirDegerlerGrid").data("kendoTreeList");
                            tree.dataSource.read();
                        }
                        _notification.warning(result.Result);
                    }
                }
            },
            update: {

                type: "POST",
                url: "/HazirDegerlerTablosu/Update",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                        try {
                            var tree = $("#hazirDegerlerTanimGrid").data("kendoTreeList");
                            tree.dataSource.read();
                        } catch (e) {
                            var tree = $("#hazirDegerlerGrid").data("kendoTreeList");
                            tree.dataSource.read();
                        }
                       
                    }
                }
            }

        },
        batch: false,
       paging:false,
        cache: false,
        schema: {
            model: hazirDegerlerTablosuModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return hazirDegerlerTablosuDatasource;

});