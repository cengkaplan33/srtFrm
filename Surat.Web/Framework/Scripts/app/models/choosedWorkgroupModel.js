define(['kendo'],
    function (kendo) {
        var choosedWorkgroupModel = new kendo.data.Model.define({

            id: "Id",
            fields: {
                Id: { type: "number", editable: false, nullable: false },
              
            }

        });
        return choosedWorkgroupModel;
    });