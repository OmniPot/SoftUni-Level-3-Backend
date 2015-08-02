namespace Movies.Data
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class MoviesEntities : DbContext
    {
        public MoviesEntities()
            : base("name=MoviesEntities")
        {
            var dropCreate = new DropCreateDatabaseAlways<MoviesEntities>();
            var migrateToLatest = new MigrateDatabaseToLatestVersion<MoviesEntities, Configuration>();

            Database.SetInitializer(migrateToLatest);
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Movie> Movies { get; set; }

        public virtual IDbSet<Rating> Ratings { get; set; }

        public virtual IDbSet<Country> Countries { get; set; }

    }
}