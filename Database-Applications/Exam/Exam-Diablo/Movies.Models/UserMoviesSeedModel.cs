namespace Movies.Models
{
    using System.Collections.Generic;

    public class UserMoviesSeedModel
    {
        public string Username { get; set; }

        public List<string> FavouriteMovies { get; set; }
    }
}
