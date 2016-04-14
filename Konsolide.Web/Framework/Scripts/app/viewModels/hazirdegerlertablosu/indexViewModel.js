define(['kendo', 'hazirDegerlerTablosuModel', 'router','util'],
function (kendo, hazirDegerlerTablosuModel, router,util) {
    var SelectedRow;
    var SelectedRowId;
    var SelectedRowParentId;
    var grid;
    var toolbar;
    var expandedId = "";
    function LoadGrid() {
        grid = $("#hazirDegerlerGrid").kendoTreeList({
           
            dataSource: new kendo.data.TreeListDataSource({
                transport: {

                    read: {
                        url: "/HazirDegerlerTablosu/GetHazirDegerler",
                        type: "POST",
                        complete: function (jqXhr, textStatus) {

                            var tl = $("#hazirDegerlerGrid").data("kendoTreeList");
                            var dataSourceView = tl.dataSource._view;
                            for (var i = 0; i < dataSourceView.length; i++) {
                                var pid = dataSourceView[i].Id;
                                if (pid != expandedId) {
                                    var uid = dataSourceView[i].uid;
                                    $('[data-uid="' + uid + '"] tr').collapse();
                                }
                                else {
                                    var uid = dataSourceView[i].uid;
                                    $('[data-uid="' + uid + '"]').attr("aria-expanded", "true");
                                    tl.expand($('[data-uid="' + uid + '"]'));
                                }
                            }
                        }
                    },
                    create: {
                        type: "POST",
                        url: "/HazirDegerlerTablosu/Add",
                        dataType: "json",
                        complete: function (jqXhr, textStatus) {
                            if (textStatus = "success") {
                                var result = jQuery.parseJSON(jqXhr.responseText);
                                
                                try {
                                    var tree = $("#hazirDegerlerGrid").data("kendoTreeList");
                                    tree.dataSource.read();
                                    
                                } catch (e) {
                                    var tree = $("#hazirDegerlerGrid").data("kendoTreeList");
                                    tree.dataSource.read();
                                }
                                _notification.info(result.Result);
                            }
                        }
                    },
                    destroy: {

                        type: "POST",
                        url: "/HazirDegerlerTablosu/Delete",
                        dataType: "Json",

                    },
                    update: {

                        type: "POST",
                        url: "/HazirDegerlerTablosu/Update",
                        dataType: "Json",
                        complete: function (jqXhr, textStatus) {
                            if (textStatus = "success") {
                                var result = jQuery.parseJSON(jqXhr.responseText);
                              
                                try {
                                    var tree = $("#hazirDegerlerGrid").data("kendoTreeList");
                                    tree.dataSource.read();
                                  
                                   
                                } catch (e) {
                                    var tree = $("#hazirDegerlerGrid").data("kendoTreeList");
                                    tree.dataSource.read();
                                   
                                    
                                }
                                _notification.info(result.Result);
                            }
                        }
                    }

                },
                paging: false,
                cache: false,
                schema: {
                    model: hazirDegerlerTablosuModel
                },
                error: function (e) {
                    util.errorHandler(e);
                }
            }),
            autosync: false,
            filterable: true,
            selectable: false,
            navigatable: false,
            pageSize: 1500,
            cache: false,
            batch: false,
            columns: [

                 {
                     field: "HazirDeger",
                     title: "Hazır Değerler",
                     width: "120px",
                     editable: false
                 },
                 {
                     field: "TL",
                     title: "TL",
                     format: "{0:n2}"
                 },
                 {
                     field: "USD",
                     title: "USD",
                     format: "{0:n2}"
                 },
                 {
                     field: "EURO",
                     title: "EURO",
                     format: "{0:n2}",
                 },
              { command: [{ name: "edit", text: "Düzenle" }], title: "İşlemler" }
            ],
            expand:function(e)
            {
                var row = e.model;
             
                expandedId = row.Id;
               
            },
            dataBound: function onDataBound(e) {
                var grid = this;
                var gridData = grid.dataSource.view();

                for (var i = 0; i < gridData.length; i++) {
                    var currentUid = gridData[i].uid;
                    if (gridData[i].parentId == null) {
                        var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                        currenRow[0].style.fontWeight = "bold";
                        var editButton = $(currenRow).find(".k-grid-edit");
                        editButton.hide();
                    }
                    else {
                        var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                        currenRow[0].style.fontWeight = "normal";
                        var editButton = $(currenRow).find(".k-grid-edit");
                        editButton.show();
                    }
                }
            },
            editable: true,
           
        });


    }
  
    function LoadToolBar() {
        toolbar = $("#FunctionsToolBar").kendoToolBar({
            items: [
                { type: "button", id: "btnAddProject", text: "Yeni Ödeme Talebi", click: pageModel.onAddProjectCard },
                //{ type: "button", id: "btnEditProject", text: "Düzenle", click: pageModel.onEditProjectCard },
                //{ type: "button", id: "btnDeleteProject", text: "Sil", click: pageModel.onRemoveProjectCard },
                { type: "button", id: "btnYenile", text: "Yenile", click: pageModel.onRefreshProjectList, enable: true, icon: "refresh" },
                { type: "button", id: "btnProjeDosyasiYukle", text: "Proje Dosyası Yükle", click: pageModel.onUploadProjectFile, enable: false },
                { type: "button", id: "btnIlgiliEkle", text: "İlgili Kişi Ekle", click: pageModel.onAddStakeHolder, enable: false },
                { type: "separator" },
                {
                    type: "splitButton", text: "Sadece şu durumda olanlar", menuButtons: [
                        { id: "chkSadeceKapali", text: "Kapalı", click: FilterProjectsByStatus },
                        { id: "chkSadeceAcik", text: "Açık", click: FilterProjectsByStatus },
                    ]
                }
            ]
        });
    }


    var pageModel = new kendo.data.ObservableObject({

        onLoad: function () {
            //LoadToolBar();
            LoadGrid();
        }
    });
    return pageModel;
});