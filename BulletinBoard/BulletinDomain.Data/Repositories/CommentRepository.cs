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
    public class CommentRepository : BaseRepository<Comment, int>, ICommentRepository
    {
        public CommentRepository(BulletinDbContext dbContext) : base(dbContext)
        {

        }

        public Comment Get(int id)
        {
            Comment result = dbContext.Comments.Where(x => x.Id == id).FirstOrDefault();
            return result;
        }

        public void Delete(int id)
        {
            var entity = Get(id);
            dbContext.Comments.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
