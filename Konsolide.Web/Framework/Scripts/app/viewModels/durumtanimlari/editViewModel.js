define(['odemeDurumuDatasource', 'odemeDurumuModel', 'util', 'router'],
    function (odemeDurumuDatasource, odemeDurumuModel, util, router) {
        var lastKonsolideStateSelectedDataItem = null;
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    saveDurumTanimi: function (s) {
                        //Validator kullanımı
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.Durum.Id > 0) {
                                //viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                //viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                odemeDurumuDatasource.sync();

                                odemeDurumuDatasource.filter({});
                                router.navigate('/DurumTanimlari/index');
                            }
                            else {

                                odemeDurumuDatasource.add(viewModel.Durum);
                                odemeDurumuDatasource.sync();
                                odemeDurumuDatasource.filter({});
                                router.navigate('/DurumTanimlari/index');
                            }
                        }
                    },
                    cancel: function (s) {
                        odemeDurumuDatasource.filter({});
                        router.navigate('/DurumTanimlari/index');
                    }
                });

                odemeDurumuDatasource.fetch(function () {
                    if (odemeDurumuDatasource.view().length > 0) {

                        var data = odemeDurumuDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("Durum", odemeDurumuDatasource.at(i));

                                setBreadCrumb("#/DurumTanimlari/Index", "Durum Tanımları");
                                break;
                            }
                            viewModel.set("Durum", new odemeDurumuModel());
                        }

                    }
                    else {
                        viewModel.set("Durum", new odemeDurumuModel());
                    }
                });

                return viewModel;
            },
        });

        return editViewModel;

    });