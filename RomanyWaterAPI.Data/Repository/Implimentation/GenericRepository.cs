using Microsoft.EntityFrameworkCore;
using RomanyWaterAPI.Data.Context;
using RomanyWaterAPI.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.Data.Repository.Implimentation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private AppDbContext _appDbContext { get; set; }
        private DbSet<T> _dbSet;
        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public virtual async Task<bool> InsertAsync(T entityToInsert)
        {
            var result = await _dbSet.AddAsync(entityToInsert);
            await _appDbContext.SaveChangesAsync();

            if (result != null) return true;

            return false;
        }

        public virtual async Task UpdateAsync(T entiyToUpdate)
        {
            _dbSet.Update(entiyToUpdate);
            await _appDbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(string id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                await DeleteAsync(entityToDelete);
                return;
            }
            var typeName = typeof(T).Name;
            throw new ArgumentException($"{typeName} with Id {id} does not exist");
        }

        public async Task DeleteAsync(T entityToDelete)
        {
            _dbSet.Remove(entityToDelete);
            await _appDbContext.SaveChangesAsync();
        }

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_dbSet == null)
                    _dbSet = _appDbContext.Set<T>();
                return _dbSet;
            }
        }
        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }
    }
}
