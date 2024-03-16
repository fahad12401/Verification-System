using System.Web;
using System.Web.Optimization;

namespace VerificationSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));




            bundles.Add(new ScriptBundle("~/bundles/Login").Include(
                   "~/UI/Login/jquery/dist/jquery.min.js",
                   "~/UI/Login/bootstrap/dist/js/bootstrap.min.js",
                   "~/UI/Login/iCheck/icheck.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/ui").Include(
                                    "~/UI/Other/bootstrap/dist/css/bootstrap.min.css",
                                    "~/UI/Other/font-awesome/css/font-awesome.min.css",
                                    "~/UI/Other/Ionicons/css/ionicons.min.css",
                                    "~/UI/Dist/css/AdminLTE.min.css",
                                    "~/UI/Dist/css/skins/_all-skins.min.css",
                                    "~/UI/Other/morris.js/morris.css",
                                    "~/UI/Dist/fontsSans.css"
                                    ));

            //for JQuery UI  
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
             "~/Scripts/jquery-ui-{version}.js",
             "~/Scripts/jquery-ui.unobtrusive-{version}.js"));
            //for jquery ui all styles
            bundles.Add(new StyleBundle("~/Content/jqueryui").IncludeDirectory(
                 "~/Content/themes/base", "*.css"
                ));

         

            bundles.Add(new ScriptBundle("~/bundles/ui").Include(
               "~/UI/Other/bootstrap/dist/js/bootstrap.min.js",
               "~/UI/Other/raphael/raphael.min.js",
               "~/UI/Other/morris.js/morris.min.js",
               "~/UI/Other/jquery-knob/dist/jquery.knob.min.js",
               "~/UI/Other/moment/min/moment.min.js",      
               "~/UI/Dist/js/adminlte.min.js",
               "~/Scripts/respond.js",
               "~/UI/Dist/js/demo.js"));

          


            bundles.Add(new StyleBundle("~/Content/Login").Include(
                     "~/UI/Login/bootstrap/dist/css/bootstrap.min.css",
                      "~/UI/Login/font-awesome/css/font-awesome.min.css",
                     "~/UI/Login/Ionicons/css/ionicons.min.css",
                     "~/UI/Login/admin-lte/css/AdminLTE.min.css",
                     "~/UI/Login/iCheck/square/blue.css"));

 
            //Scripts For grid mvc
            bundles.Add(new ScriptBundle("~/bundles/Gridmvc").Include(
                "~/Scripts/gridmvc*"));
            bundles.Add(new StyleBundle("~/Content/Gridmvc").Include(
                "~/Content/Gridmvc.css"));

            //Bundles for bootstrap chosen dropDownList
            bundles.Add(new ScriptBundle("~/bundles/chosen").Include("~/Scripts/chosen.jquery*"));
            bundles.Add(new StyleBundle("~/Content/chosen").Include("~/Content/bootstrap-chosen.css"));

            //Bundles for toastr
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include("~/Scripts/toastr*"));
            bundles.Add(new StyleBundle("~/Content/toastr").Include("~/Content/toastr*"));

            //Bootbox
            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include("~/Scripts/bootbox*"));


            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                           "~/Scripts/DataTables/jquery.dataTables.min.js"));


            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                      "~/Content/DataTables/css/jquery.dataTables.min.css"));
        }
    }
}
