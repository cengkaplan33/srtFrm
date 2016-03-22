﻿
define(['kendo'],
    function(kendo) {
        var router = new kendo.Router(),
            layout = new kendo.Layout("<div id='content'></div>");

        layout.render($("#app"));

        
        //router.route("/", function() {
        //    require(['text!/home/index'], function(view) {
        //        loadView(null, view);
        //    });
        //});
      
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
        router.route("/Bankalar/Index", function () {
            require(['bankalar-indexViewModel', 'text!/Bankalar/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#bankgrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });
        router.route("/Bankalar/Edit/:id", function () {
            require(['bankalar-editViewModel', 'text!/Bankalar/Edit'], function (viewModel, view) {
                loadView(viewModel.loadData(), view);
                kendo.bind($("#form"), viewModel);

            });
        });

        router.route("/PaymentCollecting/Index", function () {
            require(['paymentCollecting-indexViewModel', 'text!/paymentCollecting/Index'], function (viewModel, view) {
                loadView(viewModel, view, function () {
                    kendo.bind($("#payColgrid").find(".k-grid-toolbar"), viewModel);
                });
            });
        });

        router.route("/PaymentCollecting/Edit/:id", function () {
            require(['paymentCollecting-editViewModel', 'text!/paymentCollecting/Edit'], function (viewModel, view) {
                loadView(viewModel.loadData(), view);
                kendo.bind($("#form"), viewModel);

            });
        });
        //router.route("/home/index", function() {
        //    require(['text!/home/index'], function(view) {
        //        loadView(null, view);
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

                if (delegate != undefined)
                    delegate();

                kendo.fx($("#content")).slideInRight().play();
            });
        };

        return router;
    });