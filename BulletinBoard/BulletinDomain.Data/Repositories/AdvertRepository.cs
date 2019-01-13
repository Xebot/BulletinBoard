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
    public class AdvertRepository : BaseRepository<Advert, Guid>, IAdvertRepository
    {
        public AdvertRepository(BulletinDbContext dbContext) : base(dbContext)
        {

        }
       
        public Advert Get(Guid id)
        {
            Advert result = dbContext.Adverts.FirstOrDefaultAsync(x => x.Id == id).Result;
            return result;
        }

        public void Detele(Guid id)
        {
            var entity = Get(id);
            dbContext.Adverts.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
