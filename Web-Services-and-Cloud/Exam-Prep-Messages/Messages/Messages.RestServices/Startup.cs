using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Messages.RestServices.Startup))]

namespace Messages.RestServices
{
    using System.Data.Entity;

    using Messages.Data;
    using Messages.Data.Migrations;
    using Microsoft.Owin.Cors;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MessagesDbContext, MessagesDbMigrationConfiguration>());
            this.ConfigureAuth(app);
        }
    }
}
