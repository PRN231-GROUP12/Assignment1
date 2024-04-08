using PRN231_Group12.Assignment1.Repo.Repository;

namespace PRN231_Group12.Assignment1.Repo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void Save();
    }
}