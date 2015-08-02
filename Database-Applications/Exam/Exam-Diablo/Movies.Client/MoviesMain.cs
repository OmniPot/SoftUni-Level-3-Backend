namespace Movies.Client
{
    using System;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;

    public class MoviesMain
    {
        public static void Main()
        {
            var context = new MoviesEntities();

            ExportAdultMovies(context);

            ExportRatedMoviesBy(context, "jmeyery");

            ExportTop10Movies(context);
        }

        private static void ExportTop10Movies(MoviesEntities context)
        {
            var filePath = @"..\..\..\Exported-Files\";
            var fileName = @"top-10-favourite-movies.json";

            var top10Movies = context.Movies
                .Where(m => (int)m.AgeRestriction == 1)
                .Include(m => m.Ratings)
                .Include(m => m.UsersFavouriteTo)
                .Select(m => new
                {
                    isbn = m.Isbn,
                    title = m.Title,
                    favouritedBy = m.UsersFavouriteTo
                        .Select(u => u.Username)
                })
                .OrderByDescending(m => m.favouritedBy.Count())
                .ThenBy(m => m.title)
                .Take(10);

            var jsonResult = JsonConvert.SerializeObject(top10Movies);
            File.WriteAllText(filePath + fileName, jsonResult);
        }

        private static void ExportRatedMoviesBy(MoviesEntities context, string username)
        {
            var filePath = @"..\..\..\Exported-Files\";
            var fileName = @"rated-movies-by-jmeyery.json";

            var moviesRatedByUser = context.Users
                .Where(u => u.Username.Equals(username))
                .Include(u => u.RatedMovies)
                .Select(u => new
                {
                    username = u.Username,
                    ratedMovies = u.RatedMovies.Select(rm => new
                    {
                        title = rm.Movie.Title,
                        userRating = rm.Stars,
                        averageRating = Math.Round(rm.Movie.Ratings.Average(mr => mr.Stars), 2)
                    })
                    .OrderBy(m => m.title)
                }).ToList()[0];

            var jsonResult = JsonConvert.SerializeObject(moviesRatedByUser);
            File.WriteAllText(filePath + fileName, jsonResult);
        }

        private static void ExportAdultMovies(MoviesEntities context)
        {
            var filePath = @"..\..\..\Exported-Files\";
            var fileName = @"adult-movies.json";

            var adultMovies = context.Movies
                .Where(m => (int)m.AgeRestriction == 2)
                .Select(m => new
                {
                    title = m.Title,
                    ratingsCount = m.Ratings.Count
                })
                .OrderBy(m => m.title)
                .ThenBy(m => m.ratingsCount);

            var jsonResult = JsonConvert.SerializeObject(adultMovies);
            File.WriteAllText(filePath + fileName, jsonResult);
        }
    }
}
