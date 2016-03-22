require.config({
    paths: {
        //packages
        //'jquery': '/scripts/jquery-2.0.3.min',
        'kendo': '/Framework/Scripts/kendo2015.1.408/js/kendo.web.min',
        'text': '/Framework/Scripts/text',
        'router': '/Framework/Scripts/app/router',
        //models
        'userModel': 'models/userModel',       
        'bankModel': 'models/bankModel',
        'paymentCollectingModel': 'models/paymentCollectingModel',
        //viewModels        
        'user-indexViewModel': 'viewModels/user/indexViewModel',
        'user-editViewModel': 'viewModels/user/editViewModel',
        'bankalar-indexViewModel': 'viewModels/bankalar/indexViewModel',
        'bankalar-editViewModel': 'viewModels/bankalar/editViewModel',
        'paymentCollecting-indexViewModel': 'viewModels/paymentCollecting/indexViewModel',
        'paymentCollecting-editViewModel': 'viewModels/paymentCollecting/editViewModel',
        //datasources       
        'userDatasource': 'datasources/userDatasource',
        'bankDatasource': 'datasources/bankDatasource',
        'paymentCollectingDatasource': 'datasources/paymentCollectingDatasource',
        // utils
        'util': '/Framework/Scripts/util'
    },
    /*shim: {
        'kendo': ['jquery']
    },*/
    priority: ['text', 'router', 'app'],
    jquery: '2.0.3',
    waitSeconds: 30
});

require([
        'app'
    ], function(app) {
        app.initialize();
    });