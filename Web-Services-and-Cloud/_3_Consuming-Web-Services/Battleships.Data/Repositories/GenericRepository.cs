namespace Battleships.Data.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.Set = context.Set<T>();
        }

        public IDbSet<T> Set { get; private set; }

        public IQueryable<T> All()
        {
            return this.Set;
        }

        public T Find(object id)
        {
            return this.Set.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public T Delete(object id)
        {
            var entity = this.Find(id);
            this.Delete(entity);
            return entity;
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.Set.Attach(entity);
            }

            entry.State = state;
        }
    }
}