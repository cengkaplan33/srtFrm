define(['systemsDatasource', 'systemsModel', 'util', 'router'],
     function (systemsDatasource, systemsModel, util, router) {
         var lastWorkgroupSelectedDataItem = null;
         var editViewModel = new kendo.data.ObservableObject({
             loadData: function () {
                 var viewModel = new kendo.data.ObservableObject({
                     saveSystem: function (s) {
                         var validator = $("#form").kendoValidator().data("kendoValidator")
                         if (validator.validate()) {
                             if (util.getStatus() == "update") {
                                 systemsDatasource.sync();
                                 systemsDatasource.filter({});
                                 router.navigate('/Systems/index');
                             }
                             else {
                                 viewModel.set("System.parentId", viewModel.System.parentId);
                                 systemsDatasource.add(viewModel.System);
                                 systemsDatasource.sync();
                                 systemsDatasource.filter({});
                                 router.navigate('/Systems/index');
                             }
                         }
                     },
                     cancel: function (s) {
                         systemsDatasource.filter({});
                         router.navigate('/Systems/index');
                     },
                     dataSource: systemsDatasource
                 });

                 systemsDatasource.fetch(function () {
                     if (systemsDatasource.view().length > 0) {
                         if (util.getStatus() == "new") {
                             var myNewsystemsModel = new systemsModel();
                             myNewsystemsModel.parentId = util.getId();
                             viewModel.set("System", myNewsystemsModel);
                             return;
                         }
                         var data = systemsDatasource.data();
                         for (var i = 0; i < data.length; i++) {

                             if (util.getStatus() == "update") {
                                 if (data[i].Id == util.getId()) {
                                     viewModel.set("System", systemsDatasource.at(i));
                                     setBreadCrumb("#/Systems/Index", "Sistem Tanımları");
                                     break;

                                 }
                             }
                             viewModel.set("System", new systemsModel());
                         }

                     }

                 });

                 return viewModel;
             },
         });
         return editViewModel;
     });