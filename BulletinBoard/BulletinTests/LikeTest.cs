using AppServices.Services;
using AutoMapper;
using BulletinDomain.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletinTests
{
    [TestFixture]
    public class LikeTest
    {
        private CommentLikerService _service;
        private Mock<IMapper> _mapper;
        private Mock<ICommentLikerRepository> _repository;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<ICommentLikerRepository>();
            _service = new CommentLikerService(_repository.Object, _mapper.Object);
        }

        [Test]
        public void AddLike()
        {
            //Arrange
            Guid guid = new Guid();
            _repository.Setup(r => r.AddLike(1, guid)).Returns(true);

            //Act
            var result = _service.AddLike(1, guid);

            //Assets
            Assert.AreEqual(true, result);
            _repository.Verify(x => x.AddLike(1, guid), Times.Once);
        }

        [Test]
        public void DeleteLike()
        {
            //Arrange
            Guid guid = new Guid();
            _repository.Setup(r => r.DeleteLike(1, guid)).Returns(true);

            //Act
            var result = _service.DeleteLike(1, guid);

            //Assets
            Assert.AreEqual(true, result);
            _repository.Verify(x => x.DeleteLike(1, guid), Times.Once);
        }
    }
}
