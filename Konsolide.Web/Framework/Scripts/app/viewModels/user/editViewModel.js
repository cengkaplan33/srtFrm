
define(['userDatasource', 'userModel', 'userRolesDatasource', 'userRolesModel', 'userPagesDatasource', 'userPagesModel', 'userActionsDatasource', 'userActionsModel', 'workgroupDatasource', 'util', 'router'],
    function (userDatasource, userModel, userRolesDatasource, userRolesModel,userPagesDatasource,userPagesModel,userActionsDatasource,userActionsModel, workgroupDatasource, util, router) {
        var lastSelectedDataItem = null;
       // var lastRolSelectedDataItem = null;
        //var lastWorkgroupSelectedDataItem = null;
        var checkedRoles = [];
        var checkedPages = [];
        var checkedActions = [];
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    RoleSec: function (e) {
                        if (e.currentTarget.checked == true) {
                            for (var i = 0; i < checkedRoles.length; i++) {
                                if (checkedRoles[i].Id == e.data.Id)
                                {
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
                    PageIzinVerSec: function (e) {

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
                    },
                    PageYasaklaSec: function (e) {
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
                    ActionIzinVerSec: function (e) {

                        var chboxSibling = $(e.currentTarget).parent().siblings(".actionaccess").children();
                        var imgIsAccess = $(e.currentTarget).parent().siblings(".isactionaccess").children();

                        if (e.currentTarget.checked == true) {

                            AccessViewChanged(imgIsAccess, true);
                            chboxSibling.checked = false;
                            chboxSibling.prop("checked", false);

                            for (var i = 0; i < checkedActions.length; i++) {
                                if (checkedActions[i].ActionId == e.data.ActionId) {
                                    checkedActions[i].IzinVer = 1;
                                    checkedActions[i].Yasakla = 0;
                                }
                            }
                        }
                        else {

                            AccessViewChanged(imgIsAccess, e.data.IsRoleEffect);

                            for (var i = 0; i < checkedActions.length; i++) {
                                if (checkedActions[i].ActionId == e.data.ActionId) {
                                    checkedActions[i].IzinVer = 0;
                                }
                            }
                        }
                    },
                    ActionYasaklaSec: function (e) {
                        var chboxSibling = $(e.currentTarget).parent().siblings(".actionaccess").children();
                        var imgIsAccess = $(e.currentTarget).parent().siblings(".isactionaccess").children();

                        if (e.currentTarget.checked == true) {

                            AccessViewChanged(imgIsAccess, false);
                            chboxSibling.checked = false;
                            chboxSibling.prop("checked", false);

                            for (var i = 0; i < checkedActions.length; i++) {

                                if (checkedActions[i].ActionId == e.data.ActionId) {
                                    checkedActions[i].Yasakla = 1;
                                    checkedActions[i].IzinVer = 0;
                                }
                            }
                        }
                        else {

                            AccessViewChanged(imgIsAccess, e.data.IsRoleEffect);
                            for (var i = 0; i < checkedActions.length; i++) {
                                if (checkedActions[i].ActionId == e.data.ActionId) {
                                    checkedActions[i].Yasakla = 0;
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
                                viewModel.User.set("Pages", JSON.stringify(checkedPages));
                                viewModel.User.set("Actions", JSON.stringify(checkedActions));
                                userDatasource.sync();                                
                                userDatasource.filter({});
                                router.navigate('/Users/index');
                            }
                            else {
                              //  viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                               // viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                viewModel.User.set("Roles", JSON.stringify(checkedRoles));
                                viewModel.User.set("Pages", JSON.stringify(checkedPages));
                                viewModel.User.set("Actions", JSON.stringify(checkedActions));
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
                    userPagesDatasource: userPagesDatasource,
                    userActionsDatasource: userActionsDatasource,
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

                            userActionsDatasource.options.transport.read.url = "/Users/GetUserActions?userId=" + data[i].Id;
                            userActionsDatasource.read();
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

            checkedActions = [];
            userActionsDatasource.fetch(function () {
                if (userActionsDatasource.view().length > 0) {
                    var userActionData = userActionsDatasource.data();
                    for (var k = 0; k < userActionData.length; k++) {
                        if (userActionData[k].IzinVer == 1 || userActionData[k].Yasakla == 1)
                            checkedActions.push({ "ActionId": userActionData[k].ActionId, "IzinVer": userActionData[k].IzinVer, "Yasakla": userActionData[k].Yasakla });
                        else
                            checkedActions.push({ "ActionId": userActionData[k].ActionId, "IzinVer": null, "Yasakla": null });
                    }
                }
            });

            viewModel.User.Pages = checkedPages;
            viewModel.User.Roles = checkedRoles;
            viewModel.User.Actions = checkedActions;
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

    if (model.IsAccess) { return "<image src='/Framework/theme/images/check.png' / title='İzinli'>"; }
    else { return "<image src='/Framework/theme/images/cross.png' / title='Yasaklı'>"; }
}

function setRoleIcon(model) {

    if (model.IsRoleEffect) { return "<image src='/Framework/theme/images/plus-circle.png' / title='İzinli'>"; }
    else
        return "";
}