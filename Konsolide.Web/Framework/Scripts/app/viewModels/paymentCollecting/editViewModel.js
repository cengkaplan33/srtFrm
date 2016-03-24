define(['paymentCollectingDatasource', 'paymentCollectingModel', 'util', 'router'],
    function (paymentCollectingDatasource, paymentCollectingModel, util, router) {
        var lastPayColSelectedDataItem = null;
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    savePaymentCollectings: function (s) {
                        //Validator kullanımı
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.PayCol.Id > 0) {
                                //viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                //viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                paymentCollectingDatasource.sync();

                                paymentCollectingDatasource.filter({});
                                router.navigate('/PaymentCollecting/index');
                            }
                            else {

                                paymentCollectingDatasource.add(viewModel.PayCol);
                                paymentCollectingDatasource.sync();
                                paymentCollectingDatasource.filter({});
                                router.navigate('/PaymentCollecting/index');
                            }
                        }
                    },
                    cancel: function (s) {
                        paymentCollectingDatasource.filter({});
                        router.navigate('/PaymentCollecting/index');
                    },
                    onPayColsChange: function (arg) {
                        var grid = arg.sender;
                        lastPayColSelectedDataItem = grid.dataItem(grid.select());

                        //$("#DefaultRole").val(lastRolSelectedDataItem.Id);

                    }
                });

                paymentCollectingDatasource.fetch(function () {
                    if (paymentCollectingDatasource.view().length > 0) {

                        var data = paymentCollectingDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("PayCol", paymentCollectingDatasource.at(i));

                                setBreadCrumb("#/PaymentCollecting/Index", "Ödeme Tahsilat Tanımları");
                                break;
                            }
                            viewModel.set("PayCol", new paymentCollectingModel());
                        }

                    }
                    else
                    {
                        viewModel.set("PayCol", new paymentCollectingModel());
                    }
                });

                return viewModel;
            },
        });

        return editViewModel;

    });