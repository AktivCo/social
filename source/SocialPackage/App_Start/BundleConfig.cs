using System.Web;
using System.Web.Optimization;

namespace SocialPackage
{
    public class BundleConfig
    {
        // Дополнительные сведения о Bundling см. по адресу http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-browser.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/moment.js", "~/Scripts/bootstrap-datetimepicker.js", "~/Scripts/bootstrap-datepicker.ru.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/ui-bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/textAngular").Include(
                "~/Scripts/textAngular-rangy.js",
                "~/Scripts/textAngular-sanitize.js",
                "~/Scripts/textAngular.js"
                )); 

            bundles.Add(new ScriptBundle("~/bundles/angular_aktiv").Include(
                        "~/Scripts/angular/app.js",
                         "~/Scripts/angular/directives.js",
                         "~/Scripts/angular/controllers.js",
                         "~/Scripts/angular/filters.js",
                         "~/Scripts/angular/services.js",
                         "~/Scripts/angular/date.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/angular_upload").Include("~/Scripts/angular-file-upload.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/style.css", "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/textAngular").Include("~/Content/textAngular.css"));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}