using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GU_DATA.Startup))]
namespace GU_DATA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
