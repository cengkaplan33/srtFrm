using System.Web;
using System.Web.Optimization;

namespace Surat.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/js/jquery/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/Bundles/css").Include(
                     "~/theme/css/bootstrap.min.css",
                     "~/theme/css/ace-rtl.min.css",
                     "~/theme/css/ace-skins.min.css")
             .Include("~/theme/css/font-awesome.min.css", new CssRewriteUrlTransformWrapper())
             .Include("~/theme/css/ace-fonts.css")
             .Include("~/theme/css/ace.min.css")
             .Include("~/theme/css/jquery-ui-1.10.3.custom.min.css",
             "~/theme/css/chosen.min.css",
              "~/theme/css/chosen.min.css",
             "~/theme/css/datepicker.min.css",
             "~/theme/css/bootstrap-timepicker.min.css",
             "~/theme/css/daterangepicker.min.css",
             "~/theme/css/colorpicker.min.css",
             "~/theme/css/jReject.css",
             "~/theme/css/bootstrap-dialog.min.css",
             "~/kendo/styles/kendo.common.min.css",
             "~/kendo/styles/kendo.default.min.css",
             "~/kendo/styles/kendo.dataviz.min.css",
             "~/kendo/styles/kendo.default.mobile.min.css"
             )
             .Include("~/theme/css/kaynak.web.css"));

            bundles.Add(new ScriptBundle("~/Bundles/angular").Include(
                        "~/src/app.js",
                        "~/src/controllers/loginController.js",
                        "~/src/controllers/companyEditController.js",
                        "~/src/controllers/branchEditController.js",
                        "~/src/controllers/userEditController.js",
                        "~/src/directives/position.js",
                        "~/src/directives/timepicker.js",
                        "~/src/directives/timepicker-tpl.js",
                        "~/src/services/loginService.js"
                        ));

            bundles.Add(new ScriptBundle("~/Bundles/js").Include(
                "~/js/jquery-2.1.1.js",
                 "~/js/angular/angular.js",
                "~/js/ace-extra.min.js",
                "~/js/bootstrap/bootstrap.min.js",
                "~/js/jquery/jquery-ui-1.10.3.full.min.js",
                "~/js/jquery/jquery.ui.touch-punch.min.js",
                "~/js/uncompressed/bootbox.js",
                "~/js/uncompressed/example.js",
                "~/js/ace-elements.min.js",
                "~/js/ace.min.js",
                "~/js/date-time/bootstrap-datepicker.min.js",
                "~/js/date-time/bootstrap-timepicker.min.js",
                "~/js/date-time/daterangepicker.min.js",
                "~/js/chosen.jquery.min.js",
                "~/js/jquery/jquery.autosize.min.js",
                "~/js/jquery/jquery.maskedinput.min.js",
                "~/js/bootstrap/bootstrap-colorpicker.min.js",
                "~/js/jquery/jquery.knob.min.js",
                "~/js/bootstrap/bootstrap-tag.min.js",
                "~/js/additional-methods.min.js",
                "~/js/uncompressed/jquery.validate.js",
                "~/js/bootstrap-dialog/dist/js/bootstrap-dialog.min.js",
                "~/js/bootstrap-ui/ui-bootstrap-0.12.0.min.js",
                "~/js/bootstrap-ui/ui-bootstrap-tpls-0.12.0.min.js",
                "~/js/jReject.js",
                "~/kendo/js/kendo.all.min.js",
                "~/kendo/js/cultures/kendo.culture.tr-TR.min.js"
                ));


            BundleTable.EnableOptimizations = false;
        }

        public class CssRewriteUrlTransformWrapper : IItemTransform
        {
            public string Process(string includedVirtualPath, string input)
            {
                return new CssRewriteUrlTransform().Process(
                    "~" + VirtualPathUtility.ToAbsolute(includedVirtualPath),
                    input);
            }
        }
    }

}
