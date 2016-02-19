define(['kendo', 'workgroupModel'],
function (kendo, workgroupModel) {
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
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        _notification.warning(result.Result);
                    }
                }
            },
            update: {

                type: "POST",
                url: "/Workgroups/Update",
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
            model:workgroupModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });
   
    return workgroupDatasource;

});