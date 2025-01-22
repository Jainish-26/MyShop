using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyShop.WebUi.Startup))]
namespace MyShop.WebUi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
