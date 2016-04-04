
define(['userDatasource', 'userModel', 'userRolesModel', 'userRolesDatasource', 'util', 'router'],
    function (userDatasource, userModel, userRolesModel, userRolesDatasource, util, router) {
        var lastSelectedDataItem = null;
        var lastRolSelectedDataItem = null;
        var lastWorkgroupSelectedDataItem = null;
        var checkedRoles = [];
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    sec: function (e) {
                        if (e.currentTarget.checked == true) {
                            for (var i = 0; i < checkedRoles.length; i++) {
                                if (checkedRoles[i].Id == e.data.Id) {
                                    checkedRoles[i].IsAccess = true;
                                }

                            }
                        }
                        else {
                            for (var i = 0; i < checkedRoles.length; i++) {
                                if (checkedRoles[i].Id == e.data.Id) {
                                    checkedRoles[i].IsAccess = false;
                                }

                            }

                        }

                    },
                    saveUser: function (s) {
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.User.Id > 0) {
                                // viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                // viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                viewModel.User.set("Roles", JSON.stringify(checkedRoles));
                                userDatasource.sync();
                                userDatasource.filter({});
                                router.navigate('/Users/index');
                            }
                            else {
                                //  viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                // viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                viewModel.User.set("Roles", JSON.stringify(checkedRoles));
                                userDatasource.add(viewModel.User);
                                userDatasource.sync();
                                userDatasource.filter({});
                                router.navigate('/Users/index');
                            }
                        }
                    },
                    cancel: function (s) {
                        userDatasource.filter({});
                        router.navigate('/Users/index');
                    },
                    //onRolesChange:function(arg)
                    //{
                    //    var grid = arg.sender;
                    //    lastRolSelectedDataItem = grid.dataItem(grid.select());

                    //    //$("#DefaultRole").val(lastRolSelectedDataItem.Id);

                    //},
                    //onWorkgroupChange: function (arg) {
                    //    var grid = arg.sender;
                    //    lastWorkgroupSelectedDataItem = grid.dataItem(grid.select());

                    //  //  $("#DefaultWorkgroup").val(lastWorkgroupSelectedDataItem.Id);
                    //},
                    //mysource:workgroupDatasource ,
                    userRolesDatasource: userRolesDatasource,
                });

                userDatasource.fetch(function () {
                    if (userDatasource.view().length > 0) {

                        var data = userDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("User", userDatasource.at(i));

                                setBreadCrumb("#/Users/Index", "Kullanıcı Listesi");
                                userRolesDatasource.options.transport.read.url = "/Users/GetUserRoles?userId=" + data[i].Id;
                                userRolesDatasource.read();
                                break;
                            }
                            viewModel.set("User", new userModel());
                        }

                    }
                    else {
                        viewModel.set("User", new userModel());
                    }

                });
                checkedRoles = [];
                userRolesDatasource.fetch(function () {
                    if (userRolesDatasource.view().length > 0) {
                        var userRoleData = userRolesDatasource.data();
                        for (var k = 0; k < userRoleData.length; k++) {
                            if (userRoleData[k].IsAccess == true) {

                                checkedRoles.push({ "Id": userRoleData[k].Id, "IsAccess": true });
                            }
                            else {
                                checkedRoles.push({ "Id": userRoleData[k].Id, "IsAccess": false });
                            }
                        }
                    }
                });

                viewModel.User.Roles = checkedRoles;
                return viewModel;
            },
        });

        return editViewModel;

    });