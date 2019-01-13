using AppServices.Services;
using AutoMapper;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Contracts.DTO;

namespace BulletinTests
{
    [TestFixture]
    public class CommentTest
    {
        private CommentService _service;
        private Mock<IMapper> _mapper;
        private Mock<IAdvertRepository> _advertRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<ICommentLikerRepository> _likeRepository;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _likeRepository = new Mock<ICommentLikerRepository>();
            _advertRepository = new Mock<IAdvertRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _service = new CommentService(_commentRepository.Object, _advertRepository.Object, _likeRepository.Object, _mapper.Object);
        }

        [Test]
        public void GetAll()
        {
            //Arrange
            IQueryable<Comment> list = null;
            List<Comment> comments = new List<Comment>();            
            List<CommentDto> commentsDto = new List<CommentDto>();
            _mapper.Setup(r => r.Map<List<CommentDto>>(comments)).Returns(commentsDto);
            _commentRepository.Setup(r => r.GetAll()).Returns(list);
            //Act
            var result = _service.GetAll();
            //Assets
            _commentRepository.Verify(x => x.GetAll(), Times.Once);
            Assert.AreEqual(null, result);
        }
        [Test]
        public void Get()
        {
            //Arrange
            int id = 1;
            Comment comment = new Comment();
            CommentDto commentDto = new CommentDto();
            _commentRepository.Setup(r => r.Get(id)).Returns(comment);
            _mapper.Setup(r => r.Map<CommentDto>(comment)).Returns(commentDto);
            //Act
            var result = _service.Get(id);
            //Assets
            _commentRepository.Verify(x => x.Get(id), Times.Once);
            Assert.AreEqual(commentDto, result);
        }
        [Test]
        public void CreateComment()
        {
            //Arrange
            CommentDto commentDto = new CommentDto();
            Comment comment = new Comment()
            {
                CommentText = "",
                UserId = new Guid()
            };
            _commentRepository.Setup(r => r.Create(comment)).Returns(1);
            //Act
            var result = _service.CreateComment(commentDto);
            //Assets            
            Assert.AreEqual(0, result);
        }        
        [Test]
        public void GetAdvertCommentsNumber()
        {
            //Arrange
            Guid guid = new Guid();
            List<Comment> comments = new List<Comment>()
            {                
                new Comment
                {
                    CommentText = "",
                    UserId = new Guid(),
                    AdvertId = guid,
                    ParentId =0
                },
                new Comment
                {
                    CommentText = "111",
                    UserId = new Guid(),
                    AdvertId = guid,
                    ParentId = 0
                }
            };
            //Act
            var result = _service.GetAdvertCommentsNumber(guid);
            //Assets
            _commentRepository.Verify(x => x.GetAll(), Times.Once);
            Assert.AreEqual(0, result);
        }
        [Test]
        public void DeleteComment()
        {
            //Arrange
            int id = 0;
            //Act
            _service.DeleteComment(id);
            //Assets
            _commentRepository.Verify(x => x.Delete(id), Times.Once);
        }
        [Test]
        public void GetNewCommentsInformation()
        {
            //Arrange
            Guid guid = new Guid();
            DateTime dateTime = DateTime.Now;
            //Act
            var result = _service.GetNewCommentsInformation(guid, dateTime);
            //Assets
            _commentRepository.Verify(x => x.GetAll(), Times.AtLeast(3));
        }

    }
}
