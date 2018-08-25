using System.Web;
using System.Web.Optimization;

namespace EmployeeManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                "~/js/angular.js",
                "~/js/app.js"));
            
            
            bundles.Add(new StyleBundle("~/css").Include(
                "~/css/bootstrap.css"));

            
        }
    }
}
