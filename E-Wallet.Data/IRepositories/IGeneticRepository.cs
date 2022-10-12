using System.Linq.Expressions;


namespace E_Wallet.Data.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        T Delete(T entity);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
    }
}
