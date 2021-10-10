using System.Web;
using System.Web.Optimization;

namespace CustomerSupport
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //"~/Scripts/jquery-{version}.js"     "~/Content/assets/js/jquery.js"
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //// Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            //// para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/umd/popper.js",
                    //"~/Content/assets/popper.js.1.16.1/umd/popper.js",
                    "~/Scripts/bootstrap.bundle.min.js",
                    "~/Content/assets/plugins/chart.js/Chart.min.js",
                    "~/Content/assets/js/bootstrap.min.js",
                    "~/Content/assets/js/jquery.magnific-popup.min.js",
                    "~/Content/assets/js/isotope.pkgd.min.js",
                    //"~/Content/assets/js/swiper.min.js", //Se paso al layout
                    "~/Content/assets/js/wow.min.js",
                    //"~/Content/assets/js/script.js", //Se paso al layout
                    "~/Content/assets/js/respond.js"
                    //"~/Scripts/tinymce/tinymce.min.js", //Se paso al layout
                    //"~/Scripts/tinymce/jquery.tinymce.min.js" //Se paso al layout
                    ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                    "~/Content/assets/css/fonts.css",
                    "~/Content/assets/css/fontawesome.min.css",
                    "~/Content/assets/css/bootstrap.min.css",
                    "~/Content/assets/css/magnific-popup.css",
                    //"~/Content/assets/css/swiper.min.css", //Se paso al layout
                    "~/Content/assets/css/animate.css",                    
                    "~/Content/assets/css/dataTables/demo_table.css",
                    "~/Content/assets/css/dataTables/demo_table_jui.css",
                    "~/Content/assets/css/style.css",
                    //"~/Content/assets/plugins/select2/css/select2.css", //Se paso al layout
                    "~/Content/assets/css/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                        "~/Scripts/inputmask/jquery.inputmask.min.js"));
        }
    }
}
