
define(['userDatasource', 'userModel', 'userRolesModel', 'userRolesDatasource', 'userWorkGroupDataSource', 'userChoosenWorkgroupDatasource', 'util', 'router'],
    function (userDatasource, userModel, userRolesModel, userRolesDatasource, userWorkGroupDataSource,userChoosenWorkgroupDatasource, util, router) {
        if (util.getId() != 0) {
            userWorkGroupDataSource.options.transport.read.url = "/Users/GetUserWorkgroups?userId=" + util.getId();
            userChoosenWorkgroupDatasource.options.transport.read.url = "/Users/GetChoosedWorkgroupId?userId=" + util.getId();
        }
        else {
            userWorkGroupDataSource.options.transport.read.url = "/Users/GetUserWorkgroupsWithCurentUsers";

        }

       
        var lastSelectedDataItem = null;
        var lastRolSelectedDataItem = null;
        var lastWorkgroupSelectedDataItem = null;
        var checkedRoles = [];
        var checkedWorkgroups = [];
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
                            try {
                                viewModel.User.set("Roles", JSON.stringify(checkedRoles));
                                viewModel.User.set("WorkGroupId", JSON.stringify(checkedWorkgroups));
                                if (!(viewModel.User.Id > 0)) {
                                    userDatasource.add(viewModel.User);
                                }
                               
                                userDatasource.sync();
                            } catch (e) {

                            }
                            finally {
                                userDatasource.read();                                
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
                    userWorkGroupDataSource: userWorkGroupDataSource,
                    userRolesDatasource: userRolesDatasource,
                    onChange: function (arg) {
                            var grid = arg.sender;
                            lastWorkgroupSelectedDataItem = grid.dataItem(grid.select());
                            checkedWorkgroups = [];
                            checkedWorkgroups.push({ "WorkGroupId": lastWorkgroupSelectedDataItem.Id });
                          //  $("#DefaultWorkgroup").val(lastWorkgroupSelectedDataItem.Id);
                    },
                    onDataBound: function (arg) {
                       
                        var treeList = $("div[data-role='treelist']").data("kendoTreeList");
                        var rows = $("tr.k-treelist-group", treeList.tbody);
                        $.each(rows, function (idx, row) {
                            treeList.expand(row);
                            var view =userWorkGroupDataSource.view();
                            // Iterate through rows
                            for (var i = 0; i < view.length; i++) {
                                for (var k = 0; k < checkedWorkgroups.length; k++) {
                                    if (view[i].Id == checkedWorkgroups[k].WorkGroupId) {
                                        // Get the grid

                                        // Set the selected row
                                        treeList.select(treeList.table.find("tr[data-uid='" + view[i].uid + "']"));
                                        break;
                                    }
                                }
                                // Find row with the lastSelectedProduct
                                
                            }
                        });
               
                        // Check if a row was selected
                        //if (lastSelectedDataItem == null) return;
                        //// Get all the rows     
                        //var view = this.dataSource.view();
                        //// Iterate through rows
                        //for (var i = 0; i < view.length; i++) {
                        //    // Find row with the lastSelectedProduct
                        //    if (view[i].Id == lastSelectedDataItem.Id) {
                        //        // Get the grid
                        //        var grid = arg.sender;
                        //        // Set the selected row
                        //        grid.select(grid.table.find("tr[data-uid='" + view[i].uid + "']"));
                        //        break;
                        //    }
                        //}
                    },
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

              
             
                userChoosenWorkgroupDatasource.fetch(function () {
                    if (userChoosenWorkgroupDatasource.view().length > 0) {
                        var userWorkgroupId = userChoosenWorkgroupDatasource.data();

                        for (var k = 0; k < userWorkgroupId.length; k++) {
                            checkedWorkgroups = [];
                            checkedWorkgroups.push({ "WorkGroupId": userWorkgroupId[k] });
                        }
                    }
                });
                viewModel.User.WorkGroup = checkedWorkgroups;
               
                return viewModel;
            },
        });

        return editViewModel;

    });