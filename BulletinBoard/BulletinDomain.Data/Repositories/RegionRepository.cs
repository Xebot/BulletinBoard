using BulletinDomain.Data.Repositories.Base;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BulletinDomain.Data.Repositories
{
    public class RegionRepository : BaseRepository<Region, int>, IRegionRepository
    {
        public RegionRepository(BulletinDbContext dbContext) : base(dbContext)
        {

        }

        public Region Get(int id)
        {
            Region result = dbContext.Regions.Where(x => x.Id == id).FirstOrDefault();
            return result;
        }

        public void Detele(int id)
        {
            var entity = Get(id);
            dbContext.Regions.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
