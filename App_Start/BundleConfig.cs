using System.Web;
using System.Web.Optimization;

namespace GU_DATA
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/Bootstarp/css").Include(
                "~/Content/Bootstarp/css/bootstrap.min.css",
                "~/Content/Bootstarp/css/bootstrap-theme.css",
                "~/Content/Bootstarp/css/bootstrap.css"
                ));

            //打包js

            bundles.Add(new ScriptBundle("~/bundles/Bootstarp/js").Include(
                "~/Content/Bootstarp/js/bootstrap.min.js",
                "~/Content/Bootstarp/js/npm.js",
                "~/Content/Bootstarp/js/bootstrap.js",
                "~/Content/Bootstarp/jquery-{version}.js"
                ));
            //可以清除过滤规则
            //
            //BundleTable.Bundles.IgnoreList.Clear();



            bundles.Add(new StyleBundle("~/bundles/18830/css").Include(

                "~/Content/18830/css/children.css",
                "~/Content/18830/css/index.css"
                ));


            //绑定18830js
            bundles.Add(new ScriptBundle("~/bundles/18830/js").Include(
             "~/Content/18830/JS/jquery-1.11.0.min.js",
             "~/Content/18830/JS/jquery.SuperSlide.2.1.1.js",
               "~/Content/18830/JS/laydate.js",
             "~/Content/18830/JS/public.js"


             ));





            BundleTable.Bundles.IgnoreList.Clear();
        }

    }


}