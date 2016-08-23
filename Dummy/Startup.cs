using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dummy.Startup))]
namespace Dummy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
