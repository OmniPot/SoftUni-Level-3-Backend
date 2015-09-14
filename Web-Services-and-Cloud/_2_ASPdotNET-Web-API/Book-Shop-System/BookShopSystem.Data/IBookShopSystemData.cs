namespace BookShopSystem.Data
{
    using Models;
    using Repositories;

    public interface IBookShopSystemData
    {
        IRepository<Book> Books { get; }

        IRepository<Category> Categories { get; }

        IRepository<Author> Authors { get; }

        IRepository<User> Users { get; }

        void SaveChanges();
    }
}
