using System;
using AutoMapper;
using Moq;
using NUnit.Framework;
using AppServices.Services;
using BulletinDomain;
using BulletinDomain.RepositoryInterfaces;
using WebApi.Contracts.DTO;
using BulletinDomain.Entities;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace AdvertServiceTests
{
    [TestFixture]
    public class AdvertsTest
    {
        private AdvertService _service;
        private Mock<IMapper> _mapper;
        private Mock<IAdvertRepository> _repository;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<IAdvertRepository>();
            _service = new AdvertService(_repository.Object, _mapper.Object);
        }
      
        [Test]
        public void CreateAdvert()
        {
            //Arange
            Guid id = new Guid();
            var testAdvertDto = new AdvertDto { Id = id };
            var testAdvert = new Advert { Title = "Adv1" };
            _mapper.Setup(x => x.Map<Advert>(testAdvertDto)).Returns(testAdvert);
            _repository.Setup(r => r.Create(testAdvert)).Returns(testAdvertDto.Id);
            //Act
            var advert = _service.CreateAdvert(testAdvertDto);
            //Assets
            Assert.AreEqual(id, advert);
        }        
        [Test]
        public void DeleteAdvert()
        {
            //Arange
            Guid id = new Guid();
            Advert ad = new Advert { Status = "Published" };

            //Act
            _repository.Setup(r => r.Get(id)).Returns(ad);
            _service.DeleteAdvert(id);

            //Assets
            Assert.AreEqual(ad.Status, "Deleted");
        }
        [Test]
        public void DeleteAdvertTotal()
        {
            //Arange
            Guid id = new Guid();
            Advert ad = new Advert { Status = "Published" };

            //Act
            _repository.Setup(r => r.Get(id)).Returns(ad);
            _service.DeleteAdvertTotal(id);
            var newAd = _service.GetAdvertById(id);

            //Assets
            Assert.AreEqual(newAd, null);
        }
        [Test]
        public void GetAllAdvertComments()
        {
            //Arrange
            Guid id = new Guid();
            Advert ad = new Advert();            
            _repository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(ad);
            //Act
            _service.GetAllAdvertComments(id);
            //Assets
            _repository.Verify(x => x.Get(id), Times.Once);
        }
        [Test]
        public void UpdateAdvert()
        {
            //Arange
            Guid id = new Guid();
            var testAdvertDto = new AdvertDto { Id = id };
            var testAdvert = new Advert { Title = "Adv1" };
            _repository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(testAdvert);
            _repository.Setup(r => r.Update(testAdvert)).Returns(testAdvert.Id);
            //Act
            _service.UpdateAdvert(testAdvertDto);
            //Assets 
            Assert.AreEqual(testAdvert.Id, testAdvertDto.Id);
            //Assert.AreEqual(1, testAdvertDto.Id);
        }
        [Test]
        public void GetAllAdverts()
        {
            //Arrande
            IQueryable<Advert> list = null;
            _repository.Setup(r => r.GetAll()).Returns(list);
            //Act
            var result = _service.GetAllAdverts();
            //Assets
            _repository.Verify(x => x.GetAll(), Times.Once);
        }
        [Test]
        public void GetUserAdverts()
        {
            //Arrange
            IQueryable<Advert> userAdverts = null;
            _repository.Setup(r => r.GetAll()).Returns(userAdverts);

            Guid id = new Guid();
            //Act
            var adverts = _service.GetUserAdverts(id, 1);

            //Assets
            _repository.Verify(x => x.GetAll(), Times.Once);


        }
        [Test]
        public void Unpublish()
        {
            //Arrange
            Advert ad = new Advert
            {
                Status = "Published"
            };
            _repository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(ad);
            //Act
            _service.UnpublishAdvert(ad.Id);

            //Assets
            Assert.AreEqual(ad.Status, "Unactive");
        }
        
    }
}
