using System.Web;
using System.Web.Optimization;

namespace Surat.Web
{
    public static class BundleConfiguration
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/css").Include(
                     "~/Framework/theme/css/bootstrap.min.css",
                     "~/Framework/theme/css/ace-rtl.min.css",
                     "~/Framework/theme/css/ace-skins.min.css")
             .Include("~/Framework/theme/css/font-awesome.min.css", new CssRewriteUrlTransformWrapper())
             .Include("~/Framework/theme/css/ace-fonts.css")
             .Include("~/Framework/theme/css/ace.min.css")
             .Include("~/Framework/theme/css/jquery-ui-1.10.3.custom.min.css",
             "~/Framework/theme/css/chosen.min.css",
              "~/Framework/theme/css/chosen.min.css",
             "~/Framework/theme/css/datepicker.min.css",
             "~/Framework/theme/css/bootstrap-timepicker.min.css",
             "~/Framework/theme/css/daterangepicker.min.css",
             "~/Framework/theme/css/colorpicker.min.css",
             "~/Framework/theme/css/jReject.css",
             "~/Framework/theme/css/bootstrap-dialog.min.css",
             "~/Framework/kendo/styles/kendo.common.min.css",
             "~/Framework/kendo/styles/kendo.default.min.css",
             "~/Framework/kendo/styles/kendo.dataviz.min.css",
             "~/Framework/kendo/styles/kendo.default.mobile.min.css"
             )
             .Include("~/Framework/theme/css/kaynak.web.css"));
            bundles.Add(new StyleBundle("~/Bundles/FrameworkStyle").Include(
                "~/Framework/css/framework.css"
                ));

            bundles.Add(new ScriptBundle("~/Bundles/angular").Include(
                        "~/Framework/Scripts/app/app.js"
                        ));

            bundles.Add(new ScriptBundle("~/Bundles/js").Include(
                "~/Framework/js/jquery-2.1.1.js",
                "~/Framework/js/ace-extra.min.js",
                "~/Framework/js/bootstrap/bootstrap.min.js",
                "~/Framework/js/jquery/jquery-ui-1.10.3.full.min.js",
                "~/Framework/js/jquery/jquery.ui.touch-punch.min.js",
                "~/Framework/js/uncompressed/bootbox.js",
                "~/Framework/js/uncompressed/example.js",
                "~/Framework/js/ace-elements.min.js",
                "~/Framework/js/ace.min.js",
                "~/Framework/js/date-time/bootstrap-datepicker.min.js",
                "~/Framework/js/date-time/bootstrap-timepicker.min.js",
                "~/Framework/js/date-time/daterangepicker.min.js",
                "~/Framework/js/chosen.jquery.min.js",
                "~/Framework/js/jquery/jquery.autosize.min.js",
                "~/Framework/js/jquery/jquery.maskedinput.min.js",
                "~/Framework/js/bootstrap/bootstrap-colorpicker.min.js",
                "~/Framework/js/jquery/jquery.knob.min.js",
                "~/Framework/js/bootstrap/bootstrap-tag.min.js",
                "~/Framework/js/additional-methods.min.js",
                "~/Framework/js/uncompressed/jquery.validate.js",
                "~/Framework/js/bootstrap-dialog/dist/js/bootstrap-dialog.min.js",
                "~/Framework/js/jReject.js",
                "~/Framework/kendo/js/kendo.all.min.js",
                "~/Framework/Scripts/messages.js"
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
