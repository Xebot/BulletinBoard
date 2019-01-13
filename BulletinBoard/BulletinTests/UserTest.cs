using AppServices.Services;
using AutoMapper;
using BulletinDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace BulletinTests
{
    public class UserTest
    {
        private UserService _service;
        private Mock<IMapper> _mapper;
        private Mock<UserManager<User>> _userManager;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            
            var roleStore = new Mock<IRoleStore<IdentityRole<Guid>>>();
            var UserStoreMock = Mock.Of<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(UserStoreMock, null, null, null, null, null, null, null, null);
            var roleManager = new Mock<RoleManager<IdentityRole<Guid>>>(roleStore.Object, null, null, null, null);
            var userMgr = new Mock<UserManager<User>>(UserStoreMock, null, null, null, null, null, null, null, null);
            _service = new UserService(_userManager.Object, _mapper.Object, roleManager.Object, null);

        }

        [Test]
        public void GetUserAsync()
        {
            //Arrange
            Guid guid = new Guid();
            User user = new User()
            {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",

            };
            _userManager.Setup(x => x.FindByIdAsync(Convert.ToString(guid))).Returns(Task.FromResult(user));
            //Act
            var result = _service.GetUserAsync(guid);
            //Assets
            _userManager.Verify(x => x.FindByIdAsync(Convert.ToString(guid)), Times.Once);

        }
        [Test]
        public void FindByEmailAsync()
        {
            //Arrange            
            User user = new User()
            {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",

            };
            UserDto userDto = new UserDto()
            {
                UserName = "mail@mail.com",
                Login = "mail@mail.com",
                UserEmail = "mail@mail.com",
                UserTel = "+7(978)111-11-11"
            };
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _mapper.Setup(x => x.Map<UserDto>(user)).Returns(userDto);
            //Act
            var res = _service.GetUserByEmailAsync("email@email.com").Result;
            //Assets
            _userManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(userDto, res.Result);

        }
        [Test]
        public void UpdateUserAsync()
        {
            //Arrange            
            UserDto userDto = new UserDto()
            {
                UserName = "mail@mail.com",
                Login = "mail@mail.com",
                UserEmail = "mail@mail.com",
                UserTel = "+7(978)111-11-11"
            };
            User user = new User()
            {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",

            };
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = _service.UpdateUserAsync(userDto);
            //Assets
            _userManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }
        [Test]
        public void GetUserRole()
        {
            //Arrange
            Guid guid = new Guid();
            User user = new User()
            {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",

            };
            IList<string> roles = new List<string>()
            {
                "Admin"
            };
            _userManager.Setup(x => x.FindByIdAsync(Convert.ToString(guid))).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.GetRolesAsync(user)).Returns(Task.FromResult(roles));
            //Act
            var result = _service.GetUserRole(guid);
            //Assets
            _userManager.Verify(x => x.GetRolesAsync(user), Times.Once);
            _userManager.Verify(x => x.FindByIdAsync(Convert.ToString(guid)), Times.Once);
        }
    }
}
