
define(['userDatasource', 'userModel','util', 'router', 'rolesDatasource','workgroupDatasource'],
    function (userDatasource, userModel, util, router, rolesDatasource,workgroupDatasource) {
        var lastSelectedDataItem = null;
        var lastRolSelectedDataItem = null;
        var lastWorkgroupSelectedDataItem = null;
        var editViewModel = new kendo.data.ObservableObject({
            loadData: function () {
                var viewModel = new kendo.data.ObservableObject({
                    saveUser: function (s) {
                        var validator = $("#form").kendoValidator().data("kendoValidator")
                        if (validator.validate()) {
                            if (viewModel.User.Id > 0) {
                             viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
                                userDatasource.sync();                                
                                userDatasource.filter({});
                                router.navigate('/Users/index');
                            }
                            else {
                                viewModel.set("User.DefaultRole", $("#DefaultRole").val());
                                viewModel.set("User.DefaultWorkgroup", $("#DefaultWorkgroup").val());
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
                    onRolesChange:function(arg)
                    {
                        var grid = arg.sender;
                        lastRolSelectedDataItem = grid.dataItem(grid.select());
                       
                        $("#DefaultRole").val(lastRolSelectedDataItem.Id);
                      
                    },
                    onWorkgroupChange: function (arg) {
                        var grid = arg.sender;
                        lastWorkgroupSelectedDataItem = grid.dataItem(grid.select());
                       
                        $("#DefaultWorkgroup").val(lastWorkgroupSelectedDataItem.Id);
                    },
                    mysource:workgroupDatasource ,
                    rolesDatasource: rolesDatasource,
                });

                userDatasource.fetch(function () {
                    if (userDatasource.view().length > 0) {

                        var data = userDatasource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Id == util.getId()) {
                                viewModel.set("User", userDatasource.at(i));
 
                                setBreadCrumb("#/Users/Index", "Kullanıcı Listesi");
                                break;
                            }
                            viewModel.set("User", new userModel());
                        }

                    }

                });

                return viewModel;
            },
        });

        return editViewModel;

    });