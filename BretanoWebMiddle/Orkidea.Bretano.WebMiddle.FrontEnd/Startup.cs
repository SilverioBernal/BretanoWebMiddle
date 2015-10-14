using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Orkidea.Bretano.WebMiddle.FrontEnd.Startup))]
namespace Orkidea.Bretano.WebMiddle.FrontEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
