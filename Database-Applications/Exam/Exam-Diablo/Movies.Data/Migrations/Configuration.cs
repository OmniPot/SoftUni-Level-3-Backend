namespace Movies.Data.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using Models;
    using Newtonsoft.Json;

    internal sealed class Configuration : DbMigrationsConfiguration<MoviesEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MoviesEntities context)
        {
            SeedCountries(context);
            context.SaveChanges();

            SeedUsers(context);
            context.SaveChanges();

            SeedMovies(context);
            context.SaveChanges();

            SeedFavMovies(context);
            context.SaveChanges();

            SeedRatings(context);
            context.SaveChanges();
        }

        private static void SeedRatings(MoviesEntities context)
        {
            if (!context.Ratings.Any())
            {
                var sourceFile = @"..\..\..\Sources\movie-ratings.json";
                var jsonText = File.ReadAllText(sourceFile);
                var ratings = JsonConvert.DeserializeObject<List<MovieRantingsSeedModel>>(jsonText);

                foreach (var movieRating in ratings)
                {
                    var user = context.Users.First(u => u.Username.Equals(movieRating.User));
                    var movie = context.Movies.First(m => m.Isbn.Equals(movieRating.Movie));
                    var existingRating = context.Ratings
                        .Any(r => r.User.Username == user.Username &&
                            r.Movie.Isbn == movie.Isbn);

                    if (!existingRating)
                    {
                        var stars = movieRating.Rating;
                        var ratingToAdd = new Rating
                        {
                            User = user,
                            Movie = movie,
                            Stars = stars
                        };

                        context.Ratings.Add(ratingToAdd);
                    }
                }
            }
        }

        private static void SeedFavMovies(MoviesEntities context)
        {
            var sourcef = @"..\..\..\Sources\users-and-favourite-movies.json";
            var jsonT = File.ReadAllText(sourcef);
            var usersMovies = JsonConvert.DeserializeObject<List<UserMoviesSeedModel>>(jsonT);

            foreach (var user in usersMovies)
            {
                var candidateUser = context.Users.FirstOrDefault(u => u.Username.Equals(user.Username));
                var userFavMovies = user.FavouriteMovies;

                if (!candidateUser.FavouriteMovies.Any() && userFavMovies != null)
                {
                    foreach (var isbn in userFavMovies)
                    {
                        var movieToAdd = context.Movies.FirstOrDefault(m => m.Isbn.Equals(isbn));
                        if (!candidateUser.FavouriteMovies.Contains(movieToAdd))
                        {
                            candidateUser.FavouriteMovies.Add(movieToAdd);
                        }
                    }
                }
            }
        }

        private static void SeedMovies(MoviesEntities context)
        {
            if (!context.Movies.Any())
            {
                var sourceFile = @"..\..\..\Sources\movies.json";
                var jsonText = File.ReadAllText(sourceFile);
                var parsedMovies = JsonConvert.DeserializeObject<List<Movie>>(jsonText);

                foreach (var movie in parsedMovies)
                {
                    context.Movies.Add(movie);
                }
            }
        }

        private static void SeedUsers(MoviesEntities context)
        {
            if (!context.Users.Any())
            {
                var sourceFile = @"..\..\..\Sources\users.json";
                var jsonText = File.ReadAllText(sourceFile);
                var parsedUsers = JsonConvert.DeserializeObject<List<UsersSeedModel>>(jsonText);

                foreach (var user in parsedUsers)
                {
                    var userToAdd = new User()
                    {
                        Username = user.Username,
                        Age = user.Age,
                        CountryId = user.CountryId,
                        Email = user.Email,
                        FavouriteMovies = new HashSet<Movie>()
                    };

                    context.Users.Add(userToAdd);
                }
            }
        }

        private static void SeedCountries(MoviesEntities context)
        {
            if (!context.Countries.Any())
            {
                var sourceFile = @"..\..\..\Sources\countries.json";
                var jsonText = File.ReadAllText(sourceFile);
                var parsedCountries = JsonConvert.DeserializeObject<List<Country>>(jsonText);

                foreach (var country in parsedCountries)
                {
                    context.Countries.Add(country);
                }
            }
        }
    }
}
