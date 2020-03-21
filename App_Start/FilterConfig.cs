using System.Web;
using System.Web.Mvc;

namespace GU_DATA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            ////将内置的权限过滤器添加到全局过滤中
            //filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        }
    }
}
