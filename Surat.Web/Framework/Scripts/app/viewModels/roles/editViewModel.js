define(['rolesDatasource', 'rolesModel','rolePagesModel','rolePagesDatasource','roleActionsDatasource','roleActionsModel', 'util', 'router'],
    function (rolesDatasource, rolesModel, rolePagesModel, rolePagesDatasource, roleActionsDatasource, roleActionsModel, util, router) {
        roleActionsDatasource.options.transport.read.url = "/Roles/GetRoleActions?roleId=" + util.getId();
        rolePagesDatasource.options.transport.read.url = "/Roles/GetRolePages?roleId=" + util.getId();
        var lastRolSelectedDataItem = null;
        var checkedIds = [];
        var pageObject = {};
        var actionObject = [];
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
                    ActionChoose: function (e) {
                        if (e.currentTarget.checked == true) {
                            for (var i = 0; i < actionObject.length; i++) {
                                if (actionObject[i].ActionId == e.data.ActionId) {
                                    actionObject[i].IsAccessible = 1;
                                }

                            }
                        }
                        else {
                            for (var i = 0; i < actionObject.length; i++) {
                                if (actionObject[i].ActionId == e.data.ActionId) {
                                    actionObject[i].IsAccessible = 0;
                                }

                            }

                        }

                    },
                    saveRoles: function (s) {
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            try {
                                if (!(viewModel.Role.Id > 0)) {
                                    rolesDatasource.add(viewModel.Role);
                                }
                                viewModel.Role.set("Pages", JSON.stringify(checkedIds));
                                viewModel.Role.set("Actions", JSON.stringify(actionObject));
                                rolesDatasource.sync();
                            } catch (e) {

                            }
                            finally
                            {
                                roleActionsDatasource.read();
                                rolePagesDatasource.read();
                                rolesDatasource.read();
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
                    rolePagesDatasource: rolePagesDatasource,
                    roleActionsDatasource: roleActionsDatasource
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
                actionObject = [];
                roleActionsDatasource.fetch(function () {
                    if (roleActionsDatasource.view().length > 0) {
                        var roleActionsData =roleActionsDatasource.data();
                        for (var k = 0; k < roleActionsData.length; k++) {
                            if (roleActionsData[k].IsAccessible == 1) {

                                actionObject.push({ "ActionId": roleActionsData[k].ActionId, "IsAccessible": 1, "RelationGroupId": roleActionsData[k].RelationGroupId, "AccessibleItemId": roleActionsData[k].AccessibleItemId,"ActionName":roleActionsData[k].ActionName });
                            }
                            else {
                                actionObject.push({ "ActionId": roleActionsData[k].ActionId, "IsAccessible": 0, "RelationGroupId": roleActionsData[k].RelationGroupId, "AccessibleItemId": roleActionsData[k].AccessibleItemId, "ActionName": roleActionsData[k].ActionName });
                            }
                        }
                    }
                });
                viewModel.Role.Actions = actionObject;
                return viewModel;
            },
        });

        return editViewModel;

    });

function setTypeIcon(model) {

    if (model.Type == 1) { return "<image src='/Framework/theme/images/document.png' / title='Sayfa'>"; }
    if (model.Type == 2) { return "<image src='/Framework/theme/images/flash.png' / title='Aksiyon'>"; }
}