using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.Data.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }

        Task DeleteAsync(string id);
        Task DeleteAsync(T entityToDelete);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<bool> InsertAsync(T entityToInsert);
        Task UpdateAsync(T entityToUpdate);
    }
}
