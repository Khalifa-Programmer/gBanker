using System.Web;
using System.Web.Optimization;

namespace gBanker.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/gapi").Include(
                       "~/Content/ui-libs/google-chart/jsapi"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jtable").Include(
                "~/Scripts/jtable/jquery.jtable.min.js"
                ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/gbUtility").Include(
                        "~/Scripts/gBankerUtilityJs.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Scripts/jtable/themes/lightcolor/blue/jtable.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/core.css",
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/datepicker.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/theme.css"));
            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //           "~/Content/themes/base/jquery.ui.core.css",
            //           "~/Content/themes/base/jquery.ui.resizable.css",
            //           "~/Content/themes/base/jquery.ui.selectable.css",
            //           "~/Content/themes/base/jquery.ui.accordion.css",
            //           "~/Content/themes/base/jquery.ui.autocomplete.css",
            //           "~/Content/themes/base/jquery.ui.button.css",
            //           "~/Content/themes/base/jquery.ui.dialog.css",
            //           "~/Content/themes/base/jquery.ui.slider.css",
            //           "~/Content/themes/base/jquery.ui.tabs.css",
            //           "~/Content/themes/base/jquery.ui.datepicker.css",
            //           "~/Content/themes/base/jquery.ui.progressbar.css",
            //           "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/searchable").Include(
                       "~/Scripts/search_puggin.js"));

            bundles.Add(new ScriptBundle("~/bundles/validateunobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"
                        ));
        }
    }
}