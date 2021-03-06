﻿require.config({
    paths: {
        //packages
        //'jquery': '/scripts/jquery-2.0.3.min',
        'kendo': '/Framework/Scripts/kendo2015.1.408/js/kendo.web.min',
        'text': '/Framework/Scripts/text',
        'router': '/Framework/Scripts/app/router',
        //models
        'userModel': 'models/userModel',
        'rolesModel': 'models/rolesModel',
        'rolePagesModel': 'models/rolePagesModel',
        'workgroupModel': 'models/workgroupModel',
        'choosedWorkgroupModel': 'models/choosedWorkgroupModel',
        'systemsModel': 'models/systemsModel',
        'passwordChangeModel': 'models/passwordChangeModel',
        'exceptionsModel': 'models/exceptionsModel',
        'userSessionsModel': 'models/userSessionsModel',
        'pagesModel': 'models/pagesModel',
        'actionsModel': 'models/actionsModel',
        'userRolesModel': 'models/userRolesModel',
        'userPagesModel': 'models/userPagesModel',
        'roleActionsModel': 'models/roleActionsModel',
        'userActionsModel': 'models/userActionsModel',
        'userMasterDbVeritabanlariModel': 'models/userMasterDbVeritabanlariModel',
        //viewModels
        'user-indexViewModel': 'viewModels/user/indexViewModel',
        'user-editViewModel': 'viewModels/user/editViewModel',
        'workgroup-indexViewModel': 'viewModels/workgroup/indexViewModel',
        'roles-indexViewModel': 'viewModels/roles/indexViewModel',
        'roles-editViewModel': 'viewModels/roles/editViewModel',
        'systems-indexViewModel': 'viewModels/systems/indexViewModel',
        'systems-editViewModel': 'viewModels/systems/editViewModel',
        'account-passwordChangeViewModel': 'viewModels/account/passwordChangeViewModel',
        'exceptions-indexViewModel': 'viewModels/exceptions/indexViewModel',
        'userSessions-indexViewModel': 'viewModels/userSessions/indexViewModel',
        'pages-indexViewModel': 'viewModels/pages/indexViewModel',
        'pages-editViewModel': 'viewModels/pages/editViewModel',
        'actions-indexViewModel': 'viewModels/actions/indexViewModel',
        //datasources
        'userDatasource': 'datasources/userDatasource',
        'rolesDatasource': 'datasources/rolesDatasource',
        'workgroupDatasource': 'datasources/workgroupDatasource',
        'systemsDatasource': 'datasources/systemsDatasource',
        'passwordChangeDatasource': 'datasources/passwordChangeDatasource',
        'exceptionsDatasource': 'datasources/exceptionsDatasource',
        'rolePagesDatasource': 'datasources/rolePagesDatasource',
        'roleActionsDatasource': 'datasources/roleActionsDatasource',
        'userSessionsDatasource ': 'datasources/userSessionsDatasource ',
        'pagesDatasource': 'datasources/pagesDatasource',
        'actionsDatasource': 'datasources/actionsDatasource',
        'userRolesDatasource': 'datasources/userRolesDatasource',
        'userWorkGroupDataSource': 'datasources/userWorkGroupDataSource',
        'userPagesDatasource': 'datasources/userPagesDatasource',
        'userChoosenWorkgroupDatasource': 'datasources/userChoosenWorkgroupDatasource',
        'userActionsDatasource': 'datasources/userActionsDatasource',
        'userMasterDbVeritabanlariDatasource': 'datasources/userMasterDbVeritabanlariDatasource',
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
], function (app) {
    app.initialize();
});