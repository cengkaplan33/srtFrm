define(['workgroupDatasource', 'workgroupModel', 'util', 'router'],
     function (workgroupDatasource, workgroupModel, util, router) {
         var lastWorkgroupSelectedDataItem = null;
         var editViewModel = new kendo.data.ObservableObject({
             loadData: function () {
                 var viewModel = new kendo.data.ObservableObject({
                     saveWorkgroup: function (s) {
                         var validator = $("#form").kendoValidator().data("kendoValidator")
                         if (validator.validate()) {
                             if (util.getStatus()=="update") {
                                 workgroupDatasource.sync();
                                 workgroupDatasource.filter({});
                                 router.navigate('/Workgroups/index');
                             }
                             else {
                                 viewModel.set("Workgroup.parentId", viewModel.Workgroup.parentId);
                                 workgroupDatasource.add(viewModel.Workgroup);
                                 workgroupDatasource.sync();                                
                                 workgroupDatasource.filter({});                               
                                 router.navigate('/Workgroups/index');
                             }
                         }
                     },
                     cancel: function (s) {
                         workgroupDatasource.filter({});
                         router.navigate('/Workgroups/index');
                     },
                     dataSource: workgroupDatasource
                 });

                 workgroupDatasource.fetch(function () {
                     if (workgroupDatasource.view().length > 0) {
                         if (util.getStatus() == "new") {
                             var myNewWorkgroupModel = new workgroupModel();
                              myNewWorkgroupModel.parentId = util.getId();
                             viewModel.set("Workgroup", myNewWorkgroupModel);
                             return;
                         }
                         var data = workgroupDatasource.data();
                         for (var i = 0; i < data.length; i++) {
                            
                           if (util.getStatus() == "update") {
                                 if (data[i].Id == util.getId()) {
                                     viewModel.set("Workgroup", workgroupDatasource.at(i));
                                     setBreadCrumb("#/Workgroups/Index", "Çalışma Grupları");
                                     break;

                                 }
                             }
                             viewModel.set("Workgroup", new workgroupModel());
                         }

                     }

                 });

                 return viewModel;
             },
         });
         return editViewModel;
     });