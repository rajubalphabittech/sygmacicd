using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace atm
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
            //								"~/Scripts/WebForms/WebForms.js",
            //								"~/Scripts/WebForms/WebUIValidation.js",
            //								"~/Scripts/WebForms/MenuStandards.js",
            //								"~/Scripts/WebForms/Focus.js",
            //								"~/Scripts/WebForms/GridView.js",
            //								"~/Scripts/WebForms/DetailsView.js",
            //								"~/Scripts/WebForms/TreeView.js",
            //								"~/Scripts/WebForms/WebParts.js"));

            //// Order is very important for these files to work, they have explicit dependencies
            //bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
            //				"~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
            //				"~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
            //				"~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
            //				"~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            bundles.Add(new ScriptBundle("~/bundles/atm-ui").NonOrdering().Include(
                                            "~/Scripts/AJAX.js",
                                            "~/Scripts/jquery.timepicker.min.js"));

            //// Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                                            "~/Scripts/modernizr-2.8.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/routemanager").Include(
                                            "~/Scripts/ajaxhelpers.js",
                                            "~/Scripts/datehelpers.js",
                                            "~/Scripts/routecomment.js",
                                            "~/Scripts/routemanager.js"));

            bundles.Add(new ScriptBundle("~/bundles/routetracker").Include(
                                            "~/Scripts/ajaxhelpers.js",
                                            "~/Scripts/datehelpers.js",
                                            "~/Scripts/routecomment.js",
                                            "~/Scripts/routetracker.js",
                                            "~/Scripts/multi-select.js"));

            bundles.Add(new ScriptBundle("~/bundles/routenotification").Include(
                                            "~/Scripts/ajaxhelpers.js",
                                            "~/Scripts/datehelpers.js",
                                            "~/Scripts/routecomment.js",
                                            "~/Scripts/routenotification.js"));

            //ScriptManager.ScriptResourceMapping.AddDefinition(
            //		"respond",
            //		new ScriptResourceDefinition
            //		{
            //			Path = "~/Scripts/respond.min.js",
            //			DebugPath = "~/Scripts/respond.js",
            //		});
        }
    }

    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }


    static class BundleExtentions
    {
        public static Bundle NonOrdering(this Bundle bundle)
        {
            bundle.Orderer = new NonOrderingBundleOrderer();
            return bundle;
        }
    }
}