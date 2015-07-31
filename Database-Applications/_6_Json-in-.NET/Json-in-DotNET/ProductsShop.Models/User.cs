namespace ProductsShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        private ICollection<User> friendWith;
        private ICollection<User> friendTo;
        private ICollection<Product> productsSold;
        private ICollection<Product> productsBought;

        public User()
        {
            this.friendWith = new HashSet<User>();
            this.friendTo = new HashSet<User>();
            this.productsSold = new HashSet<Product>();
            this.productsBought = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        public virtual ICollection<User> FriendWith
        {
            get { return this.friendWith; }
            set { this.friendWith = value; }
        }

        public virtual ICollection<User> FriendTo
        {
            get { return this.friendTo; }
            set { this.friendTo = value; }
        }

        public virtual ICollection<Product> ProductsSold
        {
            get { return this.productsSold; }
            set { this.productsSold = value; }
        }

        public virtual ICollection<Product> ProductsBought
        {
            get { return this.productsBought; }
            set { this.productsBought = value; }
        }
    }
}
