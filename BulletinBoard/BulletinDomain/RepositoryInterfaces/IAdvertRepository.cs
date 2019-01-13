using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletinDomain.RepositoryInterfaces
{
    public interface IAdvertRepository : IRepositoryBase<Advert, Guid>
    {
        void Detele(Guid id);
        Advert Get(Guid id);
    }
}
