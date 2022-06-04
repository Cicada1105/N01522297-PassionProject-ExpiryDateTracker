using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(N01522297_PassionProject_ExpiryDateTracker.Startup))]
namespace N01522297_PassionProject_ExpiryDateTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
