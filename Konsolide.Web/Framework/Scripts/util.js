define([],
    function() {

        var util;

        util = {
            getId:
                function() {
                    var array = window.location.href.split('/');
                    var id = array[array.length - 1];
                    return id;
                },
            getStatus:
                function () {
                    var array = window.location.href.split('/');
                    var status = array[array.length-2];
                    return status;
                }

        };

        return util;

    });