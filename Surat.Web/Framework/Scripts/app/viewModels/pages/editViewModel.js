define(['pagesDatasource', 'pagesModel', 'util', 'router', 'systemsDatasource'],
    function (pagesDatasource, pagesModel, util, router, systemsDatasource) {
        var lastPageSelectedDataItem = null;
        var onClick = function (event, delegate) {
            event.preventDefault();
            $("#SystemId").kendoDropDownList({
                dataTextField: "name",
                dataValueField: "id",
                dataSource: systemsDatasource
            });
        };

        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    savePage: function (s) {
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.Page.Id > 0) {

                                pagesDatasource.sync();

                                pagesDatasource.filter({});
                                router.navigate('/Pages/index');
                            }
                            else {

                                pagesDatasource.add(viewModel.Page);
                                pagesDatasource.sync();
                                pagesDatasource.filter({});
                                router.navigate('/Pages/index');
                            }
                        }
                    },
                    cancel: function (s) {
                        pagesDatasource.filter({});
                        router.navigate('/Pages/index');
                    },
                    onRolesChange: function (arg) {
                        var grid = arg.sender;
                        lastPageSelectedDataItem = grid.dataItem(grid.select());

                    }
                });

                pagesDatasource.fetch(function () {
                    if (pagesDatasource.view().length > 0) {

                        var data = pagesDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("Page", pagesDatasource.at(i));

                                setBreadCrumb("#/Pages/Index", "Sayfalar");
                                break;
                            }
                            viewModel.set("Page", new pagesModel());
                        }

                    }

                });

                return viewModel;
            },
        });

        return editViewModel;

    });