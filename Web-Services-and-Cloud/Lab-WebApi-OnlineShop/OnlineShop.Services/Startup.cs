using Microsoft.Owin;
using OnlineShop.Services;

[assembly: OwinStartup(typeof (Startup))]

namespace OnlineShop.Services
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Web.Http;
    using Data;
    using Infrastructure;
    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);

            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IUserIdProvider>().To<AspNetUserIdProvider>();
            kernel.Bind<IOnlineShopData>().To<OnlineShopData>();
            kernel.Bind<DbContext>().To<OnlineShopContext>();

            var httpConfig = new HttpConfiguration();
            WebApiConfig.Register(httpConfig);
            app.UseNinjectMiddleware(() => kernel)
                .UseNinjectWebApi(httpConfig);
        }
    }
}