using Microsoft.EntityFrameworkCore;
using Talabat.Domain.Repositories;
using Talabat.Domain.Specifications;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.Repositories
{
    internal class Repository<T>(AppDbContext _dbContext) : IRepository<T> where T : class
    {
        public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);

        public void Add(T entity) => _dbContext.Set<T>().Add(entity);

        public void Update(T entity) => _dbContext.Set<T>().Update(entity);

        public void UpdateRange(IEnumerable<T> entities) => _dbContext.Set<T>().UpdateRange(entities);

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification) => await ApplySpecification(specification).ToListAsync();

        public async Task<T?> GetEntityWithSpecificationAsync(ISpecification<T> specification) => await ApplySpecification(specification).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> specification) => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);

    }
}
