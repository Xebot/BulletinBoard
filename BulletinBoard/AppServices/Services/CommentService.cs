using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;
using BulletinDomain;
using BulletinDomain.RepositoryInterfaces;
using AppServices.Interfaces;
using BulletinDomain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AppServices.Services
{
    public class CommentService : ICommentService
    {
        public CommentService(ICommentRepository _commentRepository, IAdvertRepository _advertRepository, ICommentLikerRepository _commentLikerRepository, IMapper _mapper)
        {
            commentRepository = _commentRepository;
            advertRepository = _advertRepository;
            commentLikerRepository = _commentLikerRepository;
            mapper = _mapper;
        }
        protected readonly ICommentRepository commentRepository;
        protected readonly IAdvertRepository advertRepository;
        protected readonly ICommentLikerRepository commentLikerRepository;
        protected readonly IMapper mapper;

        public IList<CommentDto> GetAll()
        {
            var cat = commentRepository.GetAll();
            if (cat != null)
            {
                IList<Comment> categories = cat.ToList();
                IList<CommentDto> result = mapper.Map<List<CommentDto>>(categories);
                return result;
            }
            return null;
            
        }

        public CommentDto Get(int id)
        {
            var ad = commentRepository.Get(id);
            var result = mapper.Map<CommentDto>(ad);
            return result;
        }

        public IList<CommentDto> GetAdvertComments(Guid id, int showedAdvertsNumber = 0, int addingAdvertsNumber = 5)
        {
            var ad = commentRepository.GetAll()
                                      .Include(x => x.CommentLikers)
                                      .Where(x => x.AdvertId == id && x.ParentId == 0)
                                      .OrderBy(x => x.PublicationDate).Skip(showedAdvertsNumber)
                                      .Take(addingAdvertsNumber)
                                      .ToList();
            var commentsDto = mapper.Map<IList<CommentDto>>(ad);
            foreach (var comment in commentsDto)
            {
                if (comment.CommentLikers != null)
                {
                    comment.CommentLikersNumber = comment.CommentLikers.Count();
                }
                else
                {
                    comment.CommentLikersNumber = 0;
                }
                
                var commentReplies = commentRepository.GetAll().Include(x => x.CommentLikers).Where(x => x.ParentId == comment.Id).OrderBy(x => x.PublicationDate).ToList();
                comment.CommentReplies = mapper.Map<List<CommentDto>>(commentReplies);
                foreach (var commentReply in comment.CommentReplies)
                {
                    commentReply.CommentLikersNumber = commentReply.CommentLikers.Count();
                }
            }
            return commentsDto;
        }
        
        public int CreateComment(CommentDto comment)
        {
            var result = mapper.Map<Comment>(comment);
            commentRepository.Create(result);
            return comment.Id;
        }

        public int GetAdvertCommentsNumber(Guid id)
        {
            int commentsNumber = commentRepository.GetAll().Where(x => x.AdvertId == id && x.ParentId == 0).Count();
            return commentsNumber;
        }

        public void DeleteComment(int id)
        {
            commentRepository.Delete(id);
        }

        public List<CommentInformMassege> GetNewCommentsInformation(Guid id, DateTime lastActionPublicationTime) {
            var a = lastActionPublicationTime;
            // New replies
            List<int> userCommentsId = commentRepository.GetAll().Where(x => x.UserId == id).Select(x => x.Id).ToList();
            List<CommentInformMassege> newReplies = commentRepository.GetAll()
                                                                     .Where(x => userCommentsId.Contains(x.ParentId) && x.UserId != id && x.ParentId != 0 && x.PublicationDate > lastActionPublicationTime)
                                                                     .Select(x => new CommentInformMassege {
                                                                         UserId = x.UserId,
                                                                         UserName = x.UserName,
                                                                         CommentId = x.Id,
                                                                         CommentText = x.CommentText,
                                                                         AdvertId = x.AdvertId,
                                                                         AdvertTitle = advertRepository.GetAll().Where(y => y.Id == x.AdvertId).FirstOrDefault().Title,
                                                                         UserAction = "Ответ на комментарий",
                                                                         PublicationDate = x.PublicationDate,
                                                                         IsUnreadMassege = true
                                                                     })
                                                                     .ToList();
            // New advert comments
            List<Guid> userAdvertsId = advertRepository.GetAll().Where(x => x.UserId == id).Select(x => x.Id).ToList();
            List<CommentInformMassege> newComments = commentRepository.GetAll()
                                                                      .Where(x => userAdvertsId.Contains(x.AdvertId) && x.UserId != id && x.ParentId == 0 && x.PublicationDate > lastActionPublicationTime)
                                                                      .Select(x => new CommentInformMassege
                                                                      {
                                                                          UserId = x.UserId,
                                                                          UserName = x.UserName,
                                                                          CommentId = x.Id,
                                                                          CommentText = x.CommentText,
                                                                          AdvertId = x.AdvertId,
                                                                          AdvertTitle = advertRepository.GetAll().Where(y => y.Id == x.AdvertId).FirstOrDefault().Title,
                                                                          UserAction = "Добавлен комментарий",
                                                                          PublicationDate = x.PublicationDate,
                                                                          IsUnreadMassege = true
                                                                      })
                                                                      .ToList();
            // New likes            
            List<CommentInformMassege> newLikes = commentLikerRepository.GetAll()
                                                                        .Where(x => userCommentsId.Contains(x.CommentId) && x.UserId != id && x.PublicationDate > lastActionPublicationTime)
                                                                        .Select(x => new CommentInformMassege
                                                                        {
                                                                            UserId = x.UserId,
                                                                            UserName = commentRepository.GetAll().Where(y => y.UserId == x.UserId).FirstOrDefault().UserName,
                                                                            CommentId = x.CommentId,
                                                                            CommentText = commentRepository.GetAll().Where(y => y.Id == x.CommentId).FirstOrDefault().CommentText,
                                                                            AdvertId = commentRepository.GetAll().Where(y => y.Id == x.CommentId).FirstOrDefault().AdvertId,
                                                                            AdvertTitle = advertRepository.GetAll()
                                                                                                        .Where(y => y.Id == commentRepository.GetAll()
                                                                                                                                                .Where(z => z.Id == x.CommentId)
                                                                                                                                                .FirstOrDefault()
                                                                                                                                                .AdvertId
                                                                                                        )
                                                                                                        .FirstOrDefault()
                                                                                                        .Title,
                                                                            UserAction = "Поставлен лайк",
                                                                            PublicationDate = x.PublicationDate,
                                                                            IsUnreadMassege = true
                                                                        })
                                                                        .ToList();            
            List<CommentInformMassege> result = new List<CommentInformMassege>();
            result.AddRange(newReplies);
            result.AddRange(newComments);
            result.AddRange(newLikes);
            result = result.OrderBy(x => x.PublicationDate).Take(30).ToList();
            return result;
        }
    }
}
