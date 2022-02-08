using DomainModels.Models.Base;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Implementation
{
    public class EfCoreRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly AppDbContext Db;
        public EfCoreRepository(AppDbContext db)
        {
            Db = db;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await Db.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await Db.Set<T>().FindAsync(id);
        }
        public async Task<bool> AddAsync(T item)
        {
            try
            {
                await Db.Set<T>().AddAsync(item);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> AddAsync(List<T> items)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(params T[] items)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(T item)
        {
            Db.Set<T>().Remove(item);
            await Db.SaveChangesAsync();
            return true;

        }

        public Task<bool> DeleteAsync(List<T> items)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(params T[] items)
        {
            throw new NotImplementedException();
        }

        public bool Update(T item)
        {
            try
            {
                Db.Set<T>().Update(item);
                Db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(List<T> items)
        {
            throw new NotImplementedException();
        }

        public bool Update(params T[] items)
        {
            throw new NotImplementedException();
        }
    }
}
