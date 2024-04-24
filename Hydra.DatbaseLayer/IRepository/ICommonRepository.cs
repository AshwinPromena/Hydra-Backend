using System.Linq.Expressions;

namespace Hydra.DatbaseLayer.IRepository
{
    public interface ICommonRepository<T>
    {
        Task Create(T entity);

        Task CreateRange(IEnumerable<T> entity);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entity);

        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition);

        Task CommitChanges();
    }
}
