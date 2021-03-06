﻿define(['kendo', 'workgroupModel','util'],
function (kendo, workgroupModel,util) {
    var workgroupDatasource = new kendo.data.TreeListDataSource({
        transport: {

            read: {
                url: "/Workgroups/GetWorkgroups",
                type: "POST"
            },
            create: {
                type: "POST",
                url: "/Workgroups/Add",
                dataType:"json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var tree = $("#workGroupGrid").data("kendoTreeList");
                        tree.dataSource.read();
                        tree.dataSource._destroyed = [];
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                    }
                }
            },
            update: {

                type: "POST",
                url: "/Workgroups/Update",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var tree = $("#workGroupGrid").data("kendoTreeList");
                        tree.dataSource.read();
                        tree.dataSource._destroyed = [];                   
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.info(result.Result);
                    }
                }
            },
            destroy: {

                type: "POST",
                url: "/Workgroups/Delete",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var tree = $("#workGroupGrid").data("kendoTreeList");
                        tree.dataSource.read();
                        tree.dataSource._destroyed = [];
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.warning(result.Result);
                    }
                }
            }
            

        },
        batch: false,
        schema: {
            model:workgroupModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });
   
    return workgroupDatasource;

});