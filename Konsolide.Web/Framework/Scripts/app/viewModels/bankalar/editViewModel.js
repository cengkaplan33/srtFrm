define(['bankDatasource', 'bankModel', 'util', 'router'],
    function (bankDatasource, bankModel, util, router) {
        var lastBankSelectedDataItem = null;
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    saveBanks: function (s) {
                        //Validator kullanımı
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.Bank.Id > 0) {
                                //viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                //viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                bankDatasource.sync();

                                bankDatasource.filter({});
                                router.navigate('/Bankalar/index');
                            }
                            else {

                                bankDatasource.add(viewModel.Bank);
                                bankDatasource.sync();
                                bankDatasource.filter({});
                                router.navigate('/Bankalar/index');
                            }
                        }
                    },
                    cancel: function (s) {
                        bankDatasource.filter({});
                        router.navigate('/Bankalar/index');
                    },
                    onBanksChange: function (arg) {
                        var grid = arg.sender;
                        lastBankSelectedDataItem = grid.dataItem(grid.select());

                        //$("#DefaultRole").val(lastRolSelectedDataItem.Id);

                    }
                });

                bankDatasource.fetch(function () {
                    if (bankDatasource.view().length > 0) {

                        var data = bankDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("Bank", bankDatasource.at(i));

                                setBreadCrumb("#/Bankalar/Index", "Banka Tanımları");
                                break;
                            }
                            viewModel.set("Bank", new bankModel());
                        }

                    }

                });

                return viewModel;
            },
        });

        return editViewModel;

    });