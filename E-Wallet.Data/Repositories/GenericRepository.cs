
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using E_Wallet.Data.IRepositories;
using E_Wallet.Data.Data;

namespace E_Wallet.Data.Repositories
{

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly WalletDbContext dbContext;
        private DbSet<T> dbSet;
        private bool isDisposed;

        public GenericRepository(WalletDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = this.dbContext.Set<T>();
            isDisposed = false;
        }

        public async Task<T> CreateAsync(T entity) => (await dbSet.AddAsync(entity)).Entity;

        public T Delete(T entity) => dbSet.Remove(entity).Entity;

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate) => predicate == null ? dbSet : dbSet.Where(predicate);

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) => await dbSet.FirstOrDefaultAsync(predicate);

        public T Update(T entity) => dbSet.Update(entity).Entity;


    
        public void Dispose()
        {
            if (dbContext != null && !isDisposed)
            {
                dbContext?.Dispose();
            }

            isDisposed = true;
        }
    }
}


