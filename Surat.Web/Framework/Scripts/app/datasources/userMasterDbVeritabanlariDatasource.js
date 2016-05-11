define(['kendo', 'userMasterDbVeritabanlariModel', 'util'],
function (kendo, userMasterDbVeritabanlariModel,  util) {


    var userMasterDbVeritabanlariDatasource = new kendo.data.DataSource({
        //  autoSync:false,
        transport: {
            read: {
                async: false,
                url: "/Users/GetUserMasterDbVeritabanlari",
                dataType: "json"
            },
            destroy: {
                type: "POST",
                url: "/Users/DeleteExternalUser",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        userMasterDbVeritabanlariDatasource.read();
                    }
                }
            },
            update: {

                type: "POST",
                url: "/Users/UpdateExternalUser",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                        userMasterDbVeritabanlariDatasource.read();
                    }
                }
            }
        },
        batch: false,
        serverPaging: false,
        serverSorting: false,
        serverFiltering: false,
        pageSize: 20,
        cache: false,
        schema: {
           
            model: userMasterDbVeritabanlariModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return userMasterDbVeritabanlariDatasource;

});