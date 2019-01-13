using AppServices.Services;
using AutoMapper;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO;

namespace BulletinTests
{
    [TestFixture]
    public class RegionTest
    {
        private RegionService _service;
        private Mock<IMapper> _mapper;
        private Mock<IRegionRepository> _repository;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<IRegionRepository>();
            _service = new RegionService(_repository.Object, _mapper.Object);
        }

        [Test]
        public void GetRegion()
        {
            //Arrange
            int id = 0;
            Region region = new Region();
            RegionDto regionDto = new RegionDto();
            _mapper.Setup(r => r.Map<RegionDto>(region)).Returns(regionDto);
            //Act
            var result = _service.GetRegion(id);
            //Assets
            Assert.AreEqual(null, result);
            _repository.Verify(x => x.Get(id), Times.Once);
        }
    }
}
