using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet.Data.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        DbSet<TEntity> Entities { get; }
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(Guid? id);
        Guid Add(TEntity obj);
        bool Update(TEntity obj);
        bool Remove(TEntity obj);
        void Dispose();
    }
}
