define(['kendo', 'userMasterDbVeritabanlariModel', 'util'],
function (kendo, userMasterDbVeritabanlariModel,  util) {


    var userMasterDbVeritabanlariDatasource = new kendo.data.DataSource({
        //  autoSync:false,
        transport: {
            read: {
                async: false,
                url: "/Users/KullaniciMasterDbVeritabanlari",
                dataType: "json"
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