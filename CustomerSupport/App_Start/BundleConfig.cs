using System.Web;
using System.Web.Optimization;

namespace CustomerSupport
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                        "~/Scripts/inputmask/jquery.inputmask.js"));

            //// Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            //// para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));
            //"~/Content/assets/js/jquery.min.js",

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Content/assets/popper.js.1.16.1/umd/popper.js",
                    //"~/Content/assets/popper.js.1.16.1/umd/popper.min.js",
                    //"~/Content/assets/js/bootstrap.min.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Content/assets/js/jquery.magnific-popup.min.js",
                    "~/Content/assets/js/isotope.pkgd.min.js",
                    "~/Content/assets/js/swiper.min.js",
                    "~/Content/assets/js/wow.min.js",
                    "~/Content/assets/js/script.js",
                    "~/Content/assets/plugins/summernote/summernote-bs4.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/assets/css/bootstrap.min.css",
                    "~/Content/assets/css/fonts.css",
                    "~/Content/assets/css/fontawesome.min.css",
                    "~/Content/assets/css/magnific-popup.css",
                    "~/Content/assets/css/swiper.min.css",
                    "~/Content/assets/css/animate.css",
                    "~/Content/assets/css/bootstrap.css",
                     "~/Content/assets/css/dataTables/demo_table.css",
                    "~/Content/assets/css/dataTables/demo_table_jui.css",
                    "~/Content/assets/plugins/summernote/summernote-bs4.min.css",
                    "~/Content/assets/css/style.css",
                    "~/Content/assets/plugins/select2/css/select2.min.css",
                    "~/Content/assets/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css"
                    ));

            //"~/Content/assets/plugins/select2/css/select2.css",
            //        "~/Content/assets/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css",

                    

            // "~/Content/assets/css/dataTables.bootstrap4.min.css",
            //"~/Content/assets/css/dataTables/demo_page.css",
            //         "~/Content/assets/css/dataTables/demo_table.css",
            //         "~/Content/assets/css/dataTables/demo_table_jui.css",
            //         "~/Content/assets/css/themes/base/jquery-ui.css",
            //         "~/Content/assets/css/themes/smoothness/jquery-ui-1.7.2.custom.css"
        }
    }
}
