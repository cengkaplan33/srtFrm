
define(['userDatasource', 'userModel', 'userRolesDatasource' ,'userRolesModel', 'userPagesDatasource', 'userPagesModel', 'workgroupDatasource', 'util', 'router'],
    function (userDatasource, userModel, userRolesDatasource, userRolesModel,userPagesDatasource,userPagesModel, workgroupDatasource, util, router) {
        var lastSelectedDataItem = null;
       // var lastRolSelectedDataItem = null;
        //var lastWorkgroupSelectedDataItem = null;
        var checkedRoles = [];
        var checkedPages = [];
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
                    IzinVerSec: function (e) {
                        if (e.currentTarget.checked == true) {

                            for (var i = 0; i < checkedPages.length; i++) {
                                if (checkedPages[i].PageId == e.data.PageId) {
                                    checkedPages[i].IzinVer = 1;
                                    checkedPages[i].Yasakla = 0;                        
                                }
                            }
                        }
                        else {
                            for (var i = 0; i < checkedPages.length; i++) {
                                if (checkedPages[i].PageId == e.data.PageId) {
                                    checkedPages[i].IzinVer = 0;
                                }
                            }
                        }
                    },
                    YasaklaSec: function (e) {
                        if (e.currentTarget.checked == true) {

                            for (var i = 0; i < checkedPages.length; i++) {
                                if (checkedPages[i].PageId == e.data.PageId) {
                                    checkedPages[i].Yasakla = 1;
                                    checkedPages[i].IzinVer = 0;
                                }
                            }
                        }
                        else {
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
                            if (viewModel.User.Id > 0) {
                               // viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                               // viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                viewModel.User.set("Roles", JSON.stringify(checkedRoles));
                                viewModel.User.set("Pages", JSON.stringify(checkedPages));
                                userDatasource.sync();                                
                                userDatasource.filter({});
                                router.navigate('/Users/index');
                            }
                            else {
                              //  viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                               // viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                viewModel.User.set("Roles", JSON.stringify(checkedRoles));
                                viewModel.User.set("Pages", JSON.stringify(checkedPages));
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
            viewModel.User.Roles = checkedRoles;                               
            return viewModel;
            },
        });

        return editViewModel;

    });