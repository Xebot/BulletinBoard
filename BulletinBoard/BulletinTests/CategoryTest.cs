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
using BulletinDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulletinTests
{

    [TestFixture]
    public class CategoryTest
    {
        private CategoryService _service;
        private Mock<IMapper> _mapper;
        private Mock<ICategoryRepository> _repository;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_repository.Object, _mapper.Object);
        }

        [Test]
        public void GetAll()
        {
            //Arrange            
            IQueryable<IQuarable> ad = null;
            IList<IQuarable> categories = null;
            List<CategoryDto> categoryDto = null;
            _repository.Setup(r => r.GetAll()).Returns(ad);
            _mapper.Setup(x => x.Map<List<CategoryDto>>(categories)).Returns(categoryDto);
            //Act
            _service.GetAll();
            //Assets
            _repository.Verify(x => x.GetAll(), Times.Once);
        }
        [Test]
        public void GetCategoryIdByUrl()
        {
            //Arrange
            var url = "cars";
            IQuarable cat = new IQuarable
            {
                CategoryUrl = "cars",
                RuCategoryName = "Автомобили",
                EnCategoryName = "Cars",
                ParentId = 2
            };
            _repository.Setup(r => r.Get(url)).Returns(cat);
            //Act
            var result = _service.GetCategoryIdByUrl(url);
            //Assets
            Assert.AreEqual(result, 0);

        }
        [Test]
        public void GetCategoryNameByUrl()
        {
            //Arrange
            var url = "Cars";
            var cultureEn = "en";
            IQuarable cat = new IQuarable
            {
                CategoryUrl = "cars",
                RuCategoryName = "Автомобили",
                EnCategoryName = "Cars",
                ParentId = 2
            };
            _repository.Setup(r => r.Get(url)).Returns(cat);

            //Act
            var result = _service.GetCategoryNameByUrl(url, cultureEn);

            //Assets
            Assert.AreEqual(url, result);
        }               


    }
}
