﻿using System.Web;
using System.Web.Optimization;

namespace DupacoGarageSale.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.9.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.11.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/default.js",
                        "~/Scripts/garagesale.itinerary.js",
                        "~/Scripts/html5shiv.js",
                        "~/Scripts/respond.js"));      

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/lightboxjs").Include(
                        "~/Content/lightbox/js/lightbox.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminjs").Include(
                        "~/Scripts/admin.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                        "~/Scripts/jquery.timepicker.js"));

            

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/jquery-ui-1.11.3.css",
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

            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include(
                       "~/Content/bootstrap/css/bootstrap.css",
                       "~/Content/bootstrap/css/bootstrap-theme.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard/css").Include(
                        "~/Content/bootstrap/css/dashboard.css"));

           bundles.Add(new StyleBundle("~/Content/timepicker/css").Include(
                      "~/Content/timepicker/jquery.timepicker.css"));

            bundles.Add(new StyleBundle("~/Content/lightbox/css").Include(
                      "~/Content/lightbox/css/lightbox.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            
        }
    }
}