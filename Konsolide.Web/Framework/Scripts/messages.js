function successAlert(message) {
    $("#success").kendoWindow({
        modal: true,
        visible: false,
        title: "İşlem Başarılı"
    });


    var kendoWindow = $("#success").data("kendoWindow");
    kendoWindow.wrapper.find(".k-window-titlebar.k-header").css("background-color", "lightgreen");
    kendoWindow.wrapper.find(".k-window-titlebar.k-header").css("color", "white");
    kendoWindow.content("<p>İşlem Detayı:" + message + "</p>");
    kendoWindow.center().open().center();
}
function errorAlert(message) {
    $("#error").kendoWindow({
        modal: true,
        visible: false,
        title:"Hata!"
    });

    
    var kendoWindow = $("#error").data("kendoWindow");
    kendoWindow.wrapper.find(".k-window-titlebar.k-header").css("background-color", "red");
    kendoWindow.wrapper.find(".k-window-titlebar.k-header").css("color", "white");
    kendoWindow.content("<p>Hata Detayı:" + message + "</p>");

        kendoWindow.center().open().center();
}
function warningAlert(message) {
    $("#error").kendoWindow({
        modal: true,
        visible: false,
        title: "Uyarı!"
    });


    var kendoWindow = $("#warning").data("kendoWindow");
    kendoWindow.content("<p>Açıklama:" + message + "</p>");
    kendoWindow.center().open().center();
}
