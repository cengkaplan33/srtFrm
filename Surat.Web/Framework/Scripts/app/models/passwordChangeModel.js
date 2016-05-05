define(['kendo'],
   function (kendo) {
       var passwordChangeModel = new kendo.data.Model.define({
           fields: {
               DefaultPassword: { type: "string", validation: { required: true } },
               NewPassword: { type: "string", validation: { required: true } },
               NewPasswordAgain: { type: "string", validation: { required: true } }
           }
       });
       return passwordChangeModel;
   });