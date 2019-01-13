using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces.Base;

namespace BulletinDomain.RepositoryInterfaces
{
    public interface IRegionRepository : IRepositoryBase<Region, int>
    {
        void Detele(int id);
        Region Get(int id);
        //Region Get(int id);
        //IQueryable<Region> GetAll();
    }
}
