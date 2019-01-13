using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;

namespace AppServices.Interfaces
{
    public interface ICommentLikerService
    {
        bool AddLike(int commentId, Guid userId);
        bool DeleteLike(int commentId, Guid userId);
    }
}