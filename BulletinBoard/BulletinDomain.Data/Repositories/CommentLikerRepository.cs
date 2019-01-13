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
    public class CommentLikerRepository : BaseRepository<CommentLiker, int>, ICommentLikerRepository
    {
        public CommentLikerRepository(BulletinDbContext dbContext) : base(dbContext)
        {

        }

        public CommentLiker Get(int id)
        {
            CommentLiker result = dbContext.CommentLikers.Where(x => x.Id == id).FirstOrDefault();
            return result;
        }

        public void Detele(int id)
        {
            var entity = Get(id);
            dbContext.CommentLikers.Remove(entity);
            dbContext.SaveChanges();
        }

        public bool AddLike(int commentId, Guid userId)
        {
            bool isExist = dbContext.CommentLikers.Where(x => x.CommentId == commentId && x.UserId == userId).Any();
            if (!isExist)
            {
                dbContext.CommentLikers.Add(new CommentLiker { CommentId = commentId, UserId = userId, PublicationDate = DateTime.UtcNow });
                dbContext.SaveChanges();
            }
            return !isExist;
        }

        public bool DeleteLike(int commentId, Guid userId)
        {
            bool isExist = dbContext.CommentLikers.Where(x => x.CommentId == commentId && x.UserId == userId).Any();
            if (isExist)
            {
                var entity = dbContext.CommentLikers.Where(x => x.CommentId == commentId && x.UserId == userId).First();
                dbContext.CommentLikers.Remove(entity);
                dbContext.SaveChanges();
            }
            return isExist;
        }
    }
}
