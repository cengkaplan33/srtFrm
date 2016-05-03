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
        // region #BASE Model#
        'userModel': 'models/userModel',
        'rolesModel': 'models/rolesModel',
        'rolePagesModel': 'models/rolePagesModel',
        'userRolesModel': 'models/userRolesModel',
        'workgroupModel': 'models/workgroupModel',
        'choosedWorkgroupModel': 'models/choosedWorkgroupModel',
        'roleActionsModel': 'models/roleActionsModel',
        'userPagesModel': 'models/userPagesModel',
        'userActionsModel': 'models/userActionsModel',
        'pagesModel': 'models/pagesModel',
        'exceptionsModel': 'models/exceptionsModel',
        'actionsModel': 'models/actionsModel',
        'userSessionsModel': 'models/userSessionsModel',
        'systemsModel': 'models/systemsModel',
        'passwordChangeModel': 'models/passwordChangeModel',
        //endregion #BASE Model#
        'bankModel': 'models/bankModel',
        'paymentCollectingModel': 'models/paymentCollectingModel',
        'odemeTalepModel': 'models/odemeTalepModel',
        'odemeTuruModel': 'models/odemeTuruModel',
        'odemeTuruModel': 'models/odemeTuruModel',
        'odemeDurumuModel': 'models/odemeDurumuModel',
        'hazirDegerlerTablosuModel': 'models/hazirDegerlerTablosuModel',

        //viewModels    
        // region #BASE viewModels#
        'user-indexViewModel': 'viewModels/user/indexViewModel',
        'user-editViewModel': 'viewModels/user/editViewModel',
        'roles-indexViewModel': 'viewModels/roles/indexViewModel',
        'roles-editViewModel': 'viewModels/roles/editViewModel',
        'workgroup-indexViewModel': 'viewModels/workgroup/indexViewModel',
        'workgroup-editViewModel': 'viewModels/workgroup/editViewModel',
        'exceptions-indexViewModel': 'viewModels/exceptions/indexViewModel',
        'pages-indexViewModel': 'viewModels/pages/indexViewModel',
        'actions-indexViewModel': 'viewModels/actions/indexViewModel',
        'userSessions-indexViewModel': 'viewModels/userSessions/indexViewModel',
        'systems-indexViewModel': 'viewModels/systems/indexViewModel',
        'systems-editViewModel': 'viewModels/systems/editViewModel',
        'account-passwordChangeViewModel': 'viewModels/account/passwordChangeViewModel',
        //endregion #BASE viewModels#
        'bankalar-indexViewModel': 'viewModels/bankalar/indexViewModel',
        'bankalar-editViewModel': 'viewModels/bankalar/editViewModel',
        'paymentCollecting-indexViewModel': 'viewModels/paymentCollecting/indexViewModel',
        'paymentCollecting-editViewModel': 'viewModels/paymentCollecting/editViewModel',
        'odemeTalep-indexViewModel': 'viewModels/odemetalep/indexViewModel',
        'tahsilatTalep-indexViewModel': 'viewModels/tahsilattalep/indexViewModel',
        'durumtanimlari-indexViewModel': 'viewModels/durumtanimlari/indexViewModel',
        'durumtanimlari-editViewModel': 'viewModels/durumtanimlari/editViewModel',
        'hazirdegerlertablosu-indexViewModel': 'viewModels/hazirdegerlertablosu/indexViewModel',
        'hazirdegerlertablosu-tanimViewModel': 'viewModels/hazirdegerlertablosu/tanimViewModel',


        //datasources    
        // region #BASE datasources#
        'userDatasource': 'datasources/userDatasource',
        'rolesDatasource': 'datasources/rolesDatasource',
        'rolePagesDatasource': 'datasources/rolePagesDatasource',
        'roleActionsDatasource': 'datasources/roleActionsDatasource',
        'userRolesDatasource': 'datasources/userRolesDatasource',
        'userWorkGroupDataSource': 'datasources/userWorkGroupDataSource',
        'workgroupDatasource': 'datasources/workgroupDatasource',
        'userPagesDatasource': 'datasources/userPagesDatasource',
        'userChoosenWorkgroupDatasource': 'datasources/userChoosenWorkgroupDatasource',
        'userActionsDatasource': 'datasources/userActionsDatasource',
        'exceptionsDatasource': 'datasources/exceptionsDatasource',
        'pagesDatasource': 'datasources/pagesDatasource',
        'actionsDatasource': 'datasources/actionsDatasource',
        'userSessionsDatasource ': 'datasources/userSessionsDatasource ',
        'systemsDatasource': 'datasources/systemsDatasource',
        'passwordChangeDatasource': 'datasources/passwordChangeDatasource',
        //endregion  #BASE datasources#
        'bankDatasource': 'datasources/bankDatasource',
        'paymentCollectingDatasource': 'datasources/paymentCollectingDatasource',
        'odemeTalepDatasource': 'datasources/odemeTalepDatasource',
        'tahsilatTalepDatasource': 'datasources/tahsilatTalepDatasource',
        'odemeTuruDatasource': 'datasources/odemeTuruDatasource',
        'tahsilatTuruDatasource': 'datasources/tahsilatTuruDatasource',
        'odemeDurumuDatasource': 'datasources/odemeDurumuDatasource',
        'OdemeEkraniDurumTanimiDatasource': 'datasources/OdemeEkraniDurumTanimiDatasource',
        'TahsilatEkraniDurumTanimiDatasource': 'datasources/TahsilatEkraniDurumTanimiDatasource',
        'hazirDegerlerTablosuDatasource': 'datasources/hazirDegerlerTablosuDatasource',
        'hazirDegerlerTablosuTanimDatasource':'datasources/hazirDegerlerTablosuTanimDatasource',
       
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