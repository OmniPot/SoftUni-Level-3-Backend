using Microsoft.Owin;
using News.WebServices;

[assembly: OwinStartup(typeof (Startup))]

namespace News.WebServices
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}