using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces.Base;

namespace BulletinDomain.RepositoryInterfaces
{
    public interface ICommentLikerRepository : IRepositoryBase<CommentLiker, int>
    {
        bool AddLike(int commentId, Guid userId);
        bool DeleteLike(int commentId, Guid userId);
    }
}
