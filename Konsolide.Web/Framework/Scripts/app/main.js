require.config({
    paths: {
        //packages
        //'jquery': '/scripts/jquery-2.0.3.min',
        kendo: '/Framework/Scripts/kendo2015.1.408/js/kendo.web.min',
        kendo_culture_config: '/Framework/Scripts/kendo2015.1.408/js/cultures/kendo.culture.tr-TR.min',
        kendo_culture_messages: '/Framework/Scripts/kendo2015.1.408/js//messages/kendo.messages.tr-TR.min',
        text: '/Framework/Scripts/text',
        router: '/Framework/Scripts/app/router',
        //models
        'userModel': 'models/userModel',       
        'bankModel': 'models/bankModel',
        'paymentCollectingModel': 'models/paymentCollectingModel',
        'odemeTalepModel': 'models/odemeTalepModel',
        'odemeTuruModel': 'models/odemeTuruModel',
        'odemeTuruModel': 'models/odemeTuruModel',
        'odemeDurumuModel': 'models/odemeDurumuModel',
        'userRolesModel': 'models/userRolesModel',
        'workgroupModel': 'models/workgroupModel',
        //viewModels        
        'user-indexViewModel': 'viewModels/user/indexViewModel',
        'user-editViewModel': 'viewModels/user/editViewModel',
        'bankalar-indexViewModel': 'viewModels/bankalar/indexViewModel',
        'bankalar-editViewModel': 'viewModels/bankalar/editViewModel',
        'paymentCollecting-indexViewModel': 'viewModels/paymentCollecting/indexViewModel',
        'paymentCollecting-editViewModel': 'viewModels/paymentCollecting/editViewModel',
        'odemeTalep-indexViewModel': 'viewModels/odemetalep/indexViewModel',
        'tahsilatTalep-indexViewModel': 'viewModels/tahsilattalep/indexViewModel',
        'durumtanimlari-indexViewModel': 'viewModels/durumtanimlari/indexViewModel',
        'durumtanimlari-editViewModel': 'viewModels/durumtanimlari/editViewModel',
        'workgroup-indexViewModel': 'viewModels/workgroup/indexViewModel',
        'workgroup-editViewModel': 'viewModels/workgroup/editViewModel',
        //datasources       
        'userDatasource': 'datasources/userDatasource',
        'bankDatasource': 'datasources/bankDatasource',
        'paymentCollectingDatasource': 'datasources/paymentCollectingDatasource',
        'odemeTalepDatasource': 'datasources/odemeTalepDatasource',
        'tahsilatTalepDatasource': 'datasources/tahsilatTalepDatasource',
        'odemeTuruDatasource': 'datasources/odemeTuruDatasource',
        'tahsilatTuruDatasource': 'datasources/tahsilatTuruDatasource',
        'odemeDurumuDatasource': 'datasources/odemeDurumuDatasource',
        'OdemeEkraniDurumTanimiDatasource': 'datasources/OdemeEkraniDurumTanimiDatasource',
        'TahsilatEkraniDurumTanimiDatasource': 'datasources/TahsilatEkraniDurumTanimiDatasource',
        'userRolesDatasource': 'datasources/userRolesDatasource',
        'workgroupDatasource': 'datasources/workgroupDatasource',
        // utils
        util: '/Framework/Scripts/util'
    },
        shim: {
            'kendo_culture_config': {
                deps: ['kendo']
            },
            'kendo_culture_messages': {
                    deps: ['kendo']
            },
            'kendo': {
                    exports: 'kendo'
            }
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