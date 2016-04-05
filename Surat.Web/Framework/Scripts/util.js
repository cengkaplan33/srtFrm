define([],
    function () {

        var util;

        util = {
            getId:
                function () {
                    var array = window.location.href.split('/');
                    var id = array[array.length - 1];
                    return id;
                },
            getStatus:
                function () {
                    var array = window.location.href.split('/');
                    var status = array[array.length - 2];
                    return status;
                },
            errorHandler:
             function (e) {
                 if(typeof(e.xhr.responseJSON.Status)=="undefined")
                 {
                     _notification.error(e.xhr.responseJSON);
                 }
                 else if (e.xhr.responseJSON.Status == "AccessDenied") {
                     _notification.error(e.xhr.responseJSON.Message);
                 }

                 else if (e.xhr.responseJSON.Status == "RedirectToLogin") {
                     _notification.warning(e.xhr.responseJSON.Message);
                     setTimeout(function () { window.location.href = "/Account/Login"; }, 3000);
                 }
                 else
                     _notification.warning(e.xhr.responseJSON.Message);
             }

        };

        return util;

    });