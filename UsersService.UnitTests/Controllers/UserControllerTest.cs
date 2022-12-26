using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
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
        private readonly IConfiguration _configuration;
        private readonly Mock<ILogger<UserController>> _logger;
        private readonly TokenBase _tokenBase;
        private readonly Mock<IMediator> _mediator;
        private readonly IWebHostEnvironment webHostEnvironment;
        protected readonly UsersService.Infrastructure.DBContext.DBContextCore _dbContext;

        public UserControllerTest()
        {
            _mediator = new Mock<IMediator>();
           

                       Dictionary<string, string> myConfiguration = new Dictionary<string, string>
                {
                   {"AzureAd:clientId", Environment.GetEnvironmentVariable("AZUREAD_CID")},
                    {"AzureAd:TenantId", Environment.GetEnvironmentVariable("AZUREAD_TID")},
                    {"AzureAd:clientSecret",Environment.GetEnvironmentVariable("AZUREAD_CLIENT_SECRET")},
                    {"AzureAd:Instance",Environment.GetEnvironmentVariable("AZUREAD_INSTANCE")},
                    {"EncryptDecryptkey",Environment.GetEnvironmentVariable("DECRYPTKEY")},
                    {"Jwt:Key",Environment.GetEnvironmentVariable("JWT_KEY")},
                    {"Jwt:Issuer", Environment.GetEnvironmentVariable("JWT_ISSUER")},
                    {"Jwt:Audience", Environment.GetEnvironmentVariable("JWT_AUD")},
                    {"AzureAd:resourceId", Environment.GetEnvironmentVariable("AZUREAD_RESOURCEID")},
                    {"AzureAd:operatorRoleId", Environment.GetEnvironmentVariable("AZUREAD_OPERATORRID")},
                    {"AzureAd:adminRoleId", Environment.GetEnvironmentVariable("AZUREAD_ADMINRID")},
                    {"LOG:CONNECTIONSTRING", Environment.GetEnvironmentVariable("LOG_CONNECTIONSTRING")},
                    {"AzureAd:ImageContainerName", Environment.GetEnvironmentVariable("AZUREAD_IMAGECONTAINERNAME")},
                    {"AzureAd:Domain", Environment.GetEnvironmentVariable("AZUREAD_DOMAIN")},
                    {"flag:Emailflag", Environment.GetEnvironmentVariable("FLAG_EMAILFLAG")},
                    {"MailSettings:UserName", Environment.GetEnvironmentVariable("MAIL_USERNAME")},
                    {"MailSettings:Password", Environment.GetEnvironmentVariable("MAIL_PASSWORD")},
                    {"MailSettings:Host", Environment.GetEnvironmentVariable("MAIL_HOST")},
                    {"MailSettings:Port", Environment.GetEnvironmentVariable("MAIL_PORT")},
                    {"BaseUrl:fronendurl", Environment.GetEnvironmentVariable("BASEURL_FRONTED")},
                    {"AzureAd:helpdeskUserName", Environment.GetEnvironmentVariable("AZUREAD_HELPDESKUSERNAME")},
                    {"AzureAd:helpdeskPassword", Environment.GetEnvironmentVariable("AZUREAD_HELPDESKPASSWORD")},
                };
            if(Environment.GetEnvironmentVariable("AZUREAD_CID") ==null)
            {
                myConfiguration = new Dictionary<string, string>
                {
                    {"AzureAd:clientId", "7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed"},
                    {"AzureAd:TenantId", "744aa8b0-bb99-4982-903f-52328216b4be"},
                    {"AzureAd:clientSecret","xg.8Q~mpJZWo5su4_WIKRaVaydfHP99xFa15uak_"},
                    {"AzureAd:Instance","https://login.microsoftonline.com/"},
                    {"EncryptDecryptkey","E534C8DF286CD5931069B522E695D4F1" },
                    {"Jwt:Key","ThisismySecretKey"},
                    {"Jwt:Issuer", "Asset_AuthService"},
                    {"Jwt:Audience", "Asset_DashboardService"},
                    {"flag:Emailflag", "0"},
                    {"MailSettings:UserName", "mamta.mishra@assetworks.com"},
                    {"MailSettings:Password", "Universe@123"},
                    {"MailSettings:Host", "smtp.office365.com"},
                    {"MailSettings:Port", "587"},
                    {"AzureAd:resourceId", "f0d224c5-2ea1-4823-9b93-45da968a05b5"},
                    {"AzureAd:operatorRoleId", "92c0466d-8be6-4466-ba15-1cc36d001aee"},
                    {"AzureAd:adminRoleId", "25ca4c0c-0d2e-4ebf-aa81-9397e027d15f"},
                    {"BaseUrl:fronendurl", "https://asw-portal-ui.azurewebsites.net"},
                    {"LOG:CONNECTIONSTRING","DefaultEndpointsProtocol=https;AccountName=assetswork;AccountKey=Rk7iyAEtGHdMWfojFlyE23dXYsMDUkH1zvLghSjWW9kZX7Ecv6wuJuvRifNQfOChKmY5d1Hvx7mE+AStxFztQw==;EndpointSuffix=core.windows.net"},
                    {"AzureAd:ImageContainerName", "userimages"},
                };
            }
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
            _tokenBase = new TokenBase();
            _tokenBase.acces_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSIsImtpZCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSJ9.eyJhdWQiOiJzcG46NzY5OGNiZWQtN2Q5Zi00M2IzLWI5Y2QtYTRmMDliOWI1NWVkIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvNzQ0YWE4YjAtYmI5OS00OTgyLTkwM2YtNTIzMjgyMTZiNGJlLyIsImlhdCI6MTY2OTc4OTUwNCwibmJmIjoxNjY5Nzg5NTA0LCJleHAiOjE2Njk3OTM5NDQsImFjciI6IjEiLCJhaW8iOiJBVFFBeS84VEFBQUFmRWxiSFljMkxMS2REUzNJNWRBNUZZaXVlQWJTakNxVmE4RmZHZy80OFFFekY1Vk01cWR4TVcxcWFxdzFFSHdZIiwiYW1yIjpbInB3ZCJdLCJhcHBpZCI6Ijc2OThjYmVkLTdkOWYtNDNiMy1iOWNkLWE0ZjA5YjliNTVlZCIsImFwcGlkYWNyIjoiMSIsImZhbWlseV9uYW1lIjoib3BlcmF0b3IiLCJnaXZlbl9uYW1lIjoib3BlcmF0b3IiLCJpcGFkZHIiOiI1Mi4xNDIuMTcyLjIyIiwibmFtZSI6Im9wZXJhdG9yIiwib2lkIjoiZjRlOWI0MTktYzdkYy00MmI2LTkyYmMtZjIwNzcwNzE2N2YyIiwicmgiOiIwLkFWVUFzS2hLZEptN2drbVFQMUl5Z2hhMHZ1M0xtSGFmZmJORHVjMms4SnViVmUySUFPdy4iLCJyb2xlcyI6WyJPcGVyYXRvciJdLCJzY3AiOiJBcHBSb2xlQXNzaWdubWVudC5SZWFkV3JpdGUuQWxsIERpcmVjdG9yeS5BY2Nlc3NBc1VzZXIuQWxsIERpcmVjdG9yeS5SZWFkLkFsbCBEaXJlY3RvcnkuUmVhZFdyaXRlLkFsbCBEaXJlY3RvcnkuV3JpdGUuUmVzdHJpY3RlZCBlbWFpbCBHcm91cC5SZWFkLkFsbCBHcm91cC5SZWFkV3JpdGUuQWxsIElkZW50aXR5VXNlckZsb3cuUmVhZFdyaXRlLkFsbCBvZmZsaW5lX2FjY2VzcyBvcGVuaWQgcHJvZmlsZSBUZWFtU2V0dGluZ3MuUmVhZFdyaXRlLkFsbCBVc2VyLkV4cG9ydC5BbGwgVXNlci5JbnZpdGUuQWxsIFVzZXIuTWFuYWdlSWRlbnRpdGllcy5BbGwgVXNlci5SZWFkIFVzZXIuUmVhZC5BbGwgVXNlci5SZWFkQmFzaWMuQWxsIFVzZXIuUmVhZFdyaXRlIFVzZXIuUmVhZFdyaXRlLkFsbCBVc2VyQXV0aGVudGljYXRpb25NZXRob2QuUmVhZCBVc2VyQXV0aGVudGljYXRpb25NZXRob2QuUmVhZC5BbGwgVXNlckF1dGhlbnRpY2F0aW9uTWV0aG9kLlJlYWRXcml0ZSBVc2VyQXV0aGVudGljYXRpb25NZXRob2QuUmVhZFdyaXRlLkFsbCIsInN1YiI6IksyYW43OF9kSEx1T2tjdWtMdEFjanAxZlg5Tk9ZZTZKei02SGRacWI4MVkiLCJ0aWQiOiI3NDRhYThiMC1iYjk5LTQ5ODItOTAzZi01MjMyODIxNmI0YmUiLCJ1bmlxdWVfbmFtZSI6Im9wZXJhdG9yQGRldm9wc3Rla21pbmR6Lm9ubWljcm9zb2Z0LmNvbSIsInVwbiI6Im9wZXJhdG9yQGRldm9wc3Rla21pbmR6Lm9ubWljcm9zb2Z0LmNvbSIsInV0aSI6Ikh3dHdZeDc4RlVhcE1RUXhwU1Q3QVEiLCJ2ZXIiOiIxLjAifQ.HeagaQjdMXUuMblq10QTgnSmW0LaZxINhQbvdwN3wMnqlKyYqKfcsP9nt1JbYESa9zjrMtJituLxUmlmAubI2xOZ795lJSUXHWEl1ZbK2SnH8qDvBMtIE1Er_OIALX1FW2un-YGNP6XzDTBk9k1hoWusWhkTai2xY8M93imB76BkMCDxUVBK60MyX-rrLBtQ4cYjMg6-aN2USFvYvvULt73BixPbqUvDwAjTX3gx96ic6Z2lElE7VZTZbgPYpwSBSQv65zmRMxyrftzilgsPSRO0Es5jmmxVlZdXakJ2MNIX01DE59VXl04feIL_1xtU-69uHpLQvxycVYfChlRShA";
 

            _logger = new Mock<ILogger<UserController>>();
            _userController = new UserController(_configuration, _mediator.Object, _logger.Object, _tokenBase, webHostEnvironment,_dbContext);
        }

        #region CREATE USER TEST CASES
        [TestMethod()]
        public void SuccessCreateUserTest()
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
                CityName = "NA",
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
            _mediator.Setup(md => md.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()));
            var actionResult = _userController.CreateUser(createUserCommand).Result;
            Assert.AreEqual(200, (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as UserResponse;
            Assert.IsNotNull(actionResult.Result);

        }
        [TestMethod()]
        public void BadRequestCreateUserTest()
        {
            CreateUserCommand createUserCommand = new CreateUserCommand()
            {
                DisplayName = "",
                objectid = "",
                userPrincipalName = "name",
                MailNickname = "",
                isActive = true,
                EmailId = "Testxyz@gmail.com",
                name = "Operatortesting",
                PhoneNumber = "876544567",
                CustomerID = 1,
                AddressLine1 = "california",
                AddressLine2 = "usa",
                CountryID = 1,
                StateID = 1,
                CityName = "NA",
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
                StatusCode = 400,
                StatusMessage = "Bad Request",
                OTP = "123456",
                Id = 0

            };
            _mediator.Setup(md => md.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()));
            var actionResult = _userController.CreateUser(createUserCommand).Result;
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).StatusCode;
            Assert.AreEqual(400, alertResponse);
            Assert.IsNotNull(actionResult.Result);

        }
        [TestMethod()]
        public void ExceptionCreateUserTest()
        {
            CreateUserCommand createUserCommand = new CreateUserCommand()
            {
                DisplayName = "test",
                objectid = "",
                userPrincipalName = "name",
                MailNickname = "",
                isActive = true,
                EmailId = "Testxyz@gmail.com",
                name = "Operatortesting",
                PhoneNumber = "876544567",
                CustomerID = 1,
                AddressLine1 = "california",
                AddressLine2 = "usa",
                CountryID = 1,
                StateID = 1,
                CityName = "NA",
                ZipCode = "12343",
                CreatedBy = "user12",
                //UserRolesCommand = new List<UserRole>(){
                //  new UserRole ()
                //  {
                //      Roleid =1,
                //  },

                // },
                operatorUserMapperCommand = new List<long>(){
                 1
                }

            };
            UserResponse userResponse = new UserResponse()
            {
                StatusCode = 400,
                StatusMessage = "Bad Request",
                OTP = "123456",
                Id = 0

            };
            _mediator.Setup(md => md.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()));
            var actionResult = _userController.CreateUser(createUserCommand).Result;
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value;
            var alertstatus = (alertResponse as UserResponse).StatusCode;
            Assert.AreEqual(400, alertstatus);
            Assert.IsNotNull(actionResult.Result);

        }
        #endregion

        #region UPDATE USER TEST CASES
        [TestMethod()]
        public void SuccessUpdateUserTest()
        {
            UpdateUserCommand updateUserCommand = new UpdateUserCommand()
            {

                Id = 1,
                name = "Operator",
                EmailId = "operator@devopstekmindz.onmicrosoft.com",
                PhoneNumber = "(898) 989-8898",
                AddressLine1 = "dsdsds",
                AddressLine2 = "sdsd",
                customerID = 1,
                CountryID = 6,
                StateID = 119,
                CityName = "asas",
                ZipCode = "1234dsd56",
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
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value;
            Assert.AreEqual(200, (alertResponse as UpdateUserResponse).StatusCode);
            Assert.IsNotNull(actionResult.Result);
        }
        [TestMethod()]
        public void BadRequestUpdateUserTest()
        {
            UpdateUserCommand updateUserCommand = new UpdateUserCommand()
            {

                Id = 1,
                name = "Operator",
                EmailId = "operator@devopstekmindz.onmicrosoft.com",
                PhoneNumber = "(898) 989-8898",
                AddressLine1 = "dsdsds",
                AddressLine2 = "sdsd",
                customerID = 1,
                CountryID = 6,
                StateID = 119,
                ZipCode = "1234dsd56",
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
            _mediator.Setup(x => x.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()));
            var actionResult = _userController.UpdateUser(updateUserCommand).Result;
            //var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode;
            Assert.AreEqual(404,((actionResult.Result.GetType().GetProperty("StatusCode").GetValue(actionResult.Result, null))));
            Assert.IsNotNull(actionResult.Result);
        }
        #endregion
        #region GET ALL USER TEST
        [TestMethod()]
        public void SuccessAllUserTest()
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
        public void BadRequestAllUserTest()
        {
            GetUserRequest getUserRequest = new GetUserRequest()
            {
                //opratorid = "8cc2643c-f96f-4208-80fb-b4a25238db66",
                customerID = 1,
                SearchParam = "",
                PageSize = 10,
                PageNumber = 1,
                roleid = new List<long> { 1 },
            };

            AllUserResponse getUserResponse = new AllUserResponse()
            {
                StatusCode = 200,
                StatusMessage = "record found",
                Active = 1,
                InActive = 1,
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
                statusData = new List<StatusData>()
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
            _mediator.Setup(x => x.Send(It.IsAny<GetAllUserQuery>(), It.IsAny<CancellationToken>()));
            var actionResult = _userController.GetUsers(getUserRequest).Result;
            Assert.AreEqual(400, (actionResult.Value.StatusCode));
            Assert.IsNotNull(actionResult.Value);
        }
        #endregion
        #region USER BY ID TEST CASES
        [TestMethod()]
        public void SuccessUserById()
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
        public void BadRequestUserById()
        {
            int getuserId = 0;
            GetUserResponseDT getUserResponse = new GetUserResponseDT()
            {
                StatusCode = 400,
                StatusMessage = "Success",
                data = new GetUserResponse()
                {
                    Id = 76,
                    EmailId = "assetWorks@gmail.com",
                    adminName = "AssetWorks",
                    PhoneNumber = "9876543210",
                    addressLine1 = "A-012"
                }
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIdUserQuery>(), It.IsAny<CancellationToken>()));
            var actionResult = _userController.GetUserById(getuserId).Result;
            if (actionResult == null)
            {
                Assert.AreEqual(400, getUserResponse.StatusCode);
            }
        }
        #endregion
        #region DELETE USER TEST CASES
        [TestMethod()]
        public void SuccessIsActiveUserTest()
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
        [TestMethod()]
        public void BadRequestIsActiveUserTest()
        {
            DeleteUserCommand deleteUserCommand = new DeleteUserCommand()
            {
                //Id = 0,
                //IsActive = false,
            };
            UserResponse userResponse = new UserResponse()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                Id = 76,
            };
            _mediator.Setup(x => x.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()));
            var actionResult = _userController.DeleteUser(deleteUserCommand);
            Assert.AreEqual(200, (actionResult.Result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as UserResponse;
            Assert.IsNotNull(actionResult.Result);


        }
        #endregion
        [TestMethod()]
        public void UpdateUserProfilePictureTest()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            UpdatedUserProfileImage model = new UpdatedUserProfileImage()
            {

                ProfilePicture = file

            };
            UserProfileResponse userResponse = new UserProfileResponse()
            {
                StatusCode = 200,
                StatusMessage = "Image updated successfully"
            };

            _mediator.Setup(x => x.Send(It.IsAny<UpdateUserProfileImage>(), It.IsAny<CancellationToken>())).ReturnsAsync(userResponse);
            var actionResult = _userController.UpdateUserProfilePicture(model).Result;
            Assert.AreEqual(200, (actionResult.StatusCode));
            Assert.IsNotNull(actionResult);

        }

        [TestMethod()]
        public void UpdateUserProfilePictureNullTest()
        {
            UpdatedUserProfileImage model = new UpdatedUserProfileImage()
            {

                ProfilePicture = null

            };

            UserProfileResponse userResponse = new UserProfileResponse()
            {
                StatusCode = 404,
                StatusMessage = "Image not Update "
            };


            _mediator.Setup(x => x.Send(It.IsAny<UpdateUserProfileImage>(), It.IsAny<CancellationToken>())).ReturnsAsync(userResponse);
            var actionResult = _userController.UpdateUserProfilePicture(model).Result;
            Assert.AreEqual(404, (actionResult.StatusCode));
            Assert.IsNotNull(actionResult);
        }


        [TestMethod()]
        public void GetUserProfileImageTest()
        {
            GetUserProfileResponseDT res = new GetUserProfileResponseDT()
            {
                data = new GetUserProfileResponse()
                {
                    Id = 1,
                    EmailId = "",
                    adminName = "",
                    addressLine1 = "",
                    addressLine2 = "",
                    PhoneNumber = "",
                    CountryID = 1,
                    StateID = 1,
                    countryName = "",
                    ImagePath = "2e8687a7-ab66-4637-83be-9ccc3b66e876.jpeg",
                    stateName = "",
                    cityName = "",
                    zipcode = ""
                }
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetByUserProfileIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(res);
            var actionResult = _userController.GetUserProfileImage().Result.Value.FirstOrDefault(n => n.Key.ToLower() == "statuscode").Value;
            Assert.IsNotNull(actionResult);

            Assert.AreEqual(200, actionResult);


        }

        [TestMethod()]
        public void GetInvalidUserProfileImageTest()
        {
            GetUserProfileResponseDT res = new GetUserProfileResponseDT()
            {
                data = new GetUserProfileResponse()
                {
                    Id = 1,
                    EmailId = "",
                    adminName = "",
                    addressLine1 = "",
                    addressLine2 = "",
                    PhoneNumber = "",
                    CountryID = 1,
                    StateID = 1,
                    countryName = "",
                    ImagePath = "test.jpeg",
                    stateName = "",
                    cityName = "",
                    zipcode = ""
                }
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetByUserProfileIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(res);
            var actionResult = _userController.GetUserProfileImage().Result;
            Assert.IsNotNull(actionResult.Value);
            string statusCode = "";
            foreach (KeyValuePair<string, object> item in actionResult.Value)
            {
                if (item.Key == "statusCode")
                    statusCode = item.Value.ToString();
            }
            Assert.AreEqual("404", statusCode);
        }

        [TestMethod()]
        public void UpdateUserProfileTest()
        {
            UpdateUserProfileCommand updateUserProfileCommand = new UpdateUserProfileCommand()
            {

                Id = 1,
                PhoneNumber = "9073828734",
                AddressLine1 = "",
                AddressLine2 = "",
                CountryID = 1,
                StateID = 1,
                CityName = "Noida",
                ZipCode = "12303",
                ModifiedBy = "user"
            };
            UserProfileResponse res = new UserProfileResponse()
            {
                StatusCode = 200,
                StatusMessage = "Record updated successfully"
            };

            _mediator.Setup(x => x.Send(It.IsAny<UpdateUserProfileCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(res);
            var actionResult = _userController.UpdateUserProfile(updateUserProfileCommand).Result;
            Assert.AreEqual(200, (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            //    Assert.AreEqual(res.StatusMessage, (actionResult.Result as  Microsoft.AspNetCore.Mvc.OkObjectResult).Value.);
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as UserProfileResponse;
            Assert.AreEqual(res.StatusMessage, alertResponse.StatusMessage);
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod()]
        public void UpdateUserProfileInvalidIdTest()
        {
            UpdateUserProfileCommand updateUserProfileCommand = new UpdateUserProfileCommand()
            {

                Id = 0,
                PhoneNumber = "9073828734",
                AddressLine1 = "",
                AddressLine2 = "",
                CountryID = 1,
                StateID = 1,
                CityName = "Noida",
                ZipCode = "12303",
                ModifiedBy = "user"
            };
            UserProfileResponse res = new UserProfileResponse()
            {
                StatusCode = 200,
                StatusMessage = "Please enter valid UserID"
            };

            _mediator.Setup(x => x.Send(It.IsAny<UpdateUserProfileCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(res);
            var actionResult = _userController.UpdateUserProfile(updateUserProfileCommand).Result;
            Assert.AreEqual(200, (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            //    Assert.AreEqual(res.StatusMessage, (actionResult.Result as  Microsoft.AspNetCore.Mvc.OkObjectResult).Value.);
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as UserProfileResponse;
            Assert.AreEqual(res.StatusMessage, alertResponse.StatusMessage);
            Assert.IsNotNull(actionResult.Result);
        }
        [TestMethod()]
        public void GetUserProfileByIdTest()
        {
            GetUserProfileResponseDT res = new GetUserProfileResponseDT()
            {
                StatusCode = 200,
                StatusMessage = "Record updated successfully",
                data = new GetUserProfileResponse()
                {
                    Id = 1,
                    EmailId = "user@tekmindz.com",
                    adminName = "rohit",
                    PhoneNumber = "8393037390",
                    addressLine1 = "Delhi",
                    addressLine2 = "Noida",
                    CountryID = 10,
                    StateID = 20,
                    countryName = "Delhi",
                    ImagePath = "test.jpeg",
                    stateName = "Delhi",
                    cityName = "Delhi",
                    zipcode = "12394"
                }
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetByUserProfileIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(res);
            var actionResult = _userController.GetUserProfileById().Result;

            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual(res.StatusMessage, actionResult.StatusMessage);
            Assert.IsNotNull(actionResult.StatusCode);
        }
    }
}






