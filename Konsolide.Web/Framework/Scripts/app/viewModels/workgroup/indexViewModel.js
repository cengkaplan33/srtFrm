define(['kendo', 'workgroupModel', 'workgroupDatasource', 'router'],
function (kendo, workgroupModel, workgroupDatasource, router) {
    var SelectedRow;
    var SelectedRowId;
    var SelectedRowParentId;
    var grid;
    var toolbar;
    function LoadGrid() {
        grid = $("#workGroupGrid").kendoTreeList({
            dataSource: workgroupDatasource,
            groupable: false,
            sortable: true,
            scrollable: false,
            editable: "inline",
            filterable: true,
            autoSync: false,
            batch: false,
            selectable: true,
            navigatable: false,
            toolbar: [{ name: "create", text: "Yeni Kayıt" }],
            columns: [

                 {
                     field: "Name",
                     title: "Workgroup",
                     width: "120"
                 },
                 {
                     field: "ObjectTypeName",
                     title: "Nesne Tipi",
                     editor: NesneTipiEditor,
                     filterable: {
                         ui: NesneTipiEditor
                     },
                 },
                 {

                     template: "<input name='isCompanySite'  type='checkbox' class='.ob-paid' data-bind='checked: isCompanySite' #= isCompanySite ? checked='checked' : '' #/>",
                     width: 110,
                     title: "Şirket mi?"
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
            ]
        });
        grid.on('click', '.ob-paid', function (e) {

            var row = $(e.target).closest("tr");
            var item = grid.dataItem(row);
            item.set("isCompanySite", $(e.target).is(":checked") ? 1 : 0);
        });

    }
    function NesneTipiEditor(container, options) {
        var ddEditor = $("<input id='ObjectTypeName'  data-bind='value:" + options.field + "' />");
        ddEditor.appendTo(container).kendoDropDownList({
          dataSource: {
            data: ["Firma", "Grup"]
            }
        });

    };
    var pageModel = new kendo.data.ObservableObject({

        onLoad: function () {

            //LoadToolBar();
            LoadGrid();

        }

    });
    return pageModel;
});