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
    public class CategoryRepository : BaseRepository<IQuarable, int>, ICategoryRepository
    {
        public CategoryRepository(BulletinDbContext dbContext) : base(dbContext)
        {

        }

        public IQuarable Get(int id)
        {
            IQuarable result = dbContext.Categories.Where(x => x.Id == id).FirstOrDefault();
            return result;
        }

        public IQuarable Get(string categoryUrl)
        {
            IQuarable result = dbContext.Categories.Where(x => x.CategoryUrl == categoryUrl).FirstOrDefault();
            return result;
        }

        public void Detele(int id)
        {
            var entity = Get(id);
            dbContext.Categories.Remove(entity);
            dbContext.SaveChanges();
        }        
    }
}