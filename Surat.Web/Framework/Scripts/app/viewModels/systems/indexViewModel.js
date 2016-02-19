define(['kendo', 'systemsDatasource', 'router'],
    function (kendo, systemsDatasource, router) {
        var lastSelectedDataItem = null;
        var onClick = function (event, delegate) {
            event.preventDefault();
            var treeList = $("#systemsTree").data("kendoTreeList"),
                selectedRow = treeList.select();

            var dataItem = treeList.dataItem(selectedRow);

            if (selectedRow.length > 0)
            { delegate(treeList, selectedRow, dataItem); }
            else
            {
                router.navigate('/Systems/Edit/new/null');
            }
        };

        var indexViewModel = new kendo.data.ObservableObject({
            add: function (event) {
                onClick(event, function (treeList, row, dataItem) {
                    router.navigate('/Systems/Edit/new/' + dataItem.Id);
                });
            },
            cancel: function (event) {
                onClick(event, function (grid) {
                    treeList.cancelRow();
                    $(".toolbar").toggle();
                });
            },
            details: function (event) {
                onClick(event, function (treeList, row, dataItem) {
                    router.navigate('/Systems/Edit/' + dataItem.Id);
                });
            },
            edit: function (event) {
                onClick(event, function (treeList, row, dataItem) {
                    router.navigate('/Systems/Edit/update/' + dataItem.Id);
                });
            },
            destroy: function (event) {
                onClick(event, function (treeList, row, dataItem) {

                    var r = confirm("Seçtiğiniz System tanımını  silmek istediğinize emin misiniz?!");
                    if (r == true) {

                        systemsDatasource.remove(dataItem);

                        systemsDatasource.sync();



                    }


                });
            },
            onChange: function (arg) {
                var treeList = arg.sender;
                lastSelectedDataItem = treeList.dataItem(treeList.select());
            },
            dataSource: systemsDatasource,
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
                        var treeList = arg.sender;
                        // Set the selected row
                        treeList.select(treeList.table.find("tr[data-uid='" + view[i].uid + "']"));
                        lastSelectedDataItem.loaded(false);
                        lastSelectedDataItem.load();
                        break;
                    }
                }

            },
        });
        return indexViewModel;
    });