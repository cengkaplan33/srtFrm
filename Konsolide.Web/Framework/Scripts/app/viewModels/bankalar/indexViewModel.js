﻿define(['kendo', 'bankDatasource', 'router'],
function (kendo, bankDatasource, router) {
    var lastSelectedDataItem = null;
    var onClick = function (event, delegate) {
        event.preventDefault();
        var grid = $("#bankgrid").data("kendoGrid");
        var selectedRow = grid.select();
        var dataItem = grid.dataItem(selectedRow);
        if (selectedRow.length > 0)
            delegate(grid, selectedRow, dataItem);
        else
            _notification.warning("Lütfen bir satır seçiniz");
    };
    function newRecord(e) {
        e.preventDefault();
        router.navigate('/Bankalar/Edit/0');
    };
    var indexViewModel = new kendo.data.ObservableObject({
        add: newRecord,
        cancel: function (event) {
            onClick(event, function (grid) {
                grid.cancelRow();
                $(".toolbar").toggle();
            });
        },
        details: function (event) {
            onClick(event, function (grid, row, dataItem) {
                router.navigate('/Bankalar/Edit/' + dataItem.Id);
            });
        },
        edit: function (event) {
            onClick(event, function (grid, row, dataItem) {
                router.navigate('/Bankalar/Edit/' + dataItem.Id);
            });
        },
        destroy: function (event) {
            onClick(event, function (grid, row, dataItem) {

                var r = confirm("Seçtiğiniz banka tanımını  silmek istediğinize emin misiniz?!");
                if (r == true) {
                    bankDatasource.remove(dataItem);
                    bankDatasource.sync();
                }


            });
        },
        onChange: function (arg) {
            var grid = arg.sender;
            lastSelectedDataItem = grid.dataItem(grid.select());
        },
        dataSource: bankDatasource,
        onDataBound: function (arg) {
            // Check if a row was selected
            if (lastSelectedDataItem == null) return;
            // Get all the rows     
            var view = this.dataSource.view();
            // Iterate through rows
            for (var i = 0; i < view.length; i++) {
                // Find row with the lastSelectedProduct
                if (view[i].Id == lastSelectedDataItem.Id) {
                    // Get the grid
                    var grid = arg.sender;
                    // Set the selected row
                    grid.select(grid.table.find("tr[data-uid='" + view[i].uid + "']"));
                    break;
                }
            }
        },
    });
    return indexViewModel;
});