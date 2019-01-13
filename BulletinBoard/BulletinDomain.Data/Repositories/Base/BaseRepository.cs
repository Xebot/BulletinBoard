using BulletinDomain.Entities.Base;
using BulletinDomain.RepositoryInterfaces.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletinDomain;

namespace BulletinDomain.Data.Repositories.Base
{
    public abstract class BaseRepository<T, TId> : IRepositoryBase<T, TId> where T : EntityBase<TId>
    {
        public BaseRepository(BulletinDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        protected readonly BulletinDbContext dbContext;
        public TId Create(T entity)
        {
            dbContext.Set<T>().Add(entity);
            dbContext.SaveChanges();
            return entity.Id;
        }
        /*
        public virtual void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();
        }
        
        public void Detele(TId id)
        {
            var entity = Get(id);
            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();
        }      

        public virtual T Get(TId id)
        {
            if (id.GetType().BaseType is int)
            {
                var result = dbContext.Set<T>().Where(x => x.Id == id).FirstOrDefault();
            }
            var result = dbContext.Set<T>().Where(x => x.Id == id).FirstOrDefault();
            return result;
        }
        */
        public virtual IQueryable<T> GetAll()
        {
            var result = dbContext.Set<T>();
            return result;
        }

        public virtual IQueryable<T> GetLastEight()
        {
            var result = dbContext.Set<T>();
            return result;
        }

        public T SaveOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }

        public TId Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
            dbContext.SaveChanges();
            return entity.Id;
        }
    }
}
