using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GestionFacturas.Website.Startup))]
namespace GestionFacturas.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
