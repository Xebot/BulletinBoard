using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces.Base;

namespace BulletinDomain.RepositoryInterfaces
{
    public interface ICategoryRepository : IRepositoryBase<IQuarable, int>
    {
        IQuarable Get(string categoryName);
        void Detele(int id);
        IQuarable Get(int id);
    }
}
