using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using Talabat.Domain.Repositories;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.Repositories
{
    internal class UnitOfWork(AppDbContext _dbContext) : IUnitOfWork
    {
        private Hashtable _repositories = [];
        private IDbContextTransaction? _transaction;

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                _repositories.Add(type, new Repository<T>(_dbContext));
            }

            return (IRepository<T>)_repositories[type]!;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
