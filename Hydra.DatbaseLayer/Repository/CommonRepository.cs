using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hydra.DatbaseLayer.Repository
{
    public abstract class CommonRepository<T>(HydraContext context) : ICommonRepository<T> where T : class
    {
        private readonly HydraContext _context = context;


        public virtual async Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }


        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }


        public virtual IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }


        public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition)
        {
            return _context.Set<T>().Where(condition).AsNoTracking();
        }


        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }


        public async Task CommitChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
