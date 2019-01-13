using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletinDomain.Entities.Base;

namespace BulletinDomain.RepositoryInterfaces.Base
{
    public interface IRepositoryBase<T, TId> where T : EntityWithTypedIdBase<TId>
    {
        //void Delete(T entity);
        //void Detele(TId id);
        IQueryable<T> GetAll();
        TId Create(T entity);
        TId Update(T entity);    
    }
}
