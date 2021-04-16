using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(accounting.Startup))]
namespace accounting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
