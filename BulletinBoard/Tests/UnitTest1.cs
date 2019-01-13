using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
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
        public void GetAllAdvertsShoudReturn5Adverts()
        {
            //Arrange
            var list = new List<Advert> { new Advert { Title = "adv1" }, new Advert { Title = "adv2" }, new Advert { Title = "adv3" }, new Advert { Title = "adv4" }, new Advert { Title = "adv5" } };

            IQueryable<Advert> testAdverts = list.AsQueryable<Advert>();

            _repository.Setup(r => r.GetAll()).Returns(testAdverts);
            _mapper.Setup(x => x.Map<List<AdvertDto>>(testAdverts)).Returns(new List<AdvertDto> { new AdvertDto { Title = "adv1" },
                                                                                            new AdvertDto { Title = "adv2" },
                                                                                            new AdvertDto { Title = "adv3" },
                                                                                            new AdvertDto { Title = "adv4" },
                                                                                            new AdvertDto { Title = "adv5" }
                                                                                           });
            //Act
            var adverts = _service.GetAllAdverts();

            //Assert
            Assert.AreEqual(5, adverts.Count);
        }
        [Test]
        public void GetAllAdvertsShodReturnNullWhenNoAdverts()
        {
            //Arrange
            var list = new List<Advert> { };
            IQueryable<Advert> testAdvert = list.AsQueryable<Advert>();
            _repository.Setup(r => r.GetAll()).Returns(testAdvert);
            _mapper.Setup(x => x.Map<List<AdvertDto>>(testAdvert)).Returns(new List<AdvertDto> { });

            //Act
            var advers = _service.GetAllAdverts();

            //Assert
            Assert.AreEqual(0, advers.Count);
        }
    }
}
