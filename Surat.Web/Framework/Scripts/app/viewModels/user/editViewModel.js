﻿define(['userDatasource', 'userModel', 'userRolesDatasource', 'userRolesModel', 'userPagesDatasource', 'userPagesModel', 'workgroupDatasource', 'userWorkGroupDataSource', 'userChoosenWorkgroupDatasource', 'util', 'router'],
function (userDatasource, userModel, userRolesDatasource, userRolesModel, userPagesDatasource, userPagesModel, workgroupDatasource, userWorkGroupDataSource, userChoosenWorkgroupDatasource, util, router) {
    if (util.getId() != 0) {
        userWorkGroupDataSource.options.transport.read.url = "/Users/GetUserWorkgroups?userId=" + util.getId();
        userChoosenWorkgroupDatasource.options.transport.read.url = "/Users/GetChoosedWorkgroupId?userId=" + util.getId();
    }
    else {
        userWorkGroupDataSource.options.transport.read.url = "/Users/GetUserWorkgroupsWithCurentUsers";

    }


    var lastSelectedDataItem = null;
    // var lastRolSelectedDataItem = null;
    //var lastWorkgroupSelectedDataItem = null;
    var checkedRoles = [];
    var checkedWorkgroups = [];
    var checkedPages = [];
    var editViewModel = new kendo.data.ObservableObject({
        loadData: function () {
            var viewModel = new kendo.data.ObservableObject({
                RoleSec: function (e) {
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
                IzinVerSec: function (e) {

                    var chboxSibling = $(e.currentTarget).parent().siblings(".useraccess").children();
                    var imgIsPageAccess = $(e.currentTarget).parent().siblings(".ispageaccess").children();

                    if (e.currentTarget.checked == true) {

                        AccessViewChanged(imgIsPageAccess, true);
                        chboxSibling.checked = false;
                        chboxSibling.prop("checked", false);

                        for (var i = 0; i < checkedPages.length; i++) {
                            if (checkedPages[i].PageId == e.data.PageId) {
                                checkedPages[i].IzinVer = 1;
                                checkedPages[i].Yasakla = 0;
                            }
                        }
                    }
                    else {

                        AccessViewChanged(imgIsPageAccess, e.data.IsRoleEffect);

                        for (var i = 0; i < checkedPages.length; i++) {
                            if (checkedPages[i].PageId == e.data.PageId) {
                                checkedPages[i].IzinVer = 0;
                            }
                        }
                    }

                    var etkinlik = $(".pageaccess").siblings();
                    etkinlik.data = "test";
                },
                YasaklaSec: function (e) {
                    var chboxSibling = $(e.currentTarget).parent().siblings(".useraccess").children();
                    var imgIsPageAccess = $(e.currentTarget).parent().siblings(".ispageaccess").children();

                    if (e.currentTarget.checked == true) {

                        AccessViewChanged(imgIsPageAccess, false);
                        chboxSibling.checked = false;
                        chboxSibling.prop("checked", false);

                        for (var i = 0; i < checkedPages.length; i++) {

                            if (checkedPages[i].PageId == e.data.PageId) {
                                checkedPages[i].Yasakla = 1;
                                checkedPages[i].IzinVer = 0;
                            }
                        }
                    }
                    else {

                        AccessViewChanged(imgIsPageAccess, e.data.IsRoleEffect);
                        for (var i = 0; i < checkedPages.length; i++) {
                            if (checkedPages[i].PageId == e.data.PageId) {
                                checkedPages[i].Yasakla = 0;
                            }
                        }
                    }
                },

                saveUser: function (s) {
                    var validator = $("#form").kendoValidator().data("kendoValidator")
                    if (validator.validate()) {
                        try {
                            viewModel.User.set("Pages", JSON.stringify(checkedPages));
                            viewModel.User.set("Roles", JSON.stringify(checkedRoles));
                            viewModel.User.set("WorkGroupId", JSON.stringify(checkedWorkgroups));
                            viewModel.User.set("Pages", JSON.stringify(checkedPages));
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
                userPagesDatasource: userPagesDatasource,
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
                        var view = userWorkGroupDataSource.view();
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

                            userPagesDatasource.options.transport.read.url = "/Users/GetUserPages?userId=" + data[i].Id;
                            userPagesDatasource.read();
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

            checkedPages = [];
            userPagesDatasource.fetch(function () {
                if (userPagesDatasource.view().length > 0) {
                    var userPageData = userPagesDatasource.data();
                    for (var k = 0; k < userPageData.length; k++) {
                        if (userPageData[k].IzinVer == 1 || userPageData[k].Yasakla == 1)
                            checkedPages.push({ "PageId": userPageData[k].PageId, "IzinVer": userPageData[k].IzinVer, "Yasakla": userPageData[k].Yasakla });
                        else
                            checkedPages.push({ "PageId": userPageData[k].PageId, "IzinVer": null, "Yasakla": null });
                    }
                }
            });

            viewModel.User.Pages = checkedPages;
            return viewModel;
        },
    });

    return editViewModel;

});

function AccessViewChanged(imgContainer, state) {

    if (state) { imgContainer.prop("src", "/Framework/theme/images/check.png"); }
    else { imgContainer.prop("src", "/Framework/theme/images/cross.png"); }
}

function setAcccesIcon(model) {

    if (model.IsPageAccess) { return "<image src='/Framework/theme/images/check.png' / title='İzinli'>"; }
    else { return "<image src='/Framework/theme/images/cross.png' / title='Yasaklı'>"; }
}

function setRoleIcon(model) {

    if (model.IsRoleEffect) { return "<image src='/Framework/theme/images/plus-circle.png' / title='İzinli'>"; }
    else
        return "";
}