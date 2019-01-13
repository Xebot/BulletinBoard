using AppServices.Services;
using AutoMapper;
using BulletinDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;
using System.Linq;

namespace BulletinTests
{
    [TestFixture]
    public class AdminTest
    {
        private AdminService _service;
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
            _service = new AdminService(_userManager.Object, _mapper.Object, roleManager.Object);
            
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
        [Test]
        public void GetAllUsersAsync()
        {
            //Arrange
            Guid guid = new Guid();
            List<User> users = new List<User>()
            {
                new User {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",
                Id = guid
                },
                new User {
                UserName = "mail1@mail.com",
                Email = "mail1@mail.com",
                Id = guid
                },
                new User
                {
                UserName = "mail2@mail.com",
                Email = "mail2@mail.com",
                Id = guid
                }
            };
            List<UserDto> usersDto = new List<UserDto>()
            {
                 new UserDto {
                UserName = "mail@mail.com",
                UserEmail = "mail@mail.com",
                Id = guid
                },
                new UserDto {
                UserName = "mail1@mail.com",
                UserEmail = "mail1@mail.com",
                Id = guid
                },
                new UserDto
                {
                UserName = "mail2@mail.com",
                UserEmail = "mail2@mail.com",
                Id = guid
                }
            };

            IList<string> roles = new List<string>()
            {
                "Admin"
            };
            _userManager.Setup(x => x.Users).Returns(users.AsQueryable);
            _mapper.Setup(x => x.Map<List<UserDto>>(users)).Returns(usersDto);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).Returns(Task.FromResult(roles));
            //Act
            _service.GetAllUsersAsync();

            //Assets
            _userManager.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.AtLeast(3));
            _userManager.Verify(x => x.Users, Times.AtLeastOnce);
        }
        [Test]
        public void MakeAdmin()
        {
            //Arrange
            Guid guid = new Guid();
            User user = new User()
            {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",

            };
            _userManager.Setup(x => x.FindByIdAsync(Convert.ToString(guid))).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.AddToRoleAsync(user, It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            //Act
            _service.MakeAdmin(guid);
            //Assets
            _userManager.Verify(x => x.FindByIdAsync(Convert.ToString(guid)), Times.Once);
            _userManager.Verify(x => x.AddToRoleAsync(user, "Admin"), Times.Once);
        }
        [Test]
        public void UnMakeAdmin()
        {
            //Arrange
            Guid guid = new Guid();
            User user = new User()
            {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",

            };
            _userManager.Setup(x => x.FindByIdAsync(Convert.ToString(guid))).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.RemoveFromRoleAsync(user, It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            //Act
            _service.MakeAdmin(guid);
            //Assets
            _userManager.Verify(x => x.FindByIdAsync(Convert.ToString(guid)), Times.Once);            
        }
        [Test]
        public void BanUser()
        {
            //Arrange
            Guid guid = new Guid();
            User user = new User()
            {
                UserName = "mail@mail.com",
                Email = "mail@mail.com",
                UserStatus = "unbanned"

            };
            _userManager.Setup(x => x.FindByIdAsync(Convert.ToString(guid))).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            //Act
            var res = _service.banUser(guid).Result;
            //Assets
            _userManager.Verify(x => x.FindByIdAsync(Convert.ToString(guid)), Times.Once);
            _userManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }
    }
}
