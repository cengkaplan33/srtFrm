define(['kendo', 'hazirDegerlerTablosuModel', 'util'],
function (kendo, hazirDegerlerTablosuModel, util) {


    var hazirDegerlerTablosuTanimDatasource = new kendo.data.TreeListDataSource({
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
                        var tree = $("#hazirDegerlerTanimGrid").data("kendoTreeList");
                        tree.dataSource.read();
                        $("#hazirDegerlerTanimGrid").data("kendoTreeList").dataSource = tree.dataSource;
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
                        var tree = $("#hazirDegerlerTanimGrid").data("kendoTreeList");
                        tree.dataSource.read();
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
                        var tree = $("#hazirDegerlerTanimGrid").data("kendoTreeList");
                        tree.dataSource.read();
                    }
                }
            }

        },
        batch: false,
        cache: false,
        paging:false,
        schema: {
            model: hazirDegerlerTablosuModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return hazirDegerlerTablosuTanimDatasource;

});