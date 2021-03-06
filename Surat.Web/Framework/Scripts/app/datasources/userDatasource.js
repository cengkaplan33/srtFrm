﻿define(['kendo', 'userModel','userChoosenWorkgroupDatasource','util'],
function (kendo, userModel, userChoosenWorkgroupDatasource, util) {


    var userDatasource = new kendo.data.DataSource({
      //  autoSync:false,
        transport: {
            read: {
                async: false,
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
                        userChoosenWorkgroupDatasource.read();
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
                        userChoosenWorkgroupDatasource.read();
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
                        userChoosenWorkgroupDatasource.read();
                        _notification.info(result.Result);
                    }
                }
            }

        },
        batch: false,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        pageSize: 20,
        cache: false,
        schema: {
            data: "data", // records are returned in the "data" field of the response
            total: "total",
            model: userModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return userDatasource;

});