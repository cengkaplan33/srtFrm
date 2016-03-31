define(['kendo', 'hazirDegerlerTablosuModel', 'hazirDegerlerTablosuDatasource', 'router'],
function (kendo, hazirDegerlerTablosuModel, hazirDegerlerTablosuDatasource, router) {
    var SelectedRow;
    var SelectedRowId;
    var SelectedRowParentId;
    var grid;
    var toolbar;
    function LoadGrid() {
        grid = $("#hazirDegerlerTanimGrid").kendoTreeList({
            dataSource: hazirDegerlerTablosuDatasource,
            groupable: false,
            sortable: true,
            scrollable: false,
            editable: "inline",
            filterable: true,
            autoSync: true,
            selectable: true,
            navigatable: false,
            width:"400px",
            toolbar: [{ name: "create", text: "Yeni Kayıt" }],
            columns: [

                {
                    field: "Kod",
                    title: "Kodu",
                    width: "160"
                },
                 {
                     field: "HazirDeger",
                     title: "Hazır Değerler",
                     width: "240"
                 },
                  {
                      field: "Tur",
                      title: "Tür",
                      width: "240"
                  },
                 {
                     command:
                     [
                     { name: "edit", text: "Düzenle" },
                     { name: "destroy", text: "Sil" },
                     { name: "createChild", text: "Alt Değer" }
                     ],
                     title: "İşlemler"
                 }
            ],
            change: pageModel.onSelectedRowChanged
        });
        $(grid.tbody).on('keydown', function (e) {
            if ($(e.target).closest('td').is(':last-child') && $(e.target).closest('tr').is(':last-child')) {
                grid.addRow();
            }
        });
        $("#hazirDegerlerTanimGrid").on("click", "td", function (e) {

            var rowIndex = $(this).parent().index();
            var cellIndex = $(this).index();
            $("input").on("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#hazirDegerlerTanimGrid").data("kendoGrid").editCell($(".k-grid-content").find("table").find("tbody").find("tr:eq(" + rowIndex + ")").find("td:eq(" + cellIndex + ")").next().focusin($("#batchgrid").data("kendoGrid").closeCell($(".k-grid-content").find("table").find("tbody").find("tr:eq(" + rowIndex + ")").find("td:eq(" + cellIndex + ")").parent())));
                    return false;
                }
            });

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

    function GetSelectedGridRow() {
        return grid.data("kendoTreeList").select().eq(0);
    }

    function FilterProjectsByStatus(e) {
        alert("//TO DO Filter Projects By Status")
        //ProjectCardDataSource.read();
        //grid.data("kendoGrid").refresh();
    }
    function LoadToolBar() {
        toolbar = $("#FunctionsToolBar").kendoToolBar({
            items: [
                { type: "button", id: "btnAddProject", text: "Yeni Ödeme Talebi", click: pageModel.onAddProjectCard },
                //{ type: "button", id: "btnEditProject", text: "Düzenle", click: pageModel.onEditProjectCard },
                //{ type: "button", id: "btnDeleteProject", text: "Sil", click: pageModel.onRemoveProjectCard },
                 { type: "separator" },
                { type: "button", id: "btnYenile", text: "Yenile", click: pageModel.onRefreshProjectList, enable: true, icon: "refresh" },


                { type: "separator" }

            ]
        });
    }

    var pageModel = new kendo.data.ObservableObject({

        onLoad: function () {
            //LoadToolBar();
            LoadGrid();
        },
        onAddProjectCard: function (e) {
            grid.data("kendoTreeList").addRow();
        },
        onEditProjectCard: function (e) {
            grid.data("kendoGrid").editRow(GetSelectedGridRow());
        },
        onRemoveProjectCard: function (e) {
            grid.data("kendoGrid").removeRow(GetSelectedGridRow());
        },
        onSelectedRowChanged: function (e) {
            SelectedRow = grid.data("kendoTreeList").dataItem(GetSelectedGridRow());
            if (SelectedRow) {

                SelectedRowId = SelectedRow.Id;
                SelectedRowParentId = SelectedRow.parentId;

            } else {
                SelectedRowId = -1;
                _notification.info("Öncelikle bir proje satırı seçmelisiniz!");
            }

        },
        onRefreshProjectList: function (e) {
            // hazirDegerlerTablosuDatasource.read();
            grid.data("kendoTreeList").refresh();
        }

    });
    return pageModel;
});