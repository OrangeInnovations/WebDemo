using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Domain.AggregatesModels.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Application.Commands;
using WebApp.Application.ViewModels;
using WebApp.Controllers;
using Xunit;

namespace Demo.UnitTests.Application
{
    public class MyUserWebApiTest
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IMyUserRepository> myUserRepositoryMock;
        private readonly Mock<ILogger<MyUserController>> loggerMock;

        public MyUserWebApiTest()
        {
            mediatorMock = new Mock<IMediator>();
            mapperMock = new Mock<IMapper>();
            myUserRepositoryMock = new Mock<IMyUserRepository>();
            loggerMock = new Mock<ILogger<MyUserController>>();
        }

        [Fact]
        public async Task CreateMyUser_success()
        {
            //Arrange
            Mock<MyUser> mockMyUser = new Mock<MyUser>();
            Mock<MyUserVM> mocMyUserVM = new Mock<MyUserVM>();

            mediatorMock.Setup(x => x.Send(It.IsAny<CreateMyUserCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(mockMyUser.Object));

            mapperMock.Setup(x => x.Map<MyUserVM>(mockMyUser.Object)).Returns(mocMyUserVM.Object);


            //Act
            var myUserController = new MyUserController(mediatorMock.Object, mapperMock.Object, myUserRepositoryMock.Object, loggerMock.Object);
            var actionResult = await myUserController.CreateMyUserAsync(It.IsAny<CreateMyUserCommand>());

            //Assert
            var result = actionResult.Result as OkObjectResult;
            
            Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal(result.Value, mocMyUserVM.Object);

        }

        [Fact]
        public async Task GetAllUsers_success()
        {
            //Arrange
            var mockMyUsers = new Mock<List<MyUser>>();
            var mocMyUserVMs = new Mock<List<MyUserVM>>();
            myUserRepositoryMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(mockMyUsers.Object));
            mapperMock.Setup(x => x.Map<List<MyUserVM>>(mockMyUsers.Object)).Returns(mocMyUserVMs.Object);

            //Act
            var myUserController = new MyUserController(mediatorMock.Object, mapperMock.Object, myUserRepositoryMock.Object, loggerMock.Object);
            ActionResult actionResult = await myUserController.GetAllUsers();

            //Assert
            var result= actionResult as OkObjectResult;
            Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal(result.Value, mocMyUserVMs.Object);
        }
    }
}
