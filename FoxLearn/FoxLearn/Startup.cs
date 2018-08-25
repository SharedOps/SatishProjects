using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FoxLearn.Startup))]
namespace FoxLearn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
