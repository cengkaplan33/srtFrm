﻿
define(['kendo'],
    function (kendo) {
        var router = new kendo.Router(),
            layout = new kendo.Layout("<div id='content'></div>");

        layout.render($("#app"));


        router.route("/", function () {
            require(['text!/home/index'], function (view) {
                loadView(null, view);
            });
        });
        router.route("/Account/PasswordChange", function () {
            require(['account-passwordChangeViewModel', 'text!/Account/PasswordChange'], function (viewModel, view) {
                setBreadCrumb("#/Account/PasswordChange", "Şifre İşlemleri");
                loadView(viewModel, view, function () {
                    kendo.bind($("#form"), viewModel);
                });
            });
        });
        router.route("/Users/Index", function () {
            require(['user-indexViewModel', 'text!/Users/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#usergrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });
        router.route("/Users/Edit/:id", function () {
            require(['user-editViewModel', 'text!/Users/Edit'], function (viewModel, view) {
                loadView(viewModel.loadData(), view);
                kendo.bind($("#form"), viewModel);

            });

        });
        router.route("/Workgroups/Index", function () {

            require(['workgroup-indexViewModel', 'text!/Workgroups/Index'], function (viewModel, view) {
                loadView(viewModel, view);
            });
        });
        router.route("/Roles/Index", function () {
            require(['roles-indexViewModel', 'text!/Roles/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#rolesgrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });
        router.route("/Roles/Edit/:id", function () {
            require(['roles-editViewModel', 'text!/Roles/Edit'], function (viewModel, view) {
                loadView(viewModel.loadData(), view);
                kendo.bind($("#form"), viewModel);

            });
        });
        router.route("/Systems/Index", function () {
            require(['systems-indexViewModel', 'text!/Systems/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#systemsTree"), viewModel);
                });
            });
        });
        router.route("/Systems/Edit/:status/:id", function () {
            require(['systems-editViewModel', 'text!/Systems/Edit'], function (viewModel, view) {
                loadView(viewModel.loadData(), view);
                kendo.bind($("#form"), viewModel);

            });
        });
        router.route("/home/index", function () {
            require(['text!/home/index'], function (view) {
                loadView(null, view);
            });
        });

        router.route("/Exceptions/Index", function () {
            require(['exceptions-indexViewModel', 'text!/Exceptions/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#exceptionsgrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });

        router.route("/UserSessions/Index", function () {
            require(['userSessions-indexViewModel', 'text!/UserSessions/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#sessionsgrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });

        router.route("/Pages/Index", function () {
            require(['pages-indexViewModel', 'text!/Pages/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#pagesgrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });

        router.route("/Actions/Index", function () {
            require(['actions-indexViewModel', 'text!/Actions/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#actionsgrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });

        //router.route("/Pages/Edit/:id", function () {
        //    require(['pages-editViewModel', 'text!/Pages/Edit'], function (viewModel, view) {
        //        loadView(viewModel.loadData(), view);
        //        kendo.bind($("#form"), viewModel);

        //    });
        //});

        //router.route("/home/about", function() {
        //    require(['text!/home/about'], function(view) {
        //        loadView(null, view);
        //    });
        //});

        //router.route("/home/contact", function () {
        //    require(['text!/home/contact'], function (view) {
        //        loadView(null, view);
        //    });
        //});
        //router.route("/Users/Index", function () {
        //    require(['text!/Users/Index'], function (view) {
        //        loadView(null, view);
        //    });
        //});
        //router.route("/Roles/Index", function () {
        //    require(['text!/Roles/Index'], function (view) {
        //        loadView(null, view);
        //    });
        //});
        //router.route("/Pages/Index", function () {
        //    require(['text!/pages/index'], function (view) {
        //        loadView(null, view);
        //    });
        //});

        //router.route("/Systems/Index", function () {
        //    require(['text!/Systems/Index'], function (view) {
        //        loadView(null, view);
        //    });
        //});
        //router.route("/Parameters/Index", function () {
        //    require(['text!/Parameters/Index'], function (view) {
        //        loadView(null, view);
        //    });
        //});        


        var loadView = function (viewModel, view, delegate) {
            var kendoView = new kendo.View(view, { model: viewModel });
            kendo.fx($("#content")).slideInRight().reverse().then(function () {
                layout.showIn("#content", kendoView);
                if (viewModel.onLoad != undefined)
                    viewModel.onLoad();
                if (delegate != undefined)
                    delegate();

                kendo.fx($("#content")).slideInRight().play();
            });
        };

        return router;
    });