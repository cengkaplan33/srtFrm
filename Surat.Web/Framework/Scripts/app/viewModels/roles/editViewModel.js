define(['rolesDatasource', 'rolesModel','rolePagesModel','rolePagesDatasource', 'util', 'router'],
    function (rolesDatasource, rolesModel,rolePagesModel,rolePagesDatasource, util, router) {
        var lastRolSelectedDataItem = null;
        var checkedIds = [];
        var pageObject = {};
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    sec: function (e) {
                        if (e.currentTarget.checked == true) {
                            for (var i = 0; i < checkedIds.length; i++) {
                                if (checkedIds[i].Id == e.data.Id)
                                {
                                    checkedIds[i].IsAccess = true;
                                }

                            }
                        }
                        else {
                            for (var i = 0; i < checkedIds.length; i++) {
                                if (checkedIds[i].Id == e.data.Id) {
                                    checkedIds[i].IsAccess = false;
                                }

                            }

                        }

                    },
                    saveRoles: function (s) {
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.Role.Id > 0) {
                                //viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                //viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                
                                                            
                                viewModel.Role.set("Pages",JSON.stringify(checkedIds));
                                rolesDatasource.sync();
                                rolesDatasource.filter({});
                                router.navigate('/Roles/index');
                            }
                            else {
                                viewModel.Role.set("Pages", JSON.stringify(checkedIds));
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

                    },
                    rolePagesDatasource: rolePagesDatasource
                });

                rolesDatasource.fetch(function () {
                    if (rolesDatasource.view().length > 0) {

                        var data = rolesDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("Role", rolesDatasource.at(i));

                                setBreadCrumb("#/Roles/Index", "Rol Tanımları");
                                rolePagesDatasource.options.transport.read.url = "/Roles/GetRolePages?roleId=" + data[i].Id;
                                rolePagesDatasource.read();
                               
                                break;
                            }
                            viewModel.set("Role", new rolesModel());
                        }

                    }

                });
                checkedIds = [];
                rolePagesDatasource.fetch(function () {
                    if (rolePagesDatasource.view().length > 0) {
                        var rolePageData = rolePagesDatasource.data();
                        for (var k = 0; k < rolePageData.length; k++) {
                            if (rolePageData[k].IsAccess == true) {

                                checkedIds.push({"Id":rolePageData[k].Id,"IsAccess":true});
                            }
                            else
                            {
                                checkedIds.push({ "Id": rolePageData[k].Id, "IsAccess": false });
                            }
                        }
                    }
                });
                viewModel.Role.Pages = checkedIds;
                return viewModel;
            },
        });

        return editViewModel;

    });