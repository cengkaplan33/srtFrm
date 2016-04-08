define(['kendo', 'hazirDegerlerTablosuModel', 'util','router'],
function (kendo, hazirDegerlerTablosuModel, util,router) {


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
              
            },
            destroy: {

                type: "POST",
                url: "/HazirDegerlerTablosu/Delete",
                dataType: "Json",
             
            },
            update: {

                type: "POST",
                url: "/HazirDegerlerTablosu/Update",
                dataType: "Json",
                complete: function (jqXhr, textStatus) {
                    if (textStatus = "success") {
                        var result = jQuery.parseJSON(jqXhr.responseText);
                      
                        _notification.warning(result.Result);
                       window.location.href='/HazirDegerlerTablosu/index';
                    }
                }
            }

        },
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