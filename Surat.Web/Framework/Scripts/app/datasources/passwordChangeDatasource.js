define(['kendo', 'passwordChangeModel', 'util'],
  function (kendo, passwordChangeModel, util) {


      var passwordChangeDatasource = new kendo.data.DataSource({

          transport: {
              create: {
                  async: false,
                  type: "POST",
                  url: "/Account/UserPasswordChange",
                  dataType: "json",
                  complete: function (jqXhr, textStatus) {
                      if (textStatus == "success") {
                          var result = jQuery.parseJSON(jqXhr.responseText);
                          _notification.info(result.Result);
                      }
                  }
              },
          },
          cache: false,
          schema: {
              model: passwordChangeModel
          },
          error: function (e) {
              util.errorHandler(e);
          }
      });

      return passwordChangeDatasource;

  });