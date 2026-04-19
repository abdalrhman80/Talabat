using Talabat.Domain.Specifications;

namespace Talabat.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        #region Without Specification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entity);
        void Delete(T entity);
        #endregion

        #region With Specification
        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
        Task<T?> GetEntityWithSpecificationAsync(ISpecification<T> specification);
        #endregion
    }
}
