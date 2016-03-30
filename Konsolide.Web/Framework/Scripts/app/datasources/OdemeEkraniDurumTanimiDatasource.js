define(['kendo', 'odemeDurumuModel'],
function (kendo, odemeDurumuModel) {


    var OdemeEkraniDurumTanimiDatasource = new kendo.data.DataSource({
        transport: {
            read: {
               
                url: "/DurumTanimlari/GetOdemeEkraniDurumTanimlari",
                dataType: "json"
            }
        },
        schema: {
        
            errors: function (response) {
                return response.error; // twitter's response is { "error": "Invalid query" }
            },
            model: odemeDurumuModel
        },
        error: function (e) {

            _notification.error(e.xhr.responseJSON.Result); // displays "Invalid query"
        }
    });

    return OdemeEkraniDurumTanimiDatasource;

});