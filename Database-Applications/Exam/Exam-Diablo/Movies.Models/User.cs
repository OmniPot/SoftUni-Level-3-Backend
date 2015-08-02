namespace Movies.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        private ICollection<Movie> favouriteMovies;
        private ICollection<Rating> ratedMovies;

        public User()
        {
            this.favouriteMovies = new HashSet<Movie>();
            this.ratedMovies = new HashSet<Rating>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public virtual ICollection<Movie> FavouriteMovies
        {
            get { return this.favouriteMovies; }
            set { this.favouriteMovies = value; }
        }

        public virtual ICollection<Rating> RatedMovies
        {
            get { return this.ratedMovies; }
            set { this.ratedMovies = value; }
        }
    }
}
