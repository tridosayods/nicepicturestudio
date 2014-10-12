using System.Web.Optimization;

namespace NicePictureStudio
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js")
                        );

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*", "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                       "~/Scripts/kendo/kendo.all.min.js",
                       "~/Scripts/kendo/kendo.timezones.min.js",
                       "~/Scripts/kendo/kendo.aspnetmvc.min.js",
                       "~/Scripts/kendo/kendo.web.min.js"
                       //"~/Scripts/kendo/kendo.mobile.min.js"
                       
                       ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datetimepicker.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Themes/base/*.css",
                      "~/Content/bootstrap-theme.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/styles/css").Include(
                      "~/Content/styles/kendo.common-bootstrap.min.css",
                      "~/Content/styles/kendo.bootstrap.min.css",
                      //"~/Content/styles/kendo.common.min.css",
                      //"~/Content/styles/kendo.blueopal.min.css",
                       // "~/Content/styles/kendo.silver.min.css",
                      "~/Content/font-awesome-4.2.0/css/font-awesome.css"
                      ));
            bundles.IgnoreList.Clear();
        }
    }
}
