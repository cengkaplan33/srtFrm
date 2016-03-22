define(['kendo'],
    function (kendo) {
        var paymentCollectingModel = new kendo.data.Model.define({
            id: "Id",
            fields: {
                Id: { type: "number", editable: false },
                Code: { type: "string", validation: { required: true } },
                Name: { type: "string", validation: { required: true } },
                IsPayment: { type: "boolean", title: "Ödeme mi?" },
                IsCollection: { type: "boolean", title: "Tahsilat mi?" },
                IsActive: { type: "boolean", title: "Aktif mi?" }             
            }
        });
        return paymentCollectingModel;
    });