define(['kendo', 'odemeDurumuModel','util'],
function (kendo, odemeDurumuModel,util) {


    var OdemeEkraniDurumTanimiDatasource = new kendo.data.DataSource({
        transport: {
            read: {
               
                url: "/DurumTanimlari/GetOdemeEkraniDurumTanimlari",
                dataType: "json"
            }
        },
        schema: {
            model: odemeDurumuModel
        },
        error: function (e) {
            util.errorHandler(e);
        }
    });

    return OdemeEkraniDurumTanimiDatasource;

});