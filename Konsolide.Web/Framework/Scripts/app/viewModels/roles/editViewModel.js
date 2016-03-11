define(['rolesDatasource', 'rolesModel', 'util', 'router'],
    function (rolesDatasource, rolesModel, util, router) {
        var lastRolSelectedDataItem = null;
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    saveRoles: function (s) {
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.Role.Id > 0) {
                                //viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                //viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                rolesDatasource.sync();

                                rolesDatasource.filter({});
                                router.navigate('/Roles/index');
                            }
                            else {

                                rolesDatasource.add(viewModel.Role);
                                rolesDatasource.sync();
                                rolesDatasource.filter({});
                                router.navigate('/Roles/index');
                            }
                        }
                    },
                    cancel: function (s) {
                        rolesDatasource.filter({});
                        router.navigate('/Roles/index');
                    },
                    onRolesChange: function (arg) {
                        var grid = arg.sender;
                        lastRolSelectedDataItem = grid.dataItem(grid.select());

                        //$("#DefaultRole").val(lastRolSelectedDataItem.Id);

                    }
                });

                rolesDatasource.fetch(function () {
                    if (rolesDatasource.view().length > 0) {

                        var data = rolesDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("Role", rolesDatasource.at(i));

                                setBreadCrumb("#/Roles/Index", "Rol Tanımları");
                                break;
                            }
                            viewModel.set("Role", new rolesModel());
                        }

                    }

                });

                return viewModel;
            },
        });

        return editViewModel;

    });