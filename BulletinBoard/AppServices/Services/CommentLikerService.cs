using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;
using BulletinDomain;
using BulletinDomain.RepositoryInterfaces;
using AppServices.Interfaces;
using BulletinDomain.Entities;
using System.Linq;
using AutoMapper;

namespace AppServices.Services
{
    public class CommentLikerService : ICommentLikerService
    {
        public CommentLikerService(ICommentLikerRepository commentLikerRepository, IMapper mapper)
        {
            _commentLikerRepository = commentLikerRepository;
            _mapper = mapper;
        }
        protected readonly ICommentLikerRepository _commentLikerRepository;
        protected readonly IMapper _mapper;


        public bool AddLike(int commentId, Guid userId) {
            return _commentLikerRepository.AddLike(commentId, userId);
        }

        public bool DeleteLike(int commentId, Guid userId) {
            return _commentLikerRepository.DeleteLike(commentId, userId);
        }
    }
}
