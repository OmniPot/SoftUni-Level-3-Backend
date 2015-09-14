namespace News.Data
{
    using Models;
    using Repositories;

    public interface INewsData
    {
        IRepository<News> News { get; }

        int SaveChanges();
    }
}