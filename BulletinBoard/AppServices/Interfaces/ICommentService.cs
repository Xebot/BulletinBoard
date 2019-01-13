using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Contracts.DTO;
using WebApi.Contracts.DTO.Filters;

namespace AppServices.Interfaces
{
    public interface ICommentService
    {
        IList<CommentDto> GetAll();
        CommentDto Get(int id);
        int CreateComment(CommentDto comment);
        IList<CommentDto> GetAdvertComments(Guid id, int showedAdvertsNumber, int addingAdvertsNumber);
        int GetAdvertCommentsNumber(Guid id);
        void DeleteComment(int id);

        List<CommentInformMassege> GetNewCommentsInformation(Guid userId, DateTime lastActionPublicationTime);
        /*
        int CreateCategory(string categoryName, int GroupId);
        void UpdateAdvert(int id, string categoryName, int GroupId);
        void DeleteAdvert(int id);
        */
    }
}