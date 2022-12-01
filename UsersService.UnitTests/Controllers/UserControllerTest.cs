using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UsersService.Application.Commands.Customer;
using UsersService.Application.Commands.Users;
using UsersService.Application.Queries;
using UsersService.Application.Responses.Customer;
using UsersService.Core.PagingHelper;
using UsersService.Core.Queries;
using UsersService.Core.Response;
using UsersService.Infrastructure.Helpers;
using UsersService.Responses.Users;

namespace UsersService.Api.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTest
    {
        private readonly UserController _userController;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<ILogger<UserController>> _logger;
        private readonly TokenBase _tokenBase;
        private readonly Mock<IMediator> _mediator;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _configuration = new Mock<IConfiguration>();

            _logger = new Mock<ILogger<UserController>>();


            _userController = new UserController(_configuration.Object, _mediator.Object, _logger.Object, _tokenBase, webHostEnvironment);



        }
        [TestMethod()]
        public void CreateUser()
        {
            CreateUserCommand createUserCommand = new CreateUserCommand()
            {
                DisplayName = "name123",
                objectid = "1",
                userPrincipalName = "",
                MailNickname = "",
                isActive = true,
                EmailId = "asdfg@gmail.com",
                name = "asdfgh",
                PhoneNumber = "876544567",
                CustomerID = 1,
                AddressLine1 = "california",
                AddressLine2 = "usa",
                CountryID = 1,
                StateID = 1,
                //CityID = 1,
                ZipCode = "12343",
                CreatedBy = "user12",
                UserRolesCommand = new List<UserRole>(){
                  new UserRole ()
                  {
                      Roleid =1,
                  },

                 },
                operatorUserMapperCommand = new List<long>(){
                 1
                }

            };

            UserResponse userResponse = new UserResponse()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                OTP = "123456",
                Id = 0

            };
            _mediator.Setup(md => md.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(userResponse);
            var actionResult = _userController.CreateUser(createUserCommand).Result;
            Assert.AreEqual(200, (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as UserResponse;
            Assert.IsNotNull(actionResult.Result);

        }

        [TestMethod()]
        public void UpdateUserCommand()
        {
            UpdateUserCommand updateUserCommand = new UpdateUserCommand()
            {

                Id = 88,
                name = "sharma",
                EmailId = "sharma@1234",
                PhoneNumber = "9878987899",
                AddressLine1 = "12345asdf",
                AddressLine2 = "asdfghj",
                customerID = 1,
                CountryID = 1,
                StateID = 1,
                //CityID = 1,
                ZipCode = "",
                ModifiedBy = "user1",
                UserRolesCommand = new List<upUserRole>(){
                  new upUserRole (){
                                  Id=1,
                                  RoleID =3,
                                   },

                   },
                operatorUserMapperCommand = new List<long>(){
                 1
                  }
            };
            UpdateUserResponse userResponse = new UpdateUserResponse()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                OTP = "123456",
                Id = 1,

            };
            _mediator.Setup(x => x.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(userResponse);
            var actionResult = _userController.UpdateUser(updateUserCommand).Result;
            Assert.AreEqual(200, (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as UserResponse;
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod()]
        public void GetAllUserTest()
        {
            GetUserRequest getUserRequest = new GetUserRequest()
            {
                opratorid = "8cc2643c-f96f-4208-80fb-b4a25238db66",
                customerID = 1,
                 SearchParam="",
                 PageSize=10,
                 PageNumber=1,
                roleid = new List<long> { 1 },
            };

            AllUserResponse getUserResponse = new AllUserResponse()
            {
                StatusCode = 200,
                StatusMessage = "record found",
                Active=1,
                InActive=1,
                data = new PagedList<GetUserResponse>()
                 { new GetUserResponse()
                 {
                      Id =5,
                      EmailId= "trt@gmail.com",
                      adminName= "ftrqwer",
                      PhoneNumber= "9876543211",
                      addressLine1= "asdfghjkqw",
                      addressLine2= "asdfghjkqw",
                      CountryID =1,
                      StateID =1,

                      countryName= "United Kingdom",
                      stateName="England",
                      cityName = "London",
                      createdBy="user",
                      zipcode="12345",
                      CreatedOn=DateTime.Now,
                      modifiedBy = "User12",
                      modifiedOn = DateTime.Now,
                      customername ="a",
                       customerID= 1,
                      IsActive=true,

                 }
                 },
                statusData=new List<StatusData>()
                {
                    new StatusData()
                    {
                         Key="key",
                         Color="Red",
                         Value="value"
                    }
                },
                paginationResponse = new UsersService.Core.PagingHelper.PaginationResponse()
                {

                    CurrentPage = 1,
                    HasNext = true,
                    HasPrevious = true,
                    PageSize = 1,
                    TotalCount = 20,
                    TotalPages = 100,
                }



            };
            _mediator.Setup(x => x.Send(It.IsAny<GetAllUserQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(getUserResponse);
            var actionResult = _userController.GetUsers(getUserRequest).Result;
            Assert.AreEqual(200, (actionResult.Value.StatusCode));
            Assert.IsNotNull(actionResult.Value);

        }

        [TestMethod()]
        public void GetUserById()
        {
            int getuserId = 76;
            GetUserResponseDT getUserResponse = new GetUserResponseDT()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                data=new GetUserResponse()
                {
                    Id = 76,
                    EmailId = "assetWorks@gmail.com",
                    adminName = "AssetWorks",
                    PhoneNumber = "9876543210",
                    addressLine1 = "A-012",
                    addressLine2 = "abc",
                    CountryID = 1,
                    StateID = 1,
                    countryName = "UnitedState",
                    stateName = "LosAngeles",
                    cityName = "Carson",
                    createdBy = "string",
                    zipcode = "string",
                    CreatedOn = DateTime.Now,
                    modifiedBy = "string",
                    modifiedOn = DateTime.Now,
                    customerID = 1,
                    customername = "john",
                    IsActive = true,
                }
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIdUserQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(getUserResponse);
            var actionResult = _userController.GetUserById(getuserId).Result;
            Assert.IsNotNull(actionResult.StatusCode);
            Assert.AreEqual(200, ((actionResult)).StatusCode);
        }

        [TestMethod()]
        public void IsActiveUserTest()
        {
            DeleteUserCommand deleteUserCommand = new DeleteUserCommand()
            {
                Id = 76,
                IsActive = false,

            };
            UserResponse userResponse = new UserResponse()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                Id = 76,

            };
            _mediator.Setup(x => x.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(userResponse);
            var actionResult = _userController.DeleteUser(deleteUserCommand);
            Assert.AreEqual(200, (actionResult.Result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as UserResponse;
            Assert.IsNotNull(actionResult.Result);


        }



    }
}






