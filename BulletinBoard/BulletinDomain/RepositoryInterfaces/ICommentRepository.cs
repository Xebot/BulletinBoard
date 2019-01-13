using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces.Base;

namespace BulletinDomain.RepositoryInterfaces
{
    public interface ICommentRepository : IRepositoryBase<Comment, int>
    {
        void Delete(int id);
        Comment Get(int id);
    }
}
