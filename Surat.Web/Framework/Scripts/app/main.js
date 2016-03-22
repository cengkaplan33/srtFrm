require.config({
    paths: {
        //packages
        //'jquery': '/scripts/jquery-2.0.3.min',
        'kendo': '/Framework/Scripts/kendo2015.1.408/js/kendo.web.min',
        'text': '/Framework/Scripts/text',
        'router': '/Framework/Scripts/app/router',
        //models
        'userModel': 'models/userModel',
        'rolesModel': 'models/rolesModel',
        'workgroupModel': 'models/workgroupModel',
        'systemsModel': 'models/systemsModel',
        'exceptionsModel': 'models/exceptionsModel',
        //viewModels
        'user-indexViewModel': 'viewModels/user/indexViewModel',
        'user-editViewModel': 'viewModels/user/editViewModel',
        'workgroup-indexViewModel': 'viewModels/workgroup/indexViewModel',
        'workgroup-editViewModel': 'viewModels/workgroup/editViewModel',
        'roles-indexViewModel':'viewModels/roles/indexViewModel',
        'roles-editViewModel': 'viewModels/roles/editViewModel',
        'systems-indexViewModel': 'viewModels/systems/indexViewModel',
        'systems-editViewModel': 'viewModels/systems/editViewModel',
        'exceptions-indexViewModel': 'viewModels/exceptions/indexViewModel',
        //datasources
        'userDatasource': 'datasources/userDatasource',
        'rolesDatasource': 'datasources/rolesDatasource',
        'workgroupDatasource': 'datasources/workgroupDatasource',
        'systemsDatasource': 'datasources/systemsDatasource',
        'exceptionsDatasource': 'datasources/exceptionsDatasource',
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