using System.Linq.Expressions;

namespace Hydra.DatbaseLayer.IRepository
{
    public interface ICommonRepository<T>
    {
        IQueryable<T> FindAll();


        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition);


        Task Create(T entity);


        void Update(T entity);


        void Delete(T entity);


        Task CommitChanges();
    }
}
