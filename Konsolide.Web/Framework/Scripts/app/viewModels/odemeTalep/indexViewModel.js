define(['kendo', 'odemeTalepModel', 'odemeTalepDatasource', 'odemeTuruDatasource', 'OdemeEkraniDurumTanimiDatasource', 'router'],
function (kendo, odemeTalepModel, odemeTalepDatasource, odemeTuruDatasource, OdemeEkraniDurumTanimiDatasource, router) {
  
    // OdemeEkraniDurumTanimiDatasource.read();

    // var dataOdemeTuru = odemeTuruDatasource.data;
    // var lastSelectedDataItem = null;

    // var onClick = function (event, delegate) {
    //     event.preventDefault();
    //     var grid = $("#odemeTalepGrid").data("kendoGrid");
    //     var selectedRow = grid.select();
    //     var dataItem = grid.dataItem(selectedRow);
    //     if (selectedRow.length > 0)
    //         delegate(grid, selectedRow, dataItem);
    //     else
    //         _notification.warning("Lütfen bir satır seçiniz");
    // };
    // var getGridData= function() {
    //     return odemeTalepDatasource.data();
    // }

    // indexViewModel = new kendo.observable({
    //     getOdemeTuru: function (PaymentCollectingId) {

    //         var odemeTipi = "";

    //         odemeTuruDatasource.fetch(function () {
    //             if (odemeTuruDatasource.view().length > 0) {

    //                 var data = odemeTuruDatasource.data();
    //                 for (var i = 0; i < data.length; i++) {
    //                     if (data[i].Id == PaymentCollectingId) {
    //                         odemeTipi = data[i].Name;
    //                         break;
    //                     }

    //                 }

    //             }

    //         });
    //         return odemeTipi;
    //     },
    //     getOdemeDurumTanimi: function (OdemeTalepDurumuId) {

    //         var odemeDurumu = "";

    //         OdemeEkraniDurumTanimiDatasource.fetch(function () {
    //             if (OdemeEkraniDurumTanimiDatasource.view().length > 0) {

    //                 var data = OdemeEkraniDurumTanimiDatasource.data();
    //                 for (var i = 0; i < data.length; i++) {
    //                     if (data[i].Id == OdemeTalepDurumuId) {
    //                         odemeDurumu = data[i].Durum;
    //                         break;
    //                     }

    //                 }

    //             }

    //         });
    //         return odemeDurumu;
    //     },
    //     onChange: function (arg) {
    //         var grid = arg.sender;

    //         lastSelectedDataItem = grid.dataItem(grid.select());

    //     },
    //     edit: function (event) {
    //         onClick(event, function (grid, row, dataItem) {
    //             grid.editRow(grid.select());

    //         });
    //     },
    //     dataSource: odemeTalepDatasource,
    //     onDataBound: function (arg) {
    //         // Check if a row was selected
    //         if (lastSelectedDataItem == null) return;
    //         // Get all the rows     
    //         var view = this.dataSource.view();
    //         // Iterate through rows
    //         for (var i = 0; i < view.length; i++) {

    //             // Find row with the lastSelectedProduct
    //             if (view[i].Id == lastSelectedDataItem.Id) {
    //                 // Get the grid
    //                 var grid = arg.sender;
    //                 // Set the selected row
    //                 grid.select(grid.table.find("tr[data-uid='" + view[i].uid + "']"));
    //                 break;
    //             }
    //         }
    //     },
    // });
    //// kendo.bind($("#BaseDiv"), indexViewModel);

    // return indexViewModel;

    var SelectedRow;
    var SelectedRowId;
    var grid;
    var toolbar;
    function LoadGrid() {
        grid = $("#odemeTalepGrid").kendoGrid({
            dataSource: odemeTalepDatasource,
            groupable: false,
            sortable: true,
            scrollable: false,
            editable: "inline",
            filterable: true,
            reorderable: true,
            selectable: true,
            navigatable: false,
            toolbar: ["create", { name: "excel", text: "Excele Aktar" }],
            excelExport: function (e) {
                var data = e.data;
                var rows = e.workbook.sheets[0].rows;

                for (var ri = 0; ri < rows.length; ri++) {
                    var row = rows[ri];

                    if (row.type == "group-footer" || row.type == "footer") {
                        for (var ci = 0; ci < row.cells.length; ci++) {
                            var cell = row.cells[ci];
                            if (cell.value) {
                                // Use jQuery.fn.text to remove the HTML and get only the text
                                cell.value = $(cell.value).text();
                                // Set the alignment
                                cell.hAlign = "right";
                            }
                        }
                    }
                    if (row.type == "data") {
                       
                        row.cells[2].value = data[ri - 1].Name;
                        row.cells[6].value = data[ri - 1].Durum;
                        
                    }
                }
            },
            excel: {
                fileName: "OdemeBilgiFormu.xlsx",
                filterable: true
            },
            pageable: [{ info: true, pageSizes: true, buttonCount: 5 }],
            columns: [
                 {
                     field: "Id",
                     title: "#",
                     width:"50px"
                   
                 },
                 {
                     field: "Tarih",
                     title: "Tarih",
                     format: "{0:dd-MM-yyyy}",
                     parseFormats: ["dd-MM-yyyy"],
                     width: "120px"
                 },
                 {
                  
                     template: "#:Name #",
                     field: "PaymentCollectingId",
                     title: "Ödeme Türü",
                     editor: odemeTuruEditor,
                     filterable: {
                         ui: odemeTuruFilter
                     },
                     width: "120px"
                 },
                 {
                     field: "TL",
                     title: "TL",
                     format: "{0:n2}",
                     attributes:{style:"text-align:right;"},
                     aggregates: "sum",
                     footerTemplate: "<div style='text-align:right'>Toplam:#=kendo.toString(sum,'N')#</div>"
                 },
                 {
                     field: "USD",
                     title: "USD",
                     format: "{0:n2}",
                     attributes: { style: "text-align:right;" },
                     aggregates: "sum",
                     footerTemplate: "<div style='text-align:right'>Toplam:#=kendo.toString(sum,'N')#</div>"
                 },
                 {
                     field: "EURO",
                     title: "EURO",
                     format: "{0:n2}",
                     attributes: { style: "text-align:right;" },
                     aggregates: "sum",
                     footerTemplate: "<div style='text-align:right'>Toplam:#=kendo.toString(sum,'N')#</div>"
                 },
                 {
                    template:"#:Durum#",
                     field: "OdemeTalepDurumuId",
                     title: "Ödeme Durumu",                  
                     editor: odemeDurumuEditor,
                     filterable: {
                         ui: odemeDurumuFilter
                     }
                 },
                 {
                    
                     field: "Aciklama",
                     title: "Açıklama"
                 },
                 {
                     command:
                     [
                     { name: "edit" },
                     { name: "destroy" }
                     ],
                     title: "İşlemler", width: "170px"
                 }
            ],
            change: pageModel.onSelectedRowChanged
        });
        $(grid.tbody).on('keydown', function (e) {
            if ($(e.target).closest('td').is(':last-child') && $(e.target).closest('tr').is(':last-child')) {
                grid.addRow();
            }
        });
        $("#odemeTalepGrid").on("click", "td", function (e) {

            var rowIndex = $(this).parent().index();
            var cellIndex = $(this).index();
            $("input").on("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#odemeTalepGrid").data("kendoGrid").editCell($(".k-grid-content").find("table").find("tbody").find("tr:eq(" + rowIndex + ")").find("td:eq(" + cellIndex + ")").next().focusin($("#batchgrid").data("kendoGrid").closeCell($(".k-grid-content").find("table").find("tbody").find("tr:eq(" + rowIndex + ")").find("td:eq(" + cellIndex + ")").parent())));
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
    function odemeTuruEditor(container, options) {
        var ddEditor = $("<input id='PaymentCollectingId' required data-text-field='Name' data-value-field='Id' data-bind='value:" + options.field + "' />");
        ddEditor.appendTo(container).kendoDropDownList({
            dataSource: odemeTuruDatasource,
            dataTextField: "Name",
            dataValueField: "Id"
        });

    };
    function odemeDurumuEditor(container, options) {

        var ddEditor = $("<input id='OdemeTalepDurumuId' required data-text-field='Durum' data-value-field='Id' data-bind='value:" + options.field + "' />");
        ddEditor.appendTo(container).kendoDropDownList({
            dataSource: OdemeEkraniDurumTanimiDatasource,
            dataTextField: "Durum",
            dataValueField: "Id"
        });
    };
    function GetSelectedGridRow() {
        return grid.data("kendoGrid").select().eq(0);
    }
    function odemeTuruFilter(element) {
        element.kendoDropDownList({
            dataSource: odemeTuruDatasource,
            optionLabel: "--Seçiniz--",
            dataTextField: "Name",
            dataValueField:"Id"
        });
    }
    function odemeDurumuFilter(element) {
        element.kendoDropDownList({
            dataSource: OdemeEkraniDurumTanimiDatasource,
            optionLabel: "--Seçiniz--",
            dataTextField: "Durum",
            dataValueField: "Id"
        });
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
           // LoadToolBar();
            LoadGrid();
        },
        onAddProjectCard: function (e) {
            grid.data("kendoGrid").addRow();
        },
        onEditProjectCard: function (e) {
            grid.data("kendoGrid").editRow(GetSelectedGridRow());
        },
        onRemoveProjectCard: function (e) {
            grid.data("kendoGrid").removeRow(GetSelectedGridRow());
        },
        onSelectedRowChanged: function (e) {
            SelectedProject = grid.data("kendoGrid").dataItem(GetSelectedGridRow());
            if (SelectedProject) {
                SelectedProjectId = SelectedProject.Id;
            } else {
                SelectedProjectId = -1;
                _notification.info("Öncelikle bir proje satırı seçmelisiniz!");
            }
          
        },
        onRefreshProjectList: function (e) {
            odemeTalepDatasource.read();
            grid.data("kendoGrid").refresh();
        }     
     
    });
    return pageModel;
});