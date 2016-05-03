define(['kendo', 'passwordChangeDatasource', 'passwordChangeModel', 'router','util'],
function (kendo, passwordChangeDatasource, passwordChangeModel, router,util) {
    var onClick = function (event, delegate) {
        event.preventDefault();

    };

    var indexViewModel = new kendo.data.ObservableObject({


        changeUserPassword: function (e) {
            var validator = $("#form").kendoValidator().data("kendoValidator")
            if (validator.validate()) {

                passwordChangeDatasource.data([]);
                indexViewModel.set("Password", new passwordChangeModel());
                var userPassword = indexViewModel.Password;
                userPassword.DefaultPassword = indexViewModel.DefaultPassword;
                userPassword.NewPassword = indexViewModel.NewPassword;
                userPassword.NewPasswordAgain = indexViewModel.NewPasswordAgain;
                passwordChangeDatasource.add(userPassword);
                passwordChangeDatasource.sync();






            }
        },
        cancel: function (e) {
            window.location.href = "/Home/Anasayfa";
        },
        dataSource: passwordChangeDatasource


    });
    return indexViewModel;
});