define(['kendo', 'exceptionsDatasource', 'router'],
function (kendo, exceptionsDatasource, router) {
    var lastSelectedDataItem = null;
    var onClick = function (event, delegate) {
        event.preventDefault();
        var grid = $("#exceptionsgrid").data("kendoGrid");
        var selectedRow = grid.select();
        var dataItem = grid.dataItem(selectedRow);
        if (selectedRow.length > 0)
            delegate(grid, selectedRow, dataItem);
        else
            _notification.warning("Lütfen bir satır seçiniz");
    };
    var indexViewModel = new kendo.data.ObservableObject({
        onChange: function (arg) {
            var grid = arg.sender;
            lastSelectedDataItem = grid.dataItem(grid.select());
        },
        dataSource: exceptionsDatasource,
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