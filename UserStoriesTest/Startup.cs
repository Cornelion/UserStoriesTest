using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UserStoriesTest.Startup))]
namespace UserStoriesTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
